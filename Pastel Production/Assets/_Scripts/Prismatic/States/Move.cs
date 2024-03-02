using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prismatic
{
    [System.Serializable]
    public class Move : State
    {
        private Vector2 movementInput = Vector2.zero;
        [SerializeField]
        private float speed = 5.0f;
        private float viewHeight = 2.0f;
        private float viewDistance = 3.0f;

        private float xAngle, yAngle;
        private float yAngleLimit;


        public override void Update(SimulationData data)
        {
            UpdatePosition(data);

            UpdateView(data);
        }

        private void UpdatePosition(SimulationData data)
        {
            float stepHeight=0.5f;
            float entityRadius = 0.5f;
            float groundDistance = 0.6f;
            Vector3 entityCenter = data.currentEntity.Position + Vector3.up * 0.5f;
            Quaternion view = CameraUtility.GetView(data, entityCenter, groundDistance);
            Matrix4x4 transformMatrix = Matrix4x4.Rotate(view);
            Vector3 nextPosition = data.currentEntity.Position + (Vector3)(transformMatrix * new Vector3(movementInput.x, 0, movementInput.y) * Time.deltaTime * speed);

            if (nextPosition != data.currentEntity.Position)
            {

                Vector3 movementVector = nextPosition - data.currentEntity.Position;
                data.currentEntity.Rotation = Quaternion.LookRotation(movementVector, Vector3.up);
            }

            int collisionLayer = 1 << LayerMask.NameToLayer("Default");


            //Check if next position is within a HueBarrier
            Collider[] hueBarriers = Physics.OverlapSphere(nextPosition, 0.5f, 1 << LayerMask.NameToLayer("HueBarrier"));
            if (hueBarriers.Length > 0)
            {
                if (hueBarriers[0].gameObject.GetComponent<HueBarrier>().hue != data.currentEntity.HueMix)
                {
                    collisionLayer = (1 << LayerMask.NameToLayer("Default")) + (1 << LayerMask.NameToLayer("HueBarrier"));
                }
            }
            //check for a walkable sufrace
            Vector3 nextGround= nextPosition;
            if (Physics.SphereCast(
                nextPosition + Vector3.up * (stepHeight + entityRadius), 
                entityRadius, 
                Vector3.down, 
                out RaycastHit hit, 
                stepHeight * 2, collisionLayer))
            {
                nextGround.y = hit.point.y;
            }
            else
            {
                //falling
                nextGround.y -= 10 * Time.deltaTime;
            }

            if (Physics.OverlapSphere(nextGround + Vector3.up * entityRadius, entityRadius,collisionLayer).Length == 0)
            {
                nextPosition = nextGround;
            }
            else
            {
                nextPosition = data.currentEntity.Position;
            }

            //TODO: Replace with a level end sequence
            if (Physics.CheckSphere(nextPosition, 0.5f, 1 << LayerMask.NameToLayer("LevelEnd")))
            {
                //Transition to next level
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            
            Debug.DrawRay(data.currentEntity.Position, (nextPosition - data.currentEntity.Position).normalized, Color.red);
            data.currentEntity.Position = nextPosition;
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            this.movementInput = movementInput;
        }

        public override void Enter(Action<StateType> transition, SimulationData data)
        {
            yAngleLimit = SimulationData.maxYAngle;
            xAngle = data.XYAngles.x;
            yAngle = data.XYAngles.y;
            OnMoveInput(Vector2.zero);//TODO: Make move input consistent between states, the player should be able to ready movement in the previous state
            base.Enter(transition, data);
        }

        public override void OnMouseMove(Vector2 mouseDelta)
        {
            xAngle = CameraUtility.AdjustAngle(mouseDelta.x, xAngle, 90);
            yAngle = CameraUtility.AdjustAngle(mouseDelta.y, yAngle, yAngleLimit);
           
        }

        private void UpdateView(SimulationData data)
        {
            data.ViewPosition = CameraUtility.CalculateLookAtDirection(data, viewDistance, viewHeight);
            data.ViewTarget = CameraUtility.CalculateEyeLevel(data, viewHeight);
            data.XYAngles = new Vector2(xAngle, yAngle);
        }

        public override void OnMerge(SimulationData simulationData)
        {
            Transition(StateType.Merge);
        }

        public override void OnShift(SimulationData simulationData)
        {
            Transition(StateType.Shift);
        }
    }

}
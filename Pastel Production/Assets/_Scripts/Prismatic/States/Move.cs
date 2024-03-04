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
        [SerializeField] // Here while we tune in the best-feeling value for this, then we can remove this from the inspector
        private float turnSpeedInRadians = 0.5f; 
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
            Vector3 moveInput = new Vector3(movementInput.x, 0, movementInput.y);

            Physics.SphereCast(entityCenter+ Vector3.up*entityRadius, entityRadius, Vector3.down, out RaycastHit hit, groundDistance+entityRadius, 1 << LayerMask.NameToLayer("Default"));
            Vector3 movementNormal = hit.normal;

            Vector3 viewForward = Vector3.ProjectOnPlane(data.ViewTarget - data.ViewPosition,Vector3.up).normalized;
            Vector3 moveForward = Quaternion.LookRotation(viewForward, Vector3.up)*moveInput;
            Plane movePlane = new Plane(entityCenter, entityCenter+moveForward, entityCenter+Vector3.up);
            Vector3 moveLine = Vector3.Cross(movementNormal,movePlane.normal).normalized;

            Vector3 moveDelta = moveLine * Time.deltaTime * speed;
            Vector3 nextPosition = data.currentEntity.Position + moveDelta;


            // See if the camera has to rotate to keep up with the player changing direction
            float dot_prod = Vector3.Dot(viewForward, moveForward.normalized);
            bool turning = dot_prod < 0.75f && dot_prod > -0.75f && moveForward.magnitude > 0;
            if (turning)
            {
                xAngle += turnSpeedInRadians * movementInput.x;
            }

            if (nextPosition != data.currentEntity.Position)
            {
                data.currentEntity.Rotation = Quaternion.LookRotation(moveDelta.normalized, movementNormal);
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
                out hit, 
                stepHeight * 2, collisionLayer))
            {
                nextGround.y = hit.point.y;
            }
            else
            {
                //falling
                nextGround.y -= 10 * Time.deltaTime;
            }

            if (Physics.OverlapSphere(nextGround + Vector3.up * (entityRadius+0.01f), entityRadius,collisionLayer).Length == 0)
            {
                
                nextPosition = nextGround;
            }
            else
            {
                Debug.Log("BBlocked");
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
            xAngle += mouseDelta.x;
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
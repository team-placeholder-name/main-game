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
        private float xAngle, yAngle;
        private float yAngleLimit;


        public override void Update(SimulationData data)
        {
            UpdatePosition(data);

            UpdateView(data);
        }

        private void UpdatePosition(SimulationData data)
        {
            float groundDistance = 0.6f;
            Vector3 entityCenter = data.currentEntity.Position + Vector3.up * 0.5f;
            Physics.Raycast(entityCenter, Vector3.down, out RaycastHit hit, groundDistance, 1 << LayerMask.NameToLayer("Default"));
            Vector3 movementNormal = hit.normal;



            Vector3 viewForward = (data.ViewTarget - data.ViewPosition);
            viewForward.y = 0;
            viewForward = viewForward.normalized;
            Vector3 moveForward = Vector3.ProjectOnPlane(viewForward, movementNormal).normalized;
            Quaternion view = Quaternion.LookRotation(moveForward, Vector3.up);

            
            Matrix4x4 transformMatrix = Matrix4x4.Rotate(view);

            Vector3 nextPosition = data.currentEntity.Position + (Vector3)(transformMatrix * new Vector3(movementInput.x, 0, movementInput.y) * Time.deltaTime * speed);

            if (nextPosition != data.currentEntity.Position)
            {

                Vector3 movementVector = nextPosition - data.currentEntity.Position;
                data.currentEntity.Rotation = Quaternion.LookRotation(movementVector, Vector3.up);
            }
            //Check for walls blocking movment
            if (
            Physics.SphereCast(entityCenter,
                                0.4f,
                                nextPosition - data.currentEntity.Position,
                                out hit,
                                (nextPosition - data.currentEntity.Position).magnitude,
                                1 << LayerMask.NameToLayer("Default"))
            )
            {
                nextPosition = data.currentEntity.Position;
            }

            if (Physics.SphereCast(nextPosition + Vector3.up * 0.6f, 0.5f, Vector3.down, out hit, groundDistance, 1 << LayerMask.NameToLayer("Default")))
            {
                //snap the character to the ground
                nextPosition.y = hit.point.y;

            }
            if (!Physics.Raycast(nextPosition + Vector3.up * 0.5f, Vector3.down, 1, 1 << LayerMask.NameToLayer("Default")))
            {
                //check if the next position is a valid ground position
                nextPosition = data.currentEntity.Position;
            }

            //Check if next position is within a HueBarrier
            Collider[] hueBarriers = Physics.OverlapSphere(nextPosition, 0.5f, 1 << LayerMask.NameToLayer("HueBarrier"));
            if(hueBarriers.Length>0)
            {
                if (hueBarriers[0].gameObject.GetComponent<HueBarrier>().hue != data.currentEntity.HueMix)
                {
                    nextPosition = data.currentEntity.Position;
                }
            }
            if(Physics.CheckSphere(nextPosition, 0.5f, 1 << LayerMask.NameToLayer("LevelEnd")))//TODO: Replace with a level end sequence
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
            yAngle += mouseDelta.y;
            yAngle = Mathf.Clamp(yAngle, -yAngleLimit*0.5f, yAngleLimit);
           
        }
        public override void OnSelect(SimulationData data)
        {
            Transition(StateType.Swap);
        }

        private void UpdateView(SimulationData data)
        {
            float viewDistance = 5;
            Vector3 offset = Quaternion.AngleAxis(xAngle, Vector3.up) * Quaternion.AngleAxis(yAngle, Vector3.right) * Vector3.back*viewDistance;
            data.ViewPosition = data.currentEntity.Position + offset;
            data.ViewTarget = data.currentEntity.Position;
            data.XYAngles = new Vector2(xAngle, yAngle);
        }

        public override void OnProject(SimulationData simulationData)
        {
            Transition(StateType.Project);
        }

        public override void OnRefract(SimulationData simulationData)
        {
            Transition(StateType.Refract);
        }
    }

}
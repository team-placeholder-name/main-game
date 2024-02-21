using System;
using UnityEngine;
using PastelUtilities;
namespace Prismatic
{
    [System.Serializable]
    public class Swap : State
    {
        private float xAngle, yAngle;
        private float yAngleLimit;


        public override void Update(SimulationData data)
        {
            Vector3 viewDirection = data.ViewTarget - data.ViewPosition;
            Debug.DrawRay(data.ViewPosition, viewDirection * 10, Color.yellow);
            UpdateView(data);

        }

        public override void Enter(Action<StateType> transition, SimulationData data)
        {
            yAngleLimit = SimulationData.maxYAngle;
            xAngle = data.XYAngles.x;
            yAngle = data.XYAngles.y;
            base.Enter(transition, data);
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            Transition(StateType.Move);
        }

        public override void OnMouseMove(Vector2 mouseDelta)
        {
            xAngle += mouseDelta.x;
            yAngle += mouseDelta.y;
            yAngle = Mathf.Clamp(yAngle, -yAngleLimit, yAngleLimit);
        }

        public override void OnSelect(SimulationData data)
        {
            float smallestAngle = 1000f;
            PrismaticEntity targetEntity = null;
            Vector3 viewDirection = data.ViewTarget - data.ViewPosition;
            for (int i = 0; i < data.entities.Count; i++)
            {
                if (data.entities[i] == data.currentEntity) continue;

                PrismaticEntity entityToCheck = data.entities[i];

                //First check for obstructions
                Vector3 entityOffset = entityToCheck.Position - data.currentEntity.Position;
                if (Physics.Raycast(data.currentEntity.Position, entityOffset, entityOffset.magnitude, 1 << LayerMask.NameToLayer("Default")))
                {
                    Debug.Log("Entity Blocked");
                    continue;
                }


                //Second, check if it's the closest to the players view
                Vector3 entityDirection = entityToCheck.Position - data.ViewPosition;
                float angle = Vector3.Angle(viewDirection, entityDirection);
                if (angle < smallestAngle)
                {
                    smallestAngle = angle;
                    targetEntity = data.entities[i];
                }
            }

            if (targetEntity != null)
            {
                //Do the hustle
                data.currentEntity = targetEntity;
                Transition(StateType.Move);
            }
            
        }

        public override void OnProject(SimulationData simulationData)
        {
            throw new System.NotImplementedException();
        }

        private void UpdateView(SimulationData data)
        {
            float viewDistance = 5;
            Vector3 offset = Quaternion.AngleAxis(xAngle, Vector3.up) * Quaternion.AngleAxis(yAngle, Vector3.right) * Vector3.back * viewDistance;
            data.ViewPosition = data.currentEntity.Position + offset;

            // We need to give ourselves a little headroom - otherwise we'll just intersect with the main player object.
            data.ViewTarget = data.currentEntity.Position + Vector3.up * 2 ;
            data.XYAngles = new Vector2(xAngle, yAngle);
        }

        public override void OnRefract(SimulationData simulationData)
        {
            throw new NotImplementedException();
        }
    }
}
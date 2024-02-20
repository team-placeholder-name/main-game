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
            yAngleLimit = data.maxYAngle;
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
            Vector3 viewDirection = data.ViewTarget - data.ViewPosition;
            for (int i = 0; i < data.entities.Count; i++)
            {
                PrismaticEntity entityToCheck = data.entities[i];
                Vector3 posToCheck = entityToCheck.Position;
                if (UtilityFunctions.VectorPointIntersect(data.ViewPosition, viewDirection, posToCheck, 0.5f)) // Probably should add this a controllable parameter
                {
                    // We swap
                    data.currentEntity = entityToCheck;
                    Transition(StateType.Move);
                }
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

    }
}
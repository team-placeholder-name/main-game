using System;
using UnityEngine;
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
            //Debug.DrawRay(data.ViewPosition, viewDirection * 10, Color.yellow);
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

        public override void OnSelect(SimulationData simulationData)
        {
            // On a select input, send out a ray from the camera view direction.
            // If that ray collides with another entity, swap controls with it
            RaycastHit rayHit;

            int targetLayer = 1 << LayerMask.NameToLayer("Default");

            Vector3 viewDirection = simulationData.ViewTarget - simulationData.ViewPosition;
            
            if (Physics.Raycast(simulationData.ViewPosition, viewDirection, out rayHit, Mathf.Infinity, targetLayer))
            {
                Debug.Log(rayHit.transform.gameObject.tag);
                if (rayHit.transform.gameObject.CompareTag("Player"))
                {
                    // If I hit an entity, swap with it
                    // It would be cool if the entity index was queryable directly, so we didn't have to iterate
                    Vector3 hitPosition = rayHit.transform.position;
                    for (int i = 0; i < simulationData.entities.Count; i++)
                    {
                        if (hitPosition == simulationData.entities[i].Position)
                        {
                            simulationData.currentEntity = simulationData.entities[i];
                            Transition(StateType.Move);
                            return;
                        }
                    }
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
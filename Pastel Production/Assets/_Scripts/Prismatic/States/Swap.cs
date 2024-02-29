using System;
using UnityEngine;
using PastelUtilities;
namespace Prismatic
{
    [System.Serializable]
    public class Swap : State
    {
        [SerializeField]
        private Reticule reticule;
        private float viewDistance =  3.0f;
        private float viewHeight = 2.0f;
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
            reticule.gameObject.SetActive(true);
            base.Enter(transition, data);
        }

        public override void Exit(SimulationData data)
        {
            reticule.gameObject.SetActive(false);
            base.Exit(data);
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            Transition(StateType.Move);
        }

        public override void OnMouseMove(Vector2 mouseDelta)
        {
            xAngle += mouseDelta.x;
            yAngle += mouseDelta.y;
            yAngle = Mathf.Clamp(yAngle, -yAngleLimit * 0.55f, yAngleLimit);

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
            data.ViewPosition = CameraUtility.CalculateLookAtDirection(data, viewDistance, viewHeight);
            data.ViewTarget = CameraUtility.CalculateEyeLevel(data, viewDistance);
            data.XYAngles = new Vector2(xAngle, yAngle);
        }

        public override void OnRefract(SimulationData simulationData)
        {
            throw new NotImplementedException();
        }
    }
}
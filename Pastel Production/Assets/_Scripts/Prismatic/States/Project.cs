using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prismatic
{
    [System.Serializable]
    public class Project : State
    {
        [SerializeField]
        private Reticule reticule;
        private float targetRange;
        private float viewHeight = 2.0f;
        private float viewDistance = 3.0f;
        private float xAngle, yAngle;
        private float yAngleLimit;

  

        public override void Update(SimulationData data)
        {
            UpdateView(data);
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            
        }

        public override void Enter(Action<StateType> transition, SimulationData data)
        {
            yAngleLimit = SimulationData.maxYAngle;
            xAngle = data.XYAngles.x;
            yAngle = data.XYAngles.y;
            reticule.gameObject.SetActive(true);
            base.Enter(transition, data);
        }
        public override void OnSelect(SimulationData data)
        {
            Vector3 viewDirection = data.ViewTarget - data.ViewPosition;

            float smallestAngle = Mathf.Infinity;
            PrismaticEntity targetEntity = null;


            //loop through all entities and find the one with the smallest angle to the player's view
            for(int i = 0; i<data.entities.Count; i++ )
            {
                if (data.entities[i] == data.currentEntity) continue;

                PrismaticEntity entityToCheck = data.entities[i];

                //First check for obstructions
                Vector3 entityOffset = entityToCheck.Position -data.currentEntity.Position;
                if (Physics.Raycast(data.currentEntity.Position+Vector3.up*0.5f, entityOffset, entityOffset.magnitude, 
                    (1 << LayerMask.NameToLayer("Default"))+ 
                    (1 << LayerMask.NameToLayer("HueBarrier"))))
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



            //project
            if ( targetEntity!= null)
            {
                targetEntity.HueMix.AddColor(data.currentEntity.HueMix);
                data.entities.Remove(data.currentEntity);
                data.currentEntity = targetEntity;
                Debug.Log("Selected entity: " + data.currentEntity);
                Transition(StateType.Move);
            }
        }

        public override void OnMouseMove(Vector2 mouseDelta)
        {
            xAngle += mouseDelta.x;
            yAngle += mouseDelta.y;
            yAngle = Mathf.Clamp(yAngle, -yAngleLimit * 0.55f, yAngleLimit);

        }


        private void UpdateView(SimulationData data)
        {
            data.ViewPosition = CameraUtility.CalculateEyeLevel(data, viewDistance, viewHeight);
            data.ViewTarget = CameraUtility.CalculateLookAtDirection(data, viewHeight);
            data.XYAngles = new Vector2(xAngle, yAngle);
        }

        public override void OnProject(SimulationData simulationData)
        {
            Transition(StateType.Move);
        }

        public override void Exit(SimulationData data)
        {
            reticule.gameObject.SetActive(false);
            base.Exit(data);
        }

        public override void OnRefract(SimulationData simulationData)
        {
            throw new NotImplementedException();
        }
    }
}
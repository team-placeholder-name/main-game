using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prismatic
{
    [System.Serializable]
    public class Project : State
    {
        private float targetRange;
        private float xAngle, yAngle;

  

        public override void Update(SimulationData data)
        {
            UpdateView(data);
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            
        }
        public override void OnSelect(SimulationData data)
        {
            Vector3 viewDirection = data.ViewTarget - data.ViewPosition;

            float smallestAngle= Mathf.Infinity;
            PrismaticEntity targetEntity = null;
            Debug.Log("Projecting");


            //loop through all entities and find the one with the smallest angle to the player's view
            for(int i = 0; i<data.entities.Count; i++ )
            {
                if (data.entities[i] == data.currentEntity) continue;

                PrismaticEntity entityToCheck = data.entities[i];
                Vector3 entityOffset = entityToCheck.Position -data.currentEntity.Position;
                if (Physics.Raycast(data.currentEntity.Position, entityOffset, entityOffset.magnitude, 1 << LayerMask.NameToLayer("Default")))
                {
                    Debug.Log("Entity Blocked");
                    continue;
                }                                                                                              


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
            yAngle = Mathf.Clamp(yAngle, -89, 89);

        }


        private void UpdateView(SimulationData data)
        {
            float viewDistance = 5;
            Vector3 offset = Quaternion.AngleAxis(xAngle, Vector3.up) * Quaternion.AngleAxis(yAngle, Vector3.right) * Vector3.back * viewDistance;
            data.ViewPosition = data.currentEntity.Position + offset;
            data.ViewTarget = data.currentEntity.Position + Vector3.up;
        }

        public override void OnProject(SimulationData simulationData)
        {
            Transition(StateType.Move);
        }
    }
}
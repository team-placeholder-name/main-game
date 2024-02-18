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

        public override void Enter(SimulationData data)
        {
        }

        public override void Exit(SimulationData data)
        {
        }

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
            int targetIndex = -1;
            Debug.Log("Projecting");


            //loop through all entities and find the one with the smallest angle to the player's view
            for(int i = 0; i<data.entities.Count; i++ )
            {
                if (i == data.currentEntityIndex) continue;

                PrismaticEntity entityToCheck = data.entities[i];
                Vector3 entityDirection = entityToCheck.Position - data.ViewPosition;

                float angle = Vector3.Angle(viewDirection, entityDirection);
                if (angle < smallestAngle)
                {
                    smallestAngle = angle;
                    targetIndex = i;
                }
            }


            //project
            if ( targetIndex!= -1)
            {
                data.entities[targetIndex].HueMix.AddColor(data.entities[data.currentEntityIndex].HueMix);
                data.currentEntityIndex = targetIndex;

                Debug.Log("Selected entity: " + data.currentEntityIndex);
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
            data.ViewPosition = data.entities[data.currentEntityIndex].Position + offset;
            data.ViewTarget = data.entities[data.currentEntityIndex].Position + Vector3.up;
        }
    }
}
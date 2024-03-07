using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prismatic
{
    [System.Serializable]
    public class Merge : State
    {
        [SerializeField]
        private float duration = 0.5f;
        [SerializeField]
        private float MergeAngle = 15;

        private float endTime;
        

  

        public override void Update(SimulationData data)
        {
            if (endTime <Time.time) {
                Transition(StateType.Move);
            }
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
        }

        public override void Enter(Action<StateType> transition, SimulationData data)
        {
            base.Enter(transition, data);
            MergeToTarget(data);
            endTime = Time.time+duration;
        }


        private void MergeToTarget(SimulationData data)
        {
            Vector3 viewDirection = data.ViewTarget - data.ViewPosition;

            float smallestAngle = Mathf.Infinity;
            PrismaticEntity targetEntity = null;


            //loop through all entities and find the one with the smallest angle to the player's view
            for (int i = 0; i < data.entities.Count; i++)
            {
                if (data.entities[i] == data.currentEntity) continue;

                PrismaticEntity entityToCheck = data.entities[i];

                //First check for obstructions
                Vector3 entityOffset = entityToCheck.Position - data.currentEntity.Position;
                if (Physics.Raycast(data.currentEntity.Position + Vector3.up * 0.5f, entityOffset, entityOffset.magnitude,
                    (1 << LayerMask.NameToLayer("Default")) +
                    (1 << LayerMask.NameToLayer("HueBarrier"))))
                {
                    Debug.Log("Entity Blocked");
                    continue;
                }


                //Second, check if it's the closest to the players view
                Vector3 entityDirection = entityToCheck.Position - data.ViewPosition;
                float angle = Vector3.Angle(viewDirection, entityDirection);
                if (angle < smallestAngle && angle < MergeAngle)
                {
                    smallestAngle = angle;
                    targetEntity = data.entities[i];
                }
            }



            //project
            if (targetEntity != null)
            {
                targetEntity.HueMix.AddColor(data.currentEntity.HueMix);
                data.entities.Remove(data.currentEntity);
                data.currentEntity = targetEntity;
                Debug.Log("Selected entity: " + data.currentEntity);

            }
        }

        public override void OnMouseMove(Vector2 mouseDelta)
        {
        }

        public override void OnMerge(SimulationData simulationData)
        {
        }

        public override void Exit(SimulationData data)
        {
            base.Exit(data);
        }

        public override void OnShift(SimulationData simulationData)
        {
            
        }

        public override void OnShiftSelect(Vector2 mousePos, SimulationData simulationData)
        {
          
        }
    }
}
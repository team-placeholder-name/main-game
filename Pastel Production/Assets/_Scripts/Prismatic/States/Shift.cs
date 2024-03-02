using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Prismatic
{
    [System.Serializable]
    public class Shift : State
    {
        public override void Enter(Action<StateType> transition, SimulationData data)
        {
            base.Enter(transition, data);
            Debug.Log("Shifting");
            Debug.Log("Entities: "+data.entities.Count);
            Debug.Log("Mixed: "+data.currentEntity.HueMix.Colors.Count);
        }
        public void OnSwap(SimulationData simulationData, int index)
        {
            PrismaticEntity targetEntity = simulationData.entities[index];
            if (targetEntity == simulationData.currentEntity)
            {
                Transition(StateType.Move);
            }
            else
            {
                simulationData.currentEntity = simulationData.entities[index];
            }
        }
        public void OnSplit(SimulationData simulationData, int index)
        {
            
            HueMix currentHueMix = simulationData.currentEntity.HueMix;
            Color colorToSplit = currentHueMix.Colors[index].color;
            currentHueMix.RemoveColor(colorToSplit);
            PrismaticEntity outputEntity = new PrismaticEntity(simulationData.currentEntity.Position, simulationData.currentEntity.Rotation, new HueMix(
                new List<ColorWeight>
                {
                        new ColorWeight(colorToSplit,1)
                }
            ));
            simulationData.entities.Add(outputEntity);
            simulationData.currentEntity = outputEntity;
            Transition(StateType.Move);
        }
        public override void OnMouseMove(Vector2 mousePos)
        {
            throw new System.NotImplementedException();
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            throw new System.NotImplementedException();
        }

        public override void OnMerge(SimulationData simulationData)
        {
            throw new System.NotImplementedException();
        }


        public override void Update(SimulationData data)
        {
            throw new System.NotImplementedException();
        }

        public override void OnShift(SimulationData simulationData)
        {
            throw new System.NotImplementedException();
        }
    }
}
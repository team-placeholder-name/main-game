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

        [SerializeField]
        EntityPresentation _entityPresentation;
        public override void Enter(Action<StateType> transition, SimulationData data)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            base.Enter(transition, data);
            Debug.Log("Shifting");
            Debug.Log("Entities: "+data.entities.Count);
            Debug.Log("Mixed: "+data.currentEntity.HueMix.Colors.Count);
            _entityPresentation.DisplayShiftUI();
        }
        public override void Exit(SimulationData data)
        {
            base.Exit(data);
            _entityPresentation.HideShiftUI();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        public override void OnShiftSelect(Vector2 mousePos, SimulationData simulationData)
        {
            foreach ((SelectionRegion, Color) h in _entityPresentation.shiftUIGen.hues)
            {
                if (h.Item1.CheckRegion(mousePos))
                {
                    OnSplit(simulationData, h.Item2);
                    Debug.Log("Splitting");
                }
            }
            foreach ((SelectionRegion, PrismaticEntity) e in _entityPresentation.shiftUIGen.entities)
            {
                if (e.Item1.CheckRegion(mousePos))
                {
                    OnSwap(simulationData, e.Item2);
                    Debug.Log("Swapping");
                }
            }
        }
        public void OnSwap(SimulationData simulationData, PrismaticEntity target)
        {
            
            if (target == simulationData.currentEntity)
            {
                
                Transition(StateType.Move);

            }
            else
            {
                simulationData.currentEntity = target;
                _entityPresentation.DisplayShiftUI();
            }
            
        }
        public void OnSplit(SimulationData simulationData, Color color)
        {
            if (simulationData.currentEntity.HueMix.Colors.Count > 1)
            {
                HueMix currentHueMix = simulationData.currentEntity.HueMix;
                currentHueMix.RemoveColor(color);
                PrismaticEntity outputEntity = new PrismaticEntity(simulationData.currentEntity.Position, simulationData.currentEntity.Rotation, new HueMix(
                    new List<ColorWeight>
                    {
                        new ColorWeight(color,1)
                    }
                ));
                simulationData.entities.Add(outputEntity);
                simulationData.currentEntity = outputEntity;
            }
            Transition(StateType.Move);
        }
        public override void OnMouseMove(Vector2 mousePos)
        {
        
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            
        }

        public override void OnMerge(SimulationData simulationData)
        {
   
        }


        public override void Update(SimulationData data)
        {
           
        }

        public override void OnShift(SimulationData simulationData)
        {

        }


    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Prismatic
{
    [System.Serializable]
    public class Refract : State
    {
        [SerializeField]
        private ColorWheel colorWheel;

        public override void Enter(Action<StateType> transition, SimulationData data)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            colorWheel.gameObject.SetActive(true);
            base.Enter(transition, data);
        }

        public override void Exit(SimulationData data)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            colorWheel.gameObject.SetActive(false);
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            Transition(StateType.Move);
        }

        public override void Update(SimulationData data)
        {
            if(data.currentEntity.HueMix.Weights.Sum() < 2)
            {
                Transition(StateType.Move);
            }

            if (colorWheel.ChosenColor != Color.clear)
            {
                data.currentEntity.HueMix.RemoveColor(colorWheel.ChosenColor);
                PrismaticEntity refraction = new PrismaticEntity(data.currentEntity.Position, data.currentEntity.Rotation, new HueMix(
                    new List<Color>
                    {
                        colorWheel.ChosenColor
                    },
                    new List<int>
                    {
                        1
                    }
                ));
                data.entities.Add(refraction);
                data.currentEntity = refraction;
                colorWheel.ReformColorWheel();
                Transition(StateType.Move);
            }
        }

        public override void OnSelect(SimulationData simulationData)
        {
            throw new System.NotImplementedException();
        }

        public override void OnProject(SimulationData simulationData)
        {
            throw new System.NotImplementedException();
        }

        // Cancel refraction if state input is repeated in state
        public override void OnRefract(SimulationData simulationData)
        {
            Transition(StateType.Move);
        }

        public override void OnMouseMove(Vector2 mousePos)
        {
            
        }
    }
}
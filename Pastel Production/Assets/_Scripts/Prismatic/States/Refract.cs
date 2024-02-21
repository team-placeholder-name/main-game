using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prismatic
{
    [System.Serializable]
    public class Refract : State
    {
        [SerializeField]
        private ColorWheel colorWheel;

        // The default value of target color used to compare if a color has been selected yet
        private Color targetColor = Color.clear;

        public override void Enter(Action<StateType> transition, SimulationData data)
        {
            colorWheel.gameObject.SetActive(true);
            base.Enter(transition, data);
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            colorWheel.gameObject.SetActive(false);
            targetColor = Color.clear;
        }

        public override void OnMouseMove(Vector2 mousePos)
        {
            if(colorWheel.ChosenColor != Color.clear)
            {
                Debug.Log("Refracting to " +  colorWheel.ChosenColor);
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

        public override void Update(SimulationData data)
        {
            throw new NotImplementedException();
        }
    }
}
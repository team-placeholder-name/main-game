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

        public override void Enter(SimulationData data)
        {
            colorWheel.gameObject.SetActive(true);
        }

        public override void Exit(SimulationData data)
        {
            colorWheel.gameObject.SetActive(false);
            targetColor = Color.clear;
        }

        public override void Update(SimulationData data)
        {
            if(colorWheel.ChosenColor != Color.clear)
            {
                Debug.Log("Refracting to " +  colorWheel.ChosenColor);
            }
        }
        public override void MoveInput(Vector2 movementInput)
        {
            throw new System.NotImplementedException();
        }

        public override void MoveMouse(Vector2 mousePos)
        {
            throw new System.NotImplementedException();
        }
    }
}
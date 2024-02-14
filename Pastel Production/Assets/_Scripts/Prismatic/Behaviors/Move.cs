using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Prismatic
{
    [System.Serializable]
    public class Move : State
    {
        private Vector2 movementInput = Vector2.zero;
        [SerializeField]
        private float speed = 5.0f;

        public override void Enter(SimulationData data)
        {
            throw new System.NotImplementedException();
        }

        public override void Exit(SimulationData data)
        {
            throw new System.NotImplementedException();
        }

        public override void Update(SimulationData data)
        {
            data.entities[data.currentEntityIndex].Position += new Vector3(movementInput.x, 0, movementInput.y) * Time.deltaTime * speed;
        }

        public override void MoveInput(Vector2 movementInput)
        {
            this.movementInput = movementInput;
        }
    }

}
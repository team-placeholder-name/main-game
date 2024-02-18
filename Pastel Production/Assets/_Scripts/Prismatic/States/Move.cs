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
        private float xAngle, yAngle;
        public override void Enter(SimulationData data)
        {

        }

        public override void Exit(SimulationData data)
        {
            //throw new System.NotImplementedException();
        }

        public override void Update(SimulationData data)
        {
            data.entities[data.currentEntityIndex].Position += new Vector3(movementInput.x, 0, movementInput.y) * Time.deltaTime * speed;

            UpdateView(data);
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            this.movementInput = movementInput;
        }


        public override void OnMouseMove(Vector2 mouseDelta)
        {
            xAngle += mouseDelta.x;
            yAngle += mouseDelta.y;
            yAngle = Mathf.Clamp(yAngle, -89, 89);
           
        }
        public override void OnSelect(SimulationData data)
        {

        }

        private void UpdateView(SimulationData data)
        {
            float viewDistance = 5;
            Vector3 offset =Quaternion.AngleAxis(xAngle, Vector3.up) * Quaternion.AngleAxis(yAngle, Vector3.right) * Vector3.back*viewDistance;
            data.ViewPosition = data.entities[data.currentEntityIndex].Position + offset;
            data.ViewTarget = data.entities[data.currentEntityIndex].Position;
        }
    }

}
using System;
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
        private float yAngleLimit;


        public override void Update(SimulationData data)
        {
            data.currentEntity.Position += new Vector3(movementInput.x, 0, movementInput.y) * Time.deltaTime * speed;

            UpdateView(data);
        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            this.movementInput = movementInput;
        }

        public override void Enter(Action<StateType> transition, SimulationData data)
        {
            yAngleLimit = SimulationData.maxYAngle;
            xAngle = data.XYAngles.x;
            yAngle = data.XYAngles.y;
            base.Enter(transition, data);
        }

        public override void OnMouseMove(Vector2 mouseDelta)
        {
            xAngle += mouseDelta.x;
            yAngle += mouseDelta.y;
            yAngle = Mathf.Clamp(yAngle, -yAngleLimit*0.5f, yAngleLimit);
           
        }
        public override void OnSelect(SimulationData data)
        {
            Transition(StateType.Swap);
        }

        private void UpdateView(SimulationData data)
        {
            float viewDistance = 5;
            Vector3 offset = Quaternion.AngleAxis(xAngle, Vector3.up) * Quaternion.AngleAxis(yAngle, Vector3.right) * Vector3.back*viewDistance;
            data.ViewPosition = data.currentEntity.Position + offset;
            data.ViewTarget = data.currentEntity.Position;
            data.XYAngles = new Vector2(xAngle, yAngle);
        }

        public override void OnProject(SimulationData simulationData)
        {
            Transition(StateType.Project);
        }

        public override void OnRefract(SimulationData simulationData)
        {
            Transition(StateType.Refract);
        }
    }

}
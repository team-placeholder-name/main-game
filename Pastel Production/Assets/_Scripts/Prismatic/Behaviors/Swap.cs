using UnityEngine;
namespace Prismatic
{
    [System.Serializable]
    public class Swap : State
    {
        [SerializeField]
        float theta;

        Vector2 mouseMove;

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
            // Update the camera
            Vector3 entityPosition = data.entities[data.currentEntityIndex].Position;
            Vector3 pivotDistance = data.cameraData.GetCameraPosition() - entityPosition;


        }

        public override void MoveInput(Vector2 movementInput)
        {
            throw new System.NotImplementedException();
        }

        public override void MoveMouse(Vector2 mouseDelta)
        {
            mouseMove = mouseDelta;
        }
    }
}
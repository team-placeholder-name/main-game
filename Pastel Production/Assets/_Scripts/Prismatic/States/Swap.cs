using UnityEngine;
namespace Prismatic
{
    [System.Serializable]
    public class Swap : State
    {
        [SerializeField]
        float theta;

        Vector2 mouseMove;



        public override void Update(SimulationData data)
        {
            // Update the camera
            Vector3 entityPosition = data.currentEntity.Position;
            Vector3 pivotDistance = data.cameraData.GetCameraPosition() - entityPosition;


        }

        public override void OnMoveInput(Vector2 movementInput)
        {
            throw new System.NotImplementedException();
        }

        public override void OnMouseMove(Vector2 mouseDelta)
        {
            mouseMove = mouseDelta;
        }

        public override void OnSelect(SimulationData simulationData)
        {
            throw new System.NotImplementedException();
        }

        public override void OnProject(SimulationData simulationData)
        {
            throw new System.NotImplementedException();
        }
    }
}
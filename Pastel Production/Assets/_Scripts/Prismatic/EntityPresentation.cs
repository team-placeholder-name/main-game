using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prismatic
{

    /// <summary>
    /// Visualizes the simulation target
    /// </summary>
    public class EntityPresentation : MonoBehaviour
    {
        [SerializeField]
        private PrismaticEntitySimulation simulationTarget;
        [SerializeField]
        private GameObject entityModel;

        [SerializeField]
        public ShiftUIGen shiftUIGen;//move the logic from this to the simulation

        public List<GameObject> entityModels = new List<GameObject>();


        private void Start()
        {
            shiftUIGen.SetUp();

        }

        public void HideShiftUI()
        {
            shiftUIGen.HideUI();
        }
        public void DisplayShiftUI()
        {
            shiftUIGen.DisplayUI(simulationTarget.SimulationData.CurrentEntity, simulationTarget.SimulationData.Entities, entityModels);
        }
        private void LateUpdate()
        {
            SimpleEntityUpdate();
            SimpleCameraUpdate();
        }

        //Placeholder camera follow script
        private void SimpleCameraUpdate()
        {
            Camera.main.transform.position = simulationTarget.SimulationData.ViewPosition;
            Camera.main.transform.rotation = Quaternion.LookRotation(simulationTarget.SimulationData.ViewTarget - simulationTarget.SimulationData.ViewPosition, Vector3.up);
        }


        //Placeholder entity update script
        private void SimpleEntityUpdate()
        {
            int entityCount = simulationTarget.SimulationData.Entities.Count;
            int i;
            for(i = 0; i < entityCount; i++)
            {
                
                if (entityModels.Count <= i)
                {
                    entityModels.Add(Instantiate(entityModel));
                }
                PrismaticEntity entityToRender = simulationTarget.SimulationData.Entities[i];
                entityModels[i].transform.position = entityToRender.Position;
                entityModels[i].transform.rotation = entityToRender.Rotation;
                entityModels[i].transform.GetChild(0).GetComponent<Renderer>().material.color = entityToRender.HueMix.Color;
                entityModels[i].SetActive(true);
            }
            for(; i< entityModels.Count; i++)
            {
                entityModels[i].SetActive(false);
            }
            
        }
    }
}

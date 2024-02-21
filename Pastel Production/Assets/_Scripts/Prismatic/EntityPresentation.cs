using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prismatic
{

    public class EntityPresentation : MonoBehaviour
    {
        [SerializeField]
        private PrismaticEntitySimulation simulationTarget;
        [SerializeField]
        private GameObject colorWheel;

        // Update is called once per frame
        void Update()
        {
            colorWheel.SetActive(simulationTarget.isMouseRightDown);
        }
    }
}

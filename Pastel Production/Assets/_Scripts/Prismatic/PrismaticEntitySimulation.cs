using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prismatic
{
    public class PrismaticEntitySimulation : MonoBehaviour
    {
        [SerializeField]
        private List<PrismaticEntity> entities;

        [SerializeField]
        private Swap swap;
        [SerializeField]
        private Move move;
        [SerializeField] 
        private Refract refract;
        [SerializeField] 
        private Project project;

        private Behaviour currentBehaviour;// the current behaviour acts on the list of entities. This list contains all the necessary data for the state.


        //TODO: Add Input Types to simulation and entity strategies
        private void Update()
        {
            currentBehaviour.Update(entities);
        }

    }
}
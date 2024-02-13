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
        private Move move;
        [SerializeField]
        private Swap swap;
        [SerializeField] 
        private Refract refract;
        [SerializeField] 
        private Project project;

        public Move Move { get { return move; } }
        public Swap Swap { get { return swap; } }
        public Refract Refract { get { return refract; } }
        public Project Project { get { return project; } }

        private Behaviour currentBehaviour;// the current behaviour acts on the list of entities. This list contains all the necessary data for the state.
        public Behaviour CurrentBehaviour { get { return currentBehaviour; } }
        
        
        private void Awake()
        {
            currentBehaviour = Move;
        }

        public void Transition(Behaviour nextBehaviour)
        {
            currentBehaviour.Exit();
            currentBehaviour = nextBehaviour;
            currentBehaviour.Enter();
        }

        //TODO: Add Input Types to simulation and entity strategies
        private void Update()
        {
            currentBehaviour.Update(entities);
        }

    }
} 
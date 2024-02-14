using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Prismatic
{
    public class PrismaticEntitySimulation : MonoBehaviour
    {
        public ReadOnlyCollection<PrismaticEntity> Entities { get => entities.AsReadOnly(); }
        public Move Move { get => move; }
        public Swap Swap { get => swap; }
        public Refract Refract { get => refract; }
        public Project Project { get => project; }
        public State CurrentState { get => currentState; }

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

        private State currentState;// the current behaviour acts on the list of entities. This list contains all the necessary data for the state.

        /// <summary>
        /// Updated by PlayerPrismaticEntityInput's OnMove()
        /// </summary>
        public Vector2 PlanarMovementInput { get; set; }

        private void Awake()
        {
            currentState = Move;
        }

        public void Transition(State nextBehaviour)
        {
            currentState.Exit();
            currentState = nextBehaviour;
            currentState.Enter();
        }

        //TODO: Add Input Types to simulation and entity strategies
        
        private void Update()
        {
            //currentState.Update(entities);
        }

    }
} 
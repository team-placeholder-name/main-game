using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

namespace Prismatic
{
    public class PrismaticEntitySimulation : MonoBehaviour
    {

        //Hacky workaround to utilize serialize properties
        public ReadOnlySimulationData SimulationData { get => simulationData.readOnlyData; }
        public Move Move { get => move; }
        public Swap Swap { get => swap; }
        public Refract Refract { get => refract; }
        public Project Project { get => project; }
        public State CurrentState { get => currentState; }

        [SerializeField]
        private SimulationData simulationData;

        [SerializeField]
        private Move move;
        [SerializeField]
        private Swap swap;
        [SerializeField]
        private Refract refract;
        [SerializeField]
        private Project project;

        private State currentState;// the current behaviour acts on the list of entities. This list contains all the necessary data for the state.
        [SerializeField]
        private StateType currentStateType;

        public enum StateType { Move, Swap, Refract, Project }
        private void Awake()
        {
            
            currentState = GetState(currentStateType);
        }

        public State GetState(StateType stateType)
        {
            switch (stateType)
            {
                case StateType.Move:
                    return move;
                case StateType.Swap:
                    return swap;
                case StateType.Refract:
                    return refract;
                case StateType.Project:
                    return project;
                default:
                    return null;
            }
        }

        //Intended to be called only by the current state
        public void Transition(StateType nextState)
        {
            GetState(currentStateType).Exit(simulationData);
            currentStateType = nextState;
            GetState(currentStateType).Enter(simulationData);
        }


        //TODO: Add Input Types to simulation and entity strategies
        
        private void Update()
        {
            currentState.Update(simulationData);
        }

        public void MoveInput(Vector2 movementInput)
        {
            currentState.MoveInput(movementInput);
        }
    }



    //This class is used to store the data that is shared between states. Use as sparingly as possible
    [System.Serializable]
    public class SimulationData
    {
        public SimulationData()
        {
            readOnlyData = new ReadOnlySimulationData(this);
        }
        public readonly ReadOnlySimulationData readOnlyData;
        [SerializeField]
        public List<PrismaticEntity> entities;
        [SerializeField]
        public int currentEntityIndex;
        
    }


    //If any shared state data needs to be read outside of the state, expose it here
    public class ReadOnlySimulationData
    {
        public ReadOnlySimulationData(SimulationData data)
        {
            this.data = data;
        }

        private SimulationData data;
        public ReadOnlyCollection<PrismaticEntity> Entities { get => data.entities.AsReadOnly(); }
        public int currentIndex { get => data.currentEntityIndex; }
    }

    
} 
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
        public StateType CurrentState { get => currentStateType; }

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


        [SerializeField]
        private StateType currentStateType;

        public enum StateType { Move, Swap, Refract, Project }

        private void Awake()
        {
            // Add prismatic entity to simulation
            PrismaticEntity pe1 = new PrismaticEntity(Vector3.zero, Quaternion.identity, new HueMix(
                new List<Color>
                {
                    Color.red, Color.blue, Color.green, Color.cyan, Color.magenta, Color.yellow, Color.gray, Color.black, Color.white 
                },
                new List<int>
                {
                    5, 1, 2, 3, 4, 1, 2, 3, 4,
                }
            ));
            simulationData.entities.Add(pe1);

            Transition(currentStateType);
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
            GetState(currentStateType).Update(simulationData);
        }

        public void MoveInput(Vector2 movementInput)
        {
            GetState(currentStateType).MoveInput(movementInput);
        }

        public void MouseMove(Vector2 movementInput)
        {
            GetState(currentStateType).MoveMouse(movementInput);
        }

        public void MouseRightClick(bool isMouseRightDown)
        {
            if (isMouseRightDown && currentStateType != StateType.Refract)
            {
                Transition(StateType.Refract);
            }
            else
            {
                Transition(StateType.Move);
            }
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
        public CameraData cameraData;
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
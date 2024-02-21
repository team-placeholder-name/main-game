using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

namespace Prismatic
{

    public enum StateType { Move, Swap, Refract, Project }//not sure were best to put this

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

        

        private void Awake()
        {
            simulationData.currentEntity = simulationData.entities[0];
            Transition(currentStateType);



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
            GetState(currentStateType).Enter(Transition, simulationData);
        }


        //TODO: Add Input Types to simulation and entity strategies

        private void Update()
        {
            GetState(currentStateType).Update(simulationData);
        }

        public void OnMoveInput(Vector2 movementInput)
        {
            GetState(currentStateType).OnMoveInput(movementInput);
        }

        public void OnMouseMove(Vector2 movementInput)
        {
            GetState(currentStateType).OnMouseMove(movementInput);
        }
        public void OnSelect()
        {
            GetState(currentStateType).OnSelect(simulationData);
        }
        public void OnProject()
        {
            GetState(currentStateType).OnProject(simulationData);
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



    //Stores the data that is shared between states. Use as sparingly as possible
    [System.Serializable]
    public class SimulationData
    {
        public SimulationData()
        {
            readOnlyData = new ReadOnlySimulationData(this);
            
        }
        public readonly ReadOnlySimulationData readOnlyData;

        public List<PrismaticEntity> entities;

        public PrismaticEntity currentEntity;


        // Where the player view originates from
        [HideInInspector]
        public Vector3 ViewPosition;
        // What is the player looking at. Aiming is essential for selecting targets, so this info must be preserved in the presentation
        [HideInInspector]
        public Vector3 ViewTarget;
        [HideInInspector]
        public Vector2 XYAngles;

        // Max Vertical Rotation - currently this is hardcoded but it should be configurable via editor until we find something comfortable
        public const float maxYAngle =89;

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
        public PrismaticEntity currentEntity { get => data.currentEntity; }
        public Vector3 ViewPosition { get => data.ViewPosition; }
        public Vector3 ViewTarget { get => data.ViewTarget; }
        public Vector3 XYAngles { get => data.XYAngles; }
    }

    
} 
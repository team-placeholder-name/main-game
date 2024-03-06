using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Prismatic
{

    public enum StateType { Move, Shift, Merge }//not sure were best to put this

    public class PrismaticEntitySimulation : MonoBehaviour
    {

        //Hacky workaround to utilize serialize properties
        public ReadOnlySimulationData SimulationData { get => simulationData.readOnlyData; }
        public Move Move { get => move; }
        public Shift Shift { get => shift; }
        public Merge Merge { get => merge; }
        public StateType CurrentState { get => currentStateType; }

        [SerializeField]
        private SimulationData simulationData;

        [SerializeField]
        private Move move;
        [SerializeField]
        private Shift shift;
        [SerializeField]
        private Merge merge;



        [SerializeField]
        private StateType currentStateType;

        private void Awake()
        {

            simulationData.currentEntity = simulationData.entities[0];
            Transition(currentStateType);
        }

        public State GetState(StateType stateType)
        {
            switch (stateType)
            {
                case StateType.Move:
                    return move;
                case StateType.Shift:
                    return shift;
                case StateType.Merge:
                    return merge;
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

        public void OnMerge()
        {
            GetState(currentStateType).OnMerge(simulationData);
        }
        public void OnShift()
        {
            GetState(currentStateType).OnShift(simulationData);
        }
        public void OnShiftSelect(Vector2 pos)
        {
            GetState(currentStateType).OnShiftSelect(pos, simulationData);
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

        public Vector2 MousePos;
        // Max Vertical Rotation - currently this is hardcoded but it should be configurable via editor until we find something comfortable
        public const float maxYAngle =45;
        public const float minYAngle = -30;

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
        public PrismaticEntity CurrentEntity { get => data.currentEntity; }
        public Vector3 ViewPosition { get => data.ViewPosition; }
        public Vector3 ViewTarget { get => data.ViewTarget; }
        public Vector3 XYAngles { get => data.XYAngles; }
        public Vector2 MousePos { get=> data.MousePos; }
    }

    
} 
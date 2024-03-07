using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

namespace Prismatic
{

    public abstract class State
    {
        private Action<StateType> transition;
        /// <summary>
        /// Transitions to the simulation to the specified state
        /// </summary>
        /// <param name="type"></param>
        protected void Transition(StateType type)
        {
            transition.Invoke(type);
        }

        /// <summary>
        /// Clears the transition delegate
        /// </summary>
        /// <param name="data"></param>
        public virtual void Exit(SimulationData data)
        {
            transition = null;
        }
        /// <summary>
        /// Store the transition of the current simulation
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="data"></param>
        public virtual void Enter(Action<StateType> transition, SimulationData data)
        {
            this.transition = transition;
        }

        public abstract void Update(SimulationData data);

        public abstract void OnMoveInput(Vector2 movementInput);
        public abstract void OnMouseMove(Vector2 mousePos);

        public abstract void OnShift(SimulationData simulationData);

        public abstract void OnMerge(SimulationData simulationData);

        public abstract void OnShiftSelect(Vector2 mousePos, SimulationData simulationData);
    }
}
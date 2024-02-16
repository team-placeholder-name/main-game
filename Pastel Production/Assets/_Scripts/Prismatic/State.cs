using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

namespace Prismatic
{

    public abstract class State
    {

        public abstract void Exit(SimulationData data);

        public abstract void Enter(SimulationData data);

        public abstract void Update(SimulationData data);

        public abstract void MoveInput(Vector2 movementInput);

    }
}
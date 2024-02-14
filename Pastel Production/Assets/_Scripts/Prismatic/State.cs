using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prismatic
{

    public abstract class State
    {
        protected PrismaticEntitySimulation entityController;

        public abstract void Exit(SimulationData data);

        public abstract void Enter(SimulationData data);

        public abstract void Update(SimulationData data);

    }
}
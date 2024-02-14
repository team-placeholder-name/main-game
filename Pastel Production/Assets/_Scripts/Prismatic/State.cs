using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prismatic
{

    public abstract class State
    {
        protected PrismaticEntitySimulation entityController;

        public abstract void Exit();

        public abstract void Enter();

        public abstract void Update(List<PrismaticEntity> entities);

        public abstract void MoveInput(Vector2 movementInput);

    }
}
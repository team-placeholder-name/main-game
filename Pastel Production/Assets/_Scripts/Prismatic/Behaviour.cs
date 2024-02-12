using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prismatic
{

    public abstract class Behaviour
    {
        protected PrismaticEntitySimulation entityController;


        public abstract void Update(List<PrismaticEntity> entities);
        
    }
}
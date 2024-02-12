using Prismatic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prismatic
{
    [System.Serializable]
    public class Swap : Behaviour
    {
        [SerializeField]
        int test;

        public override void Update(List<PrismaticEntity> entities)
        {
            throw new System.NotImplementedException();
        }
    }
}
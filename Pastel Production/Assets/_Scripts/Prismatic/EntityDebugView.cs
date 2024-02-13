using Prismatic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Prismatic
{
    public class EntityDebugView : MonoBehaviour
    {
        [SerializeField]
        private PrismaticEntitySimulation simulationTarget;
        private void OnDrawGizmos()
        {

            foreach (PrismaticEntity entity in simulationTarget.Entities)
            {
                Gizmos.color = entity.HueMix.color;
                float radius = 0.5f;
                Gizmos.DrawSphere(entity.Position + Vector3.up * radius, radius);//place the sphere on top of the position
            }

        }
    }
}
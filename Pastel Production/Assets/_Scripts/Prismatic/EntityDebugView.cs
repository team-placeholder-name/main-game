using Prismatic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Prismatic
{
    public class EntityDebugView : MonoBehaviour
    {
        [SerializeField]
        private PrismaticEntitySimulation simulationTarget;
        private void OnDrawGizmos()
        {

            foreach (PrismaticEntity entity in simulationTarget.SimulationData.Entities)
            {
                Gizmos.color = entity.HueMix.Color;
                float radius = 0.5f;
                Gizmos.DrawSphere(entity.Position + Vector3.up * radius, radius);//place the sphere on top of the position
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(entity.Position + Vector3.up * radius, radius);//draw an outline for the entity, in case it's invisible
            }

        }
    }
}
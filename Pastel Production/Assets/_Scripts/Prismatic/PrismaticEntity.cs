using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prismatic
{
    [System.Serializable]
    public class PrismaticEntity
    {
        public Vector3 Position = Vector3.zero; // The position represents the root/base of the character
        public Quaternion Rotation = Quaternion.identity;
        public HueMix HueMix;

        public PrismaticEntity(Vector3 Position, Quaternion Rotation, HueMix HueMix)
        {
            this.Position = Position;
            this.Rotation = Rotation;
            this.HueMix = HueMix;
        }
    }
}
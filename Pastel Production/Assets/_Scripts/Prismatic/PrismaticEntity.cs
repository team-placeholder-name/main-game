using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prismatic
{
    [System.Serializable]
    public struct PrismaticEntity
    {
        public Vector3 Position; // The position represents the root/base of the character
        public Quaternion Rotation;
        public HueMix HueMix;
    }
}
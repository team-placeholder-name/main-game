using Prismatic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueBarrier : MonoBehaviour
{
    public HueMix hue;
    private void Awake()
    {
        GetComponent<Renderer>().material.color = hue.Color;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = hue.Color;
        float radius = 1f;
        Gizmos.DrawSphere(transform.position, radius);//place the sphere on top of the position

    }
}

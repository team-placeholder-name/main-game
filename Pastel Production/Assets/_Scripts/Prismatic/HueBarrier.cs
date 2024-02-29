using Prismatic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueBarrier : MonoBehaviour
{
    public HueMix hue;
    private void Awake()
    {
        Color color = hue.Color;
        color.a*= 0.5f;
        GetComponent<Renderer>().material.color = color;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = hue.Color;
        float radius = 1f;
        Gizmos.DrawSphere(transform.position, radius);//place the sphere on top of the position

    }
}

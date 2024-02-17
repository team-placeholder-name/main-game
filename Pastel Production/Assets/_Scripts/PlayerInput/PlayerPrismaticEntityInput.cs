using Prismatic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Recieves messages from player input component, transforms the data if necessary, and then passes it to the controlled simulation
/// </summary>
public class PlayerPrismaticEntityInput : MonoBehaviour
{
    [SerializeField]
    private PrismaticEntitySimulation controlledSimulation;


    /// <summary>
    /// Called with Unity's Send Message system from Player Input
    /// </summary>
    /// <param name="value"></param>
    private void OnMove(InputValue value)
    {
        controlledSimulation.MoveInput(value.Get<Vector2>());
    }

    /// <summary>
    /// Called with Unity's Send Message system from Player Input
    /// </summary>
    /// <param name="value"></param>
    private void OnFire(InputValue value)
    {

    }

    /// <summary>
    /// Called with Unity's Send Message system from Player Input
    /// </summary>
    /// <param name="value"></param>
    private void OnLook(InputValue value)
    {
        controlledSimulation.MouseMove(value.Get<Vector2>());
    }
}

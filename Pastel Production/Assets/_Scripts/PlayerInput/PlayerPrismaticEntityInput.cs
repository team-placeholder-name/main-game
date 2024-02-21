using Prismatic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPrismaticEntityInput : MonoBehaviour
{
    [SerializeField]
    private PrismaticEntitySimulation controlledSimulation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    private void OnRightClick(InputValue value)
    {
        // controlledSimulation.MouseRightClick(value.Get<bool>());
    }
}

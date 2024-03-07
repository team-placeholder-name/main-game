using Prismatic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Recieves messages from player input component, transforms the data if necessary, and then passes it to the controlled simulation
/// </summary>
public class PlayerPrismaticEntityInput : MonoBehaviour
{
    [SerializeField]
    private PrismaticEntitySimulation controlledSimulation;
    [SerializeField]
    Vector2 mouseSensitivity = new Vector2(0.3f, 0.1f);
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;   
        Cursor.visible = false; 
    }

    /// <summary>
    /// Called with Unity's Send Message system from Player Input
    /// </summary>
    /// <param name="value"></param>
    private void OnMove(InputValue value)
    {
        controlledSimulation.OnMoveInput(value.Get<Vector2>());
    }

    /// <summary>
    /// Called with Unity's Send Message system from Player Input
    /// </summary>
    /// <param name="value"></param>
    private void OnLook(InputValue value)
    {
        Vector2 input = Vector2.Scale(value.Get<Vector2>(), mouseSensitivity);
        input.y *= -1;
        controlledSimulation.OnMouseMove(input); 

    }

    /// <summary>
    /// Called with Unity's Send Message system from Player Input
    /// </summary>
    /// <param name="value"></param>
    private void OnMerge(InputValue value)
    {
        controlledSimulation.OnMerge();
    }
    /// <summary>
    /// Called with Unity's Send Message system from Player Input
    /// </summary>
    /// <param name="value"></param>
    private void OnShift(InputValue value)
    {
        controlledSimulation.OnShift();
    }

    private void OnShiftSelect(InputValue value)
    {
        Vector2 select = Mouse.current.position.ReadValue();
        controlledSimulation.OnShiftSelect(select);

    }

    private void OnRestart(InputValue value)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

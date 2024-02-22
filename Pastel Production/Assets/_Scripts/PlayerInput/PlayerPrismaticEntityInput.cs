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
        controlledSimulation.OnMouseMove(Vector2.Scale(value.Get<Vector2>(),mouseSensitivity));

    }

    /// <summary>
    /// Called with Unity's Send Message system from Player Input
    /// </summary>
    /// <param name="value"></param>
    private void OnSelect(InputValue value)
    {
        controlledSimulation.OnSelect();
    }

    /// <summary>
    /// Called with Unity's Send Message system from Player Input
    /// </summary>
    /// <param name="value"></param>
    private void OnProject(InputValue value)
    {
        controlledSimulation.OnProject();
    }

    private void OnRefract(InputValue value)
    {
        controlledSimulation.OnRefract();
    }
    private void OnRestart(InputValue value)
    {
        SceneManager.LoadScene(0);
    }
}

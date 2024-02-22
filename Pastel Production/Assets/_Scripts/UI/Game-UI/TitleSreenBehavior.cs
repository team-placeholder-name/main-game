using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenBehavior : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject titleScreenUI;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    //Transitions to the Game Scene
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    //Quits the Game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Changes whether the options screen is shown or not
    public void ToggleOptions()
    {
        optionsMenu.SetActive(!optionsMenu.activeInHierarchy);
        titleScreenUI.SetActive(!titleScreenUI.activeInHierarchy);
    }
}
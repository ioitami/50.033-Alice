using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject continueBtn;
    public bool saveExist = false;
    // Start is called before the first frame update
    void Start()
    {
        // Continue button only shows up if save file is valid (not nil)
        if (saveExist)
        {
            continueBtn.SetActive(true);
        }
        else
        {
            continueBtn.SetActive(false);
        }
    }

    public void Continue()
    {
        // Get save file from SO, change scene via scenemanager/roommanager,
        // then teleport player to room based on SO
    }
    public void NewGame()
    {
        // play cutscene. When finish, change scene to level 1
    }
    public void Settings()
    {
        // open up popup settings page
    }

    public void Quit()
    {
        Debug.Log("QUIT GAME");
        // Save any info here before quitting

        // CODE HERE TO SAVE ROOM NAME TO Scriptable Object

        #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // events
    public UnityEvent gameStart;

    public UnityEvent restartRoom;
    public UnityEvent restartLevel;
    public UnityEvent restartGame;
    public UnityEvent pauseGame;

    public UnityEvent quitGame;

    public UnityEvent<int> changeInputMap;

    //public IntVariable gameScore;

    //public GameConstants SMB;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;
    }
    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        //SetScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        // reset score
        //gameScore.Value = 0;
        //SetScore();
        //gameRestart.Invoke();
        //Time.timeScale = 1.0f;
    }



}
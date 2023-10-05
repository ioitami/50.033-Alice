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

    public UnityEvent<string> changeRoom;

    public UnityEvent<Vector2, int> addPlayerVelocity;
    public UnityEvent<Vector2> replacePlayerVelocity;
    public UnityEvent<float> changePlayerGravity;


    //public IntVariable gameScore;

    //public GameConstants SMB;

    void Start()
    {

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

    public void AddPlayerVelocity(Vector2 vel, int numFrames)
    {
        addPlayerVelocity.Invoke(vel, numFrames);
    }
    public void ReplacePlayerVelocity(Vector2 vel)
    {
        replacePlayerVelocity.Invoke(vel);
    }
    public void ChangePlayerGravity(float gravityScale)
    {
        changePlayerGravity.Invoke(gravityScale);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CutsceneManager : MonoBehaviour
{
    public GameManager gameManager;

    public UnityEvent newgameCutscene;
    public UnityEvent level1StartCutscene;

    void Awake()
    { 
    
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void NewGameCutscene() { 

        newgameCutscene.Invoke();

        // 1. Camera slowly zooms out, pans to center of Alice (~5s). Camera pauses (~3s).
        // Make Main Menu UI unclickable, fade to transparent as 1. happens

        // 2. Camera vibrates, screen flashes white for 1 frame

        // 3. Wooden planks, Alice and her soft toys 'fall' with zoom line bg
        // Random movement for a few seconds
        // Sorta intense music plays

        // 4. Screen cuts to black instantly, music stops aruptly

        // 5. Change scene to Level 1
    }

    public void Level1StartCutscene()
    {
        // 1. Camera fades from black, Alice lying on the ground beside a tree.
        // Ambient forest music plays
        // Camera is slightly zoomed into the current room
        // Start Alice with dummy controller (player cant control)

        // 2. Alice wakes up, gets up and stands up

        // 3. Camera zooms out back to normal

        // 4. Alice controller switches to 'NoGlidingNoDashing'
        // Player can now move Alice
    }

}

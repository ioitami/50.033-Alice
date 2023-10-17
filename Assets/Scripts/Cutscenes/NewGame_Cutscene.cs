using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame_Cutscene : MonoBehaviour
{
    // 1. Camera slowly zooms out, pans to center of Alice (~5s). Camera pauses (~3s).
    // Make Main Menu UI unclickable, fade to transparent as 1. happens

    // 2. Camera vibrates, screen flashes white for 1 frame

    // 3. Wooden planks, Alice and her soft toys 'fall' with zoom line bg
    // Random movement for a few seconds
    // Sorta intense music plays

    // 4. Screen cuts to black instantly, music stops aruptly, bang sound

    // 5. Change scene to Level 1

    public Camera camera;

    public AudioSource intenseMusic;
    public AudioSource bangMusic;

    public void PlayNewGameCutscene()
    {
        Debug.Log("PLAY NEWGAME_CUTSCENE");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

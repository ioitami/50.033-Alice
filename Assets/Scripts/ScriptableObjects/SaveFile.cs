using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveFile", menuName = "ScriptableObjects/SaveFile", order = 1)]
public class SaveFile : ScriptableObject
{
    // current level (to identify scene) 
    public string level;

    // current room (for actual gameplay)
    public string roomName;

    // point to spawn player
    public string spawnPoint;

    // currrent time (for score purposes)
    public float gameTime;

    // current death counter (for score purposes)
    public int deathCount;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMovement playerMovement;
    public SaveFile saveFile;

    void Awake()
    {
        // other instructions
        gameManager.addPlayerVelocity.AddListener(AddPlayerVelocity);
        gameManager.replacePlayerVelocity.AddListener(ReplacePlayerVelocity);
        gameManager.changePlayerGravity.AddListener(ChangePlayerGravity);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Spawn(Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void AddPlayerVelocity(Vector2 vel, int numFrames)
    {
        playerMovement.AddActingVelocity(vel, numFrames);
    }
    void ReplacePlayerVelocity(Vector2 vel)
    {
        playerMovement.ReplaceActingVelocity(vel);
    }
    void ChangePlayerGravity(float gravityScale)
    {
        playerMovement.ChangePlayerGravity(gravityScale);
    }

    void Spawn(Vector2 speed)
    {
        Vector3 spawnPosition = GameObject.Find(saveFile.roomName + "/" + saveFile.spawnPoint).transform.position;
        playerMovement.Teleport(speed,spawnPosition);
    }
}

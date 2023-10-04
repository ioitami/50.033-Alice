using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMovement playerMovement;

    void Awake()
    {
        // other instructions
        gameManager.addPlayerVelocity.AddListener(AddPlayerVelocity);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void AddPlayerVelocity(Vector2 vel, int numFrames)
    {
        playerMovement.AddActingVelocity(vel, numFrames);
    }
}

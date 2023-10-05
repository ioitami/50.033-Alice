using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWind : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        //gameManager.addPlayerVelocity.AddListener(OnTriggerEnter2D);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider col)
    {
        Debug.Log("test");
        gameManager.AddPlayerVelocity(new Vector2(0f, 10f), 0);
        col.attachedRigidbody.AddForce(Vector2.up*5);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("test enter");
        //gameManager.AddPlayerVelocity(new Vector2(0f, 25f),0);
    }
}

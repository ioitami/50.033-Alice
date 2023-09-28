using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    public UnityEvent jump;
    public UnityEvent jumpHold;
    public UnityEvent<int> moveCheck;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void OnMoveAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("move started");
        }
        if (context.canceled)
        {
            Debug.Log("move stopped");
        }
    }

    public void OnJumpAction(InputAction.CallbackContext context){
        if (context.performed)
        {
            Debug.Log("Jump was performed");
        }
    }

    public void OnJumpHoldAction(InputAction.CallbackContext context){
        if (context.performed)
        {
            Debug.Log("JumpHold was performed");
        }
    }

    public void OnDashAction(InputAction.CallbackContext context){
        if (context.performed)
        {
            Debug.Log("Dash was performed");
        }
    }

    public void OnGlideAction(InputAction.CallbackContext context){
        if (context.performed)
        {
            Debug.Log("Glide was performed");
        }

    }
}

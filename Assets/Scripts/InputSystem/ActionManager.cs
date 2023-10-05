using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour
{
    public UnityEvent jump;
    public UnityEvent jumpHold;
    public UnityEvent<int> moveCheck;
    public UnityEvent dash;
    public UnityEvent glide;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void OnMoveAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("move started");
            int faceDir = context.ReadValue<float>() > 0 ? 1 : -1;
            moveCheck.Invoke(faceDir);
        }
        if (context.canceled)
        {
            Debug.Log("move stopped");
            moveCheck.Invoke(0);
        }
    }

    public void OnJumpAction(InputAction.CallbackContext context){
        if (context.performed)
        {
            jump.Invoke();
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
            dash.Invoke();
            Debug.Log("Dash was performed");
        }
    }

    public void OnGlideAction(InputAction.CallbackContext context){
        if (context.started)
        {
            glide.Invoke();
            //Debug.Log("Glide was performed");
        }
    }
}

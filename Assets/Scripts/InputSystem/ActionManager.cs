using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour
{
    public UnityEvent jump;
    public UnityEvent jumpHold;
    public UnityEvent jumpRelease;
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
            int faceDir = context.ReadValue<float>() > 0 ? 1 : -1;
            moveCheck.Invoke(faceDir);
            //Debug.Log("move started");
        }
        if (context.canceled)
        {
            moveCheck.Invoke(0);
            //Debug.Log("move stopped");
            
        }
    }

    public void OnJumpAction(InputAction.CallbackContext context){
        if (context.performed)
        {
            jump.Invoke();
            jumpHold.Invoke();
            Debug.Log("Jump was performed");     
        }
        if (context.performed)
        {

        }
        if (context.canceled)
        {
            jumpRelease.Invoke();
            Debug.Log("Jump released");
        }
    }

    public void OnDashAction(InputAction.CallbackContext context){
        if (context.performed)
        {
            dash.Invoke();
            //Debug.Log("Dash was performed");
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

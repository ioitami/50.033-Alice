using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour
{
    public UnityEvent<int> moveCheck;
    public UnityEvent dash;
    public UnityEvent jumpGlide;
    public UnityEvent jumpHold;
    public UnityEvent jumpRelease;

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

    public void OnDashAction(InputAction.CallbackContext context){
        if (context.performed)
        {
            dash.Invoke();
            //Debug.Log("Dash was performed");
        }
    }

    public void OnJumpGlideAction(InputAction.CallbackContext context){
        if (context.started)
        {
            jumpGlide.Invoke();
            //Debug.Log("Glide was performed");
        }
        if (context.performed)
        {
            jumpHold.Invoke();
        }
        if (context.canceled)
        {
            jumpRelease.Invoke();
        }
    }
}

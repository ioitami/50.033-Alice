using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public SaveFile saveFile;
    private string currentRoom = "Room1";
    public Vector3 anchor;
    private Vector3 newAnchor;
    private float timeElapsed = 0;
    
    public float roomChangeDuration = 0.9f;
    private bool isMovingComplete = true;
    private bool isPlayerDisabled = false;

    void Start()
    {
        anchor = GameObject.Find(currentRoom).transform.position;
        currentRoom = saveFile.roomName;
        newAnchor = GameObject.Find(currentRoom).transform.position;
        transform.Translate(newAnchor - anchor);
        anchor = newAnchor;
    }

    private void Update()
    {
        
        if (isMovingComplete && isPlayerDisabled)
        {
            // TODO reenable player controller

        }
    }

    public void ChangeRoom()
    {
        currentRoom = saveFile.roomName;
        newAnchor = GameObject.Find(currentRoom).transform.position;

        // TODO disable player controller
        isPlayerDisabled = true;
        

        Vector3 moveBy = newAnchor - anchor;

        anchor = newAnchor;

        // TODO Trigger Alice transition animation

        isMovingComplete = false;
        StartCoroutine(MoveCamera(moveBy));
    }

    IEnumerator MoveCamera(Vector3 moveBy)
    {
        timeElapsed = 0;
        float current = 0, next = 0;
        while(timeElapsed < roomChangeDuration)
        {
            next = Mathf.SmoothStep(0, 1, timeElapsed / roomChangeDuration);
            transform.Translate(moveBy * (next-current));
            current = next;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.Translate(moveBy * (1 - current));
        isMovingComplete = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    //public SaveFile saveFile;
    public GameManager gameManager;
    public CameraManager cameraManager;

    private void Awake()
    {

    }

    public void StartRoomChange()
    {
        CheckRooms();
        cameraManager.ChangeRoom();
    }

    public void CheckRooms()
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<DoorController>().CheckRoom();
        }
    }

}

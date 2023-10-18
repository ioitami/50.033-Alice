using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public string currentRoomName;
    public GameObject currentRoom;

    // Start is called before the first frame update
    void Start()
    {
        UpdateRoom("Room 1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRoom(string newRoomName)
    {
        currentRoomName = newRoomName;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<RoomController>().roomName == currentRoomName)
            {
                currentRoom = child.gameObject;
                Debug.Log(currentRoomName);
            }
        }
    }

    public Vector3 GetRoomAnchor()
    {
        return currentRoom.GetComponent<RoomController>().anchorPosition;
    }
}

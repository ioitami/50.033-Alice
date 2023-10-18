using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    public SaveFile saveFile;
    public string room1;
    public string room2;
    public string spawnPoint1;
    public string spawnPoint2;

    private bool inRoom1 = true;
    private DoorManager doorManager;

    // TODO Identify if door is left/right or up/down (room order can be switched easily)

    private void Start()
    {
        CheckRoom();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRoom1 = !inRoom1;

            // ensures room name and spawn point are updated only when changing rooms
            saveFile.roomName = inRoom1 ? room1 : room2;
            saveFile.spawnPoint = inRoom1 ? spawnPoint1 : spawnPoint2;

            doorManager = GetComponentInParent<DoorManager>();
            doorManager.StartRoomChange();

            // TODO Call a transition that depends on left/right vs up/down and also changed room
        }
    }

    public void CheckRoom()
    {
        inRoom1 = saveFile.roomName == room1;
    }

}

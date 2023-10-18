using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Currently Unused

public class RoomController : MonoBehaviour
{
    [System.NonSerialized]
    public Vector3 anchorPosition;
    public string roomName;

    // Start is called before the first frame update
    void Start()
    {
        anchorPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

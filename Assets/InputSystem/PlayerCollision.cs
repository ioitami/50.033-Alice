using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Ground Layer")]
    public LayerMask groundLayer;

    [Space]

    [Header("Check Collisions")]
    public bool onGround;

    [Space]

    [Header("Collision Edits")]
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    public Vector3 onGroundColliderSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, onGroundColliderSize, 90f, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireCube((Vector2)transform.position  + bottomOffset, onGroundColliderSize);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

    }
}

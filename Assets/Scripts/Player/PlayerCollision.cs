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
    public bool cannotGlide;

    [Space]

    [Header("Collision Edits")]
    public float collisionRadius = 0.25f;
    public Vector2 rightOffset, leftOffset;
    public Vector3 onGroundColliderSize;
    public Vector2 bottomOffset;
    public Vector3 minGlideColliderSize;
    public Vector2 bottomOffset2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, onGroundColliderSize, 0f, groundLayer);
        cannotGlide = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset2, minGlideColliderSize, 0f, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        // Draw visible gizmos on scene
        Gizmos.DrawWireCube((Vector2)transform.position  + bottomOffset, onGroundColliderSize);
        Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset2, minGlideColliderSize);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

    }
}

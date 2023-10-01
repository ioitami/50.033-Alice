using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerCollision collisionDetect;
    private Rigidbody2D rigidBody;

    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public float gravity = 4;

    public Vector3 velocity;
    public float xRaw;
    public float yRaw;

    [Space]
    [Header("Dashing")]
    public float dashSpeed = 20;
    public float dashCost = 1;
    public float dashDuration = 0.3f;

    [Space]
    [Header("Ability Stats")]
    public float maxStamina = 2;
    public float stamina = 2;

    [Space]
    [Header("Bool Stats")]
    public bool canMove = true;
    private bool moving = false;
    public bool faceRightState = true;
    private bool isDashing = false;


    // Start is called before the first frame update
    void Start()
    {
        collisionDetect = this.GetComponent<PlayerCollision>();
        rigidBody = this.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(xRaw, yRaw);
        //Debug.Log(dir);
    }

    void FixedUpdate()
    {
        if (moving == true && canMove == true)
        {
            Move(faceRightState == true ? 1 : -1);
        }

        // DEBUGGING PURPOSES IN THE INSPECTOR TO SHOW VELOCITY OF CHARACTER IN EACH FRAME
        velocity = rigidBody.velocity;
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipSprite(value);
            moving = true;
            Move(value);
        }
    }

    public void Move(int value)
    {
        //Vector2 movement = new Vector2(value, 0);
       rigidBody.velocity = new Vector2(value * speed, rigidBody.velocity.y);
    }

    void FlipSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
        }
        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
        }
    }

    public void Jump()
    {
        //Jump only if on ground
        if(collisionDetect.onGround == true)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            rigidBody.velocity += Vector2.up * jumpForce;
        }

        //rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Dash()
    {
        if(stamina > 0 && canMove == true)
        {
            // dont want any other inputs during the dash
            StartCoroutine(NoMove(dashDuration + 0.15f));

            // reduce stamina by dash cost amount
            // set player vector speed to 0 before executing dash. This allows finetune control for players during and after dash.
            stamina -= dashCost;
            rigidBody.velocity = Vector2.zero;

            Vector2 dashDir = new Vector2(0f, 0f);

            // dash forward depending on player facing direction if no directional inputs
            if (xRaw == 0 && yRaw == 0)
            {
                if (faceRightState)
                {
                    dashDir = new Vector2(1f, 0f).normalized; 
                }
                else
                {
                    dashDir = new Vector2(-1f, 0f).normalized;
                }
            }
            else
            {
                dashDir = new Vector2(xRaw, yRaw).normalized;
            }
            Debug.Log("Dash Direction: " + dashDir);

            rigidBody.velocity += dashDir * dashSpeed;
            StartCoroutine(Dashing(dashDuration));
        }
    }

    public void Glide()
    {
        if(collisionDetect.cannotGlide == false && collisionDetect.onGround == false)
        {
            Debug.Log("GLIDING");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 3)
        {
            stamina = maxStamina;
        }
    }

    IEnumerator Dashing(float time)
    {
        // set grav to 0 as dash will be unaffected by any other forces
        isDashing = true;
        rigidBody.gravityScale = 0;

        yield return new WaitForSeconds(time);

        // sets grav back to normal. Sets velocity to a small amount to pad the exit dash
        rigidBody.gravityScale = gravity;
        rigidBody.velocity *= 0.2f;
        isDashing = false;

        // Stamina stays at max if still on ground after dashing
        if(collisionDetect.onGround == true)
        {
            stamina = maxStamina;
        }
    }
    IEnumerator NoMove(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}

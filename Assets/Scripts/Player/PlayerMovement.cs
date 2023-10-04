using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerCollision collisionDetect;
    private Rigidbody2D rigidBody;

    [Header("Movement")]
    public float speed = 10;
    public float jumpForce = 50;
    public float gravity = 4;

    public Vector2 velocity;
    public Vector2 currentActingVelocity;
    public float xRaw;
    public float yRaw;

    [Space]
    [Header("Dash")]
    public float dashSpeed = 20;
    public float dashCost = 1;
    public float dashDuration = 0.3f;

    [Space]
    [Header("Stamina")]
    public float maxStamina = 2;
    public float stamina = 2;

    [Space]
    [Header("Bool Stats")]
    public bool canMove = true;
    public bool faceRightState = true;


    public bool moving = false;
    private bool isDashing = false;
    private bool isGliding = false;

    // Body color of Player
    private Color staminaCol = Color.white;


    // Start is called before the first frame update
    void Start()
    {
        collisionDetect = this.GetComponent<PlayerCollision>();
        rigidBody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get keyboard arrow key presses (up/down/left/right)
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
        if(canMove == true){
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
            StartCoroutine(NoMove(dashDuration + 0.13f));

            // reduce stamina by dash cost amount
            // update color of player (stamina bar)
            // set player vector speed to 0 before executing dash. This allows finetune control for players during and after dash.
            stamina -= dashCost;
            UpdateColor(stamina, maxStamina);
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

            StartCoroutine(NoGravity(dashDuration + 0.12f));
            StartCoroutine(Dashing(dashDuration));
        }
    }

    public void Glide()
    {
        if(collisionDetect.cannotGlide == false && collisionDetect.onGround == false)
        {
            Debug.Log("GLIDING");
            isGliding = true;

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // If player lands on Ground (Layer 3), recharges stamina
        if (col.gameObject.layer == 3)
        {
            stamina = maxStamina;
            UpdateColor(stamina, maxStamina);
        }
    }

    IEnumerator Dashing(float time)
    {
        isDashing = true;

        yield return new WaitForSeconds(time);

        rigidBody.velocity *= 0.2f;
        isDashing = false;

        // Stamina stays at max if still on ground after dashing
        if(collisionDetect.onGround == true)
        {
            stamina = maxStamina;
            UpdateColor(stamina, maxStamina);
        }
    }
    IEnumerator NoMove(float time)
    {
        canMove = false;
        moving = false;
        yield return new WaitForSeconds(time);
        canMove = true;
        MoveCheck((int) xRaw);
    }

    IEnumerator NoGravity(float time)
    {
        rigidBody.gravityScale = 0;
        yield return new WaitForSeconds(time);
        rigidBody.gravityScale = gravity;
    }

    void UpdateColor(float stamina, float maxStamina)
    {
        Color updatedCol = new Color(1f, (stamina / maxStamina), (stamina / maxStamina));
        this.GetComponent<SpriteRenderer>().color = updatedCol;
    }

    public void AddActingVelocity(Vector2 vel, int numFrames)
    {
        currentActingVelocity += vel;
        int frameToStop = Time.frameCount + numFrames;
        RemoveActingVelocity(vel, frameToStop);
    }
    IEnumerator RemoveActingVelocity(Vector2 vel, int frameToStop)
    {
        yield return new WaitUntil(() => Time.frameCount > frameToStop);
        currentActingVelocity -= vel;
    }

}

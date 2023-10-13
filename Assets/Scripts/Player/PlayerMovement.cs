using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerCollision collisionDetect;
    private Animator playerAnimator;
    private Rigidbody2D rigidBody;
    private SpriteRenderer playerSprite;


    [Header("Movement")]
    public float speed = 10;
    public float jumpForce = 50;
    public float gravity = 4;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public Vector2 velocity;
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
    public bool isDashing = false;
    public bool isGliding = false;
    public bool jumpHold = false;

    // Body color of Player
    private Color staminaCol = Color.white;


    // Start is called before the first frame update
    void Start()
    {
        collisionDetect = this.GetComponent<PlayerCollision>();
        rigidBody = this.GetComponent<Rigidbody2D>();
        playerAnimator = this.GetComponent<Animator>();
        playerSprite = this.GetComponent<SpriteRenderer>();
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
        // Left/Right Movement
        if (moving == true && canMove == true)
        {
            Move(faceRightState == true ? 1 : -1);
        }

        // For animator
        if(collisionDetect.onGround == true)
        {
            playerAnimator.SetBool("onGround", true);
        }
        else
        {
            playerAnimator.SetBool("onGround", false);
        }

        // Adjust fall and jump value
        if (rigidBody.velocity.y < 0 && isGliding == false && isDashing == false)
        {
            rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } 
        else if(rigidBody.velocity.y > 0 && jumpHold == false && isGliding == false && isDashing == false)
        {
            rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // DEBUGGING PURPOSES IN THE INSPECTOR TO SHOW VELOCITY OF CHARACTER IN EACH FRAME
        velocity = rigidBody.velocity;
        playerAnimator.SetFloat("ySpeed", rigidBody.velocity.y);
        playerAnimator.SetFloat("xSpeed", rigidBody.velocity.x);
    }

    public void MoveCheck(int value)
    {
        if(canMove == true){
            if (value == 0)
            {
                moving = false;
                playerAnimator.SetBool("isMoving", false);
            }
            else
            {
                FlipSprite(value);
                moving = true;
                playerAnimator.SetBool("isMoving", true);
                Move(value);
            }
        }

    }

    public void Move(int value)
    {
       //Vector2 movement = new Vector2(value, 0);
       if(isGliding == false)
        {
            rigidBody.velocity = new Vector2(value * speed, rigidBody.velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector2(value * speed * 0.75f, rigidBody.velocity.y);
        }
    }

    public void Jump()
    {
        //Jump only if on ground
        if (collisionDetect.onGround == true && isGliding == false)
        {
            ReplaceActingVelocity(new Vector2(rigidBody.velocity.x, 0));
            AddActingVelocity(Vector2.up * jumpForce, 0);
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
            ReplaceActingVelocity(Vector2.zero);

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

            AddActingVelocity(dashDir * dashSpeed, 0);

            StartCoroutine(NoGravity(dashDuration + 0.15f));
            StartCoroutine(Dashing(dashDuration));
        }
    }

    public void Glide()
    {
        if(isGliding == false && isDashing == false && canMove == true && 
            collisionDetect.cannotGlide == false && collisionDetect.onGround == false)
        {
            Debug.Log("GLIDING");
            isGliding = true;
            playerAnimator.SetBool("isGliding", true);
            ReplaceActingVelocity(Vector2.zero);
            ChangePlayerGravity(0.5f);
        }
        else if (isGliding == true && isDashing == false && canMove == true && 
            collisionDetect.onGround == false)
        {
            isGliding = false;
            playerAnimator.SetBool("isGliding", false);
            ChangePlayerGravity(gravity);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // If player lands on Ground (Layer 3), recharges stamina
        if (col.gameObject.layer == 3)
        {
            isGliding = false;
            playerAnimator.SetBool("isGliding", false);
            ChangePlayerGravity(gravity);

            stamina = maxStamina;
            UpdateColor(stamina, maxStamina);
        }
    }

    IEnumerator Dashing(float time)
    {
        isDashing = true;
        playerAnimator.SetBool("isDashing", true);

        yield return new WaitForSeconds(time);

        rigidBody.velocity = new Vector2(rigidBody.velocity.x * 0.1f, rigidBody.velocity.y * 0.1f);
        isDashing = false;
        playerAnimator.SetBool("isDashing", false);

        // Stamina stays at max if still on ground after dashing
        if (collisionDetect.onGround == true)
        {
            isGliding = false;
            playerAnimator.SetBool("isGliding", false);
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
        ChangePlayerGravity(0);
        yield return new WaitForSeconds(time);
        ChangePlayerGravity(gravity);
    }

    void FlipSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            playerSprite.flipX = true;

            if (rigidBody.velocity.x > 0.1f)
            {
                playerAnimator.SetTrigger("isTurning");
            }

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            playerSprite.flipX = false;

            if (rigidBody.velocity.x < -0.1f)
            {
                playerAnimator.SetTrigger("isTurning");
            }
        }
    }

    public void JumpHold()
    {
        jumpHold = true;
    }
    public void JumpRelease()
    {
        jumpHold = false;
    }

    void UpdateColor(float stamina, float maxStamina)
    {
        Color updatedCol = new Color(1f, (stamina / maxStamina), (stamina / maxStamina));
        this.GetComponent<SpriteRenderer>().color = updatedCol;
    }

    public void AddActingVelocity(Vector2 vel, int numFrames)
    {
        rigidBody.velocity += vel;

        if (numFrames > 0)
        {
            int frameToStop = Time.frameCount + numFrames;
            StartCoroutine(RemoveActingVelocity(vel, frameToStop));
        }

    }
    IEnumerator RemoveActingVelocity(Vector2 vel, int frameToStop)
    {
        Debug.Log("Waiting for frame");
        yield return new WaitUntil(() => Time.frameCount > frameToStop);
        rigidBody.velocity -= vel;
        Debug.Log("Frame met, removing vel");
    }
    public void ReplaceActingVelocity(Vector2 vel)
    {
        rigidBody.velocity = vel;
    }
    public void ChangePlayerGravity(float gravityScale)
    {
        rigidBody.gravityScale = gravityScale;
    }

}

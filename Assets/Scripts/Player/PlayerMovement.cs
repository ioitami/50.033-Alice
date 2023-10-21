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
    //public Vector2 speedCap;
    public float acceleration = 5f;
    public float decceleration = 5f;
    public float velocityPower = 0.9f;
    public float friction = 2f;
    [Space]
    public float jumpForce = 50;
    public float gravity = 4;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    [Space]
    public Vector2 velocity;
    public float xRaw;
    public float yRaw;


    [Space]
    [Header("Dash")]
    public float dashSpeed = 20;
    public float dashCost = 1;
    public float dashDuration = 0.3f;
    public float dashEndMultiplierX = 0.3f;
    public float dashEndMultiplierY = 0.1f;

    [Space]
    [Header("Glide")]
    public float glideGravity = 0.5f;
    public float glideSpeedMultiplier = 0.8f;
    public float glideEquipCost = 0.5f;
    public float glideStamPerSec = 0.25f;

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

        // Checks if isGliding true. Drains stamina if it is.
        StartCoroutine(GlidingStamDrain());
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

        // Friction if player stops inputting left/right and is on ground
        if (collisionDetect.onGround == true && xRaw == 0)
        {
            // Use the min of current vel or friction amount, then add that force to player
            float velocityAmt = Mathf.Min(Mathf.Abs(rigidBody.velocity.x), Mathf.Abs(friction));
            velocityAmt *= Mathf.Sign(rigidBody.velocity.x);
            rigidBody.AddForce(Vector2.right * -velocityAmt, ForceMode2D.Impulse);
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

        // unequip glider if stamina reaches 0
        if (stamina <= 0)
        {
            isGliding = false;
            playerAnimator.SetBool("isGliding", false);
            ChangePlayerGravity(gravity);
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

        // Make sure velocity doesnt go beyond speed cap
        //if (Mathf.Abs(rigidBody.velocity.x) > speedCap.x)
        //{
        //    rigidBody.velocity = (rigidBody.velocity.x > 0) ? new Vector2(speedCap.x, rigidBody.velocity.y) :
        //                                                      new Vector2(-speedCap.x, rigidBody.velocity.y);
        //}
        //if (Mathf.Abs(rigidBody.velocity.y) > speedCap.y)
        //{
        //    rigidBody.velocity = (rigidBody.velocity.y > 0) ? new Vector2(rigidBody.velocity.x, speedCap.y) :
        //                                                      new Vector2(rigidBody.velocity.y, -speedCap.y);
        //}

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
            // https://www.youtube.com/watch?v=KbtcEVCM7bw&t=324s reference

            float targetSpeed = value * speed;
            // diff between current velocity and max speed (target speed)
            float speedDifference = targetSpeed - rigidBody.velocity.x;
            // change acceleration based on if player is moving right or left
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

            float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelRate, velocityPower) * Mathf.Sign(speedDifference);

            rigidBody.AddForce(movement * Vector2.right);
        }
        else
        {
            float targetSpeed = value * speed * glideSpeedMultiplier;
            // diff between current velocity and max speed (target speed)
            float speedDifference = targetSpeed - rigidBody.velocity.x;
            // change acceleration based on if player is moving right or left
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration * 0.5f : decceleration * 0.5f;

            float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelRate, velocityPower) * Mathf.Sign(speedDifference);

            rigidBody.AddForce(movement * Vector2.right);
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

            StartCoroutine(NoGravity(dashDuration + 0.17f));
            StartCoroutine(Dashing(dashDuration));
        }
    }

    public void Glide()
    {

        if(stamina > 0 && isGliding == false && collisionDetect.cannotGlide == false)
        {
            Debug.Log("EQUIP GLIDER");

            stamina -= glideEquipCost;

            ReplaceActingVelocity(new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * glideGravity));

            isGliding = true;
            ChangePlayerGravity(glideGravity);

            playerAnimator.SetBool("isGliding", true);
        }
        else if (isGliding == true && collisionDetect.onGround == false)
        {
            Debug.Log("UNEQUIP GLIDER");

            isGliding = false;
            ChangePlayerGravity(gravity);

            playerAnimator.SetBool("isGliding", false);
        }

        if(stamina <= 0)
        {
            isGliding = false;
            playerAnimator.SetBool("isGliding", false);
            ChangePlayerGravity(gravity);
        }
    }

    IEnumerator WaitToGlide(float time)
    {
        yield return new WaitForSeconds(time);
        Glide();
    }

    IEnumerator GlidingStamDrain()
    {
        // Continue running until isGliding is set to false
        float timeElapsed = 0;
        bool timeToDrain = false;
        while (true)
        {
            if (isGliding)
            {
                if (timeToDrain)
                {
                    // Debug.Log("Stamina Drained from Gliding!");
                    stamina -= glideStamPerSec * (timeElapsed + Time.deltaTime);

                    timeElapsed = 0;
                    timeToDrain = false;
                    UpdateColor(stamina, maxStamina);
                }
                else
                {
                    timeElapsed += Time.deltaTime;
                    timeToDrain = true;
                }
            }
            yield return null;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        ContactPoint2D contact = col.contacts[0];

        // If player lands on Ground (Layer 3) AND collides from above, recharges stamina
        if (Vector2.Dot(contact.normal, Vector2.up) > 0.5 && col.gameObject.layer == 3 && collisionDetect.onGround == true)
        {
            Debug.Log("Landed. Stamina Recharged.");
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
        isGliding = false;
        playerAnimator.SetBool("isDashing", true);
        playerAnimator.SetBool("isGliding", false);

        yield return new WaitForSeconds(time);

        rigidBody.velocity = new Vector2(rigidBody.velocity.x * dashEndMultiplierX, rigidBody.velocity.y * dashEndMultiplierY);
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

        // Checks if player starts gliding before e.g dash ends
        if (isGliding)
        {
            ChangePlayerGravity(glideGravity);
        }
        else
        {
            ChangePlayerGravity(gravity);
        }
    }

    void FlipSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            playerSprite.flipX = true;

            if (rigidBody.velocity.x > 2f)
            {
                playerAnimator.SetTrigger("isTurning");
            }

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            playerSprite.flipX = false;

            if (rigidBody.velocity.x < -2f)
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
        //Sprite sprite = this.GetComponent<SpriteRenderer>().sprite;
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


    public void Teleport(Vector2 vel, Vector3 dest)
    {
        velocity = Vector2.zero;

        // TODO add some delay?

        transform.position = dest;

        // some stuff
        velocity = vel;
    }
}

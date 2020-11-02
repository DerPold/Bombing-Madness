using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerControlsUpdatet : MonoBehaviour
{
    public Image jumpImage;
    public Text jumpDurationText;
    public Text speedPowerupText;
    public float currentRotation;
  
    Animator anim;
    float rotation;
    int counter = 0;

    float forwardVel = 12;
    float rotateVel = 110;
    float turnSpeed = 10;
    float distance;
    Quaternion targetRotation;
    CharacterController controller;

    private float jumpCounter = 0f;
    private float jumpTime = 15f;
    private float speedPowerUpCounter;
    private float speedPowerUpShowTime = 3f;

    private float speedMultiplier = 1.0f;
    private float sprintMultiplier = 1.5f;
    private float speed = 6.0f;
    private float jumpSpeed = 7.2f;
    private float gravity = 10.0f;
    private Vector3 moveDirection = Vector3.zero;
    private float h, v;
    private AnimatorStateInfo currentBaseState;

    private float deltaFrameCounter;
    private bool isGroundAndJumpButtonPressed;

    static int blendtree = Animator.StringToHash("Base Layer.Blend Tree");
    static int falling = Animator.StringToHash("Base Layer.Falling");
    static int jumping = Animator.StringToHash("Base Layer.Jumping");
    static int landing = Animator.StringToHash("Base Layer.Landing");
    private bool isFalling;
    private bool isDead;
    private bool isStunned;
    private float stunnedCounter;
    private float stunDuration = 2.5f;
    private float stunDurationSelfStun = 1.5f;

    private ReadSSI ssi;
    private int buff;

    void Start()
    {
        ssi = GetComponent<ReadSSI>();
        
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        targetRotation = transform.rotation;
        jumpImage.enabled = false;
        distance = controller.radius + 0.2f;
        isFalling = false;
    }

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    public float Rotation
    {
        get { return rotation;  }
    }

    public void increaseSpeed()
    {
        if (speedMultiplier < 1.5f) speedMultiplier += 0.05f;
        if (sprintMultiplier < 2.0f) sprintMultiplier += 0.1f;
        speedPowerUpCounter += speedPowerUpShowTime;
        speedPowerupText.text = "Move speed: " + speedMultiplier * 100 + " % Sprint Speed: " + sprintMultiplier * 100 + " %";
    }

    public void increaseJumpHeightForDuration()
    {
        jumpCounter += jumpTime;
        jumpImage.enabled = true;
    }

    public void playDeathAnimation()
    {
        anim.SetBool("Died", true);
        changeAnimatorState("Death");
        isDead = true;
    }

    public void setStunned(bool selfStun)
    {
        if(!isCurrentState("Block"))
        {
            isStunned = true;
            AudioManager.instance.playStunnedSound();
            if(selfStun) stunnedCounter = stunDurationSelfStun;
            else         stunnedCounter = stunDuration;
            anim.SetBool("Stunned", true);
        }
    }


    void Update()
    {
        bool inputQ = Input.GetKey(KeyCode.Q);
        bool inputE = Input.GetKey(KeyCode.E);
        bool inputShift = Input.GetKey(KeyCode.LeftShift);

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        rotation += mouseX;
        float turnInput = mouseX;

        if (inputQ) turnInput += -1;
        if (inputE) turnInput += 1;

        if(ssi.PulseBuff == float.NaN)
        {
            ssi.PulseBuff = 0;
        }

        float jumpMultiplier = 1.0f;
        if(jumpCounter > 0)
        {
            jumpCounter -= Time.deltaTime;
            if (jumpCounter > 0)
            {
                jumpDurationText.text = "" + jumpCounter;
                jumpMultiplier = 1.5f;
            }
            else
            {
                jumpCounter = 0;
                jumpImage.enabled = false;
                jumpDurationText.text = "";
            }
        }

        if(stunnedCounter > 0)
        {
            stunnedCounter -= Time.deltaTime;
            if(stunnedCounter <= 0)
            {
                stunnedCounter = 0;
                isStunned = false;
                anim.SetBool("Stunned", false);
            }
        }

        if(speedPowerUpCounter > 0)
        {
            speedPowerUpCounter -= Time.deltaTime;
            if (speedPowerUpCounter <= 0)
            {
                speedPowerUpCounter = 0;
                speedPowerupText.text = "";
            }
        }
        currentRotation = rotateVel * turnInput * Time.deltaTime;
        targetRotation *= Quaternion.AngleAxis(currentRotation, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,Time.time);

        if (inputShift)
        {
            v *= sprintMultiplier;
        }

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(h, 0, v);
            moveDirection = transform.TransformDirection(moveDirection);
            if (h != 0 && v != 0)
            {
                moveDirection *= (speed * (1/(Math.Abs(h)+ Math.Abs(v))));
                
            }
            else
            {
                moveDirection *= (speed + ssi.PulseBuff);
            }
            if (Input.GetKey(KeyCode.Space) && !isDead && !isStunned)
            {
                moveDirection.y = jumpSpeed * jumpMultiplier + ssi.PulseBuff;
                isGroundAndJumpButtonPressed = true;
            }
        }
        else
        {
            moveDirection = new Vector3(h, moveDirection.y, v);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= speed * speedMultiplier;
            moveDirection.z *= speed * speedMultiplier;
            moveDirection.y -= gravity * Time.deltaTime;
        }
        //pushCharacterBack();
        if (isDead || stunnedCounter > 0)
        {
            moveDirection.x = 0;
            moveDirection.z = 0;
        }
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void pushCharacterBack()
    {
        RaycastHit hit;

        //Bottom of controller. Slightly above ground so it doesn't bump into slanted platforms. (Adjust to your needs)
        Vector3 p1 = transform.position + Vector3.up * 0.25f;
        //Top of controller
        Vector3 p2 = p1 + Vector3.up * this.controller.height;

        //Check around the character in a 360, 10 times (increase if more accuracy is needed)
        for (int i = 0; i < 360; i += 36)
        {
            //Check if anything with the platform layer touches this object
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, distance, 1))
            {
                if(hit.transform.gameObject.layer != LayerMask.NameToLayer("Collectible"))
                //If the object is touched by a platform, move the object away from it
                controller.Move(hit.normal * (distance - hit.distance));
            }
        }

        //[Optional] Check the players feet and push them up if something clips through their feet.
        //(Useful for vertical moving platforms)
        if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hit, 1, 1))
        {
            controller.Move(Vector3.up * (1 - hit.distance));
        }
    }

    void FixedUpdate()
    {
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
        bool inputF = Input.GetKey(KeyCode.F);

        bool inputG = Input.GetKey(KeyCode.G);

        bool inputB = Input.GetKey(KeyCode.B);

        bool inputShift = Input.GetKey(KeyCode.LeftShift);

        bool inputMouse1 = Input.GetMouseButton(0);

        if (inputB)
        {
            anim.SetBool("Block", true);
        }
        else
        {
            anim.SetBool("Block", false);
        }
        if (inputF)
        {
            changeAnimatorState("RoundKick");
        }
        if (inputG)
        {
            changeAnimatorState("Dodge");
        }
        anim.SetFloat("xDimension", h);
        anim.SetFloat("yDimension", v);


        if(isGroundAndJumpButtonPressed)
        {
            anim.SetBool("Jump",true);
            isGroundAndJumpButtonPressed = false;
        }
        if( (controller.velocity.y < 0 && currentBaseState.fullPathHash == jumping) || (controller.velocity.y < -4 && !isFalling) )
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", true);
            isFalling = true;
        }
        else if((currentBaseState.fullPathHash == falling || currentBaseState.fullPathHash == jumping) && controller.isGrounded)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
            anim.SetBool("Land", true);
            isFalling = false;
        }

        if(currentBaseState.fullPathHash == landing)
        {
            deltaFrameCounter += Time.deltaTime;
            if(deltaFrameCounter >= 0.4)
            {
                anim.SetBool("Land", false);
                deltaFrameCounter = 0;
            }
        }

        if (inputShift && v != 0)
        {
            anim.speed = sprintMultiplier;
        }
        else anim.speed = 1.0f;
    }

    private bool isCurrentState(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    private bool isCurrentStateAndValid(string name)
    {
        return isCurrentState(name);
    }

    private void changeAnimatorState(string state)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state)) anim.SetTrigger(state);
    }
}

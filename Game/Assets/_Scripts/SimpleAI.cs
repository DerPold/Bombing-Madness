using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAI : MonoBehaviour {


    public float lookRadius = 100f;
    public float rotateRadius = 40f;
    public float bombThrowRadiusPlayer = 20f;
    public float bombThrowRadiusTree = 12f;
    public float bombThrowRadiusMagicCube = 10f;
    public float moveSpeed = 5f;
    public Transform target;
    public Vector3 impact = Vector3.zero;
    public float throwForce = 30f;
    public float mass = 3.0f; // defines the character mass
    public GameObject bomb;
    public GameObject bomb2;
    public float maxTimeStuck = 7f;
    public float maxTimePursueTarget = 12.5f;
    public GameObject water;

    Animator anim;
    float cooldownCounterBombThrowing = 0f;
    float cooldownAmountBombThrowing = 5f;
    static int blendtree = Animator.StringToHash("Base Layer.Blend Tree");
    static int falling = Animator.StringToHash("Base Layer.Falling");
    static int jumping = Animator.StringToHash("Base Layer.Jumping");
    static int landing = Animator.StringToHash("Base Layer.Landing");
    float drunkTimer;

    private DecisionTree decisionTree;
    private float jumpSpeed = 10.4f;
    private float gravity = 10.0f;
    CharacterController controller;
    private bool isStunned;
    private bool isFalling;
    private float stuckCounter;
    private Vector3 lastTransformPosition;
    private float stunnedCounter;
    private float stunDuration = 3.5f;
    private Vector3 moveDirection = Vector3.zero;
    private bool jumpInAir = false;
    private float deltaFrameCounter;
    private bool isGroundedAndJumpActionAttended;
    private AnimatorStateInfo currentBaseState;
    private float counterPursueTarget;

    //flee 
    public float checkForBombRadius;
    public float fleeDistance;
    public float fleeOffset = 0.1f;
    private GameObject playerBomb;
    private Vector3 fleeOrientation;
    private Vector3 fleePoint;
    private bool reachedFleePoint;
    private bool isDead = false;
    private float fleeCounter;

    void Start ()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        decisionTree = DecisionTree.instance;
        decisionTree.loadVariables();
        target = decisionTree.getGameObjectWithHighestPrio(gameObject).transform;
        counterPursueTarget = 0f;
        cooldownCounterBombThrowing = cooldownAmountBombThrowing;
        reachedFleePoint = true;
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Block"))
        {
            Jump();
        }
    }

    public void Jump()
    {
        jumpInAir = true;
    }

    public void die()
    {
        anim.SetBool("Died", true);
        changeAnimatorState("Death");
        isDead = true;
        Destroy(gameObject, 5.0f);
    }

    private void changeAnimatorState(string state)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state)) anim.SetTrigger(state);
    }

    public void SetStunned()
    {
        isStunned = true;
        stunnedCounter = stunDuration;
        anim.SetBool("Stunned", true);
    }

    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        //if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        dir.y = 1.2f;
        impact += dir.normalized * force / mass;
    }

    // Update is called once per frame
    void Update ()
    {
        if (isDead) return;

        if (water.transform.position.y >= transform.position.y + 2f)
        {
            drunkTimer += Time.deltaTime;
            if (drunkTimer >= 2)
            {
                Jump();
            }
        }
        else
        {
            drunkTimer = 0;
        }

        counterPursueTarget += Time.deltaTime;
        if(counterPursueTarget > maxTimePursueTarget)
        {
            target = decisionTree.getGameObjectWithHighestPrio(gameObject).transform;
            counterPursueTarget = 0f;
        }
        if (transform.position == lastTransformPosition)
        {
            stuckCounter += Time.deltaTime;
            if(stuckCounter >= maxTimeStuck)
            {
                stuckCounter = 0f;
                DropBomb(bomb2);
                Jump();
            }
        }
        lastTransformPosition = transform.position;
        if (stunnedCounter > 0)
        {
            stunnedCounter -= Time.deltaTime;
            if (stunnedCounter <= 0)
            {
                stunnedCounter = 0;
                isStunned = false;
                anim.SetBool("Stunned", false);
                reachedFleePoint = true;
            }
        }

        if (cooldownCounterBombThrowing > 0)
        {
            cooldownCounterBombThrowing -= Time.deltaTime;
            if (cooldownCounterBombThrowing < 0) cooldownCounterBombThrowing = 0;
        }

        if(target == null)
        {
            target = decisionTree.getGameObjectWithHighestPrio(gameObject).transform;
            counterPursueTarget = 0f;
        }

        float distance = Vector3.Distance(this.transform.position, target.position);

        if(distance >= lookRadius)
        {
            target = decisionTree.getGameObjectWithHighestPrio(gameObject).transform;
            counterPursueTarget = 0f;
        }

        if (controller.isGrounded)
        {
            if (reachedFleePoint)
            {
                moveDirection = (target.position - transform.position).normalized;
                moveDirection.y = 0;
                bool inRange = bombInRange();
                if(inRange)
                {
                    fleeCounter = 0f;
                    reachedFleePoint = false;
                }
               
            }
            if (!reachedFleePoint)
            {
                fleeCounter += Time.deltaTime;
                if (fleeCounter > 5f) reachedFleePoint = true;
                flee();
                moveDirection = (fleePoint - transform.position).normalized;
                if (moveDirection.x <= fleeOffset && moveDirection.z <= fleeOffset)
                {
                    reachedFleePoint = true;
                }
            }
            if (jumpInAir)
            {
                isGroundedAndJumpActionAttended = true;
                jumpInAir = false;
                moveDirection.y = jumpSpeed;
            }
        }
        else
        {
            Vector3 oldMoveDirection = moveDirection;
            moveDirection = (target.position - transform.position).normalized;
            moveDirection.y = oldMoveDirection.y;
            moveDirection.y -= gravity * Time.deltaTime;
        }

        moveDirection.x *= moveSpeed;
        moveDirection.z *= moveSpeed;
        float maxMovement = Mathf.Max(Mathf.Abs(moveDirection.x), Mathf.Abs(moveDirection.z));
        float h = moveDirection.x / maxMovement;
        float v = moveDirection.z / maxMovement;
        if (isStunned || (distance < 10f && !target.CompareTag("Collectable")))
        {
            moveDirection.x = 0;
            moveDirection.z = 0;
            anim.SetFloat("xDimension", 0);
            anim.SetFloat("yDimension", 0);
        }
        else
        {
            //anim.SetFloat("xDimension", h);
            //anim.SetFloat("yDimension", Mathf.Abs(v));
            anim.SetFloat("xDimension", 0);
            anim.SetFloat("yDimension", 1);
        }

        if (distance <= rotateRadius)
        {
            if (stunnedCounter == 0)
            {
                FaceTarget();
                float throwDeeper = 0.0f;
                if(distance <= 2f)
                {
                    throwDeeper = -0.2f;
                }
                if(distance <= 1f)
                {
                    throwDeeper = -0.4f;
                }
                if (distance <= 0.5f)
                {
                    throwDeeper = -0.8f;
                }
                if (distance <= bombThrowRadiusPlayer && target.gameObject.CompareTag("Player"))
                {
                    GameObject bombToThrow;
                    if (Random.Range(0.0f, 1.0f) <= 0.33f) bombToThrow = bomb;
                    else bombToThrow = bomb2;
                    ThrowBomb(bombToThrow, 0.2f + throwDeeper);
                }
                if (distance <= bombThrowRadiusTree && target.gameObject.CompareTag("Tree"))
                {
                    GameObject bombToThrow;
                    bombToThrow = bomb;
                    ThrowBomb(bombToThrow, 0.0f + throwDeeper);
                }
                if (distance <= bombThrowRadiusMagicCube && target.gameObject.CompareTag("MagicCube"))
                {
                    GameObject bombToThrow;
                    bombToThrow = bomb;
                    ThrowBomb(bombToThrow, 0.0f + throwDeeper);
                }
            }
        }
        controller.Move(moveDirection * Time.deltaTime);

        if (impact.magnitude > 0.2)
        {
            controller.Move(impact * Time.deltaTime);
        }
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    void FixedUpdate()
    {
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
        if (isGroundedAndJumpActionAttended)
        {
            anim.SetBool("Jump", true);
            isGroundedAndJumpActionAttended = false;
        }
        if ((controller.velocity.y < 0 && currentBaseState.fullPathHash == jumping) || (controller.velocity.y < -4 && !isFalling))
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", true);
            isFalling = true;
        }
        else if ((currentBaseState.fullPathHash == falling || currentBaseState.fullPathHash == jumping) && controller.isGrounded)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
            anim.SetBool("Land", true);
            isFalling = false;
        }

        if (currentBaseState.fullPathHash == landing)
        {
            deltaFrameCounter += Time.deltaTime;
            if (deltaFrameCounter >= 0.4)
            {
                anim.SetBool("Land", false);
                deltaFrameCounter = 0;
            }
        }
    }

    public void ThrowBomb(GameObject grenadeToThrow, float heightThrowDirection)
    {
        if (cooldownCounterBombThrowing <= 0)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y += heightThrowDirection;
            GameObject bomb = Instantiate(grenadeToThrow, transform.position + new Vector3(0, 2, 0), transform.rotation);
            GrenadeExploding grenadeExploding = bomb.GetComponent<GrenadeExploding>();
            if (grenadeExploding == null) Debug.Log("Fehler!");
            grenadeExploding.throwTag = "Enemy";
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            rb.AddForce(direction * throwForce, ForceMode.VelocityChange);
            cooldownCounterBombThrowing = cooldownAmountBombThrowing;
        }
    }

    public void DropBomb(GameObject grenadeToThrow)
    {
            GameObject bomb = Instantiate(grenadeToThrow, transform.position + new Vector3(0, 0, 0), transform.rotation);
            GrenadeExploding grenadeExploding = bomb.GetComponent<GrenadeExploding>();
            if (grenadeExploding == null) Debug.Log("Fehler!");
            grenadeExploding.throwTag = "Enemy";
            cooldownCounterBombThrowing = cooldownAmountBombThrowing;
    }

    public void FaceTarget()
    {
        Vector3 direction;
        if (reachedFleePoint)
        {
            direction = (target.position - transform.position).normalized;            
        }
        else
        {
            direction = (fleePoint - transform.position).normalized;
        }
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    
    public void flee()
    {       
        Vector3 direction = (transform.position - playerBomb.transform.position).normalized;      
        fleePoint = transform.position + direction * fleeDistance;        
    }

    public bool bombInRange()
    {
        GameObject[] bombList = GameObject.FindGameObjectsWithTag("Bomb");
        if (bombList.Length > 0)
        {
            foreach (GameObject go in bombList)
            {
                float newdist = Vector3.Distance(go.transform.position, gameObject.transform.position);
                GrenadeExploding grenadeExploding = go.GetComponent<GrenadeExploding>();
                if (newdist < checkForBombRadius && grenadeExploding.throwTag == "Player")
                {
                    playerBomb = go;
                    return true;
                }
            }
        }
        return false;
    }
}

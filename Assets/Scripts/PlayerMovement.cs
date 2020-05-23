using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D PlayerRigidBody;
    Transform PlayerTransform;
    Animator animator;

    PlayerState playerState;
    float distToGround;

    float playerRightScale;

    BoxCollider2D headCollider;
    CircleCollider2D legsCollider;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerTransform = GetComponent<Transform>();
        headCollider = GetComponent<BoxCollider2D>();
        legsCollider = GetComponent<CircleCollider2D>();
        distToGround = legsCollider.radius;
        animator = GetComponent<Animator>();
        playerRightScale = PlayerTransform.localScale.x;
        

    }

    float moveVertical;
    float moveHorizontal;
    // Update is called once per frame
    void Update()
    {
        moveVertical = Input.GetAxis("Vertical");
        moveHorizontal = Input.GetAxis("Horizontal");
        
    }



    [Range(0, 10)]
    public float HorizontalSpeed = 4f;
    [Range(0,7)]
    public float CrouchSpeed = 2f;
    [Range(0, 100)]
    public float HorizontalForce = 50f;
    [Range(0, 70)]
    public float CrouchForce = 30f;
    [Range(0, 10)]
    public float JumpForce = 2f;
    private void FixedUpdate()
    {
        #region Apply forces to the player for movement
        bool shouldCrouch = moveVertical <= -0.01 && IsGrounded() || (animator.GetBool("crouch") && CanNotStand());
        bool shouldNotIncreaseSpeed = Mathf.Abs(PlayerRigidBody.velocity.x) > (shouldCrouch ? CrouchSpeed : HorizontalSpeed) && Mathf.Sign(PlayerRigidBody.velocity.x) == Mathf.Sign(moveHorizontal);
        var shouldJump = moveVertical >= 0.01 && IsGrounded() && !shouldCrouch && (PlayerRigidBody.velocity.y < 0.01);

        Vector2 movementMuliplier = new Vector2(!shouldNotIncreaseSpeed ? (shouldCrouch ? CrouchForce: HorizontalForce) * moveHorizontal : 0, shouldJump ? JumpForce : 0);

        var movementVector = new Vector2(1, 100);
        PlayerRigidBody.AddForce(movementVector * movementMuliplier);
        

        #endregion

        #region Set animation variables & state

        if (moveHorizontal != 0)
        {
            playerState = PlayerState.RUNNING;
            animator.SetBool("IsRunning", true);
        }
        else
        {
            playerState = PlayerState.IDLE;
            animator.SetBool("IsRunning", false);
        }
        if (shouldJump)
        {
            animator.SetTrigger("jump");
        }
        if (shouldCrouch)
        {
            playerState = PlayerState.CROUCH;
            animator.SetBool("crouch", true);
            headCollider.enabled = false;
        }
        else
        {
            animator.SetBool("crouch", false);
            headCollider.enabled = true;
        }
        animator.SetFloat("land", PlayerRigidBody.velocity.y);
        #endregion

        #region Change player face direction
        if (PlayerRigidBody.velocity.x > 0.01)
        {
            PlayerRigidBody.transform.localScale = new Vector2(playerRightScale, PlayerRigidBody.transform.localScale.y);
        }
        else if (PlayerRigidBody.velocity.x < -0.01)
        {
            PlayerRigidBody.transform.localScale = new Vector2(-playerRightScale, PlayerRigidBody.transform.localScale.y);
        }
        #endregion

        
    }

    //is player standing on ground
    bool IsGrounded()
    {
        var cast = Physics2D.Raycast(legsCollider.bounds.center, Vector2.down, distToGround + 0.01f, LayerMask.GetMask("Platform")).collider!=null;
        cast = cast || Physics2D.Raycast(legsCollider.bounds.center + new Vector3(legsCollider.radius,0), Vector2.down, distToGround + 0.01f, LayerMask.GetMask("Platform")).collider != null;
        cast = cast || Physics2D.Raycast(legsCollider.bounds.center - new Vector3(legsCollider.radius  , 0), Vector2.down, distToGround + 0.01f, LayerMask.GetMask("Platform")).collider != null;
        return cast;
    }
    bool CanNotStand()
    {
        var cast = Physics2D.Raycast(transform.position + new Vector3(headCollider.offset.x, headCollider.offset.y), Vector2.up, headCollider.size.y/2 + 0.01f, LayerMask.GetMask("Platform")).collider!=null;
        cast = cast || Physics2D.Raycast(transform.position + new Vector3(headCollider.offset.x, headCollider.offset.y) + new Vector3(headCollider.size.x / 2, 0), Vector2.up, headCollider.size.y / 2 + 0.01f, LayerMask.GetMask("Platform")).collider != null;
        cast = cast || Physics2D.Raycast(transform.position + new Vector3(headCollider.offset.x, headCollider.offset.y) - new Vector3(headCollider.size.x / 2, 0), Vector2.up, headCollider.size.y / 2 + 0.01f, LayerMask.GetMask("Platform")).collider != null;
        return cast;
    }


    public enum PlayerState
    {
        IDLE,
        RUNNING,
        CROUCH
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D PlayerRigidBody;
    Transform PlayerTransform;
    Animator animator;
    float distToGround;
    float playerRightScale;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
        PlayerTransform = GetComponent<Transform>();
        distToGround = GetComponent<Collider2D>().bounds.extents.y;
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
        if (moveHorizontal != 0)
        {
            animator.SetBool("IsRunning", true);


        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
    }

    [Range(0, 10)]
    public float HorizontalSpeed = 2f;
    [Range(0, 100)]
    public float HorizontalForce = 50f;
    [Range(0, 10)]
    public float JumpForce = 2f;
    private void FixedUpdate()
    {
        bool shouldIncreaseSpeed = Mathf.Abs(PlayerRigidBody.velocity.x) > HorizontalSpeed && Mathf.Sign(PlayerRigidBody.velocity.x) == Mathf.Sign(moveHorizontal);
        var shouldJump = moveVertical >= 0.01 && IsGrounded();
        bool shouldCrouch = IsGrounded() && moveVertical <= -0.01;

        Vector2 movementMuliplier = new Vector2(!shouldIncreaseSpeed ? HorizontalForce * moveHorizontal : 0, shouldJump ? JumpForce : 0);

        var movementVector = new Vector2(1, 100);
        PlayerRigidBody.AddForce(movementVector * movementMuliplier);

        if (shouldJump)
        {
            animator.SetTrigger("jump");
        }
        if (shouldCrouch)
        {
            animator.SetBool("crouch", true);
        }
        else
        {
            animator.SetBool("crouch", false);
        }
        if (PlayerRigidBody.velocity.x > 0.01)
        {
            PlayerRigidBody.transform.localScale = new Vector2(playerRightScale, PlayerRigidBody.transform.localScale.y);
        }
        else if (PlayerRigidBody.velocity.x < -0.01)
        {
            PlayerRigidBody.transform.localScale = new Vector2(-playerRightScale, PlayerRigidBody.transform.localScale.y);
        }

        animator.SetFloat("land", PlayerRigidBody.velocity.y);
    }

    //is player standing on ground
    bool IsGrounded()
    {
        var cast = Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.4f, LayerMask.GetMask("Platform"));
        var val = cast.collider != null;
        return val;
    }
}

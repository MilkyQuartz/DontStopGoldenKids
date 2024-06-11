using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("MoveMent")]
    private float moveSpeed;
    private float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    public GameObject backMirror;

    private Rigidbody rigidbody;
    public bool isTurn = false;

    PlayerAnimationController playerAnimationController;

    private void Awake()
    {
        playerAnimationController = GetComponent<PlayerAnimationController>();

        if (!TryGetComponent<Rigidbody>(out rigidbody))
        {
            Debug.Log("PlayerController.cs - Awake() - rigidbody 참조실패");
        }
    }

    public void InitController()
    {
        moveSpeed = 5;
        jumpPower = 5;
        playerAnimationController.Run();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.Self);

        Vector3 dir = transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded() && !playerAnimationController.GetJump())
        {
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);

            playerAnimationController.Jump();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            //Debug.Log(playerAnimationController.gameObject.name);
            playerAnimationController.JumpEnd();
        }
    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.5f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    Vector3 rotateAngle;

    public void OnTurnRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isTurn)
        {
            rotateAngle = new Vector3(0f, 90f, 0f);
            transform.Rotate(rotateAngle);
        }
    }

    public void OnTurnLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isTurn)
        {
            rotateAngle = new Vector3(0f, -90f, 0f);
            transform.Rotate(rotateAngle);
        }
    }

    public void OnLookBack(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            backMirror.SetActive(true);
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            backMirror.SetActive(false);
        }
    }
}

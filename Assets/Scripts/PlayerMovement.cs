using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 50f;      // prêdkoœæ chodu
    public float runSpeed = 90f;       // prêdkoœæ biegu
    public float jumpHeight = 20f;     // wysokoœæ skoku
    public float gravity = 50f;        // grawitacja

    private CharacterController controller;
    private Animator animator;
    public CameraMovement cameraOrbit;

    private Vector3 velocity;
    private bool isRunning;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(h, 0, v);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (input.magnitude > 0.1f)
        {
            Quaternion camRot = cameraOrbit.GetCameraRotation();
            Vector3 moveDir = camRot * input.normalized;
            moveDir.y = 0f;

            controller.Move(moveDir * currentSpeed * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * 10f);

            animator.SetFloat("Speed", input.magnitude);
            animator.SetBool("IsRunning", isRunning);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsRunning", false);
        }

        if (controller.isGrounded)
        {
            velocity.y = -1f;


            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
                animator.ResetTrigger("Jump");
                animator.SetTrigger("Jump");
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void PlayerAttackAnimation()
    {
        if (animator == null) return;
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
    }

    public void PlayerDefendAnimation()
    {
        if (animator == null) return;
        animator.ResetTrigger("Defend");
        animator.SetTrigger("Defend");
    }
}
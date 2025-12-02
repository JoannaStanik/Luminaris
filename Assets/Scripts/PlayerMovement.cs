using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpHeight = 8f;
    public float gravity = 20f;

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
                velocity.y = jumpHeight;
                animator.SetTrigger("Jump");
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
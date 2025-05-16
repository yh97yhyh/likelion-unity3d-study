using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CameraController cameraController;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    Quaternion targetRotation;

    Animator anim;
    string moveAmountStr = "moveAmount";
    CharacterController characterController;

    bool isGrounded;
    float ySpeed;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        var moveInput = (new Vector3(h, 0, v)).normalized;
        var moveDir = cameraController.PlannerRotation * moveInput;

        GroundCheck();

        if (isGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        if (moveAmount > 0)
        {
            characterController.Move(velocity * Time.deltaTime);
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        anim.SetFloat(moveAmountStr, moveAmount, 0.2f, Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
        Debug.Log($"isGrounded = {isGrounded}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}

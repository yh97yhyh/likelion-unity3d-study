using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �̵� �ӵ�
    [SerializeField] float moveSpeed = 5f;
    // ȸ�� �ӵ�
    [SerializeField] float rotationSpeed = 500f;

    [Header("�׶��� üũ ����")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;


    bool isGrounded;
    // ��ǥ ȸ����
    Quaternion targetRotation;
    float ySpeed;

    // ī�޶� ��Ʈ�ѷ� ����
    CameraController cameraController;
    Animator animator;
    CharacterController characterController;
    MeeleFighter meeleFighter;

    private void Awake()
    {
        // ���� ī�޶󿡼� CameraController ������Ʈ ��������
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        meeleFighter = GetComponent<MeeleFighter>();
    }

    void Update()
    {

        if (meeleFighter.inAction)
        {
            animator.SetFloat("forwardSpeed", 0f);
            return;
        }


        // ����, ���� �Է°� �ޱ�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v)); // ��ü �̵��� ���
        var moveInput = (new Vector3(h, 0, v)).normalized; // �Է� ���� ����ȭ
        var moveDir = cameraController.PlannerRotation * moveInput; // ī�޶� ������ �������� �̵� ���� ���

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

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir); // �̵� �������� ȸ�� ��ǥ ����
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // �ε巯�� ȸ�� ó��

        animator.SetFloat("forwardSpeed", moveAmount, 0.2f, Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }

}

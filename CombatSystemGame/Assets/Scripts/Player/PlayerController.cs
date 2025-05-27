using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 이동 속도
    [SerializeField] float moveSpeed = 5f;
    // 회전 속도
    [SerializeField] float rotationSpeed = 500f;

    [Header("그라운드 체크 설정")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;


    bool isGrounded;
    // 목표 회전값
    Quaternion targetRotation;
    float ySpeed;

    // 카메라 컨트롤러 참조
    CameraController cameraController;
    Animator animator;
    CharacterController characterController;
    MeeleFighter meeleFighter;
    CombatController combatController;

    private void Awake()
    {
        // 메인 카메라에서 CameraController 컴포넌트 가져오기
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        meeleFighter = GetComponent<MeeleFighter>();
        combatController = GetComponent<CombatController>();
    }

    void Update()
    {

        if (meeleFighter.inAction)
        {
            animator.SetFloat("forwardSpeed", 0f);
            return;
        }


        // 수평, 수직 입력값 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v)); // 전체 이동량 계산
        var moveInput = (new Vector3(h, 0, v)).normalized; // 입력 방향 정규화
        var moveDir = cameraController.PlannerRotation * moveInput; // 카메라 방향을 기준으로 이동 방향 계산

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

        if (combatController.CombatMode)
        {
            velocity /= 4f;
            var targetVec = combatController.TargetEnemy.transform.position - transform.position;
            targetVec.y = 0;

            if (moveAmount > 0)
            {
                targetRotation = Quaternion.LookRotation(targetVec);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            float forwardSpeed = Vector3.Dot(velocity, transform.forward);

            animator.SetFloat("forwardSpeed", forwardSpeed / moveSpeed, 0.2f, Time.deltaTime);

            float angle = Vector3.SignedAngle(transform.forward, velocity, Vector3.up);
            float strafeSpeed = Mathf.Sin(angle * Mathf.Deg2Rad);
            animator.SetFloat("strafeSpeed", strafeSpeed, 0.2f, Time.deltaTime);
        }
        else
        {
            if (moveAmount > 0)
            {
                targetRotation = Quaternion.LookRotation(moveDir); // 이동 방향으로 회전 목표 설정
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // 부드러운 회전 처리

            animator.SetFloat("forwardSpeed", moveAmount, 0.2f, Time.deltaTime);
        }
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

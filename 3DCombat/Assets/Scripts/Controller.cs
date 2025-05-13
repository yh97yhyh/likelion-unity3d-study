using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Camera")]
    public Transform camAxisCentral; // 카메라 축
    public Transform cam; // 카메라
    public float camSpeed; // 카메라 회전 속도
    float mouseX;
    float mouseY;
    float mouseWheel;

    [Header("Player")]
    public Transform playerAxis; // 플레이어 축
    public Transform player;
    public float playerSpeed;
    public Vector3 movement;

    Animator anim;
    string walkStr = "Walk";

    void Start()
    {
        mouseWheel = -5;
        mouseY = 3;

        anim = player.GetComponent<Animator>();
    }

    void Update()
    {
        CamMove();
        Zoom();
        PlayerMove();
    }

    void CamMove()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY += Input.GetAxis("Mouse Y") * -1;
        
        if (mouseY > 10)
        {
            mouseY = 10;
        }
        if (mouseY < -5)
        {
            mouseY = -5;
        }

        camAxisCentral.rotation = Quaternion.Euler(
            new Vector3(camAxisCentral.rotation.x + mouseY, camAxisCentral.rotation.y + mouseX, 0) * camSpeed); // 카메라 회전
    }

    void Zoom()
    {
        mouseWheel += Input.GetAxis("Mouse ScrollWheel") * 10;
        if (mouseWheel >= -5)
        {
            mouseWheel = -5;
        }
        if (mouseWheel < -20)
        {
            mouseWheel = -20;
        }

        cam.localPosition = new Vector3(0, 0, mouseWheel); // 카메라 위치
    }

    void PlayerMove()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (movement != Vector3.zero)
        {
            playerAxis.rotation = Quaternion.Euler(
                new Vector3(0, camAxisCentral.rotation.y + mouseX, 0) * camSpeed); // 플레이어 회전
            playerAxis.Translate(movement * playerSpeed * Time.deltaTime); // 플레이어 이동

            player.localRotation = Quaternion.Slerp(player.localRotation, Quaternion.LookRotation(movement), 5 * Time.deltaTime); // 플레이어 회전

            // 애니메이션
            anim.SetBool(walkStr, true);
        }
        else
        {
            anim.SetBool(walkStr, false);
        }

        camAxisCentral.position = new Vector3(player.position.x, player.position.y + 3, player.position.z); // 카메라 위치
    }
}

using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float distance = 5;
    [SerializeField] float minVerticalAngle = -45;
    [SerializeField] float maxVerticalAngle = 45;
    [SerializeField] Vector2 framingOffset;

    float rotationX;
    float rotationY;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        //rotationX += Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationX -= Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed;

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y, 0);

        transform.position = focusPosition - targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;
    }

    public Quaternion PlannerRotation => Quaternion.Euler(0, rotationY, 0);
}

//public class CameraController : MonoBehaviour
//{
//    [SerializeField] Transform followTarget;
//    [SerializeField] float rotationSpeed = 2f;
//    [SerializeField] float distance = 5;
//    [SerializeField] float minVerticalAngle = -45;
//    [SerializeField] float maxVerticalAngle = 45;
//    [SerializeField] Vector2 framingOffset;
//    [SerializeField] bool invertX;
//    [SerializeField] bool invertY;

//    float rotationX;
//    float rotationY;
//    float invertXVal;
//    float invertYVal;


//    private void Start()
//    {
//        Cursor.visible = false;
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    private void Update()
//    {

//        invertXVal = (invertX) ? -1 : 1;
//        invertYVal = (invertY) ? -1 : 1;
//        rotationX += Input.GetAxis("Mouse Y") * invertYVal * rotationSpeed;
//        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
//        rotationY += Input.GetAxis("Mouse X") * invertXVal * rotationSpeed;

//        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
//        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y, 0);

//        transform.position = focusPosition - targetRotation * new Vector3(0, 0, distance);
//        transform.rotation = targetRotation;
//    }
//}
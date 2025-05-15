using UnityEngine;

public class DisableObj : MonoBehaviour
{
    public float dTime;
    private void OnEnable()
    {
        CancelInvoke();
        Invoke("Disable", dTime);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}

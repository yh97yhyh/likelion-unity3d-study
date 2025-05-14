using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance;

    [Header("Time Settings")]
    public float stopTime = 0.1f;
    public float timeScaleRecoverySpeed = 10f;

    [Header("Camera Shake")]
    [SerializeField] private Transform shakeCam;
    public float shakeIntensity = 0.1f;
    public float shakeFrequency = 0.1f;

    private bool isHitStopped;
    private Vector3 originalCamPosition;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StopTime(float intensity = 1f)
    {
        if (!isHitStopped)
        {
            isHitStopped = true;
            Time.timeScale = 0f;

            if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);

            shakeCoroutine = StartCoroutine(ShakeCamera(intensity));
            StartCoroutine(ReturnTimeScale());
        }
    }

    private IEnumerator ShakeCamera(float intensity)
    {
        originalCamPosition = shakeCam.localPosition;
        float elapsed = 0f;

        while (elapsed < stopTime)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity * intensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity * intensity;

            shakeCam.localPosition = new Vector3(x, y, originalCamPosition.z);

            elapsed += Time.unscaledDeltaTime;
            yield return new WaitForSecondsRealtime(shakeFrequency);
        }

        shakeCam.localPosition = originalCamPosition;
    }

    private IEnumerator ReturnTimeScale()
    {
        yield return new WaitForSecondsRealtime(stopTime);

        while (Time.timeScale < 1f)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1f, Time.unscaledDeltaTime * timeScaleRecoverySpeed);
            yield return null;
        }

        Time.timeScale = 1f;
        isHitStopped = false;

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
    }

    public void SetNormalAttack()
    {
        stopTime = 0.1f;
        timeScaleRecoverySpeed = 10;
        shakeFrequency = 0.2f;
        shakeIntensity = 0.2f;
    }

    public void SetSmashAttack()
    {
        stopTime = 0.3f;
        timeScaleRecoverySpeed = 5;
        shakeFrequency = 0.3f;
        shakeIntensity = 0.3f;
    }
}
//public class HitStop : MonoBehaviour
//{
//    [Header("Time Setting")]
//    [SerializeField] private float stopTime = 0.1f;

//    bool stop;
//    public float stopTime;

//    public Transform shakeCam;
//    public Vector3 shake;

//    public void StopTime()
//    {
//        if (!stop)
//        {
//            stop = true;
//            shakeCam.localPosition = shake;
//            Time.timeScale = 0f;

//            StartCoroutine("ReturnTimeScale");
//        }
//    }

//    IEnumerator ReturnTimeScale()
//    {
//        yield return new WaitForSecondsRealtime(stopTime);
//        Time.timeScale = 1;
//        shakeCam.localPosition = Vector3.zero;
//        stop = false;
//    }
//}

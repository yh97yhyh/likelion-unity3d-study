using UnityEngine;

public class CombatController : MonoBehaviour
{
    MeeleFighter meeleFighter;
    Animator anim;
    CameraController cam;

    public EnemyController targetEnemy;

    private void Awake()
    {
        meeleFighter = GetComponent<MeeleFighter>();
        anim = GetComponent<Animator>();
        cam = Camera.main.GetComponent<CameraController>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var enemy = EnemyManager.Instance.GetAttackingEnemy();
            if (enemy != null && enemy.Fighter.IsCounterable && !meeleFighter.inAction)
            {
                //StartCoroutine(meeleFighter.PerformCounterAttack(enemy));
                StartCoroutine(meeleFighter.PerformCounterAttack(enemy));
            }
            else
            {
                meeleFighter.TryAttack();
            }
        }
    }

    public Vector3 GetTargetingDir()
    {
        var vecFromCam = transform.position - cam.transform.position;
        vecFromCam.y = 0f;
        return vecFromCam.normalized;
    }



    //private void OnAnimatorMove()
    //{
    //    if (!meeleFighter.inCounter)
    //    {
    //        transform.position += anim.deltaPosition;
    //    }
        
    //    transform.rotation *= anim.deltaRotation;
    //}
}

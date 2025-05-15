using UnityEngine;

public class Minotaurs : MonoBehaviour
{
    public Animator anim;
    public Transform target;
    public float minoSpeed;
    bool enableAct;
    int atkStep;
    Vector3 dir;

    string walkStr = "Walk";
    string attack1Str = "attack1";
    string attack2Str = "attack2";
    string attack3Str = "attack3";

    void Start()
    {
        anim = GetComponent<Animator>();
        enableAct = true;
    }

    void Update()
    {
        if (enableAct)
        {
            RotateMino();
            MoveMino();
        }
    }

    void RotateMino()
    {
        dir = target.position - transform.position;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);
    }

    void MoveMino()
    {
        if (dir.magnitude >= 1.5)
        {
            anim.SetBool(walkStr, true);
            transform.Translate(Vector3.forward * minoSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            anim.SetBool(walkStr, false);
        }
    }

    void AttackMino()
    {
        if (dir.magnitude < 1.5)
        {
            switch (atkStep)
            {
                case 0:
                    atkStep += 1;
                    anim.Play(attack1Str);
                    break;
                case 1:
                    atkStep += 1;
                    anim.Play(attack2Str);
                    break;
                case 2:
                    atkStep = 0;
                    anim.Play(attack3Str);
                    break;
                default:
                    break;
            }
        }
    }

    void FreezeMino()
    {
        enableAct = false;
    }

    void UnFreezeMino()
    {
        enableAct = true;
    }
}

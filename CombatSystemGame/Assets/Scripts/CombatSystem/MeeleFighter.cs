using System.Collections;
using UnityEngine;

public enum AttackState
{
    Idle,
    Windup,
    Impact,
    Cooldown
}

public class MeeleFighter : MonoBehaviour
{
    [SerializeField] GameObject sword;
    BoxCollider swordCollider;

    Animator anim;
    public bool inAction { get; set; } = false;
    AttackState attackState;

    string slashStr = "Slash1";
    string swordImapctStr = "SwordImpact";
    string hitBoxTag = "HitBox";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (sword != null)
        {
            swordCollider = sword.GetComponent<BoxCollider>();
            swordCollider.enabled = false;
        }
    }

    public void TryAttack()
    {
        if (!inAction)
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(hitBoxTag) && !inAction)
        {
            StartCoroutine(PlayHitReaction());
        }
    }

    IEnumerator Attack()
    {
        inAction = true;
        attackState = AttackState.Windup;
        float impactStartTime = 0.33f;
        float impactEndTime = 0.55f;

        anim.CrossFade(slashStr, 0.2f); // ���� �ִϸ��̼� �ð��� 20%��ŭ ���� 
        yield return null; // 1������ null�� �Ѿ�� 
        var animState = anim.GetNextAnimatorStateInfo(1);

        float timer = 0f;
        while (timer <= animState.length)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / animState.length;

            if (attackState == AttackState.Windup)
            {
                if (normalizedTime >= impactStartTime)
                {
                    attackState = AttackState.Impact;
                    swordCollider.enabled = true;
                }
            }
            else if (attackState == AttackState.Impact)
            {
                if (normalizedTime >= impactEndTime)
                {
                    attackState = AttackState.Cooldown;
                    swordCollider.enabled = false;
                }
            }
            else if (attackState == AttackState.Cooldown)
            {
                // �޺�
            }

            yield return null;
        }

        attackState = AttackState.Idle;

        //yield return new WaitForSeconds(animState.length); // �ִϸ��̼� ���� ������ ��� 

        inAction = false;
    }

    IEnumerator PlayHitReaction()
    {
        inAction = true;

        anim.CrossFade(swordImapctStr, 0.2f); // ���� �ִϸ��̼� �ð��� 20%��ŭ ���� 
        yield return null; // 1������ null�� �Ѿ�� 
        var animState = anim.GetNextAnimatorStateInfo(1);
        yield return new WaitForSeconds(animState.length);

        inAction = false;
    }
}


using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] List<AttackData> attacks;
    [SerializeField] GameObject sword;

    BoxCollider swordCollider;
    SphereCollider leftHandCollider, rightHandCollider, leftFootCollider, rightFootCollider;

    Animator anim;
    public bool inAction { get; set; } = false;
    AttackState attackState;
    bool doCombo;
    int comboCount = 0;

    string swordImapctAnim = "SwordImpact";
    string hitBoxTag = "HitBox";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (sword != null)
        {
            InitCollider();
        }
    }

    void InitCollider()
    {
        swordCollider = sword.GetComponent<BoxCollider>();
        leftHandCollider = anim.GetBoneTransform(HumanBodyBones.LeftHand).GetComponent<SphereCollider>();
        rightHandCollider = anim.GetBoneTransform(HumanBodyBones.RightHand).GetComponent<SphereCollider>();
        leftFootCollider = anim.GetBoneTransform(HumanBodyBones.LeftFoot).GetComponent<SphereCollider>();
        rightFootCollider = anim.GetBoneTransform(HumanBodyBones.RightFoot).GetComponent<SphereCollider>();
        swordCollider.enabled = false;
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;
        leftFootCollider.enabled = false;
        rightFootCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(hitBoxTag) && !inAction)
        {
            StartCoroutine(PlayHitReaction());
        }
    }

    public void TryAttack()
    {
        if (!inAction)
        {
            StartCoroutine(Attack());
        }
        else if (attackState == AttackState.Impact || attackState == AttackState.Cooldown)
        {
            doCombo = true;
        }
    }


    IEnumerator Attack()
    {
        inAction = true;
        attackState = AttackState.Windup;

        anim.CrossFade(attacks[comboCount].AnimName, 0.2f); // 현재 애니메이션 시간의 20%만큼 블렌드 
        yield return null; // 1프레임 null로 넘어가기 
        var animState = anim.GetNextAnimatorStateInfo(1);

        float timer = 0f;
        while (timer <= animState.length)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / animState.length;

            var attack = attacks[comboCount];

            if (attackState == AttackState.Windup)
            {
                if (normalizedTime >= attack.ImpactStartTime)
                {
                    attackState = AttackState.Impact;
                    SetHitBox(true, attack);
                }
            }
            else if (attackState == AttackState.Impact)
            {
                if (normalizedTime >= attack.ImpactEndTime)
                {
                    attackState = AttackState.Cooldown;
                    SetHitBox(false, attack);
                }
            }
            else if (attackState == AttackState.Cooldown)
            {
                if (doCombo)
                {
                    doCombo = false;
                    comboCount = (comboCount + 1) % attacks.Count;
                    StartCoroutine(Attack());
                    yield break;
                }
            }

            yield return null;
        }

        attackState = AttackState.Idle;
        comboCount = 0;
        inAction = false;
    }

    void SetHitBox(bool enable, AttackData attackData)
    {
        switch (attackData.HitboxToUse)
        {
            case AttackHitbox.LeftHand:
                leftHandCollider.enabled = enable;
                break;
            case AttackHitbox.RightHand:
                rightHandCollider.enabled = enable;
                break;
            case AttackHitbox.LeftFoot:
                leftFootCollider.enabled = enable;
                break;
            case AttackHitbox.RightFoot:
                rightFootCollider.enabled = enable;
                break;
            case AttackHitbox.Sword:
                swordCollider.enabled = enable;
                break;
        }
    }

    IEnumerator PlayHitReaction()
    {
        inAction = true;

        anim.CrossFade(swordImapctAnim, 0.2f); // 현재 애니메이션 시간의 20%만큼 블렌드 
        yield return null; // 1프레임 null로 넘어가기 
        var animState = anim.GetNextAnimatorStateInfo(1);
        yield return new WaitForSeconds(animState.length * 0.8f);

        inAction = false;
    }
}

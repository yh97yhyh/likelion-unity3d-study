
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState
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
    public CombatState combatState;
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
        DisableAllHitBox();
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
        else if (combatState == CombatState.Impact || combatState == CombatState.Cooldown)
        {
            doCombo = true;
        }
    }


    IEnumerator Attack()
    {
        inAction = true;
        combatState = CombatState.Windup;

        anim.CrossFade(attacks[comboCount].AnimName, 0.2f); // 현재 애니메이션 시간의 20%만큼 블렌드 
        yield return null; // 1프레임 null로 넘어가기 
        var animState = anim.GetNextAnimatorStateInfo(1);

        float timer = 0f;
        while (timer <= animState.length)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / animState.length;

            var attack = attacks[comboCount];

            if (combatState == CombatState.Windup)
            {
                if (normalizedTime >= attack.ImpactStartTime)
                {
                    combatState = CombatState.Impact;
                    EnableHitBox(attack);
                }
            }
            else if (combatState == CombatState.Impact)
            {
                if (normalizedTime >= attack.ImpactEndTime)
                {
                    combatState = CombatState.Cooldown;
                    DisableAllHitBox();
                }
            }
            else if (combatState == CombatState.Cooldown)
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

        combatState = CombatState.Idle;
        comboCount = 0;
        inAction = false;
    }

    void SetHitBox(bool enable, AttackData attackData)
    {
        switch (attackData.HitboxToUse)
        {
            case AttackHitbox.LeftHand:
                if (swordCollider != null)
                    leftHandCollider.enabled = enable;
                break;
            case AttackHitbox.RightHand:
                if (swordCollider != null)
                    rightHandCollider.enabled = enable;
                break;
            case AttackHitbox.LeftFoot:
                if (swordCollider != null)
                    leftFootCollider.enabled = enable;
                break;
            case AttackHitbox.RightFoot:
                if (swordCollider != null)
                    rightFootCollider.enabled = enable;
                break;
            case AttackHitbox.Sword:
                if (swordCollider != null)
                    swordCollider.enabled = enable;
                break;
        }
    }

    void EnableHitBox(AttackData attack)
    {
        switch (attack.HitboxToUse)
        {
            case AttackHitbox.LeftHand:
                leftHandCollider.enabled = true;
                break;
            case AttackHitbox.RightHand:
                rightHandCollider.enabled = true;
                break;
            case AttackHitbox.LeftFoot:
                leftFootCollider.enabled = true;
                break;
            case AttackHitbox.RightFoot:
                rightFootCollider.enabled = true;
                break;
            case AttackHitbox.Sword:
                swordCollider.enabled = true;
                break;
            default:
                break;
        }
    }

    void DisableAllHitBox()
    {
        if (swordCollider != null)
            swordCollider.enabled = false;

        if (leftHandCollider != null)
            leftHandCollider.enabled = false;
        if (rightHandCollider != null)
            rightHandCollider.enabled = false;
        if (leftFootCollider != null)
            leftFootCollider.enabled = false;
        if (rightFootCollider != null)
            rightFootCollider.enabled = false;
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

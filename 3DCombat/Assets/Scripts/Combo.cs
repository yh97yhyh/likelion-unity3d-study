using UnityEngine;

public class Combo : MonoBehaviour
{
    Animator playerAnim;
    bool comboPossible;
    public int comboStep; // �޺� �ܰ�
    bool inputSmash;

    public GameObject hitBox;

    string combo1Str = "ARPG_Samurai_Attack_Combo2";
    string combo2Str = "ARPG_Samurai_Attack_Combo3";
    string combo3Str = "ARPG_Samurai_Attack_Combo4";
    string smash1Str = "ARPG_Samurai_Attack_Sprint";
    string smash2Str = "ARPG_Samurai_Attack_Heavy2";
    string smash3_1Str = "ARPG_Samurai_Attack_Heavy1_Start";
    string smash3_2Str = "ARPG_Samurai_Attack_Heavy1";
    string parryStr = "ARPG_Samurai_Parry";

    void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NormalAttack();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            SmashAttack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnim.Play(parryStr);
        }
    }

    void NormalAttack()
    {
        if (comboStep == 0)
        {
            playerAnim.Play(combo1Str);
            comboStep = 1;
            return;
        }
        else
        {
            if (comboPossible)
            {
                comboPossible = false;
                comboStep += 1;
            }
        }
    }

    public void NextAttack()
    {
        if (!inputSmash) // �⺻ ���� 
        {
            HitStop.Instance.SetNormalAttack();

            if (comboStep == 2)
            {
                playerAnim.Play(combo2Str);
            }
            else if (comboStep == 3)
            {
                playerAnim.Play(combo3Str);
            }
        }
        else
        {
            HitStop.Instance.SetSmashAttack();

            if (comboStep == 1)
            {
                playerAnim.Play(smash1Str);
            }
            else if (comboStep == 2)
            {
                playerAnim.Play(smash2Str);
            }
            else if (comboStep == 3)
            {
                playerAnim.Play(smash3_1Str);
            }
        }
    }

    void SmashAttack()
    {
        if (comboPossible)
        {
            comboPossible = false;
            inputSmash = true;
        }
    }

    void ComboPossible()
    {
        comboPossible = true;
    }

    public void ResetCombo()
    {
        comboPossible = false;
        comboStep = 0;
        inputSmash = false;
    }

    void ChangeHitBoxTag(string t)
    {
        hitBox.tag = t;
    }
}

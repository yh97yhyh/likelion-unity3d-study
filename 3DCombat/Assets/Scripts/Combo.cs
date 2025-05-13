using UnityEngine;

public class Combo : MonoBehaviour
{
    Animator playerAnim;
    bool comboPossible;
    public int comboStep; // 콤보 단계
    bool inputSmash;

    public GameObject hitBox;

    string combo1Str = "ARPG_Samurai_Attack_Combo2";
    string combo2Str = "ARPG_Samurai_Attack_Combo3";
    string combo3Str = "ARPG_Samurai_Attack_Combo4";

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
            // 방어
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
        if (!inputSmash) // 기본 공격 
        {
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
            if (comboStep == 1)
            {
                //playerAnim.Play("");
            }
            else if (comboStep == 2)
            {
                //playerAnim.Play("");
            }
            else if (comboStep == 3)
            {
                //playerAnim.Play("");
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

using TMPro;
using UnityEngine;

public class HitBox_Player : MonoBehaviour
{
    public Animator playerAnim;
    public TextMeshProUGUI message;

    string counterStr = "Samurai_Attack_HeavyB";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Col_EnemyAtk"))
        {
            if (gameObject.CompareTag("HitBox_Player"))
            {
                // 데미지 입은 애니메이션
                message.text = "Player took damage";
                message.gameObject.SetActive(true);
            }
            else if (gameObject.CompareTag("Defence"))
            {
                message.text = "Block";
                message.gameObject.SetActive(true);
            }
            else if (gameObject.CompareTag("Parrying"))
            {
                playerAnim.Play(counterStr);
                message.text = "Parrying";
                message.gameObject.SetActive(true);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class HP_Player : MonoBehaviour
{
    public float hp;
    public float hp_Cur;

    public Image hpBar;

    public GameObject player;
    public Animator playerAnim;

    string deathStr = "ARPG_Samurai_Death";

    void Start()
    {
        hp_Cur = hp;
    }

    void Update()
    {
        SyncBar();
    }

    void SyncBar()
    {
        hpBar.fillAmount = hp_Cur / hp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Col_EnemyAtk"))
        {
            hp_Cur -= Random.Range(100, 500);

            if (hp_Cur <= 0)
            {
                playerAnim.Play(deathStr);
                GameObject go = GameObject.FindGameObjectWithTag("HPBar_Player");
                Destroy(player, 2);
            }
        }
    }
}

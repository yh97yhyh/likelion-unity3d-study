using UnityEngine;
using UnityEngine.UI;

public class HP_Mino : MonoBehaviour
{
    public float hp;
    public float hp_Cur;

    public Image hpBar_Front;
    public Image hpBar_Back;

    public GameObject mino;

    string deathStr = "death";

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
        hpBar_Front.fillAmount = hp_Cur / hp;

        if (hpBar_Back.fillAmount > hpBar_Front.fillAmount)
        {
            hpBar_Back.fillAmount = Mathf.Lerp(hpBar_Back.fillAmount, hpBar_Front.fillAmount, Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Col_PlayerAtk"))
        {
            hp_Cur -= Random.Range(100, 500);

            if (hp_Cur <= 0)
            {
                mino.GetComponent<Minotaurs>().anim.Play(deathStr);
                GameObject go = GameObject.FindGameObjectWithTag("HPBar_Boss");
                if (go != null)
                {
                    go.SetActive(false);
                }
                Destroy(mino, 2);
            }
        }
    }
}

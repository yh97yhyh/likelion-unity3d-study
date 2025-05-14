using TMPro;
using UnityEngine;

public class Col_PlayerAttk : MonoBehaviour
{
    public Combo combo;
    public string type_Attk;

    int comboStep;
    public string dmg;
    public TextMeshProUGUI dmgText;

    private void OnEnable() // ������Ʈ Ȱ��ȭ�Ǹ�
    {
        comboStep = combo.comboStep;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HitBox_Enemy"))
        {
            dmg = $"{type_Attk} {comboStep}";
            dmgText.text = dmg;
            dmgText.gameObject.SetActive(true);
            HitStop.Instance.StopTime();
        }
    }
}

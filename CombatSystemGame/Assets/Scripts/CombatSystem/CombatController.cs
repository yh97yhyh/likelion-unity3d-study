using UnityEngine;

public class CombatController : MonoBehaviour
{
    MeeleFighter meeleFighter;

    private void Awake()
    {
        meeleFighter = GetComponent<MeeleFighter>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            meeleFighter.TryAttack();
        }
    }
}

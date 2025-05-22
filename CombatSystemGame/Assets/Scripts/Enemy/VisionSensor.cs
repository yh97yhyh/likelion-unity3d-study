using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    [SerializeField] EnemyController enemy;

    private void OnTriggerEnter(Collider other)
    {
        var fighter = other.GetComponent<MeeleFighter>();

        if (fighter != null)
        {
            enemy.TargetsInRange.Add(fighter);
            EnemyManager.Instance.AddEnemyInRange(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var fighter = other.GetComponent<MeeleFighter>();

        if (fighter != null)
        {
            enemy.TargetsInRange.Remove(fighter);
            EnemyManager.Instance.RemoveEnemyInRange(enemy);
        }
    }
}

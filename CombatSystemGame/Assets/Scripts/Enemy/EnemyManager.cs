using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [SerializeField] Vector2 timeRangeBetweenAttacks = new Vector2(1, 4);

    public List<EnemyController> enemiesInRange = new List<EnemyController>();
    float notAttackingTimer = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (enemiesInRange.Count == 0)
        {
            return;
        }

        if (!enemiesInRange.Any(e => e.IsInState(EnemyState.Attack))) // 공격상태 아무도 없을 때
        {
            if (notAttackingTimer > 0)
            {
                notAttackingTimer -= Time.deltaTime;
            }
            else
            {
                // 공격
                var attackingEnemy = SelectEnemyForAttack();
                attackingEnemy.ChangeState(EnemyState.Attack);
                notAttackingTimer = Random.Range(timeRangeBetweenAttacks.x, timeRangeBetweenAttacks.y);
            }
        }
    }

    EnemyController SelectEnemyForAttack()
    {
        return enemiesInRange.OrderByDescending(e => e.CombatMovementTimer).FirstOrDefault();
    }

    public void AddEnemyInRange(EnemyController enemy)
    {
        if (!enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    public void RemoveEnemyInRange(EnemyController enemy)
    {
        enemiesInRange.Remove(enemy);
    }
}

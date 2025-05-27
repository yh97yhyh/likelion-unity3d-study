using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [SerializeField] Vector2 timeRangeBetweenAttacks = new Vector2(1, 4);
    [SerializeField] CombatController player;

    public List<EnemyController> enemiesInRange = new List<EnemyController>();
    float notAttackingTimer = 2f;
    float targetingTimer;

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
                
                if (attackingEnemy != null)
                {
                    attackingEnemy.ChangeState(EnemyState.Attack);
                    notAttackingTimer = Random.Range(timeRangeBetweenAttacks.x, timeRangeBetweenAttacks.y);
                }
            }
        }

        if (targetingTimer > 0.1f)
        {
            targetingTimer = 0f;
            var closestEnemy = GetClosesEnemyToPlayerDir();
            if (closestEnemy != null && closestEnemy != player.TargetEnemy)
            {
                var prevEnemy = player.TargetEnemy;
                player.TargetEnemy = closestEnemy;
                player.TargetEnemy?.MeshHighligher.HighlightMesh(true);
                prevEnemy?.MeshHighligher.HighlightMesh(false);
            }
        }
        targetingTimer += Time.deltaTime;
    }

    EnemyController SelectEnemyForAttack()
    {
        return enemiesInRange.OrderByDescending(e => e.CombatMovementTimer).FirstOrDefault(e => e.Target != null);
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

        if (enemy == player.TargetEnemy)
        {
            enemy.MeshHighligher?.HighlightMesh(false);
            player.TargetEnemy = GetClosesEnemyToPlayerDir();
            player.TargetEnemy?.MeshHighligher?.HighlightMesh(true);
        }
    }

    public EnemyController GetAttackingEnemy()
    {
        return enemiesInRange.FirstOrDefault(e => e.IsInState(EnemyState.Attack));
    }

    public EnemyController GetClosesEnemyToPlayerDir()
    {
        var targetingDir = player.GetTargetingDir();

        float minDistance = Mathf.Infinity;
        EnemyController closestEnemy = null;

        foreach(var enemy in enemiesInRange)
        {
            var vecToEnemy = enemy.transform.position - player.transform.position;
            vecToEnemy.y = 0;

            float angle = Vector3.Angle(targetingDir, vecToEnemy);
            float distance = vecToEnemy.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}

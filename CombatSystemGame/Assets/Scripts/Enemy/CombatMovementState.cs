using UnityEngine;

public enum AICombatState
{ 
    Idle, 
    Chase, 
    Circling 
}


public class CombatMovementState : State<EnemyController>
{
    [SerializeField] float ciclingSpeed = 20f;
    [SerializeField] float distanceToStand = 3f;
    [SerializeField] float adjustDistanceThreshold = 1f;
    [SerializeField] Vector2 idleTimeRange = new Vector2(2, 5);
    [SerializeField] Vector2 circlingTimeRange = new Vector2(3, 6);

    float timer = 0f;

    int circlingDir = 1;

    AICombatState state;

    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;

        enemy.NavAgent.stoppingDistance = distanceToStand;
        enemy.CombatMovementTimer = 0f;
    }

    public override void Execute()
    {

        if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position) > distanceToStand + adjustDistanceThreshold)
        {
            StartChase();
        }

        if (state == AICombatState.Idle)
        {
            if (timer <= 0)
            {
                if (Random.Range(0, 2) == 0)
                {
                    StartIdle();
                }
                else
                {
                    StartCircling();
                }
            }
        }
        else if (state == AICombatState.Chase)
        {
            if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position) <= distanceToStand + 0.03f)
            {
                StartIdle();
                return;
            }
            enemy.NavAgent.SetDestination(enemy.Target.transform.position);
        }
        else if (state == AICombatState.Circling)
        {
            if (timer <= 0)
            {
                StartIdle();
                return;
            }

            var vecToTarget = enemy.transform.position - enemy.Target.transform.position;
            var rotatePos = Quaternion.Euler(0, ciclingSpeed * circlingDir * Time.deltaTime, 0) * vecToTarget;
            enemy.NavAgent.Move(rotatePos - vecToTarget);
            enemy.transform.rotation = Quaternion.LookRotation(-rotatePos);
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }

        enemy.CombatMovementTimer += Time.deltaTime;
    }

    void StartCircling()
    {
        state = AICombatState.Circling;
        enemy.NavAgent.ResetPath();
        timer = Random.Range(circlingTimeRange.x, circlingTimeRange.y);
        circlingDir = Random.Range(0, 2) == 0 ? 1 : -1;
    }

    void StartChase()
    {
        state = AICombatState.Chase;
        enemy.Anim.SetBool("combatMode", false);

    }

    void StartIdle()
    {
        state = AICombatState.Idle;
        timer = Random.Range(idleTimeRange.x, idleTimeRange.y);
        enemy.Anim.SetBool("combatMode", true);
    }


    public override void Exit()
    {
        enemy.CombatMovementTimer = 0f;
    }


}

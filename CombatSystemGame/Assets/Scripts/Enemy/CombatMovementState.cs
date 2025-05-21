using System.Threading;
using UnityEngine;

public enum AICombatState
{
    Idle,
    Chase,
    Circling
}

public class CombatMovementState : State<EnemyController>
{
    EnemyController enemy;
    AICombatState state;

    [SerializeField] float circlingSpeed = 20f;
    [SerializeField] float distanceToStand = 3f;
    [SerializeField] float adjustDistanceThreshold = 1f;
    [SerializeField] Vector2 idleTimeRange = new Vector2(2, 5);
    [SerializeField] Vector2 circlingTimeRange = new Vector2(3, 6);
    float timer = 0f;
    int circlingDir = 1;

    public override void Enter(EnemyController _owner)
    {
        base.Enter(_owner);

        enemy = _owner;
        enemy.NavAgent.stoppingDistance = distanceToStand;
    }

    public override void Execute()
    {
        base.Execute();

        if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position) > distanceToStand + adjustDistanceThreshold)
        {
            StartChase();
        }

        switch (state)
        {
            case AICombatState.Idle:
                if (timer <= 0f)
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
                break;
            case AICombatState.Chase:
                if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position) <= distanceToStand + 0.03f)
                {
                    StartIdle();
                    return;
                }
                enemy.NavAgent.SetDestination(enemy.Target.transform.position);
                break;
            case AICombatState.Circling:
                if (timer <= 0)
                {
                    StartIdle();
                    return;
                }
                transform.RotateAround(enemy.Target.transform.position, Vector3.up, circlingSpeed * circlingDir * Time.deltaTime);
                break;
        }

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }

        //enemy.Anim.SetFloat(enemy.moveAmountStr, enemy.NavAgent.velocity.magnitude / enemy.NavAgent.speed);
    }

    void StartIdle()
    {
        state = AICombatState.Idle;
        timer = Random.Range(idleTimeRange.x, idleTimeRange.y);
        enemy.Anim.SetBool("combatMode", true);
        enemy.Anim.SetBool("Circling", false);
    }

    void StartChase()
    {
        state = AICombatState.Chase;
        enemy.Anim.SetBool("combatMode", false);
        enemy.Anim.SetBool("Circling", false);
    }

    void StartCircling()
    {
        state = AICombatState.Circling;
        timer = Random.Range(circlingTimeRange.x, circlingTimeRange.y);
        circlingDir = Random.Range(0, 2) == 0 ? 1 : -1;
        enemy.Anim.SetBool("Circling", true);
        enemy.Anim.SetFloat("circlingDir", circlingDir);
    }

    public override void Exit()
    {
        base.Exit();

    }
}

using UnityEngine;

public class IdleState : State<EnemyController>
{
    EnemyController enemy;

    public override void Enter(EnemyController _owner)
    {
        base.Enter(_owner);

        enemy = _owner;
    }

    public override void Execute()
    {
        base.Execute();

        foreach (var target in enemy.TargetsInRange)
        {
            var vecToTarget = target.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, vecToTarget);

            if (angle <= enemy.Fov / 2)
            {
                enemy.Target = target;
                enemy.ChangeState(EnemyState.CombatMovement);
                break;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

    }
}

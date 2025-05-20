using UnityEngine;

public class ChaseState : State<EnemyController>
{
    EnemyController enemy;

    public override void Enter(EnemyController _owner)
    {
        base.Enter(_owner);
        Debug.Log("ChaseState Enter");

        enemy = _owner;
    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log("ChaseState Execute");

        if (Input.GetKeyDown(KeyCode.T))
        {
            enemy.ChangeState(EnemyState.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("ChaseState Exit");
    }
}

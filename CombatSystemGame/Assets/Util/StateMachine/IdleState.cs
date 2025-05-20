using UnityEngine;

public class IdleState : State<EnemyController>
{
    EnemyController enemy;

    public override void Enter(EnemyController _owner)
    {
        base.Enter(_owner);
        Debug.Log("IdleState Enter");

        enemy = _owner;
    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log("IdleState Execute");

        if (Input.GetKeyDown(KeyCode.T))
        {
            enemy.ChangeState(EnemyState.Chase);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("IdleState Exit");
    }
}

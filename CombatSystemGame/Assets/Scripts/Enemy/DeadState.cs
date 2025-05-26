using UnityEngine;

public class DeadState : State<EnemyController>
{
    public override void Enter(EnemyController _owner)
    {
        base.Enter(_owner);

        _owner.VSeonsor.gameObject.SetActive(false);
        EnemyManager.Instance.RemoveEnemyInRange(_owner);
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
    }
}

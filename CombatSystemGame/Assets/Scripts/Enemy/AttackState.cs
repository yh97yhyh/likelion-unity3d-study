using System.Collections;
using UnityEngine;

public class AttackState : State<EnemyController>
{
    [SerializeField] float attackDistance = 1f;

    bool isAttacking;
    EnemyController enemy;

    public override void Enter(EnemyController _owner)
    {
        base.Enter(_owner);

        enemy = _owner;
        enemy.NavAgent.stoppingDistance = attackDistance;
    }

    public override void Execute()
    {
        base.Execute();

        if (isAttacking)
        {
            return;
        }

        enemy.NavAgent.SetDestination(enemy.Target.transform.position);

        if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position) <= attackDistance + 0.03f)
        {
            StartCoroutine(Attack());
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        enemy.Anim.applyRootMotion = true;
        enemy.Fighter.TryAttack();

        yield return new WaitUntil(() => enemy.Fighter.combatState == CombatState.Idle);

        enemy.Anim.applyRootMotion = false;
        isAttacking = false;
    }
}

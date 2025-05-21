using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    CombatMove
}

public class EnemyController : MonoBehaviour
{
    public StateMachine<EnemyController> StateMachine { get; private set; }
    Dictionary<EnemyState, State<EnemyController>> stateDict;

    public Animator Anim { get; private set; }
    public string moveAmountStr = "moveAmount";

    public List<MeeleFighter> TargetsInRange { get; private set; }
    public MeeleFighter Target { get; set; }
    public float Fov { get; private set; } = 180f;
    public NavMeshAgent NavAgent { get; private set; }

    private void Start()
    {
        stateDict = new Dictionary<EnemyState, State<EnemyController>>();
        stateDict[EnemyState.Idle] = GetComponent<IdleState>();
        stateDict[EnemyState.CombatMove] = GetComponent<CombatMovementState>();

        Anim = GetComponent<Animator>();
        StateMachine = new StateMachine<EnemyController>(this);
        StateMachine.ChangeState(stateDict[EnemyState.Idle]);
        TargetsInRange = new List<MeeleFighter>();
        NavAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        StateMachine.Execute();
        Anim.SetFloat(moveAmountStr, NavAgent.velocity.magnitude / NavAgent.speed);
    }

    public void ChangeState(EnemyState state)
    {
        StateMachine.ChangeState(stateDict[state]);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState { 
    Idle, 
    CombatMovement,
    Attack,
    RetreatAfterAttack
}

public class EnemyController : MonoBehaviour
{
    [field: SerializeField] public float Fov { get; private set; } = 180f;
    public List<MeeleFighter> TargetsInRange { get; private set; } = new List<MeeleFighter>();
    public MeeleFighter Target { get; set; }
    public float CombatMovementTimer { get; set; } = 0f;

    public StateMachine<EnemyController> StateMachine { get; private set; }
    Dictionary<EnemyState, State<EnemyController>> stateDict;
    public NavMeshAgent NavAgent { get; private set; }
    public Animator Anim { get; private set; }
    public MeeleFighter Fighter { get; private set; }
    Vector3 prevPos;

    private void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        Fighter = GetComponent<MeeleFighter>();

        stateDict = new Dictionary<EnemyState, State<EnemyController>>();
        stateDict[EnemyState.Idle] = GetComponent<IdleState>();
        stateDict[EnemyState.CombatMovement] = GetComponent<CombatMovementState>();
        stateDict[EnemyState.Attack] = GetComponent<AttackState>();
        stateDict[EnemyState.RetreatAfterAttack] = GetComponent<RetreatAfterAttackState>();

        StateMachine = new StateMachine<EnemyController>(this);
        StateMachine.ChangeState(stateDict[EnemyState.Idle]);
    }

    public void ChangeState(EnemyState state)
    {
        StateMachine.ChangeState(stateDict[state]);
    }

    public bool IsInState(EnemyState state)
    {
        return StateMachine.CurrentState == stateDict[state];
    }

    private void Update()
    {
        StateMachine.Execute();

        //var deltaPos = transform.position - prevPos;
        var deltaPos = Anim.applyRootMotion ? Vector3.zero : transform.position - prevPos;
        var velocity = deltaPos / Time.deltaTime;

        float forwardSpeed = Vector3.Dot(velocity, transform.forward);

        Anim.SetFloat("forwardSpeed", forwardSpeed / NavAgent.speed, 0.2f, Time.deltaTime);

        float angle = Vector3.SignedAngle(transform.forward, velocity, Vector3.up);
        float strafeSpeed = Mathf.Sin(angle * Mathf.Deg2Rad);
        Anim.SetFloat("strafeSpeed", strafeSpeed, 0.2f, Time.deltaTime);

        prevPos = transform.position;
    }
}

using UnityEngine;

public class StateMachine<T>
{
    public State<T> CurrentState { get; private set; }

    T owner;

    public StateMachine(T _onwer)
    {
        owner = _onwer;
    }

    public void ChangeState(State<T> newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter(owner);
    }

    public void Execute()
    {
        CurrentState?.Execute();
    }
}


using UnityEngine;

public class State<T> : MonoBehaviour
{
    public virtual void Enter(T _owner) { }

    public virtual void Execute() { }

    public virtual void Exit() { }
}

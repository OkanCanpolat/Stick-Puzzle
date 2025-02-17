using UnityEngine;
using Zenject;

public class MouseEventStateMachine 
{
    public IMouseEventState CurrentState;

    public MouseEventStateMachine([Inject(Id ="MouseLockState")] IMouseEventState initialState)
    {
        CurrentState = initialState;
    }

    public void ChangeState(IMouseEventState state)
    {
        CurrentState = state;
    }
}

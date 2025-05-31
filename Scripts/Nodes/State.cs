using Godot;
using System;

[GlobalClass]
public abstract partial class State : Node
{
    public event Action<string> TransitionedEvent;

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Update(double delta);

    public abstract void PhysicsUpdate(double delta);

    public void TransitionTo(string stateName)
    {
        TransitionedEvent?.Invoke(stateName);
    }
}

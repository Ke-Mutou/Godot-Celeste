using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

[GlobalClass]
public partial class StateMachine : Node
{
    [Export] public State InitialState { get; set; }

    public State CurrentState { get; set; }


    private readonly Dictionary<StringName, State> _states = new();


    public override void _Ready()
    {
        Debug.Assert(InitialState is not null, "状态机未初始化");
        InitialState.Enter();
        CurrentState = InitialState;

        foreach (var child in GetChildren())
        {
            if (child is not State state)
                continue;
            _states[state.Name] = state;
            state.TransitionedEvent += On_State_TransitionedEvent;
        }
    }

    private void On_State_TransitionedEvent(string stateName)
    {
        if (string.IsNullOrWhiteSpace(stateName))
            return;
        if (stateName == CurrentState.Name)
            return;
        if (!_states.TryGetValue(stateName, out var state))
            return;
        CurrentState?.Exit();
        state?.Enter();
        CurrentState = state;
    }

    public override void _Process(double delta)
    {
        CurrentState?.Update(delta);
        // GD.Print(CurrentState?.Name);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState?.PhysicsUpdate(delta);
    }
}
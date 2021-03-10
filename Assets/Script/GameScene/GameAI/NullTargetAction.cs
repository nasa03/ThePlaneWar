using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityStandardAssets.Vehicles.Aeroplane;

public class NullTargetAction : Action
{
    private BehaviorTree _behaviorTree;
    private AeroplaneAiControl _aeroplaneAiControl;

    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {
        _behaviorTree.SetVariableValue("target", null);
        _aeroplaneAiControl.SetTarget(null);

        return TaskStatus.Success;
    }

    public override void OnAwake()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
        _aeroplaneAiControl = GetComponent<AeroplaneAiControl>();
    }
}
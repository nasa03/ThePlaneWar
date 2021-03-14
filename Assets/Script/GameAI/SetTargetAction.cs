using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityStandardAssets.Vehicles.Aeroplane;

public class SetTargetAction : Action
{
    private BehaviorTree _behaviorTree;
    private AeroplaneAiControl _aeroplaneAiControl;

    public override void OnStart()
    {
        Transform target = (Transform) _behaviorTree.GetVariable("target").GetValue();
        _aeroplaneAiControl.SetTarget(target);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }

    public override void OnAwake()
    {
        _aeroplaneAiControl = GetComponent<AeroplaneAiControl>();
        _behaviorTree = GetComponent<BehaviorTree>();
    }
}
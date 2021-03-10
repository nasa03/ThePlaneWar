using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class NullTargetConditional : Conditional
{
    private BehaviorTree _behaviorTree;

    public override TaskStatus OnUpdate()
    {
        Transform tempTarget = Global.GetNearTargetTransform(transform);
        Transform target = (Transform) _behaviorTree.GetVariable("target").GetValue();
        
        if (tempTarget || !target)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
    }

    public override void OnAwake()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
    }
}
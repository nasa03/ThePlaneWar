using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class NullTargetConditional : Conditional
{
    private BehaviorTree _behaviorTree;

    public override TaskStatus OnUpdate()
    {
        Transform target = (Transform) _behaviorTree.GetVariable("target").GetValue();

        return !target ? TaskStatus.Success : TaskStatus.Failure;
    }

    public override void OnAwake()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
    }
}
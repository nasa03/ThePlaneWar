using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class bPlaneConditional : Conditional
{
    private BehaviorTree _behaviorTree;

    public override TaskStatus OnUpdate()
    {
        Transform target = (Transform) _behaviorTree.GetVariable("target").GetValue();

        if (target && target.CompareTag("Plane"))
            return TaskStatus.Success;

        if (target.CompareTag("AI") && !target.GetComponent<AIProperty>().isDead)
            return TaskStatus.Success;

        return TaskStatus.Failure;
    }

    public override void OnAwake()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
    }
}
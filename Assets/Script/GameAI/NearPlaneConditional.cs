using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class NearPlaneConditional : Conditional
{
    public float maxDistance = 0.0f;
    private BehaviorTree _behaviorTree;

    public override TaskStatus OnUpdate()
    {
        Transform tempTarget = Global.GetNearTargetTransform(transform);

        if (!tempTarget)
            return TaskStatus.Failure;

        if (Global.GetDistance(transform, tempTarget) < maxDistance && Global.GetDistance(transform, tempTarget) != 0)
        {
            _behaviorTree.SetVariableValue("target", tempTarget);

            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }

    public override void OnAwake()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
    }
}
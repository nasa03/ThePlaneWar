using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class bPlaneConditional : Conditional
{
    public float maxDistance = 0.0f;
    private BehaviorTree _behaviorTree;

    public override TaskStatus OnUpdate()
    {
        Transform target = (Transform) _behaviorTree.GetVariable("target").GetValue();

        if (target && Global.GetDistance(transform, target) < maxDistance && Global.GetDistance(transform, target) != 0)
        {
            if (target.CompareTag("Plane"))
                return TaskStatus.Success;

            if (target.CompareTag("AI") && !target.GetComponent<AIProperty>().isDead)
                return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    public override void OnAwake()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
    }
}
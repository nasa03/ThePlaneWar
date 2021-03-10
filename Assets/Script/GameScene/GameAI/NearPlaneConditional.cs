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

        if (GetDistance(tempTarget) < maxDistance && GetDistance(tempTarget) != 0)
        {
            _behaviorTree.SetVariableValue("target", tempTarget);

            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }

    private float GetDistance(Transform target)
    {
        float distance = 0;

        if (target.CompareTag("Plane") || target.CompareTag("AI"))
        {
            Vector3 thisPosition = transform.position;
            Vector3 targetPosition = target.position;
            Vector3 dir = targetPosition - thisPosition;
            float dot = Vector3.Dot(Vector3.forward, dir);
            if (dot > 0)
                distance = Vector3.Distance(thisPosition, targetPosition);
        }

        return distance;
    }

    public override void OnAwake()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
    }
}
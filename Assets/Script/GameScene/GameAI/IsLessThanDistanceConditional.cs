using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsLessThanDistanceConditional : Conditional
{
    public float maxDistance = 0.0f;

    public override TaskStatus OnUpdate()
    {
        Transform target = (Transform) GlobalVariables.Instance.GetVariable("target").GetValue();

        if (GetDistance(target) < maxDistance && GetDistance(target) != 0)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }

    private float GetDistance(Transform target)
    {
        float distance = 0;

        if (target.CompareTag("Plane") && target.CompareTag("AI"))
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
}
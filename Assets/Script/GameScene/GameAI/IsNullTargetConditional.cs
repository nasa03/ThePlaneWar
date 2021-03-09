using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsNullTargetConditional : Conditional
{

    public override TaskStatus OnUpdate()
    {
        Transform target = (Transform) GlobalVariables.Instance.GetVariable("target").GetValue();
        
        if (target)
            return TaskStatus.Failure;
        else
        {
            target = Global.GetNearTargetTransform(transform);
            if (target)
                return TaskStatus.Success;
            else
                return TaskStatus.Failure;
        }
    }
    
    
}
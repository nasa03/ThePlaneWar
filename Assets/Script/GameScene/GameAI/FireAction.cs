using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FireAction : Action
{
    private projectileActor _projectileActor;
    private AIProperty _aiProperty;
    private BehaviorTree _behaviorTree;

    public override void OnStart()
    {
        
    }

    public override TaskStatus OnUpdate()
    {
        Transform target = (Transform) _behaviorTree.GetVariable("target").GetValue();

        if (!_aiProperty.isDead)
            _projectileActor.AIShoot();

        if (target && target.CompareTag("Plane"))
            return TaskStatus.Running;

        if (target.CompareTag("AI") && !target.GetComponent<AIProperty>().isDead)
            return TaskStatus.Running;

        return TaskStatus.Success;
    }

    public override void OnAwake()
    {
        _projectileActor = GetComponent<projectileActor>();
        _aiProperty = GetComponent<AIProperty>();
        _behaviorTree = GetComponent<BehaviorTree>();
    }
}
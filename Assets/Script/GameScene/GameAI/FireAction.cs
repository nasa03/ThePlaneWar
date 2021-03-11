using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FireAction : Action
{
    private projectileActor _projectileActor;
    private MissileActor _missileActor;
    private AIProperty _aiProperty;
    private BehaviorTree _behaviorTree;
    private float _projectileTime = MAXProjectileTime;
    private float _missileTime = MAXMissileTime;
    private int _missileCount = MAXMissileCount;
    private const float MAXProjectileTime = 0.2f;
    private const float MAXMissileTime = 10.0f;
    private const int MAXMissileCount = 3;

    public override void OnStart()
    {
    }

    public override TaskStatus OnUpdate()
    {
        Transform target = (Transform) _behaviorTree.GetVariable("target").GetValue();

        if (!_aiProperty.isDead)
        {
            _projectileTime -= Time.deltaTime;
            if (_projectileTime <= 0)
            {
                _projectileActor.AIShoot();
                _projectileTime = MAXProjectileTime;
            }

            if (_missileTime > 0 && _missileCount < MAXMissileCount)
                _missileTime -= Time.deltaTime;
            else
            {
                if (_missileCount < MAXMissileCount)
                    _missileCount++;

                _missileTime = MAXMissileTime;
            }

            if (_missileCount > 0)
            {
                _missileActor.AIShoot();
                _missileCount--;
            }
        }

        if (Global.GetDistance(transform, target) <= 0)
            return TaskStatus.Success;

        if (target && target.CompareTag("Plane"))
            return TaskStatus.Running;

        if (target.CompareTag("AI") && !target.GetComponent<AIProperty>().isDead)
            return TaskStatus.Running;

        return TaskStatus.Success;
    }

    public override void OnAwake()
    {
        _projectileActor = GetComponent<projectileActor>();
        _missileActor = GetComponent<MissileActor>();
        _aiProperty = GetComponent<AIProperty>();
        _behaviorTree = GetComponent<BehaviorTree>();
    }
}
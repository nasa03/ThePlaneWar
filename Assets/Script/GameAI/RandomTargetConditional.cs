using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class RandomTargetConditional : Conditional
{
    private BehaviorTree _behaviorTree;
    private int _random = -1;

    public override TaskStatus OnUpdate()
    {
        SharedVariable target = _behaviorTree.GetVariable("target");

        if (!(Transform) target.GetValue())
            target.SetValue(GetRandomPosition());
        
        return TaskStatus.Success;
    }

    public override void OnAwake()
    {
        _behaviorTree = GetComponent<BehaviorTree>();
    }

    private Transform GetRandomPosition()
    {
        int random = -1;

        do random = Random.Range(0, GameObject.FindObjectOfType<PhotonGameAI>().RandomPositions.Length);
        while (random == _random);

        _random = random;
        return GameObject.FindObjectOfType<PhotonGameAI>().RandomPositions[random];
    }
}
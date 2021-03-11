using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class bDeadConditional : Conditional
{
	private AIProperty _aiProperty;
	
	public override TaskStatus OnUpdate()
	{
		return !_aiProperty.isDead ? TaskStatus.Failure : TaskStatus.Running;
	}

	public override void OnAwake()
	{
		_aiProperty = GetComponent<AIProperty>();
	}
}
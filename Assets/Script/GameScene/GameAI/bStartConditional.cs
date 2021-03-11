using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class bStartConditional : Conditional
{
	public bool start = true;

	public override TaskStatus OnUpdate()
	{
		if (start)
		{
			start = !start;
			
			return TaskStatus.Success;
		}

		return TaskStatus.Failure;
	}
}
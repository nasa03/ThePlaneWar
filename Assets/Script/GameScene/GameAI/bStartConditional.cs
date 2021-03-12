using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class bStartConditional : Conditional
{
	private bool _start = true;

	public override TaskStatus OnUpdate()
	{
		if (_start)
		{
			_start = !_start;
			
			return TaskStatus.Success;
		}

		return TaskStatus.Failure;
	}
}
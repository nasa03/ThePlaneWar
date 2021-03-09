using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityStandardAssets.Vehicles.Aeroplane;

public class SetTargetAction : Action
{
	private AeroplaneAiControl _aeroplaneAiControl;
	private int _random = -1;
	
	public override void OnStart()
	{
		Transform target = (Transform) GlobalVariables.Instance.GetVariable("target").GetValue();
		
		target = GetRandomPosition();
		_aeroplaneAiControl.SetTarget(target);
	}

	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}

	public override void OnAwake()
	{
		_aeroplaneAiControl = GetComponent<AeroplaneAiControl>();
	}

	private Transform GetRandomPosition()
	{
		int random = -1;

		do random = Random.Range(0,  GameObject.FindObjectOfType<PhotonGameAI>().RandomPositions.Length);
		while (random == _random);

		_random = random;
		return GameObject.FindObjectOfType<PhotonGameAI>().RandomPositions[random];
	}

}
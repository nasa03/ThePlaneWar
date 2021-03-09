using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Photon.Pun;

public class bMasterClientConditional : Conditional
{
	public override TaskStatus OnUpdate()
	{
		if (PhotonNetwork.IsMasterClient)
			return TaskStatus.Success;
		else
			return TaskStatus.Failure;
	}
}
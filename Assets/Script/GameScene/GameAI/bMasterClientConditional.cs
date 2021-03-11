using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Photon.Pun;

public class bMasterClientConditional : Conditional
{
	public override TaskStatus OnUpdate()
	{
		return PhotonNetwork.IsMasterClient ? TaskStatus.Success : TaskStatus.Running;
	}
}
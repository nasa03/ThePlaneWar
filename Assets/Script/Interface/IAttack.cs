using System.Collections;
using Photon.Realtime;
using UnityEngine;

public interface IAttack
{
    void Attack(Player player, Transform target);

    void AttackAI(Transform target);
    
    IEnumerator Suicide();

    void OnCollisionEnter(Collision collision);
}
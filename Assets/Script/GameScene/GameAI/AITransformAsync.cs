using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AITransformAsync : MonoBehaviour, IPunObservable
{
    private Vector3 _position;
    private Quaternion _rotation;

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient) return;

        transform.position = _position;
        transform.rotation = _rotation;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            _position = (Vector3) stream.ReceiveNext();
            _rotation = (Quaternion) stream.ReceiveNext();
        }
    }
}
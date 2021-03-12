using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AITransformAsync : MonoBehaviour, IPunObservable
{
    private Rigidbody _rigidbody;
    private Vector3 _networkPosition;
    private Quaternion _networkRotation;

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient) return;

        _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _networkPosition, Time.fixedDeltaTime);
        _rigidbody.rotation =
            Quaternion.RotateTowards(_rigidbody.rotation, _networkRotation, Time.fixedDeltaTime * 100.0f);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rigidbody.position);
            stream.SendNext(_rigidbody.rotation);
            stream.SendNext(_rigidbody.velocity);
        }
        else
        {
            _networkPosition = (Vector3) stream.ReceiveNext();
            _networkRotation = (Quaternion) stream.ReceiveNext();
            _rigidbody.velocity = (Vector3) stream.ReceiveNext();

            float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.timestamp));
            _networkPosition += (this._rigidbody.velocity * lag);
        }
    }
}
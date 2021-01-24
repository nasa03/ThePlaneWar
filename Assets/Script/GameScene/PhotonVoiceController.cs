using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class PhotonVoiceController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Recorder recorder;
    private GameObject _speaker;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.InRoom) return;
        
        _speaker = PhotonNetwork.Instantiate("Speaker", new Vector3(), new Quaternion());
        _speaker.GetComponent<PhotonVoiceView>().RecorderInUse = recorder;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        _speaker = PhotonNetwork.Instantiate("Speaker", new Vector3(), new Quaternion());
        _speaker.GetComponent<PhotonVoiceView>().RecorderInUse = recorder;
    }
    
    public override void OnConnected()
    {
        base.OnConnected();
        
        if (!PhotonNetwork.InRoom) return;
        
        _speaker = PhotonNetwork.Instantiate("Speaker", new Vector3(), new Quaternion());
        _speaker.GetComponent<PhotonVoiceView>().RecorderInUse = recorder;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        
        Destroy(_speaker);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        
        Destroy(_speaker);
    }
}

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

public class PhotonVoiceController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Recorder recorder;
    [SerializeField] private UIToggle mainToggle;
    [SerializeField] private Button gameButton;
    private GameObject _speaker;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.InRoom) 
            Initialize();
        
    }
    
    private void Initialize()
    {
        if (_speaker != null) return;
        
        _speaker = PhotonNetwork.Instantiate("Speaker", new Vector3(), new Quaternion());
        _speaker.GetComponent<PhotonVoiceView>().RecorderInUse = recorder;
    }

    public void OnMainVoiceCheck()
    {
        recorder.TransmitEnabled = mainToggle.value;
    }

    public void OnGameVoiceEnter()
    {
        recorder.TransmitEnabled = true;
    }

    public void OnGameVoiceExit()
    {
        recorder.TransmitEnabled = false;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        Initialize();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        
        PhotonNetwork.Destroy(_speaker);
    }
}

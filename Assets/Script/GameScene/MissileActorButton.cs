using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileActorButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private Text text;
    private float _time = MAXTime;
    private int _totalCount = MAXCount;
    private const float MAXTime = 10.0f;
    private const int MAXCount = 3;

    public bool Missile => _totalCount > 0;

    // Update is called once per frame
    private void Update()
    {
        if (_time > 0 && _totalCount < MAXCount)
        {
            image.fillAmount = _time / MAXTime;
            _time -= Time.deltaTime;
        }
        else if (_totalCount == MAXCount)
            image.fillAmount = 0.0f;
        else
            ShootEnd();
    }

    public void ShootStart()
    {
        _totalCount--;
        button.enabled = Missile;
        text.text = _totalCount.ToString();
        GetComponent<AudioPlayer>().PlayAudio(1);
    }

    private void ShootEnd()
    {
        if (_totalCount < MAXCount)
            _totalCount++;

        _time = MAXTime;
        image.fillAmount = 1.0f;
        button.enabled = Missile;
        text.text = _totalCount.ToString();
    }
}
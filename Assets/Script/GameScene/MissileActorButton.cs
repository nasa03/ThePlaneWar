using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileActorButton : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Button button;
    [SerializeField] Text text;
    float time = 10.0f;
    int totalCount = 3;
    const float maxTime = 10.0f;
    const int maxCount = 3;

    public bool Missile => totalCount > 0;

    // Update is called once per frame
    void Update()
    {
        if (time > 0 && totalCount < maxCount)
        {
            image.fillAmount = time / maxTime;
            time -= Time.deltaTime;
        }
        else if (totalCount == maxCount)
        {
            image.fillAmount = 0.0f;
        }
        else
        {
            ShootEnd();
        }
    }

    public void ShootStart()
    {
        totalCount--;
        button.enabled = Missile;
        text.text = totalCount.ToString();
        GetComponent<AudioPlayer>().PlayAudio(1);
    }

    void ShootEnd()
    {
        if (totalCount < maxCount)
            totalCount++;

        time = maxTime;
        image.fillAmount = 1.0f;
        button.enabled = Missile;
        text.text = totalCount.ToString();
    }
}

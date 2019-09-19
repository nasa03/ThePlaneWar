using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissleActorButton : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Button button;
    float time = 0.0f;
    bool isMissle = true;

    public bool Missle
    {
        get
        {
            return isMissle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            image.fillAmount = time / 10;
            time -= Time.deltaTime;
        }
        else
        {
            if (!isMissle)
            {
                ShootEnd();
            }
        }
    }

    public void ShootStart()
    {
        button.enabled = false;
        isMissle = false;
        time = 10.0f;
        image.fillAmount = 1.0f;
    }

    void ShootEnd()
    {
        button.enabled = true;
        isMissle = true;
    }
}

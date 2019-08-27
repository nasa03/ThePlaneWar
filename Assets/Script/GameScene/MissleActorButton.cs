using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissleActorButton : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Button button;

    public IEnumerator Shoot()
    {
        button.enabled = false;
        image.fillAmount = 1.0f;
        for (int i = 0; i < 100; i++)
        {
            image.fillAmount -= 0.01f;
            yield return new WaitForSeconds(0.1f);
        }
        button.enabled = true;
    }
}

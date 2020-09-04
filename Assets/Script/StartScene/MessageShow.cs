using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageShow : MonoBehaviour {
    [SerializeField] private TweenAlpha tween;
    [SerializeField] private UILabel label;

    public IEnumerator Show(string message)
    {
        tween.gameObject.SetActive(true);
        tween.PlayForward();
        label.text = message;
        yield return new WaitForSeconds(2.0f);
        tween.PlayReverse();
        yield return new WaitForSeconds(1.0f);
        tween.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageShow : MonoBehaviour {

    public IEnumerator Show(string message)
    {
        gameObject.SetActive(true);
        GetComponent<UILabel>().text = message;
        yield return new WaitForSeconds(4.0f);
        gameObject.SetActive(false);
    }
}

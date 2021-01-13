using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class AttackMessage : MonoBehaviour
{
    [SerializeField] private Text text;
    private readonly Queue<string> _queue = new Queue<string>();

    [PunRPC]
    public void AddAttackMessage(string message)
    {
        _queue.Enqueue(message);
        Show();
        StartCoroutine(RemoveLine());
    }

    private IEnumerator RemoveLine()
    {
        yield return new WaitForSeconds(6.0f);
        _queue.Dequeue();
        Show();
    }

    private void Show()
    {
        string[] array = _queue.ToArray();
        string data = array.Aggregate("", (current, str) => current + (str + "\n"));
        text.text = data;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class AttackMessage : MonoBehaviour
{
    [SerializeField] Text text;
    Queue<string> queue = new Queue<string>();

    [PunRPC]
    public void AddAttackMessage(Player attackPlayer,Player deadPlayer)
    {
        string attackPlayerNickName = attackPlayer.NickName;
        string deadPlayerNickName = deadPlayer.NickName;

        queue.Enqueue(string.Format("{0} killed {1}", attackPlayerNickName, deadPlayerNickName));

        Show();

        StartCoroutine(RemoveLine());
    }

    IEnumerator RemoveLine()
    {
        yield return new WaitForSeconds(6.0f);
        queue.Dequeue();
        Show();
    }

    void Show()
    {
        string[] array = queue.ToArray();
        string data = "";
        for(int i = 0; i < array.Length; i++)
        {
            data += array[i] + "\n";
        }

        text.text = data;
    }
}

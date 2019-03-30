using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxPlayersSlider : MonoBehaviour {
    [SerializeField] UISlider playerSlider;
    [SerializeField] UILabel label;
    int maxPlayers;
    public int MaxPlayers { get { return maxPlayers; } }

    public void OnValueChange()
    {
        maxPlayers = (int)(playerSlider.value * 5 + 1);
        label.text = string.Format("人数：{0}", maxPlayers);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxPlayersSlider : MonoBehaviour {
    [SerializeField] private UISlider playerSlider;
    [SerializeField] private UILabel label;

    public int MaxPlayers { get; private set; }

    public void OnValueChange()
    {
        MaxPlayers = (int)(playerSlider.value * 5 + 1);
        label.text = $"人数：{MaxPlayers}";
    }
}
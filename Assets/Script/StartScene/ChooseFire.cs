using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFire : SingletonManager<ChooseFire> {
    [SerializeField] UIPopupList popupList;
    [HideInInspector] public int index = 0;

    public void Choose()
    {
        for (int i = 0; i < popupList.items.Count; i++)
        {
            if (popupList.value == popupList.items[i])
                index = i;
        }
    }
}

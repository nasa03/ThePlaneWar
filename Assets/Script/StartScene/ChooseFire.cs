using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFire : SingletonManager<ChooseFire> {
    [SerializeField] UIPopupList fireList;
    [SerializeField] UIPopupList sizeList;
    [HideInInspector] public int index = 0;
    int fireInt = 0;
    int sizeInt = 0;

    public void Fire()
    {
        fireInt = GetItemsInt(fireList);

        GetIndex();
    }

    public void Size()
    {
        sizeInt = GetItemsInt(sizeList);

        GetIndex();
    }

    int GetItemsInt(UIPopupList list)
    {
        for (int i = 0; i < list.items.Count; i++)
        {
            if (list.value == list.items[i])
                return i;
        }

        return 0;
    }

    void GetIndex()
    {
        if (fireInt < 2)
        {
            if (sizeList.items.Count == 3)
                sizeList.items.Add("Shotgun");

            index = fireInt * 4 + sizeInt;
        }
        else
        {
            if (sizeList.items.Count == 4)
                sizeList.items.Remove("Shotgun");

            index = 8 + (fireInt - 2) * 3 + sizeInt;
        }
    }
}

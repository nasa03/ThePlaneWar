using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMissle : MonoBehaviour
{
    [SerializeField] UIPopupList missleList;
    [SerializeField] UIPopupList sizeList;
    int missleInt = 0;
    int sizeInt = 0;

    public void Fire()
    {
        missleInt = GetItemsInt(missleList);

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
        Global.totalMissleInt = missleInt * 3 + sizeInt;
    }
}

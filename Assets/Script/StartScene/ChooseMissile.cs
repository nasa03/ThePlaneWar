using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMissile : MonoBehaviour
{
    [SerializeField] UIPopupList missileList;
    [SerializeField] UIPopupList sizeList;
    int missileInt = 0;
    int sizeInt = 0;

    public void Fire()
    {
        missileInt = GetItemsInt(missileList);

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
        Global.totalMissileInt = missileInt * 3 + sizeInt;
    }
}

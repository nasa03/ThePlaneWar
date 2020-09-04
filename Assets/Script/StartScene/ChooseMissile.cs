using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMissile : MonoBehaviour
{
    [SerializeField] private UIPopupList missileList;
    [SerializeField] private UIPopupList sizeList;
    private int _missileInt = 0;
    private int _sizeInt = 0;

    public void Fire()
    {
        _missileInt = GetItemsInt(missileList);

        GetIndex();
    }

    public void Size()
    {
        _sizeInt = GetItemsInt(sizeList);

        GetIndex();
    }

    private int GetItemsInt(UIPopupList list)
    {
        for (int i = 0; i < list.items.Count; i++)
        {
            if (list.value == list.items[i])
                return i;
        }

        return 0;
    }

    private void GetIndex()
    {
        Global.totalMissileInt = _missileInt * 3 + _sizeInt;
    }
}

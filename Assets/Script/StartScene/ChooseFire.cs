using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseFire : MonoBehaviour {
    [SerializeField] private UIPopupList fireList;
    [SerializeField] private UIPopupList sizeList;
    private int _fireInt = 0;
    private int _sizeInt = 0;

    public void Fire()
    {
        _fireInt = GetItemsInt(fireList);

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
        if (_fireInt < 2)
        {
            if (sizeList.items.Count == 3)
                sizeList.items.Add("Shotgun");

            Global.totalFireInt = _fireInt * 4 + _sizeInt;
        }
        else
        {
            if (sizeList.items.Count == 4)
                sizeList.items.Remove("Shotgun");

            Global.totalFireInt = 8 + (_fireInt - 2) * 3 + _sizeInt;
        }
    }
}

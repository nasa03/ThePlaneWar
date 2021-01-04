using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonController : MonoBehaviour
{
    [SerializeField] private UIPopupList fireList;
    [SerializeField] private UIPopupList fireSizeList;
    [SerializeField] private UIPopupList missileList;
    [SerializeField] private UIPopupList missileSizeList;
    [SerializeField] private UIToggle gyroscopeCheckBox;
    [SerializeField] private UISlider gyroscopeMultipleSlider;
    [SerializeField] private UIToggle offlineCheckBox;
    private JSONObject _jsonObject = null;

    // Start is called before the first frame update
    void Start()
    {
        if (!File.Exists(Global.JsonPath)) return;

        StreamReader reader = new StreamReader(Global.JsonPath);
        _jsonObject = new JSONObject(reader.ReadToEnd());

        Global.totalFireInt = (int) _jsonObject.GetField("totalFireInt").i;
        Global.totalMissileInt = (int) _jsonObject.GetField("totalMissileInt").i;
        Global.bGyroscopeEnabled = _jsonObject.GetField("bGyroscopeEnabled").b;
        Global.gyroscopeMultiple = _jsonObject.GetField("gyroscopeMultiple").f;
        Global.isOffline = _jsonObject.GetField("isOffline").b;

        reader.Dispose();
        reader.Close();
    }
    
    public void ShowSettingUI()
    {
        if (!File.Exists(Global.JsonPath)) return;
        
        fireList.value = _jsonObject.GetField("fireStr").str;
        fireSizeList.value = _jsonObject.GetField("fireSizeStr").str;
        missileList.value = _jsonObject.GetField("missileStr").str;
        missileSizeList.value = _jsonObject.GetField("missileSizeList").str;
        gyroscopeCheckBox.value = _jsonObject.GetField("bGyroscopeEnabled").b;
        gyroscopeMultipleSlider.value = _jsonObject.GetField("gyroscopeMultiple").f / 2.0f;
        offlineCheckBox.value = _jsonObject.GetField("isOffline").b;
    }

    public void Save()
    {
        if (!File.Exists(Global.JsonPath))
        {
            _jsonObject = new JSONObject();
            
            _jsonObject.AddField("fireStr", fireList.GetComponentInChildren<UILabel>().text);
            _jsonObject.AddField("fireSizeStr", fireSizeList.GetComponentInChildren<UILabel>().text);
            _jsonObject.AddField("missileStr", missileList.GetComponentInChildren<UILabel>().text);
            _jsonObject.AddField("missileSizeList", missileSizeList.GetComponentInChildren<UILabel>().text);
            _jsonObject.AddField("totalFireInt", Global.totalFireInt);
            _jsonObject.AddField("totalMissileInt", Global.totalMissileInt);
            _jsonObject.AddField("bGyroscopeEnabled", Global.bGyroscopeEnabled);
            _jsonObject.AddField("gyroscopeMultiple", Global.gyroscopeMultiple);
            _jsonObject.AddField("isOffline", Global.isOffline);
        }
        else
        {
            _jsonObject.SetField("fireStr", fireList.GetComponentInChildren<UILabel>().text);
            _jsonObject.SetField("fireSizeStr", fireSizeList.GetComponentInChildren<UILabel>().text);
            _jsonObject.SetField("missileStr",missileList.GetComponentInChildren<UILabel>().text);
            _jsonObject.SetField("missileSizeList",missileSizeList.GetComponentInChildren<UILabel>().text);
            _jsonObject.SetField("totalFireInt", Global.totalFireInt);
            _jsonObject.SetField("totalMissileInt", Global.totalMissileInt);
            _jsonObject.SetField("bGyroscopeEnabled", Global.bGyroscopeEnabled);
            _jsonObject.SetField("gyroscopeMultiple", Global.gyroscopeMultiple);
            _jsonObject.SetField("isOffline", Global.isOffline);
        }

        StreamWriter writer = new StreamWriter(Global.JsonPath, false);
        writer.Write(_jsonObject.ToString());
        writer.Dispose();
        writer.Close();
    }
}

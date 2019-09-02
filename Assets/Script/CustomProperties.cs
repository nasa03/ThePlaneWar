//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CustomProperties : MonoBehaviour
{
    public static void SetProperties(Player player, string key, object value)
    {
        Hashtable keyValues = player.CustomProperties;

        if (keyValues.ContainsKey(key))
        {
            keyValues[key] = value;
        }
        else
        {
            keyValues.Add(key, value);
        }

        player.SetCustomProperties(keyValues);

    }

    public static object GetProperties(Player player, string key, object defaultValue = null)
    {
        Hashtable keyValues = player.CustomProperties;

        if (keyValues.ContainsKey(key))
        {
            return keyValues[key];
        }
        else
        {
            return defaultValue;
        }
    }
}

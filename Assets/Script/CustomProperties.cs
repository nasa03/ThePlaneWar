//using System.Collections;

using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;

public static class CustomProperties
{
    public static void SetProperties(this Player player, string key, object value)
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

    public static object GetProperties(this Player player, string key, object defaultValue = null)
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
    
    public static void SetProperties(this Room room, string key, object value)
    {
        Hashtable keyValues = room.CustomProperties;

        if (keyValues.ContainsKey(key))
        {
            keyValues[key] = value;
        }
        else
        {
            keyValues.Add(key, value);
        }

        room.SetCustomProperties(keyValues);

    }
    
    public static object GetProperties(this Room room, string key, object defaultValue = null)
    {
        Hashtable keyValues = room.CustomProperties;

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

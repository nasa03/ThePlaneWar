// Copyright © 2018 Procedural Worlds Pty Limited.  All Rights Reserved.
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace GaiaCommon1
{
    /// <summary>
    /// These remote content object will automatically load and save their data and use the updater to automatically update according to their settings
    /// </summary>
    [Serializable]
    public abstract class RemoteContent
    {
        // The last time the content was upated
        private int m_lastUpdated = 0;

        public readonly string SOURCE_URL;

        private readonly int SECONDS_BETWEEN_UPDATES;

        public RemoteContent(string url, int hoursBetweenUpdates = 24)
        {
            SOURCE_URL = url;
            SECONDS_BETWEEN_UPDATES = hoursBetweenUpdates * 60 * 60;

            if (Utils.GetFrapoch() - m_lastUpdated > SECONDS_BETWEEN_UPDATES)
            {
                //RemoteContentUpdater updater = new RemoteContentUpdater(SOURCE_URL, ProcessUpdate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save(string path)
        {
            // Store in './Library'
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, this);
            }
        }

        public static RemoteContent Load(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (RemoteContent)formatter.Deserialize(stream);
            }
        }

        private void ProcessUpdate(PWMessage message)
        {
            if (message != null)
            {
                m_lastUpdated = Utils.GetFrapoch();
            }
        }
    }
}

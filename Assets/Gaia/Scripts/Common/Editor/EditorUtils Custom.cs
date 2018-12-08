// Copyright © 2018 Procedural Worlds Pty Limited.  All Rights Reserved.
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using GaiaCommon1.Localization;

namespace GaiaCommon1 {
    /// <summary>
    /// Handy editor utils - Custom controls
    /// </summary>
    public partial class EditorUtils {
        
        /// <summary>
        /// Make an Enum popup selection field with localized value names
        /// </summary>
        /// <param name="key">Localization key of the label in front of the field.</param>
        /// <param name="valueKeyPrefix">Localization key prefix of the option names. (option name will be appended)</param>
        /// <param name="property">SerializedProperty to edit</param>
        /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
        /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
        public void EnumPopupLocalized(string key, string valueKeyPrefix, SerializedProperty property, params GUILayoutOption[] options) {
            string[] EnumNames = property.enumDisplayNames;
            GUIContent[] displayNames = new GUIContent[EnumNames.Length];
            int[] displayValues = new int[EnumNames.Length];
            for (int x = 0; x < EnumNames.Length; ++x) {
                displayValues[x] = x;
                displayNames[x] = GetContent(valueKeyPrefix + EnumNames[x]);
            }
            int newValue = property.hasMultipleDifferentValues ? -1 : property.enumValueIndex;
            EditorGUI.BeginChangeCheck();
            newValue = EditorGUILayout.IntPopup(GetContent(key), newValue, displayNames, displayValues, options);
            if (EditorGUI.EndChangeCheck()) {
                property.enumValueIndex = newValue;
            }
        }
        /// <summary>
        /// Make an Enum popup selection field with localized value names
        /// </summary>
        /// <param name="key">Localization key of the label in front of the field.</param>
        /// <param name="valueKeyPrefix">Localization key prefix of the option names. (option name will be appended)</param>
        /// <param name="property">SerializedProperty to edit</param>
        /// <param name="helpSwitch">The <see langword="bool"/> that the user interacts with to switch help On/Off.</param>
        /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
        /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
        public void EnumPopupLocalized(string key, string valueKeyPrefix, SerializedProperty property, bool helpSwitch, params GUILayoutOption[] options) {
            string[] EnumNames = property.enumDisplayNames;
            GUIContent[] displayNames = new GUIContent[EnumNames.Length];
            int[] displayValues = new int[EnumNames.Length];
            for (int x = 0; x < EnumNames.Length; ++x) {
                displayValues[x] = x;
                displayNames[x] = GetContent(valueKeyPrefix + EnumNames[x]);
            }
            int newValue = property.hasMultipleDifferentValues ? -1 : property.enumValueIndex;
            EditorGUI.BeginChangeCheck();
            newValue = EditorGUILayout.IntPopup(GetContent(key), newValue, displayNames, displayValues, options);
            if (EditorGUI.EndChangeCheck()) {
                property.enumValueIndex = newValue;
            }
            InlineHelp(key, helpSwitch);
        }
        /// <summary>
        /// Make an Enum popup selection field with localized value names
        /// </summary>
        /// <param name="key">Localization key of the label in front of the field.</param>
        /// <param name="valueKeyPrefix">Localization key prefix of the option names. (option name will be appended)</param>
        /// <param name="property">SerializedProperty to edit</param>
        /// <param name="style">Optional GUIStyle.</param>
        /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
        /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
        public void EnumPopupLocalized(string key, string valueKeyPrefix, SerializedProperty property, GUIStyle style, params GUILayoutOption[] options) {
            string[] EnumNames = property.enumDisplayNames;
            GUIContent[] displayNames = new GUIContent[EnumNames.Length];
            int[] displayValues = new int[EnumNames.Length];
            for (int x = 0; x < EnumNames.Length; ++x) {
                displayValues[x] = x;
                displayNames[x] = GetContent(valueKeyPrefix + EnumNames[x]);
            }
            int newValue = property.hasMultipleDifferentValues ? -1 : property.enumValueIndex;
            EditorGUI.BeginChangeCheck();
            newValue = EditorGUILayout.IntPopup(GetContent(key), newValue, displayNames, displayValues, style, options);
            if (EditorGUI.EndChangeCheck()) {
                property.enumValueIndex = newValue;
            }
        }
        /// <summary>
        /// Make an Enum popup selection field with localized value names
        /// </summary>
        /// <param name="key">Localization key of the label in front of the field.</param>
        /// <param name="valueKeyPrefix">Localization key prefix of the option names. (option name will be appended)</param>
        /// <param name="property">SerializedProperty to edit</param>
        /// <param name="style">Optional GUIStyle.</param>
        /// <param name="helpSwitch">The <see langword="bool"/> that the user interacts with to switch help On/Off.</param>
        /// <param name="options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.&lt;br&gt;
        /// See Also: GUILayout.Width, GUILayout.Height, GUILayout.MinWidth, GUILayout.MaxWidth, GUILayout.MinHeight,
        /// GUILayout.MaxHeight, GUILayout.ExpandWidth, GUILayout.ExpandHeight.</param>
        public void EnumPopupLocalized(string key, string valueKeyPrefix, SerializedProperty property, GUIStyle style, bool helpSwitch, params GUILayoutOption[] options) {
            string[] EnumNames = property.enumDisplayNames;
            GUIContent[] displayNames = new GUIContent[EnumNames.Length];
            int[] displayValues = new int[EnumNames.Length];
            for (int x = 0; x < EnumNames.Length; ++x) {
                displayValues[x] = x;
                displayNames[x] = GetContent(valueKeyPrefix + EnumNames[x]);
            }
            int newValue = property.hasMultipleDifferentValues ? -1 : property.enumValueIndex;
            EditorGUI.BeginChangeCheck();
            newValue = EditorGUILayout.IntPopup(GetContent(key), newValue, displayNames, displayValues, style, options);
            if (EditorGUI.EndChangeCheck()) {
                property.enumValueIndex = newValue;
            }
            InlineHelp(key, helpSwitch);
        }
    }
}

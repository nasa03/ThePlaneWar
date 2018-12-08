// Copyright © 2018 Procedural Worlds Pty Limited.  All Rights Reserved.
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GaiaCommon1
{
    public class PWWelcomeScreen : EditorWindow, IPWEditor
    {
        public List<AppConfig> DefaultWelcomeList
        {
            set
            {
                m_defaultWelcomeList = value;
                m_defaultTabCount = 0;

                for (int i = 0; i < m_defaultWelcomeList.Count; i++)
                {
                    EditorPrefs.SetInt(m_defaultWelcomeList[i].Name + "WelcomeLastShown", Utils.GetFrapoch());
                    m_prodNamesForDefaultWelcome = (i == 0) ? m_defaultWelcomeList[i].Name : ", " + m_defaultWelcomeList[i].Name;
                    m_defaultTabCount = 1;
                }
            }
        }
        public List<CustomWelcome> CustomWelcomeList
        {
            set
            {
                m_customWelcomeList = value;
                for (int i = 0; i < m_customWelcomeList.Count; i++)
                {
                    EditorPrefs.SetInt(m_customWelcomeList[i].AppConfig.Name + "WelcomeLastShown", Utils.GetFrapoch());
                    m_customWelcomeList[i].repaintDelegate -= Repaint;
                    m_customWelcomeList[i].repaintDelegate += Repaint;
                }
            }
        }

        public bool PositionChecked { get; set; }

        [SerializeField] private Vector2 m_windowScrollPos = Vector2.right;

        private EditorUtils m_editorUtils;
        private float m_openingTime = -1f;
        private bool showAtStartup = true;
        private List<AppConfig> m_defaultWelcomeList = new List<AppConfig>();
        private List<CustomWelcome> m_customWelcomeList = new List<CustomWelcome>();
        private string m_prodNamesForDefaultWelcome;
        private int m_defaultTabCount;

        // Our products buttons
        private Texture2D m_prodImgCTS;
        private Texture2D m_prodImgGaia;
        private Texture2D m_prodImgGeNa;
        private Texture2D m_prodImgPegasus;
        private Texture2D m_prodImgPP;
        private Texture2D m_prodImgSECTR;
        private float m_maxProdImgWidth;

        // and links
        private const string LINK_CTS = "https://www.assetstore.unity3d.com/#!/content/91938";
        private const string LINK_GAIA = "https://www.assetstore.unity3d.com/#!/content/42618";
        private const string LINK_GENA = "https://www.assetstore.unity3d.com/#!/content/127636";
        private const string LINK_PEGASUS = "https://www.assetstore.unity3d.com/#!/content/65397";
        private const string LINK_PP = "https://www.assetstore.unity3d.com/#!/content/127506";
        private const string LINK_SECTR = "https://www.assetstore.unity3d.com/#!/content/15356";


        PWWelcomeScreen()
        {
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
        }

        #region Main delegates

        private void OnEnable()
        {
            minSize = new Vector2(980, 675);

            if (m_editorUtils == null)
            {
                m_editorUtils = new EditorUtils(PWConst.COMMON_APP_CONF, this);
            }

            if (m_prodImgCTS == null)
            {
                m_prodImgCTS = Resources.Load("CTS" + PWConst.VERSION_IN_FILENAMES) as Texture2D;
                m_maxProdImgWidth = m_prodImgCTS.width > m_maxProdImgWidth ? m_prodImgCTS.width : m_maxProdImgWidth;
            }

            if (m_prodImgGaia == null)
            {
                m_prodImgGaia = Resources.Load("Gaia" + PWConst.VERSION_IN_FILENAMES) as Texture2D;
                m_maxProdImgWidth = m_prodImgCTS.width > m_maxProdImgWidth ? m_prodImgCTS.width : m_maxProdImgWidth;
            }

            if (m_prodImgGeNa == null)
            {
                m_prodImgGeNa = Resources.Load("GeNa" + PWConst.VERSION_IN_FILENAMES) as Texture2D;
                m_maxProdImgWidth = m_prodImgCTS.width > m_maxProdImgWidth ? m_prodImgCTS.width : m_maxProdImgWidth;
            }

            if (m_prodImgPegasus == null)
            {
                m_prodImgPegasus = Resources.Load("Pegasus" + PWConst.VERSION_IN_FILENAMES) as Texture2D;
                m_maxProdImgWidth = m_prodImgCTS.width > m_maxProdImgWidth ? m_prodImgCTS.width : m_maxProdImgWidth;
            }

            if (m_prodImgPP == null)
            {
                m_prodImgPP = Resources.Load("PathPainter" + PWConst.VERSION_IN_FILENAMES) as Texture2D;
                m_maxProdImgWidth = m_prodImgCTS.width > m_maxProdImgWidth ? m_prodImgCTS.width : m_maxProdImgWidth;
            }

            if (m_prodImgSECTR == null)
            {
                m_prodImgSECTR = Resources.Load("SECTR" + PWConst.VERSION_IN_FILENAMES) as Texture2D;
                m_maxProdImgWidth = m_prodImgCTS.width > m_maxProdImgWidth ? m_prodImgCTS.width : m_maxProdImgWidth;
            }
        }

        private void OnDestroy()
        {
            if (m_editorUtils != null)
            {
                m_editorUtils.Dispose();
            }

            for (int i = 0; i < m_defaultWelcomeList.Count; i++)
            {
                SetShowPrefForApp(m_defaultWelcomeList[i]);
            }

            for (int i = 0; i < m_customWelcomeList.Count; i++)
            {
                SetShowPrefForApp(m_customWelcomeList[i].AppConfig);
                m_customWelcomeList[i].repaintDelegate -= Repaint;
            }
        }

        /// <summary>
        /// Need the delay otherwise the calling window can cover this
        /// </summary>
        private void OnEditorUpdate()
        {
            if (m_openingTime < 0f)
            {
                m_openingTime = Time.realtimeSinceStartup;
                return;
            }

            if (Time.realtimeSinceStartup - m_openingTime > 1f)
            {
                Focus();
                EditorApplication.update -= OnEditorUpdate;
            }
        }

        #endregion

        #region Unity GUI
        private void InitGUI()
        {
        }

        protected void OnGUI()
        {
            m_editorUtils.Initialize(); // Do not remove this!
            m_windowScrollPos = EditorGUILayout.BeginScrollView(m_windowScrollPos);
            {
                Welcome();

                GUILayout.Space(6f);
                OurProducts();
            }
            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            {
                m_editorUtils.LeftCheckbox("Show welcome again checkbox", ref showAtStartup);

                GUILayout.FlexibleSpace();
                if (m_editorUtils.Button("Close button", GUILayout.Width(150f)))
                {
                    Close();
                }
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            InitGUI();
        }
        #endregion

        #region UI Sections

        private void Welcome()
        {
            // This is really just a hack
            // Not yet implemented!
            if (m_customWelcomeList.Count + m_defaultTabCount > 1)
            {
                //m_editorUtils.Tabs();
            }
            else
            {
                if (m_customWelcomeList.Count == 1)
                {
                    m_customWelcomeList[0].WelcomeGUI();
                }
                else
                {
                    DefaultWelcome();
                }
            }
        }

        private void DefaultWelcome()
        {
            if (m_defaultWelcomeList.Count < 1)
            {
                return;
            }
            else if (m_defaultWelcomeList.Count == 1)
            {
                m_editorUtils.GUIHeader(m_defaultWelcomeList[0]);
            }
            else
            {
                m_editorUtils.GUIHeader("Procedural Worlds");
            }

            m_editorUtils.TitleNonLocalized(m_editorUtils.GetTextValue("Welcome title") + m_prodNamesForDefaultWelcome + "!");

            EditorGUILayout.Space();
            m_editorUtils.Text("Welcome message");

            GUILayout.Space(15f);
            if (m_editorUtils.ButtonCentered("Recommend Tutorials", GUILayout.Width(250f)))
            {
                Application.OpenURL(PWConst.COMMON_APP_CONF.TutorialsLink);
            }
        }

        private void OurProducts()
        {
            // Later we can get this from the website
            m_editorUtils.Heading("Our Products");

            float spacing = 10f;
            float descriptionHeight = 45f;

            GUILayout.Space(spacing);
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                {
                    // CTS
                    if (m_editorUtils.ClickableImage(m_prodImgCTS))
                    {
                        Application.OpenURL(LINK_CTS);
                    }
                    m_editorUtils.ClickableImgDescriptionBold("CTSDescription", GUILayout.Width(m_maxProdImgWidth), GUILayout.Height(descriptionHeight));

                    GUILayout.Space(spacing);

                    // PP
                    if (m_editorUtils.ClickableImage(m_prodImgPP))
                    {
                        Application.OpenURL(LINK_PP);
                    }
                    m_editorUtils.ClickableImgDescriptionBold("PPDescription", GUILayout.Width(m_maxProdImgWidth), GUILayout.Height(descriptionHeight));
                }
                GUILayout.EndVertical();

                GUILayout.Space(spacing);
                GUILayout.BeginVertical();
                {
                    // Gaia
                    if (m_editorUtils.ClickableImage(m_prodImgGaia))
                    {
                        Application.OpenURL(LINK_GAIA);
                    }
                    m_editorUtils.ClickableImgDescriptionBold("GaiaDescription", GUILayout.Width(m_maxProdImgWidth), GUILayout.Height(descriptionHeight));

                    GUILayout.Space(spacing);

                    // Pegasus
                    if (m_editorUtils.ClickableImage(m_prodImgPegasus))
                    {
                        Application.OpenURL(LINK_PEGASUS);
                    }
                    m_editorUtils.ClickableImgDescriptionBold("PegasusDescription", GUILayout.Width(m_maxProdImgWidth), GUILayout.Height(descriptionHeight));
                }
                GUILayout.EndVertical();

                GUILayout.Space(spacing);
                GUILayout.BeginVertical();
                {
                    // GeNa
                    if (m_editorUtils.ClickableImage(m_prodImgGeNa))
                    {
                        Application.OpenURL(LINK_GENA);
                    }
                    m_editorUtils.ClickableImgDescriptionBold("GeNaDescription", GUILayout.Width(m_maxProdImgWidth), GUILayout.Height(descriptionHeight));

                    GUILayout.Space(spacing);

                    // SECTR
                    if (m_editorUtils.ClickableImage(m_prodImgSECTR))
                    {
                        Application.OpenURL(LINK_SECTR);
                    }
                    m_editorUtils.ClickableImgDescriptionBold("SECTRDescription", GUILayout.Width(m_maxProdImgWidth), GUILayout.Height(descriptionHeight));
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        #endregion

        #region Helper methods

        private void SetShowPrefForApp(AppConfig appConfig)
        {
            EditorPrefs.SetBool("Show" + appConfig.Name + "Welcome", showAtStartup);
        }

        #endregion
    }
}

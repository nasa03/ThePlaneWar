using System.Collections;
using UnityEngine;
#if UNITY_POST_PROCESSING_STACK_V2
using UnityEngine.Rendering.PostProcessing;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Gaia
{
    /// <summary>
    /// Setup for the underwater FX features   
    /// </summary>
    [RequireComponent(typeof(Light))]
    public class GaiaUnderWaterEffects : MonoBehaviour
    {
        #region Variables
        [Header("Caustic Settings")]
        [Tooltip("Sets if the light object follows the player")]
        public bool m_followPlayer = false;
        [Tooltip("Creates gameobject walls around the player to fix the horizon color issues with the fog rendering")]
        public bool m_useHorizonFix = true;
        [Tooltip("Creates simple underwater particles effect")]
        public bool m_useUnderwaterparticles = true;
        [Tooltip("Sets the caustics size, not this only works on directonial lights")]
        [Range(0, 2000)]
        public int m_causticsSize = 5;
        [Tooltip("Caustic textures used to generate the effect")]
        public Texture[] m_cookies = new Texture[16];
        [Tooltip("How many frame renders are made, higher the number the faster the animation. Recommend between 15-30 for optimial performance and visuals")]
        public float m_framesPerSecond = 25f;
        [Tooltip("What the current sea level is. Gaias default is 50")]
        public float m_sealevel = 50f;
        [Header("Underwater Settings")]
        [Range(0f, 1f)]
        [Tooltip("Sets the underwater ambiance audio volume")]
        public float m_underwaterSoundFXVolume = 0.4f;
        [Range(0f, 1f)]
        [Tooltip("Sets the water submerge audio volume")]
        public float m_waterSubmergeSounfFXVolume = 0.4f;
        [Tooltip("Sets the submerge down sound fx")]
        public AudioClip m_submergeSoundFXDown;
        [Tooltip("Sets the submerge up sound fx")]
        public AudioClip m_submergeSoundFXUp;
        [Tooltip("Sets the underwater fog color")]
        public Color32 m_underWaterFogColor = new Color32(76, 112, 142, 255);
        [Tooltip("Sets the underwater fog distance")]
        public float m_underWaterFogDistance = 70f;
        //The light
        Light mainlight;
        //This object it's attached too
        Transform causticsObject;
        //The player object
        //[HideInInspector]
        public Transform player;
        //What texture number it's on
        int indexNumber = 0;
        //Status of the coroutine
        bool coroutineStatus = false;
        //Fog color stored
        [HideInInspector]
        public Color32 storedFogColor;
        //Fog distance stored
        [HideInInspector]
        public float storedFogDistance;
        //Ambient audio
        GameObject ambientAudio;
        //Underwater audio
        GameObject underwaterAudio;
        //The horizon fix gameobject
        GameObject horizonObject;
        [HideInInspector]
        public GameObject horizonObjectStored;
        //The objects audio source
        AudioSource objectAudioSource;
        //The underwater particles
        GameObject underwaterParticles;
        [HideInInspector]
        //Stored underwater particles gameobject
        public GameObject underwaterParticlesStored;

        private Transform partentObject;
        #endregion

        #region Setup
        void Start()
        {
            mainlight = gameObject.GetComponent<Light>();
            mainlight.cookie = null;
            causticsObject = gameObject.transform;
            storedFogColor = RenderSettings.fogColor;
            storedFogDistance = RenderSettings.fogEndDistance;
            partentObject = GetOrCreateEnvironmentParent().transform; 

            ambientAudio = null;
            if (ambientAudio == null)
            {
                ambientAudio = GameObject.Find("Ambient Audio");
            }

            underwaterAudio = null;
            if (underwaterAudio == null)
            {
                underwaterAudio = GameObject.Find("Underwater SoundFX");
            }
            #if UNITY_EDITOR
            if (player == null)
            {
                player = GetThePlayer();
            }
            #endif
            GaiaSceneInfo sceneInfo = GaiaSceneInfo.GetSceneInfo();
            if (sceneInfo != null)
            {
                m_sealevel = sceneInfo.m_seaLevel;
            }

            if (m_useHorizonFix)
            {
                horizonObject = GameObject.Find("Ambient Underwater Horizon");
                if (horizonObjectStored != null)
                {
                    horizonObject = Instantiate(horizonObjectStored);
                    horizonObject.name = "Ambient Underwater Horizon";
                    if (partentObject != null)
                    {
                        horizonObject.transform.parent = partentObject;
                    }

                    MeshRenderer[] theRenders = horizonObject.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer rends in theRenders)
                    {
                        rends.enabled = false;
                    }
                }
            }

            if (m_useUnderwaterparticles)
            {
                underwaterParticles = GameObject.Find("Underwater Particles Effects");
                if (underwaterParticlesStored != null)
                {
                    underwaterParticles = Instantiate(underwaterParticlesStored);
                    underwaterParticles.name = "Underwater Particles Effects";
                    underwaterParticles.SetActive(false);
                    if (partentObject != null)
                    {
                        underwaterParticles.transform.parent = partentObject;
                    }
                }
            }

            if (gameObject.GetComponent<AudioSource>() == null)
            {
                gameObject.AddComponent<AudioSource>();
                objectAudioSource = gameObject.GetComponent<AudioSource>();
                objectAudioSource.volume = m_waterSubmergeSounfFXVolume;
            }
            else
            {
                objectAudioSource = gameObject.GetComponent<AudioSource>();
                objectAudioSource.volume = m_waterSubmergeSounfFXVolume;
            }

            if (mainlight.type == LightType.Directional)
            {
                mainlight.cookieSize = m_causticsSize;
            }
        }

        public Transform GetThePlayer()
        {
            #if UNITY_EDITOR
            GaiaSettings settings = AssetDatabase.LoadAssetAtPath<GaiaSettings>(GaiaUtils.GetAssetPath("GaiaSettings.asset"));
            Transform thePlayer = null;
            if (settings.m_currentController == GaiaConstants.EnvironmentControllerType.FirstPerson)
            {
                if (GameObject.Find("FirstPersonCharacter") != null)
                {
                    thePlayer = GameObject.Find("FirstPersonCharacter").transform;

                    if (thePlayer != null)
                    {
                        return thePlayer;
                    }
                }
            }

            if (settings.m_currentController == GaiaConstants.EnvironmentControllerType.FlyingCamera)
            {
                if (GameObject.Find("FlyCam") != null)
                {
                    thePlayer = GameObject.Find("FlyCam").transform;

                    if (thePlayer != null)
                    {
                        return thePlayer;
                    }
                }
            }

            if (settings.m_currentController == GaiaConstants.EnvironmentControllerType.ThirdPerson)
            {
                if (GameObject.Find("Main Camera") != null)
                {
                    thePlayer = GameObject.Find("Main Camera").transform;

                    if (thePlayer != null)
                    {
                        return thePlayer;
                    }
                }
            }
            #endif
            return null;
        }

        private static GameObject GetOrCreateEnvironmentParent()
        {
            GameObject parent = GameObject.Find("Gaia Environment");
            if (parent == null)
            {
                parent = new GameObject("Gaia Environment");
            }
            return parent;
        }
        #endregion

        #region Positioning and Enabling
        void LateUpdate()
        {
            if (m_useHorizonFix && horizonObject != null)
            {
                horizonObject.transform.position = new Vector3(player.position.x + 500f, m_sealevel - 300f, player.position.z);
            }

            if (m_useUnderwaterparticles && underwaterParticles != null)
            {
                underwaterParticles.transform.position = player.position;
            }

            if (m_followPlayer && player != null)
            {
                causticsObject.position = new Vector3(player.position.x, m_sealevel, player.position.z);
            }

            if (player != null)
            {
                if (player.position.y >= m_sealevel)
                {
                    if (coroutineStatus)
                    {
                        StopAllCoroutines();
                        RenderSettings.fogColor = storedFogColor;
                        RenderSettings.fogEndDistance = storedFogDistance;
                        indexNumber = 0;
                        mainlight.cookie = null;
                        coroutineStatus = false;

                        if (m_submergeSoundFXUp != null && objectAudioSource != null)
                        {
                            objectAudioSource.PlayOneShot(m_submergeSoundFXUp, m_waterSubmergeSounfFXVolume);
                        }

                        if (m_useHorizonFix)
                        {
                            MeshRenderer[] theRenders = horizonObject.GetComponentsInChildren<MeshRenderer>();
                            foreach (MeshRenderer rends in theRenders)
                            {
                                rends.enabled = false;
                            }
                        }

                        if (m_useUnderwaterparticles)
                        {
                            underwaterParticles.SetActive(false);
                        }
                        
                        if (ambientAudio != null)
                        {
                            AudioSource ambientAudioSource = ambientAudio.GetComponent<AudioSource>();
                            if (ambientAudioSource != null)
                            {
                                ambientAudioSource.volume = 0.5f;
                            }
                        }

                        if (underwaterAudio != null)
                        {
                            AudioSource underwaterAudioSource = underwaterAudio.GetComponent<AudioSource>();
                            if (underwaterAudioSource != null)
                            {
                                underwaterAudioSource.volume = 0f;
                            }
                        }                        
                    }
                }

                if (player.position.y < m_sealevel)
                {
                    if (!coroutineStatus)
                    {
                        StartCoroutine(CausticsAnimation(true));
                        RenderSettings.fogColor = m_underWaterFogColor;
                        RenderSettings.fogEndDistance = m_underWaterFogDistance;
                        coroutineStatus = true;

                        if (m_submergeSoundFXDown != null && objectAudioSource != null)
                        {
                            objectAudioSource.PlayOneShot(m_submergeSoundFXDown, m_waterSubmergeSounfFXVolume);
                        }

                        if (m_useHorizonFix)
                        {
                            MeshRenderer[] theRenders = horizonObject.GetComponentsInChildren<MeshRenderer>();
                            foreach (MeshRenderer rends in theRenders)
                            {
                                rends.enabled = true;
                            }
                        }

                        if (m_useUnderwaterparticles)
                        {
                            underwaterParticles.SetActive(true);
                            underwaterParticles.GetComponent<ParticleSystem>().Play();
                        }
                        
                        if (ambientAudio != null)
                        {
                            AudioSource ambientAudioSource = ambientAudio.GetComponent<AudioSource>();
                            if (ambientAudioSource != null)
                            {
                                ambientAudioSource.volume = 0f;
                            }
                        }

                        if (underwaterAudio != null)
                        {
                            AudioSource underwaterAudioSource = underwaterAudio.GetComponent<AudioSource>();
                            if (underwaterAudioSource != null)
                            {
                                underwaterAudioSource.volume = m_underwaterSoundFXVolume;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Caustics Setup
        IEnumerator CausticsAnimation(bool systemOn)
        {
            while (systemOn)
            {               
                mainlight.cookie = m_cookies[indexNumber];
                indexNumber++;
                if (indexNumber == m_cookies.Length)
                {
                    indexNumber = 0;
                }
                yield return new WaitForSeconds(1 / m_framesPerSecond);
            }
        }
        
        public void LoadCaustics()
        {
            #if UNITY_EDITOR
            m_cookies = new Texture2D[16];
            m_cookies[0] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_001.tif"));
            m_cookies[1] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_002.tif"));
            m_cookies[2] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_003.tif"));
            m_cookies[3] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_004.tif"));
            m_cookies[4] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_005.tif"));
            m_cookies[5] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_006.tif"));
            m_cookies[6] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_007.tif"));
            m_cookies[7] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_008.tif"));
            m_cookies[8] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_009.tif"));
            m_cookies[9] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_010.tif"));
            m_cookies[10] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_011.tif"));
            m_cookies[11] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_012.tif"));
            m_cookies[12] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_013.tif"));
            m_cookies[13] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_014.tif"));
            m_cookies[14] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_015.tif"));
            m_cookies[15] = AssetDatabase.LoadAssetAtPath<Texture2D>(GaiaUtils.GetAssetPath("CausticsRender_016.tif"));

            m_submergeSoundFXDown = AssetDatabase.LoadAssetAtPath<AudioClip>(GaiaUtils.GetAssetPath("Gaia Ambient Submerge Down.mp3"));
            m_submergeSoundFXUp = AssetDatabase.LoadAssetAtPath<AudioClip>(GaiaUtils.GetAssetPath("Gaia Ambient Submerge Up.mp3"));
            underwaterParticlesStored = AssetDatabase.LoadAssetAtPath<GameObject>(GaiaUtils.GetAssetPath("Underwater Particles Effects.prefab"));
            horizonObjectStored = AssetDatabase.LoadAssetAtPath<GameObject>(GaiaUtils.GetAssetPath("Ambient Underwater Horizon.prefab"));
            #endif
        }
        #endregion
    }
}
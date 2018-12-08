#if !AMBIENT_SKIES

using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEditor.Rendering;
using System;
#if UNITY_POST_PROCESSING_STACK_V2
using UnityEngine.Rendering.PostProcessing;
#endif

namespace Gaia.GX.ProceduralWorlds
{
    public class AmbientSkiesSamples : MonoBehaviour
    {
        #region Generic informational methods

        /// <summary>
        /// Returns the publisher name if provided. 
        /// This will override the publisher name in the namespace ie Gaia.GX.PublisherName
        /// </summary>
        /// <returns>Publisher name</returns>
        public static string GetPublisherName()
        {
            return "Procedural Worlds";
        }

        /// <summary>
        /// Returns the package name if provided
        /// This will override the package name in the class name ie public class PackageName.
        /// </summary>
        /// <returns>Package name</returns>
        public static string GetPackageName()
        {
            return "Ambient Skies Samples";
        }

        #endregion

        #region Methods exposed by Gaia as buttons must be prefixed with GX_
        //Says what the product is about
        public static void GX_About()
        {
            EditorUtility.DisplayDialog("About Ambient Skies Samples", "Ambient Skies Samples allows you to give your scene a quick makeover. Using HDRI skyboxes and quick time of day setups allows you to view your scene in different lighting. Also enjoy our own simple water included with gaia, a very lightweight shader allows you to view your scene and experiment with water. Lastly we offer post processing v2 support this system allows you to use the new stacks v2 setup and it's automatically sets this up for you in your scene and auto sets for the time of day. Note: Post processing only works in 5.6.1f1 or higher and MSVO Ambient Occlusion only works in 2017.1 or higher", "OK");
        }

        //Links to full version of the product
        public static void GX_GetFullVersion()
        {
            EditorUtility.DisplayDialog("Ambient Skies Full Version", "Ambient Skies full version coming soon!", "OK");
            //Application.OpenURL("LinkGoesHere");
        }

        //Sets the time of day to morning
        public static void GX_Skies_Morning()
        {
            SetHDRISky(new Vector3(171.5f, 0f, 0f), "FFDFCA00", 1.14f, "Ambient Skies Sample Skybox", "AmbientSkiesSampleMorningAndEvening", "ABA49900", 0.65f, 0f, "88715C00", 5f, 1.44f, "A3A99E", "767266", "6C6E70");
            SetPostProcessingStyle("Ambient Sample Default Morning Post Processing");
            SetAmbientAudio("Gaia Ambient Audio Morning");
            SetUnderwaterFogSettings(RenderSettings.fogColor, RenderSettings.fogEndDistance);
            GetOrCreateReflectionProbe("Global Reflection Probe");
            BakeGlobalReflectionProbe(true);
        }

        //Sets the time of day to day
        public static void GX_Skies_Day()
        {
            SetHDRISky(new Vector3(70f, 180f, 0f), "FFF6E9FF", 1.2f, "Ambient Skies Sample Skybox", "AmbientSkiesSampleDay", "5A5A5AFF", 1.2f, 0f, "C8CDCDFF", -25f, 1f, "B7C1C9", "A9A395", "8E8166");
            SetPostProcessingStyle("Ambient Sample Default Day Post Processing");
            SetAmbientAudio("Gaia Ambient Audio Day");
            SetUnderwaterFogSettings(RenderSettings.fogColor, RenderSettings.fogEndDistance);
            GetOrCreateReflectionProbe("Global Reflection Probe");
            BakeGlobalReflectionProbe(true);
        }

        //Sets the time of day to morning
        public static void GX_Skies_Evening()
        {
            SetHDRISky(new Vector3(10f, 0f, 0f), "FFDFCA00", 1.14f, "Ambient Skies Sample Skybox", "AmbientSkiesSampleMorningAndEvening", "B4B4B400", 0.65f, 180f, "8C7B6D00", 5f, 1.2f, "9E8F75", "726E61", "7E7474");
            SetPostProcessingStyle("Ambient Sample Default Evening Post Processing");
            SetAmbientAudio("Gaia Ambient Audio Evening");
            SetUnderwaterFogSettings(RenderSettings.fogColor, RenderSettings.fogEndDistance);
            GetOrCreateReflectionProbe("Global Reflection Probe");
            BakeGlobalReflectionProbe(true);
        }

        //Sets the time of day to night
        public static void GX_Skies_Night()
        {
            SetHDRISky(new Vector3(135f, 0f, 0f), "ABC8FFFF", 0.7f, "Ambient Skies Sample Skybox", "AmbientSkiesSampleNight", "404040FF", 1f, 0f, "1A1F2FFF", 50f, 2f, "2A303A", "4D4E51", "4B4B4B");
            SetPostProcessingStyle("Ambient Sample Default Night Post Processing");
            SetAmbientAudio("Gaia Ambient Audio Night");
            SetUnderwaterFogSettings(RenderSettings.fogColor, RenderSettings.fogEndDistance);
            GetOrCreateReflectionProbe("Global Reflection Probe");
            BakeGlobalReflectionProbe(true);
        }

        //Sets the time of day to default procedural skybox
        public static void GX_Skies_DefaultProcedural()
        {
            GameObject parentObject = GetOrCreateEnvironmentParent();
            GameObject lightObject = GetOrCreateDirectionalLight();
            lightObject.transform.parent = parentObject.transform;
            lightObject.transform.localRotation = Quaternion.Euler(50f, -30f, 0f);
            Light light = lightObject.GetComponent<Light>();
            if (light != null)
            {
                light.color = new Color32(255, 244, 214, 255);
                light.intensity = 1f;
                RenderSettings.sun = light;
            }

            //Set the skybox material
            string skyMatPath = GetAssetPath("Ambient Skies Default Sky");
            if (!string.IsNullOrEmpty(skyMatPath))
            {
                #if UNITY_EDITOR
                RenderSettings.skybox = AssetDatabase.LoadAssetAtPath<Material>(skyMatPath);
                #endif
            }

            //Set render settings
            if (!Application.isPlaying)
            {
                RenderSettings.ambientMode = AmbientMode.Skybox;
            }
            else
            {
                RenderSettings.ambientMode = AmbientMode.Trilight;
                RenderSettings.ambientSkyColor = GetColorFromHTML("B7C1C9");
                RenderSettings.ambientEquatorColor = GetColorFromHTML("A9A395");
                RenderSettings.ambientGroundColor = GetColorFromHTML("8E8166");
            }

            //Set the fog 
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color32(218, 224, 225, 255);
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = -50f;
            Terrain theTerrain = GetActiveTerrain();
            if (theTerrain != null)
            {
                RenderSettings.fogEndDistance = theTerrain.terrainData.size.y / 1.25f;
            }
            else
            {
                RenderSettings.fogEndDistance = 1000f / 1.25f;
            }

            SetPostProcessingStyle("Ambient Sample Default Day Post Processing");
            SetAmbientAudio("Gaia Ambient Audio Day");
            SetUnderwaterFogSettings(RenderSettings.fogColor, RenderSettings.fogEndDistance);
            GetOrCreateReflectionProbe("Global Reflection Probe");           
            BakeGlobalReflectionProbe(true);
        }

        //Adds global reflection probe to your scene
        public static void GX_Skies_AddGlobalReflectionProbe()
        {
            GetOrCreateReflectionProbe("Global Reflection Probe");
        }

        //Removes the global reflection probe from your scene
        public static void GX_Skies_RemoveGlobalReflectionProbe()
        {
            RemoveGlobalProbe("Global Reflection Probe");
            RemoveGlobalProbe("Camera Reflection Probe");
        }

        //Bakes the lighting for your current open scene
        public static void GX_Skies_BakeLighting()
        {
            if (!Lightmapping.isRunning)
            {
                RenderSettings.ambientMode = AmbientMode.Skybox;
                Lightmapping.BakeAsync();
            }
        }

        //Sets postprocessing to blockbuster1
        public static void GX_PostProcessing_DefaultMorning()
        {
            SetPostProcessingStyle("Ambient Sample Default Morning Post Processing");
        }

        //Sets postprocessing to default
        public static void GX_PostProcessing_DefaultDay()
        {
            SetPostProcessingStyle("Ambient Sample Default Day Post Processing");
        }

        //Sets postprocessing to real low contrast
        public static void GX_PostProcessing_DefaultEvening()
        {
            SetPostProcessingStyle("Ambient Sample Default Evening Post Processing");
        }

        //Sets postprocessing to vibrant1
        public static void GX_PostProcessing_DefaultNight()
        {
            SetPostProcessingStyle("Ambient Sample Default Night Post Processing");
        }

        //Removes post processing v2 from your scene
        public static void GX_PostProcessing_RemovePostProcessing()
        {
            RemovePostProcessingV2();
        }

        //Adds new gaia water to your scene
        public static void GX_Water_AddWater()
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning("Can only add water when application is not playing!");
                return;
            }

            //Get relevant information
            GaiaSceneInfo sceneInfo = GaiaSceneInfo.GetSceneInfo();
            GameObject parentObject = GetOrCreateEnvironmentParent();
            Terrain activeTerrain = GetActiveTerrain();
            Material waterMaterial = GetWaterMaterial("Ambient Water Sample Material");

            //Get main directional light and make sure its set up properly
            var lightObject = GetOrCreateDirectionalLight();
            var effectsSettings = lightObject.GetComponent<GaiaUnderWaterEffects>();
            if (lightObject.GetComponent<GaiaUnderWaterEffects>() == null)
            {
                effectsSettings = lightObject.AddComponent<GaiaUnderWaterEffects>();
            }
            effectsSettings.m_causticsSize = 5;
            effectsSettings.m_followPlayer = false;
            effectsSettings.m_framesPerSecond = 25f;
            effectsSettings.m_sealevel = sceneInfo.m_seaLevel;
            effectsSettings.m_underWaterFogColor = new Color32(76, 112, 142, 255);
            effectsSettings.m_underWaterFogDistance = 65f;
            effectsSettings.LoadCaustics();
            #if UNITY_EDITOR
            effectsSettings.player = effectsSettings.GetThePlayer();
            #endif

            #if UNITY_POST_PROCESSING_STACK_V2
            var underwaterPostFxObject = GameObject.Find("Underwater PostFX");
            if (underwaterPostFxObject == null)
            {
                underwaterPostFxObject = new GameObject("Underwater PostFX");
                underwaterPostFxObject.transform.position = new Vector3(0f, sceneInfo.m_seaLevel - 506.5f, 0f);
                underwaterPostFxObject.transform.parent = parentObject.transform;
                underwaterPostFxObject.layer = LayerMask.NameToLayer("TransparentFX");

                //Add the pp volume
                var ppVol = underwaterPostFxObject.AddComponent<PostProcessVolume>();
                ppVol.sharedProfile = AssetDatabase.LoadAssetAtPath<PostProcessProfile>(GetAssetPath("Ambient Sample Underwater Post Processing"));
                ppVol.blendDistance = 4f;
                ppVol.priority = 1;

                var colliderSettings = underwaterPostFxObject.AddComponent<BoxCollider>();
                if (activeTerrain != null)
                {
                    colliderSettings.size = new Vector3(activeTerrain.terrainData.size.x * 1.5f, 1000f, activeTerrain.terrainData.size.z * 1.5f);
                }
                else
                {
                    colliderSettings.size = new Vector3(2560f, 1000f, 2560f);
                }
                colliderSettings.isTrigger = true;
            }

            var underwaterTransitionFXObject = GameObject.Find("Underwater Transition PostFX");
            if (underwaterTransitionFXObject == null)
            {
                underwaterTransitionFXObject = new GameObject("Underwater Transition PostFX");
                underwaterTransitionFXObject.transform.position = new Vector3(0f, sceneInfo.m_seaLevel, 0f);
                underwaterTransitionFXObject.transform.parent = parentObject.transform;
                underwaterTransitionFXObject.layer = LayerMask.NameToLayer("TransparentFX");

                var ppVol = underwaterTransitionFXObject.AddComponent<PostProcessVolume>();
                ppVol.sharedProfile = AssetDatabase.LoadAssetAtPath<PostProcessProfile>(GetAssetPath("Ambient Sample Underwater Transaction Post Processing"));
                ppVol.blendDistance = 0.15f;
                ppVol.priority = 2;

                var colliderSettings = underwaterTransitionFXObject.AddComponent<BoxCollider>();
                if (activeTerrain != null)
                {
                    colliderSettings.size = new Vector3(activeTerrain.terrainData.size.x * 1.5f, 0.1f, activeTerrain.terrainData.size.z * 1.5f);
                }
                else
                {
                    colliderSettings.size = new Vector3(2560f, 0.1f, 2560f);
                }
                colliderSettings.isTrigger = true;
            }
            #endif

            var underwaterAudioFXObject = GameObject.Find("Underwater SoundFX");
            Terrain terrain = GetActiveTerrain();
            if (underwaterAudioFXObject == null)
            {
                underwaterAudioFXObject = new GameObject("Underwater SoundFX");
                underwaterAudioFXObject.transform.parent = parentObject.transform;
                var audio = underwaterAudioFXObject.AddComponent<AudioSource>();
                audio.clip = AssetDatabase.LoadAssetAtPath<AudioClip>(GetAssetPath("Gaia Ambient Underwater Sound Effect"));
                audio.volume = 0f;
                audio.loop = true;

                if (terrain != null)
                {
                    audio.maxDistance = terrain.terrainData.size.x * 1.5f;
                }
                else
                {
                    audio.maxDistance = 3000f;
                }
            }

            //Grab or create the water
            GameObject theWaterObject = GameObject.Find("Ambient Water Sample");
            if (theWaterObject == null)
            {
                theWaterObject = Instantiate(GetAssetPrefab("Ambient Water Sample"));
                theWaterObject.name = "Ambient Water Sample";
                theWaterObject.transform.parent = parentObject.transform;
            }

            //And update it
            Vector3 waterPosition = sceneInfo.m_centrePointOnTerrain;
            waterPosition.y = sceneInfo.m_seaLevel;
            theWaterObject.transform.position = waterPosition;
            if (activeTerrain != null)
            {
                theWaterObject.transform.localScale = new Vector3(sceneInfo.m_sceneBounds.size.x, 1f, sceneInfo.m_sceneBounds.size.z);
            }
            else 
            {
                theWaterObject.transform.localScale = new Vector3(256f, 1f, 256f);
            }

            //Update water material
            if (waterMaterial != null)
            {
                if (activeTerrain != null)
                {
                    waterMaterial.SetFloat("_GlobalTiling", sceneInfo.m_sceneBounds.size.x);
                }
                else
                {
                    waterMaterial.SetFloat("_GlobalTiling", 128f);
                }
            }

            //Adds reflection probe updater script if missing
            GetOrCreateReflectionProbe("Camera Reflection Probe");
            GameObject theReflectionProbeObject = GameObject.Find("Camera Reflection Probe");
            if (theReflectionProbeObject != null)
            {
                if (theReflectionProbeObject.GetComponent<GaiaReflectionProbeUpdate>() == null)
                {
                    GaiaReflectionProbeUpdate probeObject = theReflectionProbeObject.AddComponent<GaiaReflectionProbeUpdate>();
                    probeObject.m_followCamera = true;
                    probeObject.SetProbeSettings();
                }
            }
        }

        //Sets the water settings to deep blue
        public static void GX_Water_DeepBlueStyle()
        {
            SetWater(0.97f, 0.15f, "4C708EFF", 0.31f, 1f, 1f, "878787FF", 0.4f);
            SetTheUnderwaterFogColor("4C708EFF");
        }

        //Sets the water settings to clear blue
        public static void GX_Water_ClearBlueStyle()
        {
            SetWater(0.97f, 0.15f, "4781B2FF", 0.5f, 1f, 1f, "878787FF", 0.4f);
            SetTheUnderwaterFogColor("7C97AEFF");
        }

        //Sets the water settings to toxic green
        public static void GX_Water_ToxicGreenStyle()
        {
            SetWater(0.985f, 0.15f, "257C31FF", 0.31f, 1f, 1f, "878787FF", 0.4f);
            SetTheUnderwaterFogColor("6C9E77FF");
        }

        //Sets the water settings to cyan
        public static void GX_Water_CyanStyle()
        {
            SetWater(0.945f, 0.15f, "4BA3A7FF", 0.3f, 1f, 1f, "878787FF", 0.4f);
            SetTheUnderwaterFogColor("6C9E94FF");
        }

        //Removes new gaia water from your scene
        public static void GX_Water_RemoveWater()
        {
            if (!Application.isPlaying)
            {
                GameObject theWaterObject = GameObject.Find("Ambient Water Sample");
                if (theWaterObject != null)
                {
                    DestroyImmediate(theWaterObject);
                }

                GameObject underwaterFX = GameObject.Find("Underwater PostFX");
                if (underwaterFX != null)
                {
                    DestroyImmediate(underwaterFX);
                }

                GaiaUnderWaterEffects underwaterFXScript = FindObjectOfType<GaiaUnderWaterEffects>();
                if (underwaterFXScript != null)
                {
                    DestroyImmediate(underwaterFXScript);
                }

                GameObject underwaterAudioFX = GameObject.Find("Underwater SoundFX");
                if (underwaterAudioFX != null)
                {
                    DestroyImmediate(underwaterAudioFX);
                }

                GameObject underwaterTransactionPostFX = GameObject.Find("Underwater Transition PostFX");
                if (underwaterTransactionPostFX != null)
                {
                    DestroyImmediate(underwaterTransactionPostFX);
                }

                GaiaReflectionProbeUpdate reflectionProbeUpdater = FindObjectOfType<GaiaReflectionProbeUpdate>();
                if (reflectionProbeUpdater != null)
                {
                    DestroyImmediate(reflectionProbeUpdater);
                }
            }
        }

        //Removes new gaia underwater FX from your scene
        public static void GX_Water_RemoveUnderwaterFx()
        {
            if (!Application.isPlaying)
            {
                GameObject underwaterFX = GameObject.Find("Underwater PostFX");
                if (underwaterFX != null)
                {
                    DestroyImmediate(underwaterFX);
                }

                GaiaUnderWaterEffects underwaterFXScript = FindObjectOfType<GaiaUnderWaterEffects>();
                if (underwaterFXScript != null)
                {
                    DestroyImmediate(underwaterFXScript);
                }

                GameObject underwaterAudioFX = GameObject.Find("Underwater SoundFX");
                if (underwaterAudioFX != null)
                {
                    DestroyImmediate(underwaterAudioFX);
                }

                GameObject underwaterTransactionPostFX = GameObject.Find("Underwater Transition PostFX");
                if (underwaterTransactionPostFX != null)
                {
                    DestroyImmediate(underwaterTransactionPostFX);
                }

                GaiaReflectionProbeUpdate reflectionProbeUpdater = FindObjectOfType<GaiaReflectionProbeUpdate>();
                if (reflectionProbeUpdater != null)
                {
                    DestroyImmediate(reflectionProbeUpdater);
                }
            }
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Get the asset path of the first thing that matches the name
        /// </summary>
        /// <param name="name">Name to search for</param>
        /// <returns></returns>
        private static string GetAssetPath(string name)
        {
            string[] assets = AssetDatabase.FindAssets(name, null);
            if (assets.Length > 0)
            {
                return AssetDatabase.GUIDToAssetPath(assets[0]);
            }
            return null;
        }

        /// <summary>
        /// Bakes global lighting
        /// </summary>
        public static void BakeGlobalLighting()
        {
            //Bakes the lightmaps
            if (!Application.isPlaying && !Lightmapping.isRunning)
            {
                Lightmapping.BakeAsync();
            }
        }

        /// <summary>
        /// Sets the fog settings for underwater sotred settings
        /// </summary>
        /// <param name="fogColor"></param>
        /// <param name="fogEndDistance"></param>
        public static void SetUnderwaterFogSettings(Color32 fogColor, float fogEndDistance)
        {
            GaiaUnderWaterEffects underWaterFXSettings = FindObjectOfType<GaiaUnderWaterEffects>();
            if (underWaterFXSettings != null)
            {
                underWaterFXSettings.storedFogColor = fogColor;
                underWaterFXSettings.storedFogDistance = fogEndDistance;
            }
        }

        /// <summary>
        /// Sets sets the underwater fogColor
        /// </summary>
        /// <param name="fogColor"></param>
        public static void SetTheUnderwaterFogColor (string fogColor)
        {
            GaiaUnderWaterEffects underWaterFXSettings = FindObjectOfType<GaiaUnderWaterEffects>();
            if (underWaterFXSettings != null)
            {
                underWaterFXSettings.m_underWaterFogColor = GetColorFromHTML(fogColor);
            }
        }

        /// <summary>
        /// Bake our global reflection probe - if no lighting baked yet and requested then a global bake is kicked off
        /// </summary>
        /// <param name="doGlobalBakeIfNecessary">If no previous bake has been done then a global bake will be kicked off</param>
        public static void BakeGlobalReflectionProbe(bool doGlobalBakeIfNecessary)
        {
            if (Lightmapping.isRunning)
            {
                return;
            }

            //Get global reflection probe
            ReflectionProbe[] reflectionProbes = FindObjectsOfType<ReflectionProbe>();
            if (reflectionProbes == null || reflectionProbes.Length == 0)
            {
                return;
            }

            GameObject reflectionProbeObject = GameObject.Find("Global Reflection Probe");
            if (reflectionProbeObject != null)
            {
                var probe = reflectionProbeObject.GetComponent<ReflectionProbe>();
                if (probe.mode == ReflectionProbeMode.Baked)
                {

                    if (probe.bakedTexture == null)
                    {
                        if (doGlobalBakeIfNecessary)
                        {
                            BakeGlobalLighting();
                        }
                        return;
                    }

                    BakeAmbientLight();
                    Lightmapping.BakeReflectionProbe(probe, AssetDatabase.GetAssetPath(probe.bakedTexture));
                }
                else
                {
                    probe.RenderProbe();
                }
            }
        }

        /// <summary>
        /// Bakes ambient lighting
        /// </summary>
        public static void BakeAmbientLight()
        {
            if (Lightmapping.lightingDataAsset == null)
            {
                Debug.Log("No baked probes found, have you baked your lighting?");
                return;
            }

            if (Lightmapping.lightingDataAsset != null)
            {
                Color Ambient = RenderSettings.ambientSkyColor;
                Light[] Lights = FindObjectsOfType<Light>();

                SphericalHarmonicsL2[] bakedProbes = LightmapSettings.lightProbes.bakedProbes;
                Vector3[] probePositions = LightmapSettings.lightProbes.positions;
                int probeCount = LightmapSettings.lightProbes.count;

                // Clear all probes
                for (int i = 0; i < probeCount; i++)
                {
                    if (i < 0)
                    {
                        return;
                    }
                }

                for (int i = 0; i < probeCount; i++)
                {
                    if (i > 15)
                    {
                        bakedProbes[i].Clear();
                    }
                }
                
                // Add ambient light to all probes
                for (int i = 0; i < probeCount; i++)
                {
                    if (i > 15)
                    {
                        bakedProbes[i].AddAmbientLight(Ambient);
                    }
                }

                // Add directional and point lights' contribution to all probes
                foreach (Light allLights in Lights)
                {
                    if (allLights.type == LightType.Directional)
                    {
                        for (int dirLight = 0; dirLight < probeCount; dirLight++)
                        {
                            if (dirLight > 15)
                            {
                                bakedProbes[dirLight].AddDirectionalLight(-allLights.transform.forward, allLights.color, allLights.intensity);
                            }
                        }
                    }
                    else if (allLights.type == LightType.Point)
                    {
                        for (int poiLight = 0; poiLight < probeCount; poiLight++)
                        {
                            if (poiLight > 15)
                            {
                                SHAddPointLight(probePositions[poiLight], allLights.transform.position, allLights.range, allLights.color, allLights.intensity, ref bakedProbes[poiLight]);
                            }
                        }                           
                    }
                }

                //Set the bake probes settings to new setup
                if (LightmapSettings.lightProbes == null)
                {
                    LightmapSettings.lightProbes.bakedProbes = bakedProbes;
                }
            }
        }

        /// <summary>
        /// Bake SphericalHarmonicsL2 values
        /// </summary>
        /// <param name="probePosition"></param>
        /// <param name="position"></param>
        /// <param name="range"></param>
        /// <param name="color"></param>
        /// <param name="intensity"></param>
        /// <param name="shL2"></param>
        public static void SHAddPointLight(Vector3 probePosition, Vector3 position, float range, Color color, float intensity, ref SphericalHarmonicsL2 sphericalHarmonicsL2)
        {
            Vector3 probeToLight = position - probePosition;
            float attenuation = 1.0F / (1.0F + 25.0F * probeToLight.sqrMagnitude / (range * range));
            sphericalHarmonicsL2.AddDirectionalLight(probeToLight.normalized, color, intensity * attenuation);
        }

        /// <summary>
        /// Get the asset prefab if we can find it in the project
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject GetAssetPrefab(string name)
        {
            string[] assets = AssetDatabase.FindAssets(name, null);
            for (int idx = 0; idx < assets.Length; idx++)
            {
                string path = AssetDatabase.GUIDToAssetPath(assets[idx]);
                if (path.Contains(".prefab"))
                {
                    return AssetDatabase.LoadAssetAtPath<GameObject>(path);
                }
            }
            return null;
        }

        /// <summary>
        /// Adds ambiance audio to the scene 
        /// </summary>
        /// <param name="audioName"></param>
        public static void SetAmbientAudio(string audioName)
        {
            string audioFile = GetAssetPath(audioName);
            if (string.IsNullOrEmpty(audioFile))
            {
                Debug.LogWarning("Audio " + audioFile + " is missing");
                return;
            }

            GameObject audioSource = GameObject.Find("Ambient Audio");
            if (audioSource == null)
            {
                audioSource = new GameObject("Ambient Audio");
                audioSource.transform.parent = GetOrCreateEnvironmentParent().transform;
                audioSource.AddComponent<AudioSource>();
            }

            AudioSource theAudioSource = audioSource.GetComponent<AudioSource>();
            theAudioSource.clip = AssetDatabase.LoadAssetAtPath<AudioClip>(audioFile);
            theAudioSource.volume = 0.5f;            
            theAudioSource.loop = true;

            Terrain terrain = GetActiveTerrain();
            if (terrain != null)
            {
                theAudioSource.maxDistance = GetActiveTerrain().terrainData.size.x * 1.1f;
            }
            else
            {
                theAudioSource.maxDistance = 3000f;
            }

            if (Application.isPlaying)
            {
                theAudioSource.Play();
            }
        }

        /// <summary>
        /// Get the currently active terrain - or any terrain
        /// </summary>
        /// <returns>A terrain if there is one</returns>
        public static Terrain GetActiveTerrain()
        {
            //Grab active terrain if we can
            Terrain terrain = Terrain.activeTerrain;
            if (terrain != null && terrain.isActiveAndEnabled)
            {
                return terrain;
            }

            //Then check rest of terrains
            for (int idx = 0; idx < Terrain.activeTerrains.Length; idx++)
            {
                terrain = Terrain.activeTerrains[idx];
                if (terrain != null && terrain.isActiveAndEnabled)
                {
                    return terrain;
                }
            }
            return null;
        }

        /// <summary>
        /// Get or create the main scene camera
        /// </summary>
        /// <returns>The gameobject camera</returns>
        private static GameObject GetOrCreateMainCamera()
        {
            GameObject mainCameraObject = GameObject.Find("Main Camera");
            if (mainCameraObject != null)
            {
                return mainCameraObject;
            }

            mainCameraObject = GameObject.Find("Camera");
            if (mainCameraObject != null)
            {
                return mainCameraObject;
            }

            mainCameraObject = GameObject.Find("FirstPersonCharacter");
            if (mainCameraObject != null)
            {
                return mainCameraObject;
            }

            mainCameraObject = GameObject.Find("FlyCam");
            if (mainCameraObject != null)
            {
                return mainCameraObject;
            }

            if (Camera.main != null)
            {
                mainCameraObject = Camera.main.gameObject;
                return mainCameraObject;
            }

            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
            foreach (var camera in cameras)
            {
                return camera.gameObject;
            }

            //Create new camera
            mainCameraObject = new GameObject("Main Camera");
            mainCameraObject.AddComponent<Camera>();
#if !UNITY_2017_1_OR_NEWER
            mainCameraObject.AddComponent<GUILayer>();
#endif
            mainCameraObject.AddComponent<FlareLayer>();
            mainCameraObject.AddComponent<AudioListener>();
            mainCameraObject.tag = "MainCamera";

            return mainCameraObject;
        }

        /// <summary>
        /// Get or create a directional light
        /// </summary>
        /// <returns>First directional light it finds, or a new one</returns>
        private static GameObject GetOrCreateDirectionalLight()
        {
            GameObject lightObject = GameObject.Find("Directional Light");
            if (lightObject == null)
            {
                //Check to see if we have one in the scene
                Light[] lights = GameObject.FindObjectsOfType<Light>();
                foreach (var light in lights)
                {
                    if (light.type == LightType.Directional)
                    {
                        return light.gameObject;
                    }
                }

                //Create a new one
                lightObject = new GameObject("Directional Light");
                Light newLight = lightObject.AddComponent<Light>();
                newLight.type = LightType.Directional;
                newLight.shadows = LightShadows.Soft;
            }
            return lightObject;
        }

        /// <summary>
        /// Get or create the environment parent object - all environment stuff added to this
        /// </summary>
        /// <returns>Existing or new environment parent</returns>
        private static GameObject GetOrCreateEnvironmentParent()
        {
            GameObject parent = GameObject.Find("Gaia Environment");
            if (parent == null)
            {
                parent = new GameObject("Gaia Environment");
            }
            return parent;
        }

        /// <summary>
        /// Setup the sky settings using trilight ambient and skybox ambient
        /// </summary>
        /// <param name="sunRotation"></param>
        /// <param name="sunColor"></param>
        /// <param name="sunIntensity"></param>
        /// <param name="skyMaterial"></param>
        /// <param name="hdrSkyTexture"></param>
        /// <param name="skyTint"></param>
        /// <param name="skyExposure"></param>
        /// <param name="skyRotation"></param>
        /// <param name="fogColor"></param>
        /// <param name="fogStartDistance"></param>
        /// <param name="skyGroundIntensity"></param>
        /// <param name="skyColor"></param>
        /// <param name="equatorColor"></param>
        /// <param name="groundColor"></param>
        private static void SetHDRISky(Vector3 sunRotation, string sunColor, float sunIntensity, string skyMaterial, string hdrSkyTexture, string skyTint, float skyExposure, float skyRotation, string fogColor, float fogStartDistance, float skyGroundIntensity, string skyColor, string equatorColor, string groundColor)
        {
            GameObject parentObject = GetOrCreateEnvironmentParent();
            GameObject lightObject = GetOrCreateDirectionalLight();

            lightObject.transform.parent = parentObject.transform;
            lightObject.transform.localRotation = Quaternion.Euler(sunRotation);
            Light light = lightObject.GetComponent<Light>();
            if (light != null)
            {
                light.color = GetColorFromHTML(sunColor);
                light.intensity = sunIntensity;
                RenderSettings.sun = light;
            }

            //Set the skybox material
#if UNITY_EDITOR
            string skyMatPath = GetAssetPath(skyMaterial);
            string hdrSkyPath = GetAssetPath(hdrSkyTexture);
            if (!string.IsNullOrEmpty(skyMatPath) && !string.IsNullOrEmpty(hdrSkyPath))
            {
                Material hdrSkyMaterial = AssetDatabase.LoadAssetAtPath<Material>(skyMatPath);
                hdrSkyMaterial.SetColor("_Tint", GetColorFromHTML(skyTint));
                hdrSkyMaterial.SetFloat("_Exposure", skyExposure);
                hdrSkyMaterial.SetFloat("_Rotation", skyRotation);
                hdrSkyMaterial.SetTexture("_Tex", AssetDatabase.LoadAssetAtPath<Texture>(hdrSkyPath));
                RenderSettings.skybox = hdrSkyMaterial;
            }
#endif

            //Set render settings
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = GetColorFromHTML(skyColor);
            RenderSettings.ambientEquatorColor = GetColorFromHTML(equatorColor);
            RenderSettings.ambientGroundColor = GetColorFromHTML(groundColor);
            RenderSettings.fog = true;
            RenderSettings.fogColor = GetColorFromHTML(fogColor);
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = -fogStartDistance;

            //Gets far clip plane distance from camera
            float fogDistance = GetOrCreateMainCamera().GetComponent<Camera>().farClipPlane;
            //If fogDistance is below the min clamp value it's default to 1500
            if (fogDistance < 250)
            {
                RenderSettings.fogEndDistance = 1500f;
            }
            //If fogDistance great or equals to min clamp value it'll assign the value fogDistance and clamp it
            else
            {
                RenderSettings.fogEndDistance = Mathf.Clamp(fogDistance, 250f, 2300f);
            }

            GaiaReflectionProbeUpdate[] rpuArray = FindObjectsOfType<GaiaReflectionProbeUpdate>();
            foreach(GaiaReflectionProbeUpdate rpu in rpuArray)
            {
                if (rpu.gameObject.name == "Camera Reflection Probe")
                {                   
                    rpu.SetProbeSettings();
                }
            }
        }

        /// <summary>
        /// Get a color from a html string
        /// </summary>
        /// <param name="htmlString">Color in RRGGBB or RRGGBBBAA or #RRGGBB or #RRGGBBAA format.</param>
        /// <returns>Color or white if unable to parse it.</returns>
        public static Color GetColorFromHTML(string htmlString)
        {
            Color color = Color.white;
            if (!htmlString.StartsWith("#"))
            {
                htmlString = "#" + htmlString;
            }
            if (!ColorUtility.TryParseHtmlString(htmlString, out color))
            {
                color = Color.white;
            }
            return color;
        }

        /// <summary>
        /// Set the post processing in the scene
        /// </summary>
        private static void SetPostProcessingStyle(string ppName)
        {
            //Hack to not set PP on ultralight and mobile - because it is too expensive
            GaiaSettings settings = GaiaUtils.GetGaiaSettings();
            if (settings.m_currentEnvironment == GaiaConstants.EnvironmentTarget.UltraLight ||
                settings.m_currentEnvironment == GaiaConstants.EnvironmentTarget.MobileAndVR)
            {
                return;
            }

            #if UNITY_5_6_0
                return;
            #endif

            #if UNITY_POST_PROCESSING_STACK_V2
            RemovePostProcessingV1();
            GameObject theParentObject = GetOrCreateEnvironmentParent();
            GameObject postProcessingVolumeObject = GameObject.Find("Global Post Processing");
            GameObject mainCameraObject = GetOrCreateMainCamera();

            //If the post processing volume is null it creates one
            if (postProcessingVolumeObject == null)
            {
                postProcessingVolumeObject = new GameObject("Global Post Processing");
                postProcessingVolumeObject.transform.parent = theParentObject.transform;
                postProcessingVolumeObject.layer = LayerMask.NameToLayer("TransparentFX");

                var ppVol = postProcessingVolumeObject.AddComponent<PostProcessVolume>();
                ppVol.isGlobal = true;
                ppVol.priority = 0f;
                ppVol.sharedProfile = AssetDatabase.LoadAssetAtPath<PostProcessProfile>(GetAssetPath(ppName));
                ppVol.weight = 1f;
                ppVol.blendDistance = 0f;
                UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(ppVol, false);
            }                 
            else
            {
                var ppVol = postProcessingVolumeObject.GetComponent<PostProcessVolume>();
                ppVol.isGlobal = true;
                ppVol.priority = 0f;
                ppVol.sharedProfile = AssetDatabase.LoadAssetAtPath<PostProcessProfile>(GetAssetPath(ppName));
                ppVol.weight = 1f;
                ppVol.blendDistance = 0f;
                UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(ppVol, false);
            }

            //Set the main camera to support post processing
            mainCameraObject.GetComponent<Camera>().renderingPath = RenderingPath.UsePlayerSettings;
            mainCameraObject.GetComponent<Camera>().allowMSAA = false;

            var ppLayer = mainCameraObject.GetComponent<PostProcessLayer>();
            if (ppLayer == null)
            {
                ppLayer = mainCameraObject.AddComponent<PostProcessLayer>();
                ppLayer.volumeTrigger = mainCameraObject.transform;
                ppLayer.volumeLayer = 2;
                #if !UNITY_2017_1_OR_NEWER
                ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                #else
                ppLayer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                #endif
                ppLayer.fog.excludeSkybox = true;
                ppLayer.fog.enabled = true;
                ppLayer.stopNaNPropagation = true;
            }
            #endif
        }

        /// <summary>
        /// Removes Gaia post processing V2
        /// </summary>
        private static void RemovePostProcessingV2()
        {
            #if UNITY_POST_PROCESSING_STACK_V2
            GameObject mainCameraObject = GetOrCreateMainCamera();
            PostProcessLayer ppLayer = mainCameraObject.GetComponent<PostProcessLayer>();
            if (ppLayer != null)
            {
                DestroyImmediate(ppLayer);
            }

            GameObject postProcessVolumeObject = GameObject.Find("Global Post Processing");
            if (postProcessVolumeObject != null)
            {
                DestroyImmediate(postProcessVolumeObject);
            }
            #endif
        }

        /// <summary>
        /// Removes Gaia post processing V1
        /// </summary>
        private static void RemovePostProcessingV1()
        {
            #if UNITY_5_5_OR_NEWER
            //If post processing volume not in project exit.
            Type postProcessingBehaviourType = GaiaCommon1.Utils.GetType("UnityEngine.PostProcessing.PostProcessingBehaviour");
            if (postProcessingBehaviourType == null)
            {
                return;
            }

            //Find camera
            GameObject cameraObject = GetOrCreateMainCamera();
            var postProcessingBehaviour = cameraObject.GetComponent(postProcessingBehaviourType);
            if (postProcessingBehaviour != null)
            {
                DestroyImmediate(postProcessingBehaviour);
            }
            #endif
        }

        /// <summary>
        /// Removes the probeName from the scene
        /// </summary>
        /// <param name="probeName"></param>
        private static void RemoveGlobalProbe(string probeName)
        {
            GameObject probeAsset = GameObject.Find(probeName);

            if (probeAsset != null)
            {
                DestroyImmediate(probeAsset);
            }
        }

        /// <summary>
        /// Sets the water paramaters
        /// </summary>
        /// <param name="surfaceOpacity"></param>
        /// <param name="normalScale"></param>
        /// <param name="surfaceColor"></param>
        /// <param name="surfaceColorBlend"></param>
        /// <param name="waterSpecular"></param>
        /// <param name="waterSmoothness"></param>
        /// <param name="foamTint"></param>
        /// <param name="foamOpacity"></param>
        private static void SetWater(float surfaceOpacity,float normalScale, string surfaceColor, float surfaceColorBlend, float waterSpecular, float waterSmoothness, string foamTint, float foamOpacity)
        {
            Material waterMat = GetWaterMaterial("Ambient Water Sample Material");
            if (waterMat != null)
            {
                waterMat.SetFloat("_SurfaceOpacity", surfaceOpacity);
                waterMat.SetFloat("NormalScale", normalScale);
                waterMat.SetColor("_SurfaceColor", GetColorFromHTML(surfaceColor));
                waterMat.SetFloat("_SurfaceColorBlend", surfaceColorBlend);
                waterMat.SetFloat("_WaterSpecular", waterSpecular);
                waterMat.SetFloat("_WaterSmoothness", waterSmoothness);
                waterMat.SetColor("_FoamTint", GetColorFromHTML(foamTint));
                waterMat.SetFloat("_FoamOpacity", foamOpacity);
            }
        }

        /// <summary>
        /// Gets our own water shader if in the scene
        /// </summary>
        /// <returns>The water material if there is one</returns>
        public static Material GetWaterMaterial(string waterName)
        {
            string gaiaWater = GetAssetPath(waterName);
            if (!string.IsNullOrEmpty(gaiaWater))
            {
                return AssetDatabase.LoadAssetAtPath<Material>(gaiaWater);
            }
            return null;
        }

        /// <summary>
        /// Gets or creates a reflection probe
        /// </summary>
        private static void GetOrCreateReflectionProbe(string probeName)
        {
            GameObject theParentObject = GetOrCreateEnvironmentParent();
            GameObject reflectionProbeObject = GameObject.Find(probeName);
            if (reflectionProbeObject == null)
            {
                reflectionProbeObject = new GameObject(probeName);
                reflectionProbeObject.transform.parent = theParentObject.transform;

                ReflectionProbe probe = reflectionProbeObject.AddComponent<ReflectionProbe>();
                probe.importance = 1;
                probe.intensity = 1f;
                probe.blendDistance = 0f;
                probe.resolution = 64;
                probe.shadowDistance = 50f;
                probe.clearFlags = ReflectionProbeClearFlags.Skybox;
                probe.mode = ReflectionProbeMode.Realtime;
                probe.timeSlicingMode = ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
                probe.refreshMode = ReflectionProbeRefreshMode.OnAwake;

                Terrain theTerrain = GetActiveTerrain();
                if (theTerrain != null)
                {
                    probe.size = new Vector3(theTerrain.terrainData.size.x, theTerrain.terrainData.size.y, theTerrain.terrainData.size.z);
                    probe.farClipPlane = theTerrain.terrainData.size.x;
                    Vector3 probeLocation = new Vector3(0f, 0f, 0f);
                    probeLocation.y = theTerrain.SampleHeight(probeLocation) + 50f;
                    reflectionProbeObject.transform.localPosition = probeLocation;
                }
                else
                {
                    probe.size = new Vector3(3000f, 1500f, 3000f);
                    probe.farClipPlane = 3000f;
                    reflectionProbeObject.transform.localPosition = new Vector3(0f, 250f, 0f);
                }
            }

            BakeGlobalReflectionProbe(true);
        }
        #endregion
    }
}
#endif
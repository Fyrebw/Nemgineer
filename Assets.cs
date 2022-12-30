using R2API;
using RoR2;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

namespace NemgineerMod.Modules
{
    internal static class Assets
    {
        internal static AssetBundle mainAssetBundle;
        private const string assetbundleName = "rmorassetbundle";
        private const string csProjName = "Nemesis_Engineer";
        internal static GameObject lockOnTarget;

        internal static void Initialize()
        {
            Assets.LoadAssetBundle();
            Assets.LoadSoundbank();
            Assets.PopulateAssets();
        }

        internal static void LoadAssetBundle()
        {
            try
            {
                if (!((UnityEngine.Object)Assets.mainAssetBundle == (UnityEngine.Object)null))
                    return;
                using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Nemesis_Engineer.rmorassetbundle"))
                    Assets.mainAssetBundle = AssetBundle.LoadFromStream(manifestResourceStream);
            }
            catch (Exception ex)
            {
                NemgineerMod.Log.Error((object)("Failed to load assetbundle. Make sure your assetbundle name is setup correctly\n" + ex?.ToString()));
            }
        }

        internal static void LoadSoundbank()
        {
            using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RMOR_Reforged.HAND_Overclocked_Soundbank.bnk"))
            {
                byte[] buffer = new byte[manifestResourceStream.Length];
                manifestResourceStream.Read(buffer, 0, buffer.Length);
                int num = (int)SoundAPI.SoundBanks.Add(buffer);
            }
        }

        internal static void PopulateAssets()
        {
            if (!(bool)(UnityEngine.Object)Assets.mainAssetBundle)
                NemgineerMod.Log.Error((object)"There is no AssetBundle to load assets from.");
            else
                Assets.lockOnTarget = Assets.CreateLockOnIndicator();
        }

        private static GameObject CreateLockOnIndicator()
        {
            GameObject lockOnIndicator = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/LightningIndicator"), "LockOnIndicator", false);
            lockOnIndicator.transform.localScale = Vector3.one * 0.15f;
            lockOnIndicator.transform.localPosition = Vector3.zero;
            lockOnIndicator.transform.Find("Holder").rotation = Quaternion.identity;
            lockOnIndicator.transform.Find("Holder/Brackets").rotation = Quaternion.identity;
            SpriteRenderer componentInChildren = lockOnIndicator.GetComponentInChildren<SpriteRenderer>();
            componentInChildren.sprite = (Sprite)null;
            componentInChildren.color = Color.green;
            componentInChildren.size = Vector2.zero;
            componentInChildren.transform.localRotation = Quaternion.identity;
            componentInChildren.transform.localPosition = Vector3.zero;
            return lockOnIndicator;
        }

        private static GameObject CreateTracer(string originalTracerName, string newTracerName)
        {
            if ((UnityEngine.Object)LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == (UnityEngine.Object)null)
                return (GameObject)null;
            GameObject effectPrefab = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);
            if (!(bool)(UnityEngine.Object)effectPrefab.GetComponent<EffectComponent>())
                effectPrefab.AddComponent<EffectComponent>();
            if (!(bool)(UnityEngine.Object)effectPrefab.GetComponent<VFXAttributes>())
                effectPrefab.AddComponent<VFXAttributes>();
            if (!(bool)(UnityEngine.Object)effectPrefab.GetComponent<NetworkIdentity>())
                effectPrefab.AddComponent<NetworkIdentity>();
            effectPrefab.GetComponent<Tracer>().speed = 250f;
            effectPrefab.GetComponent<Tracer>().length = 50f;
            Assets.AddNewEffectDef(effectPrefab);
            return effectPrefab;
        }

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef instance = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            instance.akId = AkSoundEngine.GetIDFromString(eventName);
            instance.eventName = eventName;
            Content.AddNetworkSoundEventDef(instance);
            return instance;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!(bool)(UnityEngine.Object)objectToConvert)
                return;
            foreach (Renderer componentsInChild in objectToConvert.GetComponentsInChildren<Renderer>())
            {
                if (!(componentsInChild is ParticleSystemRenderer) && componentsInChild != null)
                {
                    Material material = componentsInChild.material;
                    if (material != null)
                        material.SetHopooMaterial();
                }
            }
        }

        internal static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
        {
            MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] rendererInfoArray = new CharacterModel.RendererInfo[componentsInChildren.Length];
            for (int index = 0; index < componentsInChildren.Length; ++index)
                rendererInfoArray[index] = new CharacterModel.RendererInfo()
                {
                    defaultMaterial = componentsInChildren[index].material,
                    renderer = (Renderer)componentsInChildren[index],
                    defaultShadowCastingMode = ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            return rendererInfoArray;
        }

        public static GameObject LoadSurvivorModel(string modelName)
        {
            GameObject gameObject = Assets.mainAssetBundle.LoadAsset<GameObject>(modelName);
            if (!((UnityEngine.Object)gameObject == (UnityEngine.Object)null))
                return PrefabAPI.InstantiateClone(gameObject, gameObject.name, false);
            NemgineerMod.Log.Error((object)"Trying to load a null model- check to see if the BodyName in your code matches the prefab name of the object in Unity\nFor Example, if your prefab in unity is 'mdlHenry', then your BodyName must be 'Henry'");
            return (GameObject)null;
        }

        internal static Texture LoadCharacterIconGeneric(string characterName) => Assets.mainAssetBundle.LoadAsset<Texture>("tex" + characterName + "Icon");

        internal static GameObject LoadCrosshair(string crosshairName) => (UnityEngine.Object)LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == (UnityEngine.Object)null ? LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair") : LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");

        private static GameObject LoadEffect(string resourceName) => Assets.LoadEffect(resourceName, "", false);

        private static GameObject LoadEffect(string resourceName, string soundName) => Assets.LoadEffect(resourceName, soundName, false);

        private static GameObject LoadEffect(string resourceName, bool parentToTransform) => Assets.LoadEffect(resourceName, "", parentToTransform);

        private static GameObject LoadEffect(
          string resourceName,
          string soundName,
          bool parentToTransform)
        {
            GameObject effectPrefab = Assets.mainAssetBundle.LoadAsset<GameObject>(resourceName);
            if (!(bool)(UnityEngine.Object)effectPrefab)
            {
                NemgineerMod.Log.Error((object)("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle"));
                return (GameObject)null;
            }
            effectPrefab.AddComponent<DestroyOnTimer>().duration = 12f;
            effectPrefab.AddComponent<NetworkIdentity>();
            effectPrefab.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            EffectComponent effectComponent = effectPrefab.AddComponent<EffectComponent>();
            effectComponent.applyScale = false;
            effectComponent.effectIndex = EffectIndex.Invalid;
            effectComponent.parentToReferencedTransform = parentToTransform;
            effectComponent.positionAtReferencedTransform = true;
            effectComponent.soundName = soundName;
            Assets.AddNewEffectDef(effectPrefab, soundName);
            return effectPrefab;
        }

        private static void AddNewEffectDef(GameObject effectPrefab) => Assets.AddNewEffectDef(effectPrefab, "");

        private static void AddNewEffectDef(GameObject effectPrefab, string soundName) => Content.AddEffectDef(new EffectDef()
        {
            prefab = effectPrefab,
            prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>(),
            prefabName = effectPrefab.name,
            prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>(),
            spawnSoundEventName = soundName
        });
    }
}

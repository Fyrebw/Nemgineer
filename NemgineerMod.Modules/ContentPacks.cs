using RoR2;
using RoR2.ContentManagement;
using RoR2.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NemgineerMod.Modules
{
    internal class ContentPacks : IContentPackProvider
    {
        internal ContentPack contentPack = new ContentPack();
        public static List<GameObject> bodyPrefabs = new List<GameObject>();
        public static List<GameObject> masterPrefabs = new List<GameObject>();
        public static List<GameObject> projectilePrefabs = new List<GameObject>();
        public static List<GameObject> networkedObjectPrefabs = new List<GameObject>();
        public static List<SurvivorDef> survivorDefs = new List<SurvivorDef>();
        public static List<UnlockableDef> unlockableDefs = new List<UnlockableDef>();
        public static List<SkillFamily> skillFamilies = new List<SkillFamily>();
        public static List<SkillDef> skillDefs = new List<SkillDef>();
        public static List<System.Type> entityStates = new List<System.Type>();
        public static List<BuffDef> buffDefs = new List<BuffDef>();
        public static List<EffectDef> effectDefs = new List<EffectDef>();
        public static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();

        public string identifier => "com.Fyrebw.Nemgineer";

        public void Initialize() => ContentManager.collectContentPackProviders += new ContentManager.CollectContentPackProvidersDelegate(this.ContentManager_collectContentPackProviders);

        private void ContentManager_collectContentPackProviders(
          ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider((IContentPackProvider)this);
        }

        public IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            this.contentPack.identifier = this.identifier;
            this.contentPack.bodyPrefabs.Add(ContentPacks.bodyPrefabs.ToArray());
            this.contentPack.masterPrefabs.Add(ContentPacks.masterPrefabs.ToArray());
            this.contentPack.projectilePrefabs.Add(ContentPacks.projectilePrefabs.ToArray());
            this.contentPack.survivorDefs.Add(ContentPacks.survivorDefs.ToArray());
            this.contentPack.unlockableDefs.Add(ContentPacks.unlockableDefs.ToArray());
            this.contentPack.skillDefs.Add(ContentPacks.skillDefs.ToArray());
            this.contentPack.skillFamilies.Add(ContentPacks.skillFamilies.ToArray());
            this.contentPack.entityStateTypes.Add(ContentPacks.entityStates.ToArray());
            this.contentPack.buffDefs.Add(ContentPacks.buffDefs.ToArray());
            this.contentPack.effectDefs.Add(ContentPacks.effectDefs.ToArray());
            this.contentPack.networkSoundEventDefs.Add(ContentPacks.networkSoundEventDefs.ToArray());
            this.contentPack.networkedObjectPrefabs.Add(ContentPacks.networkedObjectPrefabs.ToArray());
            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(this.contentPack, args.output);
            args.ReportProgress(1f);
            yield break;
        }

        public IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            args.ReportProgress(1f);
            yield break;
        }
    }
}

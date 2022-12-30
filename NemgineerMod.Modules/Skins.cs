using On.RoR2;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NemgineerMod.Modules
{
    internal static class Skins
    {
        internal static SkinDef CreateSkinDef(
          string skinName,
          Sprite skinIcon,
          CharacterModel.RendererInfo[] defaultRendererInfos,
          GameObject root,
          UnlockableDef unlockableDef = null)
        {
            Skins.SkinDefInfo skinDefInfo = new Skins.SkinDefInfo()
            {
                BaseSkins = Array.Empty<SkinDef>(),
                GameObjectActivations = new SkinDef.GameObjectActivation[0],
                Icon = skinIcon,
                MeshReplacements = new SkinDef.MeshReplacement[0],
                MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0],
                Name = skinName,
                NameToken = skinName,
                ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
                RendererInfos = new CharacterModel.RendererInfo[defaultRendererInfos.Length],
                RootObject = root,
                UnlockableDef = unlockableDef
            };
            // ISSUE: method pointer
            SkinDef.Awake += new SkinDef.hook_Awake((object)null, __methodptr(DoNothing));
            SkinDef instance = ScriptableObject.CreateInstance<SkinDef>();
            instance.baseSkins = skinDefInfo.BaseSkins;
            instance.icon = skinDefInfo.Icon;
            instance.unlockableDef = skinDefInfo.UnlockableDef;
            instance.rootObject = skinDefInfo.RootObject;
            defaultRendererInfos.CopyTo((Array)skinDefInfo.RendererInfos, 0);
            instance.rendererInfos = skinDefInfo.RendererInfos;
            instance.gameObjectActivations = skinDefInfo.GameObjectActivations;
            instance.meshReplacements = skinDefInfo.MeshReplacements;
            instance.projectileGhostReplacements = skinDefInfo.ProjectileGhostReplacements;
            instance.minionSkinReplacements = skinDefInfo.MinionSkinReplacements;
            instance.nameToken = skinDefInfo.NameToken;
            instance.name = skinDefInfo.Name;
            // ISSUE: method pointer
            SkinDef.Awake -= new SkinDef.hook_Awake((object)null, __methodptr(DoNothing));
            return instance;
        }

        private static void DoNothing(SkinDef.orig_Awake orig, SkinDef self)
        {
        }

        private static CharacterModel.RendererInfo[] getRendererMaterials(
          CharacterModel.RendererInfo[] defaultRenderers,
          params Material[] materials)
        {
            CharacterModel.RendererInfo[] rendererMaterials = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo((Array)rendererMaterials, 0);
            for (int index = 0; index < rendererMaterials.Length; ++index)
            {
                try
                {
                    rendererMaterials[index].defaultMaterial = materials[index];
                }
                catch
                {
                    NemgineerMod.Log.Error((object)"error adding skin rendererinfo material. make sure you're not passing in too many");
                }
            }
            return rendererMaterials;
        }

        internal static SkinDef.MeshReplacement[] getMeshReplacements(
          CharacterModel.RendererInfo[] defaultRendererInfos,
          params string[] meshes)
        {
            List<SkinDef.MeshReplacement> meshReplacementList = new List<SkinDef.MeshReplacement>();
            for (int index = 0; index < defaultRendererInfos.Length; ++index)
            {
                if (!string.IsNullOrEmpty(meshes[index]))
                    meshReplacementList.Add(new SkinDef.MeshReplacement()
                    {
                        renderer = defaultRendererInfos[index].renderer,
                        mesh = Assets.mainAssetBundle.LoadAsset<Mesh>(meshes[index])
                    });
            }
            return meshReplacementList.ToArray();
        }

        internal struct SkinDefInfo
        {
            internal SkinDef[] BaseSkins;
            internal Sprite Icon;
            internal string NameToken;
            internal UnlockableDef UnlockableDef;
            internal GameObject RootObject;
            internal CharacterModel.RendererInfo[] RendererInfos;
            internal SkinDef.MeshReplacement[] MeshReplacements;
            internal SkinDef.GameObjectActivation[] GameObjectActivations;
            internal SkinDef.ProjectileGhostReplacement[] ProjectileGhostReplacements;
            internal SkinDef.MinionSkinReplacement[] MinionSkinReplacements;
            internal string Name;
        }
    }
}

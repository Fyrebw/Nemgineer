using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NemgineerMod.Modules
{
    internal static class Materials
    {
        private static List<Material> cachedMaterials = new List<Material>();
        internal static Shader hotpoo = LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/HGStandard");

        public static Material CreateHopooMaterial(string materialName)
        {
            Material hopooMaterial = Materials.cachedMaterials.Find((Predicate<Material>)(mat =>
            {
                materialName.Replace(" (Instance)", "");
                return mat.name.Contains(materialName);
            }));
            if ((bool)(UnityEngine.Object)hopooMaterial)
                return hopooMaterial;
            Material tempMat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);
            if ((bool)(UnityEngine.Object)tempMat)
                return tempMat.SetHopooMaterial();
            NemgineerMod.Log.Error((object)("Failed to load material: " + materialName + " - Check to see that the material in your Unity project matches this name"));
            return new Material(Materials.hotpoo);
        }

        public static Material SetHopooMaterial(this Material tempMat)
        {
            if (Materials.cachedMaterials.Contains(tempMat))
                return tempMat;
            float? nullable1 = new float?();
            Color? nullable2 = new Color?();
            if (tempMat.IsKeywordEnabled("_NORMALMAP"))
                nullable1 = new float?(tempMat.GetFloat("_BumpScale"));
            if (tempMat.IsKeywordEnabled("_EMISSION"))
                nullable2 = new Color?(tempMat.GetColor("_EmissionColor"));
            tempMat.shader = Materials.hotpoo;
            tempMat.SetColor("_Color", tempMat.GetColor("_Color"));
            tempMat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            tempMat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            tempMat.EnableKeyword("DITHER");
            if (nullable1.HasValue)
                tempMat.SetFloat("_NormalStrength", nullable1.Value);
            if (nullable2.HasValue)
            {
                tempMat.SetColor("_EmColor", nullable2.Value);
                tempMat.SetFloat("_EmPower", 1f);
            }
            if (tempMat.IsKeywordEnabled("NOCULL"))
                tempMat.SetInt("_Cull", 0);
            if (tempMat.IsKeywordEnabled("LIMBREMOVAL"))
                tempMat.SetInt("_LimbRemovalOn", 1);
            Materials.cachedMaterials.Add(tempMat);
            return tempMat;
        }

        public static Material SetMaterialShader(this Material tempMat, Shader shader)
        {
            float? nullable1 = new float?();
            Color? nullable2 = new Color?();
            if (tempMat.IsKeywordEnabled("_NORMALMAP"))
                nullable1 = new float?(tempMat.GetFloat("_BumpScale"));
            if (tempMat.IsKeywordEnabled("_EMISSION"))
                nullable2 = new Color?(tempMat.GetColor("_EmissionColor"));
            tempMat.shader = shader;
            tempMat.SetColor("_Color", tempMat.GetColor("_Color"));
            tempMat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            tempMat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            tempMat.EnableKeyword("DITHER");
            if (nullable1.HasValue)
                tempMat.SetFloat("_NormalStrength", nullable1.Value);
            if (nullable2.HasValue)
            {
                tempMat.SetColor("_EmColor", nullable2.Value);
                tempMat.SetFloat("_EmPower", 1f);
            }
            if (tempMat.IsKeywordEnabled("NOCULL"))
                tempMat.SetInt("_Cull", 0);
            if (tempMat.IsKeywordEnabled("LIMBREMOVAL"))
                tempMat.SetInt("_LimbRemovalOn", 1);
            Materials.cachedMaterials.Add(tempMat);
            return tempMat;
        }

        public static Material MakeUnique(this Material material) => Materials.cachedMaterials.Contains(material) ? new Material(material) : material;

        public static Material SetColor(this Material material, Color color)
        {
            material.SetColor("_Color", color);
            return material;
        }

        public static Material SetNormal(this Material material, float normalStrength = 1f)
        {
            material.SetFloat("_NormalStrength", normalStrength);
            return material;
        }

        public static Material SetEmission(this Material material) => material.SetEmission(1f);

        public static Material SetEmission(this Material material, float emission) => material.SetEmission(emission, Color.white);

        public static Material SetEmission(this Material material, float emission, Color emissionColor)
        {
            material.SetFloat("_EmPower", emission);
            material.SetColor("_EmColor", emissionColor);
            return material;
        }

        public static Material SetCull(this Material material, bool cull = false)
        {
            material.SetInt("_Cull", cull ? 1 : 0);
            return material;
        }
    }
}
using R2API;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NemgineerMod.Modules
//Since we are using effects from Commando's Barrage skill, we will also be using the associated namespace
//You can also use Addressables or LegacyResourcesAPI to load whichever effects you like

{
    public static class Projectiles
    {
        public static GameObject PummelProjectile;
        public static void LateSetup()
        {
            ProjectileOverlapAttack component = Projectiles.PummelProjectile.GetComponent<ProjectileOverlapAttack>();
            if (!(bool)(Object)component)
                return;
            component.damageCoefficient = 1f;
            component.overlapProcCoefficient = 0.8f;
        }
        public static void RegisterProjectiles()
        {
            Projectiles.PummelProjectile = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/FMJ"), "PummelProjectile", true);
            Projectiles.PummelProjectile.transform.localScale = new Vector3(6f, 3f, 2f);
            GameObject ghostPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/EvisProjectile").GetComponent<ProjectileController>().ghostPrefab;
            Projectiles.PummelProjectile.GetComponent<ProjectileController>().ghostPrefab = ghostPrefab;
            Projectiles.PummelProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            Object.Destroy((Object)Projectiles.PummelProjectile.transform.Find("SweetSpotBehavior").gameObject);
            if ((bool)(Object)Projectiles.PummelProjectile.GetComponent<ProjectileProximityBeamController>())
                Object.Destroy((Object)Projectiles.PummelProjectile.GetComponent<ProjectileProximityBeamController>());
            Projectiles.PummelProjectile.AddComponent<DestroyOnTimer>().duration = 0.3f;
            {
                GameObject pummelProjectile = Projectiles.PummelProjectile;
            }
        }
        public static GameObject CloneAndColorPummelProjectile(Color beamColor, float lightBright = 0.8f) => Projectiles.CloneAndColorGhost(LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/EvisProjectile").GetComponent<ProjectileController>().ghostPrefab, beamColor, lightBright);

        public static GameObject CloneAndColorGhost(
          GameObject projectileGhost,
          Color PummelProjectileColor,
          float lightBright = 0.8f)
        {
            GameObject gameObject = PrefabAPI.InstantiateClone(projectileGhost, "EvisProjectileClone", false);
            foreach (ParticleSystemRenderer componentsInChild in gameObject.GetComponentsInChildren<ParticleSystemRenderer>())
            {
                if ((bool)(Object)componentsInChild)
                {
                    Material material = Object.Instantiate<Material>(componentsInChild.material);
                    material.SetColor("_TintColor", PummelProjectileColor);
                    componentsInChild.material = material;
                }
            }
            gameObject.GetComponentInChildren<Light>().color = PummelProjectileColor * lightBright;
            return gameObject;
        }

       
    }
}
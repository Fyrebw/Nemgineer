using RoR2;
using System;
using UnityEngine;

namespace Modules
{
    internal static class Survivors
    {
        [Obsolete("moving to survivorbase")]
        internal static void RegisterNewSurvivor(
          GameObject bodyPrefab,
          GameObject displayPrefab,
          string namePrefix,
          UnlockableDef unlockableDef,
          float sortPosition)
        {
            string str1 = namePrefix + "_NAME";
            string str2 = namePrefix + "_DESCRIPTION";
            string str3 = namePrefix + "_OUTRO_FLAVOR";
            string str4 = namePrefix + "_OUTRO_FAILURE";
            SurvivorDef instance = ScriptableObject.CreateInstance<SurvivorDef>();
            instance.bodyPrefab = bodyPrefab;
            instance.displayPrefab = displayPrefab;
            instance.displayNameToken = str1;
            instance.cachedName = bodyPrefab.name.Replace("Body", "");
            instance.descriptionToken = str2;
            instance.outroFlavorToken = str3;
            instance.mainEndingEscapeFailureFlavorToken = str4;
            instance.desiredSortPosition = sortPosition;
            instance.unlockableDef = unlockableDef;
            NemgineerMod.Modules.Content.AddSurvivorDef(instance);
        }

        [Obsolete("moving to survivorbase")]
        internal static void RegisterNewSurvivor(
          GameObject bodyPrefab,
          GameObject displayPrefab,
          string namePrefix)
        {
            Survivors.RegisterNewSurvivor(bodyPrefab, displayPrefab, namePrefix, (UnlockableDef)null, 4f);
        }
    }
}
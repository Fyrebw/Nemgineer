using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace NemgineerMod.Modules
{
    internal static class Prefabs
    {
        private static PhysicMaterial ragdollMaterial;

        public static GameObject CreateDisplayPrefab(
          string displayModelName,
          GameObject prefab,
          RMORMod.Modules.Characters.BodyInfo bodyInfo)
        {
            GameObject objectToConvert = Assets.LoadSurvivorModel(displayModelName);
            CharacterModel characterModel = objectToConvert.GetComponent<CharacterModel>();
            if (!(bool)(Object)characterModel)
                characterModel = objectToConvert.AddComponent<CharacterModel>();
            characterModel.baseRendererInfos = prefab.GetComponentInChildren<CharacterModel>().baseRendererInfos;
            Assets.ConvertAllRenderersToHopooShader(objectToConvert);
            return objectToConvert.gameObject;
        }

        public static GameObject CreateBodyPrefab(string bodyName, string modelName, RMORMod.Modules.Characters.BodyInfo bodyInfo)
        {
            GameObject gameObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/" + bodyInfo.bodyNameToClone + "Body");
            if (!(bool)(Object)gameObject)
            {
                NemgineerMod.Log.Error((object)(bodyInfo.bodyNameToClone + "Body is not a valid body, character creation failed"));
                return (GameObject)null;
            }
            GameObject bodyPrefab = PrefabAPI.InstantiateClone(gameObject, bodyName);
            Transform modelBaseTransform = (Transform)null;
            GameObject model = (GameObject)null;
            if (modelName != "mdl")
            {
                model = Assets.LoadSurvivorModel(modelName);
                if ((Object)model == (Object)null)
                    model = bodyPrefab.GetComponentInChildren<CharacterModel>().gameObject;
                modelBaseTransform = Prefabs.AddCharacterModelToSurvivorBody(bodyPrefab, model.transform, bodyInfo);
            }
            CharacterBody component = bodyPrefab.GetComponent<CharacterBody>();
            component.name = bodyInfo.bodyName;
            component.baseNameToken = bodyInfo.bodyNameToken;
            component.subtitleNameToken = bodyInfo.subtitleNameToken;
            component.portraitIcon = bodyInfo.characterPortrait;
            component.bodyColor = bodyInfo.bodyColor;
            component._defaultCrosshairPrefab = bodyInfo.crosshair;
            component.hideCrosshair = false;
            component.preferredPodPrefab = bodyInfo.podPrefab;
            component.baseMaxHealth = bodyInfo.maxHealth;
            component.baseRegen = bodyInfo.healthRegen;
            component.levelArmor = bodyInfo.armorGrowth;
            component.baseMaxShield = bodyInfo.shield;
            component.baseDamage = bodyInfo.damage;
            component.baseAttackSpeed = bodyInfo.attackSpeed;
            component.baseCrit = bodyInfo.crit;
            component.baseMoveSpeed = bodyInfo.moveSpeed;
            component.baseJumpPower = bodyInfo.jumpPower;
            component.autoCalculateLevelStats = bodyInfo.autoCalculateLevelStats;
            component.levelDamage = bodyInfo.damageGrowth;
            component.levelAttackSpeed = bodyInfo.attackSpeedGrowth;
            component.levelCrit = bodyInfo.critGrowth;
            component.levelMaxHealth = bodyInfo.healthGrowth;
            component.levelRegen = bodyInfo.regenGrowth;
            component.baseArmor = bodyInfo.armor;
            component.levelMaxShield = bodyInfo.shieldGrowth;
            component.levelMoveSpeed = bodyInfo.moveSpeedGrowth;
            component.levelJumpPower = bodyInfo.jumpPowerGrowth;
            component.baseAcceleration = bodyInfo.acceleration;
            component.baseJumpCount = bodyInfo.jumpCount;
            component.sprintingSpeedMultiplier = 1.45f;
            component.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            component.rootMotionInMainState = false;
            component.hullClassification = HullClassification.Human;
            component.isChampion = false;
            Prefabs.SetupCameraTargetParams(bodyPrefab, bodyInfo);
            Prefabs.SetupModelLocator(bodyPrefab, modelBaseTransform, model.transform);
            Prefabs.SetupCapsuleCollider(bodyPrefab);
            Prefabs.SetupMainHurtbox(bodyPrefab, model);
            Prefabs.SetupAimAnimator(bodyPrefab, model);
            if ((Object)modelBaseTransform != (Object)null)
                Prefabs.SetupCharacterDirection(bodyPrefab, modelBaseTransform, model.transform);
            Prefabs.SetupFootstepController(model);
            Prefabs.SetupRagdoll(model);
            Content.AddCharacterBodyPrefab(bodyPrefab);
            return bodyPrefab;
        }

        public static void CreateGenericDoppelganger(
          GameObject bodyPrefab,
          string masterName,
          string masterToCopy)
        {
            GameObject prefab = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/" + masterToCopy + "MonsterMaster"), masterName, true);
            prefab.GetComponent<CharacterMaster>().bodyPrefab = bodyPrefab;
            Content.AddMasterPrefab(prefab);
        }

        private static Transform AddCharacterModelToSurvivorBody(
          GameObject bodyPrefab,
          Transform modelTransform,
          NemgineerMod.Modules.Characters.BodyInfo bodyInfo)
        {
            for (int index = bodyPrefab.transform.childCount - 1; index >= 0; --index)
                Object.DestroyImmediate((Object)bodyPrefab.transform.GetChild(index).gameObject);
            Transform transform = new GameObject("ModelBase").transform;
            transform.parent = bodyPrefab.transform;
            transform.localPosition = bodyInfo.modelBasePosition;
            transform.localRotation = Quaternion.identity;
            modelTransform.parent = transform.transform;
            modelTransform.localPosition = Vector3.zero;
            modelTransform.localRotation = Quaternion.identity;
            GameObject gameObject = new GameObject("CameraPivot")
            {
                transform = {
          parent = bodyPrefab.transform,
          localPosition = bodyInfo.cameraPivotPosition,
          localRotation = Quaternion.identity
        }
            };
            bodyPrefab.GetComponent<CharacterBody>().aimOriginTransform = new GameObject("AimOrigin")
            {
                transform = {
          parent = bodyPrefab.transform,
          localPosition = bodyInfo.aimOriginPosition,
          localRotation = Quaternion.identity
        }
            }.transform;
            return transform.transform;
        }

        public static CharacterModel SetupCharacterModel(GameObject prefab) => Prefabs.SetupCharacterModel(prefab, (CustomRendererInfo[])null);

        public static CharacterModel SetupCharacterModel(
          GameObject prefab,
          CustomRendererInfo[] customInfos)
        {
            CharacterModel characterModel = prefab.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>();
            bool flag = (Object)characterModel != (Object)null;
            if (!flag)
                characterModel = prefab.GetComponent<ModelLocator>().modelTransform.gameObject.AddComponent<CharacterModel>();
            characterModel.body = prefab.GetComponent<CharacterBody>();
            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();
            if (!flag)
                Prefabs.SetupCustomRendererInfos(characterModel, customInfos);
            else
                Prefabs.SetupPreAttachedRendererInfos(characterModel);
            return characterModel;
        }

        public static void SetupPreAttachedRendererInfos(CharacterModel characterModel)
        {
            for (int index = 0; index < characterModel.baseRendererInfos.Length; ++index)
            {
                if ((Object)characterModel.baseRendererInfos[index].defaultMaterial == (Object)null)
                    characterModel.baseRendererInfos[index].defaultMaterial = characterModel.baseRendererInfos[index].renderer.sharedMaterial;
                characterModel.baseRendererInfos[index].defaultMaterial.SetHopooMaterial();
            }
        }

        public static void SetupCustomRendererInfos(
          CharacterModel characterModel,
          CustomRendererInfo[] customInfos)
        {
            ChildLocator component1 = characterModel.GetComponent<ChildLocator>();
            if (!(bool)(Object)component1)
            {
                NemgineerMod.Log.Error((object)"Failed CharacterModel setup: ChildLocator component does not exist on the model");
            }
            else
            {
                List<CharacterModel.RendererInfo> rendererInfoList = new List<CharacterModel.RendererInfo>();
                for (int index = 0; index < customInfos.Length; ++index)
                {
                    if (!(bool)(Object)component1.FindChild(customInfos[index].childName))
                    {
                        NemgineerMod.Log.Error((object)("Trying to add a RendererInfo for a renderer that does not exist: " + customInfos[index].childName));
                    }
                    else
                    {
                        Renderer component2 = component1.FindChild(customInfos[index].childName).GetComponent<Renderer>();
                        if ((bool)(Object)component2)
                        {
                            Material material = customInfos[index].material;
                            if ((Object)material == (Object)null)
                                material = !customInfos[index].dontHotpoo ? component2.material.SetHopooMaterial() : component2.material;
                            rendererInfoList.Add(new CharacterModel.RendererInfo()
                            {
                                renderer = component2,
                                defaultMaterial = material,
                                ignoreOverlays = customInfos[index].ignoreOverlays,
                                defaultShadowCastingMode = ShadowCastingMode.On
                            });
                        }
                    }
                }
                characterModel.baseRendererInfos = rendererInfoList.ToArray();
            }
        }

        private static void SetupCharacterDirection(
          GameObject prefab,
          Transform modelBaseTransform,
          Transform modelTransform)
        {
            if (!(bool)(Object)prefab.GetComponent<CharacterDirection>())
                return;
            CharacterDirection component = prefab.GetComponent<CharacterDirection>();
            component.targetTransform = modelBaseTransform;
            component.overrideAnimatorForwardTransform = (Transform)null;
            component.rootMotionAccumulator = (RootMotionAccumulator)null;
            component.modelAnimator = modelTransform.GetComponent<Animator>();
            component.driveFromRootRotation = false;
            component.turnSpeed = 720f;
        }

        private static void SetupCameraTargetParams(GameObject prefab, NemgineerMod.Modules.Characters.BodyInfo bodyInfo)
        {
            CameraTargetParams component = prefab.GetComponent<CameraTargetParams>();
            component.cameraParams = bodyInfo.cameraParams;
            component.cameraPivotTransform = prefab.transform.Find("CameraPivot");
        }

        private static void SetupModelLocator(
          GameObject prefab,
          Transform modelBaseTransform,
          Transform modelTransform)
        {
            ModelLocator component = prefab.GetComponent<ModelLocator>();
            component.modelTransform = modelTransform;
            component.modelBaseTransform = modelBaseTransform;
        }

        private static void SetupCapsuleCollider(GameObject prefab)
        {
            CapsuleCollider component = prefab.GetComponent<CapsuleCollider>();
            component.center = new Vector3(0.0f, 0.0f, 0.0f);
            component.radius = 0.5f;
            component.height = 1.82f;
            component.direction = 1;
        }

        private static void SetupMainHurtbox(GameObject prefab, GameObject model)
        {
            ChildLocator component1 = model.GetComponent<ChildLocator>();
            if (!(bool)(Object)component1.FindChild("MainHurtbox"))
            {
                Debug.LogWarning((object)"Could not set up main hurtbox: make sure you have a transform pair in your prefab's ChildLocator component called 'MainHurtbox'");
            }
            else
            {
                HealthComponent component2 = prefab.GetComponent<HealthComponent>();
                HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();
                HurtBox hurtBox1 = component1.FindChildGameObject("MainHurtbox").AddComponent<HurtBox>();
                hurtBox1.gameObject.layer = LayerIndex.entityPrecise.intVal;
                hurtBox1.healthComponent = component2;
                hurtBox1.isBullseye = true;
                hurtBox1.damageModifier = HurtBox.DamageModifier.Normal;
                hurtBox1.hurtBoxGroup = hurtBoxGroup;
                hurtBox1.isSniperTarget = true;
                hurtBoxGroup.hurtBoxes = new HurtBox[1] { hurtBox1 };
                hurtBoxGroup.mainHurtBox = hurtBox1;
                hurtBoxGroup.bullseyeCount = 1;
                if (!(bool)(Object)component1.FindChild("HeadHurtbox"))
                    return;
                HurtBox hurtBox2 = component1.FindChild("HeadHurtbox").gameObject.AddComponent<HurtBox>();
                hurtBox2.gameObject.layer = LayerIndex.entityPrecise.intVal;
                hurtBox2.healthComponent = component2;
                hurtBox2.isBullseye = false;
                hurtBox2.damageModifier = HurtBox.DamageModifier.Normal;
                hurtBox2.hurtBoxGroup = hurtBoxGroup;
                hurtBox2.isSniperTarget = true;
                hurtBox1.isSniperTarget = false;
                hurtBoxGroup.hurtBoxes = new HurtBox[2]
                {
          hurtBox1,
          hurtBox2
                };
                hurtBox1.indexInGroup = (short)0;
                hurtBox2.indexInGroup = (short)1;
            }
        }

        public static void SetupHurtBoxes(GameObject bodyPrefab)
        {
            HealthComponent component = bodyPrefab.GetComponent<HealthComponent>();
            foreach (HurtBoxGroup componentsInChild in bodyPrefab.GetComponentsInChildren<HurtBoxGroup>())
            {
                componentsInChild.mainHurtBox.healthComponent = component;
                for (int index = 0; index < componentsInChild.hurtBoxes.Length; ++index)
                    componentsInChild.hurtBoxes[index].healthComponent = component;
            }
        }

        private static void SetupFootstepController(GameObject model)
        {
            FootstepHandler footstepHandler = model.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/GenericFootstepDust");
        }

        private static void SetupRagdoll(GameObject model)
        {
            RagdollController component1 = model.GetComponent<RagdollController>();
            if (!(bool)(Object)component1)
                return;
            if ((Object)Prefabs.ragdollMaterial == (Object)null)
                Prefabs.ragdollMaterial = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RagdollController>().bones[1].GetComponent<Collider>().material;
            foreach (Transform bone in component1.bones)
            {
                if ((bool)(Object)bone)
                {
                    bone.gameObject.layer = LayerIndex.ragdoll.intVal;
                    Collider component2 = bone.GetComponent<Collider>();
                    if ((bool)(Object)component2)
                    {
                        component2.material = Prefabs.ragdollMaterial;
                        component2.sharedMaterial = Prefabs.ragdollMaterial;
                    }
                }
            }
        }

        private static void SetupAimAnimator(GameObject prefab, GameObject model)
        {
            AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
            aimAnimator.directionComponent = prefab.GetComponent<CharacterDirection>();
            aimAnimator.pitchRangeMax = 60f;
            aimAnimator.pitchRangeMin = -60f;
            aimAnimator.yawRangeMin = -80f;
            aimAnimator.yawRangeMax = 80f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 3f;
            aimAnimator.inputBank = prefab.GetComponent<InputBankTest>();
        }

        public static void SetupHitbox(GameObject prefab, Transform hitboxTransform, string hitboxName)
        {
            HitBoxGroup hitBoxGroup = prefab.AddComponent<HitBoxGroup>();
            HitBox hitBox = hitboxTransform.gameObject.AddComponent<HitBox>();
            hitboxTransform.gameObject.layer = LayerIndex.projectile.intVal;
            hitBoxGroup.hitBoxes = new HitBox[1] { hitBox };
            hitBoxGroup.groupName = hitboxName;
        }

        public static void SetupHitbox(
          GameObject prefab,
          string hitboxName,
          params Transform[] hitboxTransforms)
        {
            HitBoxGroup hitBoxGroup = prefab.AddComponent<HitBoxGroup>();
            List<HitBox> hitBoxList = new List<HitBox>();
            foreach (Transform hitboxTransform in hitboxTransforms)
            {
                HitBox hitBox = hitboxTransform.gameObject.AddComponent<HitBox>();
                hitboxTransform.gameObject.layer = LayerIndex.projectile.intVal;
                hitBoxList.Add(hitBox);
            }
            hitBoxGroup.hitBoxes = hitBoxList.ToArray();
            hitBoxGroup.groupName = hitboxName;
        }
    }
}


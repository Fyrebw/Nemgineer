using EntityStates;
using HG;
using RoR2;
using RoR2.ContentManagement;
using System;
using UnityEngine;

namespace NemgineerMod.Modules.Characters
{
    internal abstract class CharacterBase
    {
        public static CharacterBase instance;

        public abstract string bodyName { get; }

        public abstract BodyInfo bodyInfo { get; set; }

        public abstract CustomRendererInfo[] customRendererInfos { get; set; }

        public abstract System.Type characterMainState { get; }

        public virtual System.Type characterSpawnState { get; }

        public virtual ItemDisplaysBase itemDisplays { get; } = (ItemDisplaysBase)null;

        public virtual GameObject bodyPrefab { get; set; }

        public virtual CharacterModel characterBodyModel { get; set; }

        public string fullBodyName => this.bodyName + "Body";

        public virtual void Initialize()
        {
            CharacterBase.instance = this;
            this.InitializeCharacter();
        }

        public virtual void InitializeCharacter()
        {
            this.InitializeCharacterBodyAndModel();
            this.InitializeCharacterMaster();
            this.InitializeEntityStateMachine();
            this.InitializeSkills();
            this.InitializeHitboxes();
            this.InitializeHurtboxes();
            this.InitializeSkins();
            this.InitializeItemDisplays();
            this.InitializeDoppelganger("Merc");
        }

        protected virtual void InitializeCharacterBodyAndModel()
        {
            this.bodyPrefab = Prefabs.CreateBodyPrefab(this.bodyName + "Body", "mdl" + this.bodyName, this.bodyInfo);
            this.InitializeCharacterModel();
        }

        protected virtual void InitializeCharacterModel() => this.characterBodyModel = Prefabs.SetupCharacterModel(this.bodyPrefab, this.customRendererInfos);

        protected virtual void InitializeCharacterMaster()
        {
        }

        protected virtual void InitializeEntityStateMachine()
        {
            this.bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new SerializableEntityStateType(this.characterMainState);
            Content.AddEntityState(this.characterMainState);
            if (!System.Type.op_Inequality(this.characterSpawnState, (System.Type)null))
                return;
            this.bodyPrefab.GetComponent<EntityStateMachine>().initialStateType = new SerializableEntityStateType(this.characterSpawnState);
            Content.AddEntityState(this.characterSpawnState);
        }

        public abstract void InitializeSkills();

        public virtual void InitializeHitboxes()
        {
        }

        public virtual void InitializeHurtboxes() => Prefabs.SetupHurtBoxes(this.bodyPrefab);

        public virtual void InitializeSkins()
        {
        }

        public virtual void InitializeDoppelganger(string clone) => Prefabs.CreateGenericDoppelganger(CharacterBase.instance.bodyPrefab, this.bodyName + "MonsterMaster", clone);

        public virtual void InitializeItemDisplays()
        {
            ItemDisplayRuleSet instance = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            instance.name = "idrs" + this.bodyName;
            this.characterBodyModel.itemDisplayRuleSet = instance;
            if (this.itemDisplays == null)
                return;
            ContentManager.onContentPacksAssigned += new Action<ReadOnlyArray<ReadOnlyContentPack>>(this.SetItemDisplays);
        }

        public void SetItemDisplays(ReadOnlyArray<ReadOnlyContentPack> obj) => this.itemDisplays.SetItemDisplays(this.characterBodyModel.itemDisplayRuleSet);
    }
}


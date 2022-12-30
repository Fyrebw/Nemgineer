using RoR2;
using RoR2.Skills;
using RoR2.UI;
using System.Collections.Generic;
using UnityEngine;

namespace EntityStates.Nemgineer.NemgineerMissilePainter
{
    public class Paint : BaseNemgineerMissilePainterState
    {
        public static GameObject crosshairOverridePrefab;
        public static GameObject stickyTargetIndicatorPrefab;
        public static float stackInterval;
        public static string enterSoundString;
        public static string exitSoundString;
        public static string loopSoundString;
        public static string lockOnSoundString;
        public static string stopLoopSoundString;
        public static float maxAngle;
        public static float maxDistance;
        private List<HurtBox> targetsList;
        private Dictionary<HurtBox, Paint.IndicatorInfo> targetIndicators;
        private Indicator stickyTargetIndicator;
        private SkillDef engiConfirmTargetDummySkillDef;
        private SkillDef engiCancelTargetingDummySkillDef;
        private bool releasedKeyOnce;
        private float stackStopwatch;
        private CrosshairUtils.OverrideRequest crosshairOverrideRequest;
        private BullseyeSearch search;
        private bool queuedFiringState;
        private uint loopSoundID;
        private HealthComponent previousHighlightTargetHealthComponent;
        private HurtBox previousHighlightTargetHurtBox;

        public override void OnEnter()
        {
            base.OnEnter();
            if (this.isAuthority)
            {
                this.targetsList = new List<HurtBox>();
                this.targetIndicators = new Dictionary<HurtBox, Paint.IndicatorInfo>();
                this.stickyTargetIndicator = new Indicator(this.gameObject, Paint.stickyTargetIndicatorPrefab);
                this.search = new BullseyeSearch();
            }
            this.PlayCrossfade("Gesture, Additive", "PrepHarpoons", 0.1f);
            int num = (int)Util.PlaySound(Paint.enterSoundString, this.gameObject);
            this.loopSoundID = Util.PlaySound(Paint.loopSoundString, this.gameObject);
            if ((bool)(Object)Paint.crosshairOverridePrefab)
                this.crosshairOverrideRequest = CrosshairUtils.RequestOverrideForBody(this.characterBody, Paint.crosshairOverridePrefab, CrosshairUtils.OverridePriority.Skill);
            this.engiConfirmTargetDummySkillDef = SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("EngiConfirmTargetDummy"));
            this.engiCancelTargetingDummySkillDef = SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("EngiCancelTargetingDummy"));
            this.skillLocator.primary.SetSkillOverride((object)this, this.engiConfirmTargetDummySkillDef, GenericSkill.SkillOverridePriority.Contextual);
            this.skillLocator.secondary.SetSkillOverride((object)this, this.engiCancelTargetingDummySkillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        public override void OnExit()
        {
            if (this.isAuthority && !this.outer.destroying && !this.queuedFiringState)
            {
                for (int index = 0; index < this.targetsList.Count; ++index)
                    this.activatorSkillSlot.AddOneStock();
            }
            this.skillLocator.secondary.UnsetSkillOverride((object)this, this.engiCancelTargetingDummySkillDef, GenericSkill.SkillOverridePriority.Contextual);
            this.skillLocator.primary.UnsetSkillOverride((object)this, this.engiConfirmTargetDummySkillDef, GenericSkill.SkillOverridePriority.Contextual);
            if (this.targetIndicators != null)
            {
                foreach (KeyValuePair<HurtBox, Paint.IndicatorInfo> targetIndicator in this.targetIndicators)
                    targetIndicator.Value.indicator.active = false;
            }
            if (this.stickyTargetIndicator != null)
                this.stickyTargetIndicator.active = false;
            this.crosshairOverrideRequest?.Dispose();
            this.PlayCrossfade("Gesture, Additive", "ExitHarpoons", 0.1f);
            int num1 = (int)Util.PlaySound(Paint.exitSoundString, this.gameObject);
            int num2 = (int)Util.PlaySound(Paint.stopLoopSoundString, this.gameObject);
            base.OnExit();
        }

        private void AddTargetAuthority(HurtBox hurtBox)
        {
            if (this.activatorSkillSlot.stock == 0)
                return;
            int num = (int)Util.PlaySound(Paint.lockOnSoundString, this.gameObject);
            this.targetsList.Add(hurtBox);
            Paint.IndicatorInfo indicatorInfo;
            if (!this.targetIndicators.TryGetValue(hurtBox, out indicatorInfo))
            {
                indicatorInfo = new Paint.IndicatorInfo()
                {
                    refCount = 0,
                    indicator = new Paint.EngiMissileIndicator(this.gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/EngiMissileTrackingIndicator"))
                };
                indicatorInfo.indicator.targetTransform = hurtBox.transform;
                indicatorInfo.indicator.active = true;
            }
            ++indicatorInfo.refCount;
            indicatorInfo.indicator.missileCount = indicatorInfo.refCount;
            this.targetIndicators[hurtBox] = indicatorInfo;
            this.activatorSkillSlot.DeductStock(1);
        }

        private void RemoveTargetAtAuthority(int i)
        {
            HurtBox targets = this.targetsList[i];
            this.targetsList.RemoveAt(i);
            Paint.IndicatorInfo indicatorInfo;
            if (!this.targetIndicators.TryGetValue(targets, out indicatorInfo))
                return;
            --indicatorInfo.refCount;
            indicatorInfo.indicator.missileCount = indicatorInfo.refCount;
            this.targetIndicators[targets] = indicatorInfo;
            if (indicatorInfo.refCount != 0)
                return;
            indicatorInfo.indicator.active = false;
            this.targetIndicators.Remove(targets);
        }

        private void CleanTargetsList()
        {
            for (int index = this.targetsList.Count - 1; index >= 0; --index)
            {
                HurtBox targets = this.targetsList[index];
                if (!(bool)(Object)targets.healthComponent || !targets.healthComponent.alive)
                {
                    this.RemoveTargetAtAuthority(index);
                    this.activatorSkillSlot.AddOneStock();
                }
            }
            for (int i = this.targetsList.Count - 1; i >= this.activatorSkillSlot.maxStock; --i)
                this.RemoveTargetAtAuthority(i);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.characterBody.SetAimTimer(3f);
            if (!this.isAuthority)
                return;
            this.AuthorityFixedUpdate();
        }

        private void GetCurrentTargetInfo(
          out HurtBox currentTargetHurtBox,
          out HealthComponent currentTargetHealthComponent)
        {
            Ray aimRay = this.GetAimRay();
            this.search.filterByDistinctEntity = true;
            this.search.filterByLoS = true;
            this.search.minDistanceFilter = 0.0f;
            this.search.maxDistanceFilter = Paint.maxDistance;
            this.search.minAngleFilter = 0.0f;
            this.search.maxAngleFilter = Paint.maxAngle;
            this.search.viewer = this.characterBody;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(this.GetTeam());
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(this.gameObject);
            foreach (HurtBox result in this.search.GetResults())
            {
                if ((bool)(Object)result.healthComponent && result.healthComponent.alive)
                {
                    currentTargetHurtBox = result;
                    currentTargetHealthComponent = result.healthComponent;
                    return;
                }
            }
            currentTargetHurtBox = (HurtBox)null;
            currentTargetHealthComponent = (HealthComponent)null;
        }

        private void AuthorityFixedUpdate()
        {
            this.CleanTargetsList();
            bool flag = false;
            HurtBox currentTargetHurtBox;
            HealthComponent currentTargetHealthComponent;
            this.GetCurrentTargetInfo(out currentTargetHurtBox, out currentTargetHealthComponent);
            if ((bool)(Object)currentTargetHurtBox)
            {
                this.stackStopwatch += Time.fixedDeltaTime;
                if (this.inputBank.skill1.down && ((Object)this.previousHighlightTargetHealthComponent != (Object)currentTargetHealthComponent || (double)this.stackStopwatch > (double)Paint.stackInterval / (double)this.attackSpeedStat || this.inputBank.skill1.justPressed))
                {
                    this.stackStopwatch = 0.0f;
                    this.AddTargetAuthority(currentTargetHurtBox);
                }
            }
            if (this.inputBank.skill1.justReleased)
                flag = true;
            if (this.inputBank.skill2.justReleased)
            {
                this.outer.SetNextStateToMain();
            }
            else
            {
                if (this.inputBank.skill3.justReleased)
                {
                    if (this.releasedKeyOnce)
                        flag = true;
                    this.releasedKeyOnce = true;
                }
                if (currentTargetHurtBox != this.previousHighlightTargetHurtBox)
                {
                    this.previousHighlightTargetHurtBox = currentTargetHurtBox;
                    this.previousHighlightTargetHealthComponent = currentTargetHealthComponent;
                    this.stickyTargetIndicator.targetTransform = !(bool)(Object)currentTargetHurtBox || this.activatorSkillSlot.stock == 0 ? (Transform)null : currentTargetHurtBox.transform;
                    this.stackStopwatch = 0.0f;
                }
                this.stickyTargetIndicator.active = (bool)(Object)this.stickyTargetIndicator.targetTransform;
                if (!flag)
                    return;
                this.queuedFiringState = true;
                EntityStateMachine outer = this.outer;
                Fire newNextState = new Fire();
                newNextState.targetsList = this.targetsList;
                newNextState.activatorSkillSlot = this.activatorSkillSlot;
                outer.SetNextState((EntityState)newNextState);
            }
        }

        private struct IndicatorInfo
        {
            public int refCount;
            public Paint.EngiMissileIndicator indicator;
        }

        private class EngiMissileIndicator : Indicator
        {
            public int missileCount;

            public override void UpdateVisualizer()
            {
                base.UpdateVisualizer();
                Transform parent = this.visualizerTransform.Find("DotOrigin");
                for (int index = parent.childCount - 1; index >= this.missileCount; --index)
                    EntityState.Destroy((Object)parent.GetChild(index));
                for (int childCount = parent.childCount; childCount < this.missileCount; ++childCount)
                    Object.Instantiate<GameObject>(this.visualizerPrefab.transform.Find("DotOrigin/DotTemplate").gameObject, parent);
                if (parent.childCount <= 0)
                    return;
                float num1 = 360f / (float)parent.childCount;
                float num2 = (float)(parent.childCount - 1) * 90f;
                for (int index = 0; index < parent.childCount; ++index)
                {
                    Transform child = parent.GetChild(index);
                    child.gameObject.SetActive(true);
                    child.localRotation = Quaternion.Euler(0.0f, 0.0f, num2 + (float)index * num1);
                }
            }

            public EngiMissileIndicator(GameObject owner, GameObject visualizerPrefab)
              : base(owner, visualizerPrefab)
            {
            }
        }
    }
}


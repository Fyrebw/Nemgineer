
using HG.BlendableTypes;
using RoR2;
using UnityEngine;

namespace NemgineerMod.Modules.Characters
{
    internal class BodyInfo
    {
        public string bodyName = "";
        public string bodyNameToken = "";
        public string subtitleNameToken = "";
        public string bodyNameToClone = "Commando";
        public Color bodyColor = Color.white;
        public Texture characterPortrait = (Texture)null;
        public float sortPosition = 100f;
        public GameObject crosshair = (GameObject)null;
        public GameObject podPrefab = (GameObject)null;
        public float maxHealth = 400f;
        public float healthRegen = 2f;
        public float armor = 30f;
        public float shield = 0.0f;
        public int jumpCount = 1;
        public float damage = 20f;
        public float attackSpeed = 1f;
        public float crit = 1f;
        public float moveSpeed = 5f;
        public float acceleration = 80f;
        public float jumpPower = 15f;
        public bool autoCalculateLevelStats = true;
        public float healthGrowth = 100f;
        public float regenGrowth = 0.8f;
        public float armorGrowth = 0.5f;
        public float shieldGrowth = 0.0f;
        public float damageGrowth = 5f;
        public float attackSpeedGrowth = 0.0f;
        public float critGrowth = 0.0f;
        public float moveSpeedGrowth = 0.0f;
        public float jumpPowerGrowth = 0.0f;
        public Vector3 aimOriginPosition = new Vector3(0.0f, 1.6f, 0.0f);
        public Vector3 modelBasePosition = new Vector3(0.0f, -0.92f, 0.0f);
        public Vector3 cameraPivotPosition = new Vector3(0.0f, 0.8f, 0.0f);
        public float cameraParamsVerticalOffset = 1.37f;
        public float cameraParamsDepth = -10f;
        private CharacterCameraParams _cameraParams;

        public CharacterCameraParams cameraParams
        {
            get
            {
                if ((Object)this._cameraParams == (Object)null)
                {
                    this._cameraParams = ScriptableObject.CreateInstance<CharacterCameraParams>();
                    this._cameraParams.data.minPitch = (BlendableFloat) - 70f;
                    this._cameraParams.data.maxPitch = (BlendableFloat)70f;
                    this._cameraParams.data.wallCushion = (BlendableFloat)0.1f;
                    this._cameraParams.data.pivotVerticalOffset = (BlendableFloat)this.cameraParamsVerticalOffset;
                    this._cameraParams.data.idealLocalCameraPos = (BlendableVector3)new Vector3(0.0f, 0.0f, this.cameraParamsDepth);
                }
                return this._cameraParams;
            }
            set => this._cameraParams = value;
        }
    }
}
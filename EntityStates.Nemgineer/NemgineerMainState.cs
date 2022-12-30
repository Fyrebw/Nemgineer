using NemgineerMod.Modules;
using RoR2;
using UnityEngine;

namespace EntityStates.Nemgineer
{
    public class NemgineerMainState : GenericCharacterMain
    {
        public LocalUser localUser;

        public override void OnEnter()
        {
            base.OnEnter();
            this.FindLocalUser();
        }

        private void FindLocalUser()
        {
            if (this.localUser != null || !(bool)(Object)this.characterBody)
                return;
            foreach (LocalUser readOnlyLocalUsers in LocalUserManager.readOnlyLocalUsersList)
            {
                if ((Object)readOnlyLocalUsers.cachedBody == (Object)this.characterBody)
                {
                    this.localUser = readOnlyLocalUsers;
                    break;
                }
            }
        }
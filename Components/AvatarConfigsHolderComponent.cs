using HECSFramework.Core;
using System;
using UnityEngine;

namespace Components
{
    [Serializable][Documentation(Doc.Holder, "here we hold avatars")]
    public sealed class AvatarConfigsHolderComponent : BaseComponent, IWorldSingleComponent
    {
        [SerializeField] private AvatarConfig[] avatarConfigs;

        public Sprite GetAvatarByBrawlID(int brawlID)
        {
            for (int i = 0; i < avatarConfigs.Length; i++)
            {
                if (avatarConfigs[i].AvatarID == brawlID)
                {
                    return avatarConfigs[i].AvatarSprite;
                }
            }

            throw new Exception("unknown avatar ID");
        }

        public Sprite GetAvatarByUpgradeID(int upgradeID)
        {
            for (int i = 0; i < avatarConfigs.Length; i++)
            {
                if (avatarConfigs[i].UpgradeID == upgradeID)
                {
                    return avatarConfigs[i].AvatarSprite;
                }
            }

            throw new Exception("unknown upgrade ID");
        }
    }
}
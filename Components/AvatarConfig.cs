using Helpers;
using System;
using UnityEngine;

namespace Components
{
    [Serializable]
    public sealed class AvatarConfig
    {
        [SerializeField][IdentifierDropDown(nameof(AvatarIdentifier))] private int avatarID;
        [SerializeField] private int upgradeID;
        [SerializeField] private Sprite avatarSprite;

        public int AvatarID => avatarID;
        public int UpgradeID => upgradeID;
        public Sprite AvatarSprite => avatarSprite;
    }
}
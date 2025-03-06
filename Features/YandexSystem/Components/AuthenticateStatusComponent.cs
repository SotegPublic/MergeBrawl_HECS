using HECSFramework.Core;
using HECSFramework.Unity;
using System;
using UnityEngine;

namespace Components
{
    [Serializable][Documentation(Doc.Player, "here we hold authenticate player status and his user info")]
    public sealed class AuthenticateStatusComponent : BaseComponent, IWorldSingleComponent
    {
        public bool IsAuthenticated;
        public string PlayerName;
        public string PlayerUID;
    }
}
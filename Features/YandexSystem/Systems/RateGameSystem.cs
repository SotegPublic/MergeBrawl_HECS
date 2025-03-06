using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;

namespace Systems
{
	[Serializable][Documentation(Doc.Yandex, "this system send rate game request")]
    public sealed class RateGameSystem : BaseSystem, IReactGlobalCommand<UpdateProgressBarCommand>
    {
        [Single]
        private YandexReceiverSystem yandexSystem;

        public void CommandGlobalReact(UpdateProgressBarCommand command)
        {
            if(command.Stage % 3 == 0)
            {
                if(Owner.World.GetSingleComponent<AuthenticateStatusComponent>().IsAuthenticated)
                {
                    yandexSystem.YandexReceiver.SendRateRequest();
                }
            }
        }

        public override void InitSystem()
        {
        }
    }
}
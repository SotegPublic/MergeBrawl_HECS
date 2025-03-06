using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;

namespace Systems
{
	[Serializable][Documentation(Doc.State, "AuthenticateStateSystem")]
    public sealed class AuthenticateStateSystem : BaseGameStateSystem, IReactGlobalCommand<AuthResultCommand>, IReactGlobalCommand<AuthCheckResultCommand>
    {
        [Single]
        private YandexReceiverSystem yandexSystem;

        protected override int State => GameStateIdentifierMap.AuthenticateState;

        public void CommandGlobalReact(AuthResultCommand command)
        {
            var authComponent = Owner.World.GetSingleComponent<AuthenticateStatusComponent>();
            authComponent.IsAuthenticated = command.IsAuthenticated;

            if (command.IsAuthenticated)
            {
                authComponent.PlayerName = yandexSystem.YandexReceiver.GetName();
                authComponent.PlayerUID = yandexSystem.YandexReceiver.GetUID();
            }

            EndState();
        }

        public void CommandGlobalReact(AuthCheckResultCommand command)
        {
            if(command.IsAuthenticated)
            {
                var authComponent = Owner.World.GetSingleComponent<AuthenticateStatusComponent>();

                authComponent.IsAuthenticated = true;
                authComponent.PlayerName = yandexSystem.YandexReceiver.GetName();
                authComponent.PlayerUID = yandexSystem.YandexReceiver.GetUID();
                EndState();
            }
            else
            {
                //Owner.World.Command(new AuthResultCommand { IsAuthenticated = false }); // todo - show auth UI and send AuthResultCommand
                yandexSystem.YandexReceiver.YandexDebug("We open login form");
                Owner.World.Command(new SetLoginStateCommand());
            }
        }

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {
            yandexSystem.YandexReceiver.YandexDebug("We check auth");
            yandexSystem.YandexReceiver.CheckAuthentificated();
        }
    }
}
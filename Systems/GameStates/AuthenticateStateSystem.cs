using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;
using Cysharp.Threading.Tasks;

namespace Systems
{
	[Serializable][Documentation(Doc.State, "AuthenticateStateSystem")]
    public sealed class AuthenticateStateSystem : BaseGameStateSystem, IReactGlobalCommand<AuthStateEndCommand>, IReactGlobalCommand<AuthCheckResultCommand>
    {
        [Single]
        private YandexReceiverSystem yandexSystem;


        protected override int State => GameStateIdentifierMap.AuthenticateState;

        public void CommandGlobalReact(AuthStateEndCommand command)
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
                yandexSystem.YandexReceiver.YandexDebug("We open login form");
                Owner.World.Command(new SetLoginStateCommand());
            }
        }

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {
            GetSystemLanguage();

            yandexSystem.YandexReceiver.YandexDebug("We check auth");
            yandexSystem.YandexReceiver.CheckAuthentificated();
        }

        private void GetSystemLanguage()
        {
            var lang = yandexSystem.YandexReceiver.GetLang();
            var playerProgress = Owner.World.GetSingleComponent<PlayerProgressComponent>();

            switch (lang)
            {
                case "ru":
                    playerProgress.CurrentLanguage = LanguageTypes.Rus;
                    break;
                case "tr":
                    playerProgress.CurrentLanguage = LanguageTypes.Trk;
                    break;
                case "en":
                default:
                    playerProgress.CurrentLanguage = LanguageTypes.Eng;
                    break;
            }

            Owner.World.Command(new SetLanguageCommand { LanguageType = playerProgress.CurrentLanguage });
        }
    }
}
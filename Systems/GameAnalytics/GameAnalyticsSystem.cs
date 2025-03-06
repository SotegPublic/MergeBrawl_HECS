using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using GameAnalyticsSDK;
using Commands;
using UnityEditor;

namespace Systems
{
	[Serializable][Documentation(Doc.GameAnalytics, "system for control game analytics")]
    public sealed class GameAnalyticsSystem : BaseSystem, IGlobalStart, IReactGlobalCommand<TransitionGameStateCommand>,
        IReactGlobalCommand<UpdateProgressBarCommand>, IReactGlobalCommand<SendRewardedAdvShowEventCommand>, IReactGlobalCommand<SendAdvShowEventCommand>
    {
        private PlayerProgressComponent playerProgress;
        private string sessionID;
        private string playerID;
        private bool isSessionFirstVisit = true;

        public void GlobalStart()
        {
            playerProgress = Owner.World.GetSingleComponent<PlayerProgressComponent>();
        }

        public override void InitSystem()
        {
            GameAnalytics.Initialize();
            sessionID = Guid.NewGuid().ToString();
#if UNITY_EDITOR
            playerID = "editor";
#else
            playerID = GameAnalytics.GetUserId();
#endif
        }

        public void CommandGlobalReact(UpdateProgressBarCommand command)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, command.Stage.ToString());
        }

        public void CommandGlobalReact(TransitionGameStateCommand command)
        {
            switch(command.To)
            {
                case GameStateIdentifierMap.AuthenticateState:
                    SendFunnelEvent("Auth");
                    break;
                case GameStateIdentifierMap.LoadPlayerState:
                    SendFunnelEvent("LoadPlayer");
                    break;
                case GameStateIdentifierMap.WarmUpState:
                    SendFunnelEvent("WarmUp");
                    break;
                case GameStateIdentifierMap.LoadLevelState:
                    if(command.From == GameStateIdentifierMap.WarmUpState)
                    {
                        SendFunnelEvent("LoadLevel");
                    }
                    break;
                case GameStateIdentifierMap.GameInProgressState:
                    if(isSessionFirstVisit)
                    {
                        SendFunnelEvent("Game");
                        isSessionFirstVisit = false;
                    }
                    break;
            }
        }

        public void CommandGlobalReact(SendAdvShowEventCommand command)
        {
            SendInterAdvEvent();
        }

        public void CommandGlobalReact(SendRewardedAdvShowEventCommand command)
        {
            var placment = Enum.GetName(typeof(AbilitiesTypes), command.RewardID); 

            SendRewardedAdvEvent(placment);
        }

        private void SendFunnelEvent(string stageName)
        {
            GameAnalytics.NewDesignEvent($"Funnel:{stageName}:{sessionID}:{playerID}");
        }

        public void SendRewardedAdvEvent(string placment)
        {
            GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo, "Yandex", placment);
        }

        public void SendInterAdvEvent()
        {
            GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial, "Yandex", "FullScreen");
        }
    }
}
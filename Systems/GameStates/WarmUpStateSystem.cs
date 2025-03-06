using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Commands;

namespace Systems
{
	[Serializable][Documentation(Doc.State, "Warmup state system")]
    public sealed class WarmUpStateSystem : BaseGameStateSystem, IReactGlobalCommand<AdvClosedCommand>
    {
        [Required]
        private WarmupVariablesComponent variables;

        [Required]
        private UpgradesGlobalHolderComponent upgradesHolder;

        [Single]
        private PoolingSystem poolingSystem;

        [Single]
        private YandexReceiverSystem yandexSystem;

        protected override int State => GameStateIdentifierMap.WarmUpState;

        private List<UniTask> taskList = new List<UniTask>();

        public override void InitSystem()
        {
        }

        public void CommandGlobalReact(AdvClosedCommand command)
        {
            if (Owner.World.GetSingleComponent<GameStateComponent>().CurrentState != GameStateIdentifierMap.WarmUpState)
                return;

            EndState();
        }

        protected async override void ProcessState(int from, int to)
        {
            WarmUpViews();

            await UniTask.WhenAll(taskList);

            ShowAdv();
        }

        private void WarmUpViews()
        {
            if (upgradesHolder.TryGetContainerByID(EntityContainersMap._Shelly, out var container))
            {
                var upgradeComponent = container.GetComponent<UpgradeComponent>();

                for (int i = 0; i < upgradeComponent.Upgrades.Length; i++)
                {
                    var viewRef = upgradeComponent.Upgrades[i].GetComponent<ViewReferenceGameObjectComponent>().ViewReference;
                    taskList.Add(poolingSystem.Warmup(viewRef, variables.WarmupViewsCount));
                }
            }
        }

        private void ShowAdv()
        {
            var playerProgress = Owner.World.GetSingleComponent<PlayerProgressComponent>();

            var currentTime = yandexSystem.YandexReceiver.GetTime();
            yandexSystem.YandexReceiver.YandexDebug("we get time = " + currentTime.ToString("yy:MM:dd HH/mm/ss"));

            playerProgress.LastAdvWatchDate = currentTime;
            yandexSystem.YandexReceiver.ShowAdvertising();
            yandexSystem.YandexReceiver.YandexDebug("we show adv");
        }
    }
}
using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;
using Cysharp.Threading.Tasks;

namespace Systems
{
	[Serializable][Documentation(Doc.State, "Clear level state")]
    public sealed class ClearStateSystem : BaseGameStateSystem, IGlobalStart 
    {
        protected override int State => GameStateIdentifierMap.ClearState;

        private EntitiesFilter filter;
        private PlayerProgressComponent playerProgress;
        private PlayerScoreCounterComponent playerScore;

        public override void InitSystem()
        {
            filter = Owner.World.GetFilter<IsOnSceneTagComponent>();
        }

        protected async override void ProcessState(int from, int to)
        {
            filter.ForceUpdateFilter();

            foreach (var view in filter)
            {
                view.HecsDestroy();
            }

            await UniTask.DelayFrame(1);

            playerScore.SetValue(0);
            playerProgress.ModelsOnField.Clear();
            playerProgress.Stage = 1;

            Owner.World.Command(new SaveCommand());

            EndState();
        }

        public void GlobalStart()
        {
            playerProgress = Owner.World.GetSingleComponent<PlayerProgressComponent>();
            playerScore = Owner.World.GetSingleComponent<PlayerScoreCounterComponent>();
        }
    }
}
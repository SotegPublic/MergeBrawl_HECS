using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;

namespace Systems
{
	[Serializable][Documentation(Doc.Sound, "Mute system")]
    public sealed class MuteSoundSystem : BaseSystem, IGlobalStart, IReactGlobalCommand<ChangeSoundMuteCommand>, IReactGlobalCommand<ShowAdvCommand>,
        IReactGlobalCommand<AdvClosedCommand>, IReactGlobalCommand<RewardedAdvClosedCommand>
    {
        private PlayerProgressComponent playerProgress;

        public void GlobalStart()
        {
            playerProgress = Owner.World.GetSingleComponent<PlayerProgressComponent>();
        }

        public override void InitSystem()
        {
        }

        public void CommandGlobalReact(ChangeSoundMuteCommand command)
        {
            AudioListener.volume = command.IsSoundOff ? 0 : 1;
        }

        public void CommandGlobalReact(ShowAdvCommand command)
        {
            if (playerProgress.IsSoundOff)
                return;

            AudioListener.volume = 0f;
        }

        public void CommandGlobalReact(AdvClosedCommand command)
        {
            if (playerProgress.IsSoundOff)
                return;

            AudioListener.volume = 1f;
        }

        public void CommandGlobalReact(RewardedAdvClosedCommand command)
        {
            if (playerProgress.IsSoundOff)
                return;

            AudioListener.volume = 1f;
        }
    }
}
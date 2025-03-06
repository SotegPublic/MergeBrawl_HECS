using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;
using Cysharp.Threading.Tasks;

namespace Systems
{
	[Serializable][Documentation(Doc.Sound, "this system start and stop back music")]
    public sealed class BackgroundSoundSystem : BaseSystem, IReactGlobalCommand<StartGameSoundCommand>, IReactGlobalCommand<StoptGameSoundCommand>,
        IReactGlobalCommand<ChangeMusicCommand>
    {
        [Required]
        private ActionsHolderComponent actionsHolder;

        public void CommandGlobalReact(StoptGameSoundCommand command)
        {
            actionsHolder.ExecuteAction(ActionIdentifierMap.StopBackgroundSound);
        }

        public void CommandGlobalReact(StartGameSoundCommand command)
        {
            actionsHolder.ExecuteAction(ActionIdentifierMap.StartBackgroundSound);
        }

        public async void CommandGlobalReact(ChangeMusicCommand command)
        {
            actionsHolder.ExecuteAction(ActionIdentifierMap.StopBackgroundSound);
            await UniTask.Delay(1050);
            actionsHolder.ExecuteAction(ActionIdentifierMap.StartBackgroundSound);
        }

        public override void InitSystem()
        {

        }
    }
}
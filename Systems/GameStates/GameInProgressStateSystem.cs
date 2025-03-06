using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;

namespace Systems
{
	[Serializable][Documentation(Doc.State, "Game in progress state")]
    public sealed class GameInProgressStateSystem : BaseGameStateSystem, IReactGlobalCommand<EndGameCommand>
    {
        protected override int State => GameStateIdentifierMap.GameInProgressState;

        public void CommandGlobalReact(EndGameCommand command)
        {
            EndState();
        }

        public override void InitSystem()
        {
        }

        protected override void ProcessState(int from, int to)
        {

        }
    }
}
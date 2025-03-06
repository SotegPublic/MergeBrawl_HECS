using System;
using HECSFramework.Core;
using Components;
using Commands;

namespace Systems
{
	[Serializable][Documentation(Doc.GameLogic, "CheckRecordSystem")]
    public sealed class CheckRecordSystem : BaseSystem, IReactGlobalCommand<CheckRecordScoreCommand>
    {
        [Required]
        private PlayerProgressComponent playerProgress;

        public void CommandGlobalReact(CheckRecordScoreCommand command)
        {
            if(command.NewScore > playerProgress.Record)
            {
                playerProgress.Record = command.NewScore;
                Owner.World.Command(new NewRecordScoreCommand { NewRecord = command.NewScore });
            }
        }

        public override void InitSystem()
        {
        }
    }
}
using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;
using System.Collections.Generic;

namespace Systems
{
	[Serializable][Documentation(Doc.Sound, "this system play fx sounds")]
    public sealed class FXSoundSystem : BaseSystem, IReactGlobalCommand<FXSoundCommand>,
        IReactGlobalCommand<BrawlSoundCommand>, IUpdatable
    {
        [Required]
        private ActionsHolderComponent actionsHolder;

        private List<BrawlSoundCommand> comands = new List<BrawlSoundCommand>(16);
        private bool isActive;
        private float waitTime = 0.3f;
        private float currentTime;

        public void CommandGlobalReact(BrawlSoundCommand command)
        {
            if (Owner.World.GetSingleComponent<PlayerProgressComponent>().IsVoicesOff)
                return;
            
            if(!isActive)
            {
                isActive = true;
            }
            comands.Add(command);
        }

        public void CommandGlobalReact(FXSoundCommand command)
        {
            actionsHolder.ExecuteAction(command.FXActionId, Owner);
        }

        public override void InitSystem()
        {
        }

        public void UpdateLocal()
        {
            if (!isActive)
                return;

            currentTime += Time.deltaTime;

            if(currentTime >= waitTime)
            {
                var max = 0;
                var index = 0;

                for (int i = 0; i < comands.Count; i++)
                {
                    if (comands[i].GradeIndex > max)
                    {
                        index = i;
                        max = comands[i].GradeIndex;
                    }
                }

                actionsHolder.ExecuteAction(comands[index].BrawlID, Owner);

                isActive = false;
                currentTime = 0;
                comands.Clear();
            }

        }
    }
}
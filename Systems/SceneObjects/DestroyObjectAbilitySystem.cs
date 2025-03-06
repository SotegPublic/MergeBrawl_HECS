using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;
using Cysharp.Threading.Tasks;

namespace Systems
{
	[Serializable][Documentation(Doc.Abilities, "this system control destroy ability")]
    public sealed class DestroyObjectAbilitySystem : BaseSystem, IReactGlobalCommand<DestroyObjectCommand>
    {
        [Single]
        private PlayerScoreCounterComponent playerScore;
        [Single]
        private ScoreConfigsHolderComponent scoreConfigs;

        public void CommandGlobalReact(DestroyObjectCommand command)
        {
            var targetTransform = command.Actor.Entity.GetTransform();
            var targetPosition = targetTransform.position;
            var objectID = command.Actor.Entity.GetComponent<SpawnObjectTagComponent>().ObjectID;

            command.Actor.Entity.RemoveComponent<IsOnSceneTagComponent>();
            command.Actor.Entity.HecsDestroy();

            Owner.World.Command(new FXSoundCommand { FXActionId = ActionIdentifierMap.Bomb });

            Owner.World.Command(new SpawnFXToCoordCommand
            {
                Coord = targetPosition,
                FXId = FXIdentifierMap.Bomb
            });

            var score = scoreConfigs.GetScore(objectID);
            playerScore.ChangeValue(score);
            Owner.World.Command(new SaveCommand());
        }

        public override void InitSystem()
        {
        }
    }
}
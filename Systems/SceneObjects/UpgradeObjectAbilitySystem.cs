using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;
using Cysharp.Threading.Tasks;
using GameAnalyticsSDK;
using UnityEngine.Analytics;
using DG.Tweening;

namespace Systems
{
	[Serializable][Documentation(Doc.Abilities, "this system control upgrade ability")]
    public sealed class UpgradeObjectAbilitySystem : BaseSystem, IReactGlobalCommand<UpgradeAbilityCommand>
    {
        [Single]
        private SpawnObjectsSystem spawnSystem;
        [Single]
        private PlayerScoreCounterComponent playerScore;
        [Single]
        private ScoreConfigsHolderComponent scoreConfigs;

        public async void CommandGlobalReact(UpgradeAbilityCommand command)
        {
            var targetTransform = command.Actor.Entity.GetTransform();
            var targetPosition = targetTransform.position;
            var targetRotation = targetTransform.rotation;
            var grade = command.Actor.Entity.GetComponent<UpgradeRootIndexComponent>().UpgradeLevel;
            var objectID = command.Actor.Entity.GetComponent<SpawnObjectTagComponent>().ObjectID;

            command.Actor.Entity.RemoveComponent<IsOnSceneTagComponent>();
            command.Actor.Entity.HecsDestroy();
            Owner.World.Command(new FXSoundCommand { FXActionId = ActionIdentifierMap.Upgrade});
            await UniTask.DelayFrame(1);

            var newBrawler = await spawnSystem.SpawnBrawlerObject(targetPosition, grade + 1, targetRotation);
            newBrawler.Init();
            newBrawler.Entity.AddComponent<IsOnSceneTagComponent>();
            var aliveEntity = new AliveEntity(newBrawler.Entity);

            Owner.World.Command(new SpawnFXToCoordCommand
            {
                Coord = targetPosition,
                FXId = FXIdentifierMap.Merge
            });

            var score = scoreConfigs.GetScore(objectID);
            playerScore.ChangeValue(score);

            await new WaitFor<ViewReadyTagComponent>(newBrawler.Entity).RunJob();

            if (!aliveEntity.IsAlive)
            {
                Owner.World.Command(new SaveCommand());
                return;
            }

            var rb = newBrawler.GetComponent<Rigidbody2D>();
            rb.isKinematic = false;

            var brawlID = aliveEntity.Entity.GetComponent<SpawnObjectTagComponent>().ObjectID;
            Owner.World.Command(new BrawlSoundCommand { BrawlID = brawlID, GradeIndex = grade + 1 });
            Owner.World.Command(new SaveCommand());
        }

        public override void InitSystem()
        {
        }
    }
}
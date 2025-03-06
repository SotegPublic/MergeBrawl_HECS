using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace Systems
{
	[Serializable][Documentation(Doc.MergeLogic, "this system merging brawls")]
    public sealed class MergeSystem : BaseSystem, IReactGlobalCommand<MergeCommand> 
    {
        [Single]
        private SpawnObjectsSystem spawnObjectsSystem;

        [Single]
        private PlayerScoreCounterComponent playerScore;

        [Required]
        private ScoreConfigsHolderComponent scoreConfigs;
        [Required]
        private MergeParametersComponent mergeParameters;

        public async void CommandGlobalReact(MergeCommand command)
        {
            Owner.World.GetEntityBySingleComponent<PlayerTagComponent>().GetOrAddComponent<VisualLocalLockComponent>().AddLock();

            var grade = command.FirstBrawler.GetComponent<UpgradeRootIndexComponent>().UpgradeLevel;
            var objectID = command.FirstBrawler.GetComponent<SpawnObjectTagComponent>().ObjectID;

            var targetTransform = command.FirstBrawler.GetPosition().y < command.SecondBrawler.GetPosition().y ? command.FirstBrawler.GetTransform() : command.SecondBrawler.GetTransform();
            var targetRotation = targetTransform.rotation;

            command.FirstBrawler.RemoveComponent<IsOnSceneTagComponent>();
            command.SecondBrawler.RemoveComponent<IsOnSceneTagComponent>();
            command.FirstBrawler.HecsDestroy();
            command.SecondBrawler.HecsDestroy();

            Owner.World.Command(new FXSoundCommand { FXActionId = ActionIdentifierMap.Merge });

            Owner.World.Command(new SpawnFXToCoordCommand
            {
                Coord = command.SpawnPosition,
                FXId = FXIdentifierMap.Merge
            });

            await UniTask.DelayFrame(1);

            var score = scoreConfigs.GetScore(objectID);
            playerScore.ChangeValue(score);
            Owner.World.Command(new SaveCommand());

            if (grade < mergeParameters.MaxGrade)
            {
                var newBrawler = await spawnObjectsSystem.SpawnBrawlerObject(command.SpawnPosition, grade + 1, targetRotation);
                newBrawler.Init();
                newBrawler.Entity.AddComponent<IsOnSceneTagComponent>();
                var aliveEntity = new AliveEntity(newBrawler.Entity);

                await new WaitFor<ViewReadyTagComponent>(newBrawler.Entity).RunJob();

                if(!aliveEntity.IsAlive)
                {
                    Owner.World.GetEntityBySingleComponent<PlayerTagComponent>().GetOrAddComponent<VisualLocalLockComponent>().Remove();
                    return;
                }

                var rb = newBrawler.GetComponent<Rigidbody2D>();
                rb.isKinematic = false;
                rb.AddForce(new Vector2(Random.Range(mergeParameters.HorizontalForceMin, mergeParameters.HorizontalForceMax), mergeParameters.VerticalForce), ForceMode2D.Impulse);
                rb.AddTorque(mergeParameters.TorqueForce, ForceMode2D.Impulse);

                var brawlID = aliveEntity.Entity.GetComponent<SpawnObjectTagComponent>().ObjectID;
                Owner.World.Command(new BrawlSoundCommand {  BrawlID = brawlID, GradeIndex = grade + 1 });
            }

            Owner.World.GetEntityBySingleComponent<PlayerTagComponent>().GetOrAddComponent<VisualLocalLockComponent>().Remove();
        }

        public override void InitSystem()
        {
        }
    }
}
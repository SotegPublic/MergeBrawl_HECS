using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;
using Helpers;

namespace Systems
{
	[Serializable][Documentation(Doc.MergeLogic, "this system detect colissions and detect necessity of merge")]
    public sealed class MergeDetectSystem : BaseSystem, IReactCommand<Collision2dCommand>
    {
        [Required]
        private SpawnObjectTagComponent tagComponent;

        public void CommandReact(Collision2dCommand command)
        {
            if(command.Collision.collider.TryGetActorFromCollision(out var actor))
            {
                if (!actor.Entity.ContainsMask<SpawnObjectTagComponent>())
                    return;
                if (actor.Entity.ContainsMask<IsMergingTagComponent>())
                    return;

                var id = actor.Entity.GetComponent<SpawnObjectTagComponent>().ObjectID;

                if(id == tagComponent.ObjectID)
                {
                    Owner.AddComponent<IsMergingTagComponent>();
                    actor.Entity.AddComponent<IsMergingTagComponent>();

                    var spawnPosition = command.Collision.GetContact(0).point;
                    Owner.World.Command(new MergeCommand { FirstBrawler = Owner, SecondBrawler = actor.Entity, SpawnPosition = spawnPosition });
                }
            }
        }

        public override void InitSystem()
        {
        }
    }
}
using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;
using Helpers;
using System.Collections.Generic;
using System.Buffers;

namespace Systems
{
	[Serializable][Documentation(Doc.GameLogic, "this system check end game conditions")]
    public sealed class EndGameZoneSystem : BaseSystem, IReactCommand<Trigger2dEnterCommand>, IReactCommand<TriggerStay2DCommand>
    {
        public void CommandReact(Trigger2dEnterCommand command)
        {
            if (command.Collider.TryGetActorFromCollision(out var actor))
            {
                if (!actor.Entity.ContainsMask<IsOnSceneTagComponent>())
                    return;

                using var contacts = HECSPooledArray<ContactPoint2D>.GetArray(128);               
                var contactsCount = command.Collider.GetContacts(contacts.Items);

                if (contactsCount > 0)
                {
                    for (int i = 0; i < contactsCount; i++)
                    {
                        if (contacts.Items[i].collider == null)
                            continue;
                        if (!contacts.Items[i].collider.TryGetActorFromCollision(out var colliderActor))
                            continue;

                        if (colliderActor.Entity.ContainsMask<IsOnSceneTagComponent>())
                        {
                            if (actor.Entity.GetComponent<UpgradeRootIndexComponent>().UpgradeLevel != colliderActor.Entity.GetComponent<UpgradeRootIndexComponent>().UpgradeLevel)
                            {
                                Owner.World.Command(new EndGameCommand());
                                Owner.World.Command(new FXSoundCommand { FXActionId = ActionIdentifierMap.GameOver });
                                return;
                            }
                        }
                    }
                }
            }
        }

        public void CommandReact(TriggerStay2DCommand command)
        {
            if (command.Collider.TryGetActorFromCollision(out var actor))
            {
                if (!actor.Entity.ContainsMask<IsOnSceneTagComponent>())
                    return;

                using var contacts = HECSPooledArray<ContactPoint2D>.GetArray(128);
                var contactsCount = command.Collider.GetContacts(contacts.Items);

                if (contactsCount > 0)
                {
                    for(int i = 0; i < contactsCount; i++)
                    {
                        if (contacts.Items[i].collider == null) 
                            continue;
                        if (!contacts.Items[i].collider.TryGetActorFromCollision(out var colliderActor))
                            continue;

                        if (colliderActor.Entity.ContainsMask<IsOnSceneTagComponent>())
                        {
                            if(actor.Entity.GetComponent<UpgradeRootIndexComponent>().UpgradeLevel != colliderActor.Entity.GetComponent<UpgradeRootIndexComponent>().UpgradeLevel)
                            {
                                Owner.World.Command(new EndGameCommand());
                                Owner.World.Command(new FXSoundCommand { FXActionId = ActionIdentifierMap.GameOver });
                                return;
                            }
                        }
                    }
                }
            }
        }

        public override void InitSystem()
        {
        }


    }
}
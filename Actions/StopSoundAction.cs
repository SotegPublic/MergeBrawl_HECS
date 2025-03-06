using System;
using Commands;
using HECSFramework.Core;
using Systems;
using UnityEngine;

[Serializable]
[Documentation(Doc.Action, Doc.Sound, "StopSoundAction")]
public sealed class StopSoundAction : IAction
{
    public AudioClip AudioClip;

    public void Action(Entity entity)
    {
        entity.World.Command(new StopSoundCommand { Owner = entity.GUID, Clip = AudioClip });
    }
}

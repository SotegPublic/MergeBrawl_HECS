using System;
using Commands;
using HECSFramework.Core;
using Systems;
using UnityEngine;

[Serializable]
[Documentation(Doc.Action, Doc.Sound, "PlaySoundAction")]
public sealed class PlaySoundAction : IAction
{
    public AudioClip AudioClip;
    public SoundType SoundType;
    public bool IsRepeatable;

    public void Action(Entity entity)
    {
        entity.World.Command(new PlaySoundCommand { AudioType = SoundType, Clip = AudioClip, IsRepeatable = IsRepeatable, Owner = entity.GUID});
    }
}

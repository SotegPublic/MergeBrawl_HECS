using System;
using Commands;
using HECSFramework.Core;
using Systems;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
[Documentation(Doc.Action, Doc.Sound, "PlayBrawlSoundAction")]
public sealed class PlayBrawlSoundAction : IAction
{
    public AudioClip[] AudioClips;
    public SoundType SoundType;
    public bool IsRepeatable;

    public void Action(Entity entity)
    {
        var isPlay = Random.Range(0, 101) > 40 ? true : false;

        if(isPlay)
        {
            var audioClip = AudioClips[Random.Range(0, AudioClips.Length)];
            entity.World.Command(new PlaySoundCommand { AudioType = SoundType, Clip = audioClip, IsRepeatable = IsRepeatable, Owner = entity.GUID });
        }
    }
}

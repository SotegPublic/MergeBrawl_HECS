using System;
using Commands;
using Components;
using HECSFramework.Core;
using Systems;

[Serializable]
[Documentation(Doc.Action, Doc.Sound, "PlayBackgroundSoundAction")]
public sealed class PlayBackgroundSoundAction : IAction
{
    public BackgroundSoundModel[] models;

    public void Action(Entity entity)
    {
        var playerProgress = entity.World.GetSingleComponent<PlayerProgressComponent>();
        var soundType = playerProgress.BackgroundSound;
        var currentBGType = entity.GetComponent<CurrentBGSoundTypeComponent>();

        for(int i = 0; i < models.Length; i++)
        {
            if(soundType == models[i].BackgroundType)
            {
                entity.World.Command(new PlaySoundCommand { AudioType = SoundType.FX, Clip = models[i].AudioClip, IsRepeatable = true, Owner = entity.GUID });
                currentBGType.CurrentSoundType = soundType;
            }
        }
    }
}

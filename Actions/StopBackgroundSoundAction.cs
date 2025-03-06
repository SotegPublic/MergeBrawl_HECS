using System;
using Commands;
using Components;
using HECSFramework.Core;

[Serializable]
[Documentation(Doc.Action, Doc.Sound, "StopSoundAction")]
public sealed class StopBackgroundSoundAction : IAction
{
    public BackgroundSoundModel[] models;

    public void Action(Entity entity)
    {
        var soundType = entity.GetComponent<CurrentBGSoundTypeComponent>().CurrentSoundType;

        for (int i = 0; i < models.Length; i++)
        {
            if (soundType == models[i].BackgroundType)
            {
                entity.World.Command(new StopSoundCommand { Clip = models[i].AudioClip, Owner = entity.GUID });
            }
        }
    }
}
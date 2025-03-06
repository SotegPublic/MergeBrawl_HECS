using System;
using System.Collections.Generic;
using Commands;
using Components;
using Cysharp.Threading.Tasks;
using HECSFramework.Core;
using HECSFramework.Unity;
using Newtonsoft.Json;
using UnityEngine;

namespace Systems
{
    [Serializable][Documentation(Doc.Player, "LoadPlayerSystem")]
    public sealed class LoadPlayerStateSystem : BaseGameStateSystem 
    {
        [Required]
        private PlayerProgressComponent playerProgress;

        [Single]
        private YandexReceiverSystem yandexSystem;

        protected override int State { get; } = GameStateIdentifierMap.LoadPlayerState;

        public override void InitSystem()
        {
        }

        protected async override void ProcessState(int from, int to)
        {
            var playerData = await yandexSystem.LoadPlayerData();

            if(playerData != null )
            {
                LoadToPlayerData(playerData);
                yandexSystem.YandexReceiver.YandexDebug("We load player data");
            }
            else
            {
                LoadBaseData();
                yandexSystem.YandexReceiver.YandexDebug("We load base data");
            }

            Owner.World.Command(new ChangeSoundMuteCommand { IsSoundOff = playerProgress.IsSoundOff });

            EndState();
        }

        private void LoadToPlayerData(string save)
        {
            var player = Owner.World.GetEntityBySingleComponent<PlayerTagComponent>();

            var container = JsonConvert.DeserializeObject<JSONEntityContainer>(save);
            container.DeserializeToEntity(player);

            if (!playerProgress.IsSaveInited)
            {
                yandexSystem.YandexReceiver.YandexDebug("We load first base data");
                LoadBaseData();
            }
        }

        private void LoadBaseData()
        {
            InitDefaultLanguageSettings();

            playerProgress.IsSoundOff = false;
            playerProgress.BackgroundSound = BackgroundSoundTypes.Funny;
            playerProgress.IsSaveInited = true;
            playerProgress.Stage = 1;

            Owner.World.Command(new SaveCommand());
        }

        private void InitDefaultLanguageSettings()
        {
            var lang = yandexSystem.YandexReceiver.GetLang();

            switch (lang)
            {
                case "ru":
                    playerProgress.CurrentLanguage = LanguageTypes.Rus;
                    break;
                case "tr":
                    playerProgress.CurrentLanguage = LanguageTypes.Trk;
                    break;
                case "en":
                default:
                    playerProgress.CurrentLanguage = LanguageTypes.Eng;
                    break;
            }
        }
    }
}
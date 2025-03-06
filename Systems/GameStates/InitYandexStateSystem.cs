using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;

namespace Systems
{
	[Serializable][Documentation(Doc.State, "Init Yandex State System")]
    public sealed class InitYandexStateSystem : BaseGameStateSystem, IUpdatable
    {
        [Single]
        private YandexReceiverSystem yandexSystem;

        private float time;
        private bool isUpdateActive;

        protected override int State => GameStateIdentifierMap.InitYndexState;

        public override void InitSystem()
        {
        }

        public void UpdateLocal()
        {
            if(isUpdateActive)
            {
                time += Time.deltaTime;

                if(time > 2)
                {
                    time = 0;

                    if (yandexSystem.YandexReceiver.IsYandexInit())
                    {
                        var deviceType = yandexSystem.YandexReceiver.GetDevice();
                        SetQuality(deviceType);
                        isUpdateActive = false;
                        EndState();
                    }
                }
            }
        }

        protected override void ProcessState(int from, int to)
        {
            yandexSystem.YandexReceiver.YandexDebug("Yandex inited: " + DateTime.Now.ToString("HH:mm:ss"));

            if (yandexSystem.YandexReceiver.IsYandexInit())
            {
                var deviceType = yandexSystem.YandexReceiver.GetDevice();
                SetQuality(deviceType);
                EndState();
            }
            else
            {
                isUpdateActive = true;
            }
        }

        public void SetQuality(string deviceType)
        {
            if (deviceType == "desktop")
            {
                Screen.SetResolution(1080, 1920, true);
                string[] names = QualitySettings.names;

                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] == "High Fidelity")
                    {
                        QualitySettings.SetQualityLevel(i, true);
                        yandexSystem.YandexReceiver.YandexDebug("We set High Fidelity");
                    }
                }
            }
            else
            {
                Screen.SetResolution(720, 1280, true);
                string[] names = QualitySettings.names;

                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] == "Performant")
                    {
                        QualitySettings.SetQualityLevel(i, true);
                        yandexSystem.YandexReceiver.YandexDebug("We set Performant");
                    }
                }
            }
        }
    }
}
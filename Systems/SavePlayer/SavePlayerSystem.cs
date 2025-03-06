using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using Commands;
using Newtonsoft.Json;
using Components;

namespace Systems
{
	[Serializable][Documentation(Doc.Save, Doc.Player, "this system save player data")]
    public sealed class SavePlayerSystem : BaseSystem, IReactGlobalCommand<SaveCommand> 
    {
        [Required]
        private PlayerProgressComponent playerProgress;

        [Single]
        private YandexReceiverSystem yandexSystem;

        private EntitiesFilter filter;
        private bool isAwaitingSave;

        public async void CommandGlobalReact(SaveCommand command)
        {
            if (isAwaitingSave)
                return;

            isAwaitingSave = true;

            await new WaitRemove<VisualLocalLockComponent>(Owner).RunJob();

            GetSceneObjects();

            var saveContainer = new JSONEntityContainer();
            saveContainer.SerializeEntitySavebleOnly(Owner);
            var data = JsonConvert.SerializeObject(saveContainer);

            yandexSystem.SavePlayerData(data);

            isAwaitingSave = false;
        }

        public override void InitSystem()
        {
            filter = Owner.World.GetFilter<IsOnSceneTagComponent>();
        }

        private void GetSceneObjects()
        {
            playerProgress.ModelsOnField.Clear();
            filter.ForceUpdateFilter();

            foreach(var view in filter)
            {
                var rotation = view.GetRotation();

                playerProgress.ModelsOnField.Add(
                    new ViewModel(
                        new Vector3Serialize(view.GetPosition()),
                        new QuaternionSerialize(rotation.w, rotation.x, rotation.y, rotation.z),
                        view.GetComponent<UpgradeRootIndexComponent>().UpgradeLevel
                        )); 
            }
        }
    }
}

namespace Commands
{
    [Documentation(Doc.Commands, "Save player data command")]
    public struct SaveCommand : IGlobalCommand
    {
    }
}

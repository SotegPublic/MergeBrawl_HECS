using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using System.Collections.Generic;
using Commands;

namespace Systems
{
	[Serializable][Documentation(Doc.State, "Load level state")]
    public sealed class LoadLevelStateSystem : BaseGameStateSystem, IGlobalStart
    {
        [Single]
        private SpawnObjectsSystem spawnSystem;

        protected override int State => GameStateIdentifierMap.LoadLevelState;

        private PlayerProgressComponent playerProgress;
        private List<UniTask<Actor>> taslList = new List<UniTask<Actor>>(64);

        public void GlobalStart()
        {
            playerProgress = Owner.World.GetSingleComponent<PlayerProgressComponent>();
        }

        public override void InitSystem()
        {
        }

        protected async override void ProcessState(int from, int to)
        {
            var authenticateStatusComponent = Owner.World.GetSingleComponent<AuthenticateStatusComponent>();

            taslList.Clear();

            LoadSceneViewsFromField();

            if (taslList.Count > 0)
            {
                var result = await UniTask.WhenAll(taslList);

                for (int i = 0; i < result.Length; i++)
                {
                    result[i].GetComponent<Rigidbody2D>().isKinematic = false;
                }
            }

            if (!authenticateStatusComponent.IsAuthenticated)
            {
                Owner.World.Command(new HideUICommand { UIViewType = UIIdentifierMap.LeaderBordPanel_UIIdentifier });
            }

            EndState();
        }

        private void LoadSceneViewsFromField()
        {
            if (playerProgress.ModelsOnField.Count == 0)
                return;
            
            foreach (var model in playerProgress.ModelsOnField)
            {
                taslList.Add(SpawnView(model));
            }
        }

        private async UniTask<Actor> SpawnView(ViewModel model)
        {
            var rotation = new Quaternion(model.Rotation.X, model.Rotation.Y, model.Rotation.Z, model.Rotation.W);
            var view = await spawnSystem.SpawnBrawlerObject(new Vector3(model.Position.X, model.Position.Y, model.Position.Z), model.GradeID, rotation);
            view.Init();

            view.Entity.AddComponent<IsOnSceneTagComponent>();
            await new WaitFor<ViewReadyTagComponent>(view.Entity).RunJob();

            return view;
        }
    }
}
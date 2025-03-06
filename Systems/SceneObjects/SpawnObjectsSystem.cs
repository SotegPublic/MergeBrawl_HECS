using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;
using Sirenix.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace Systems
{
	[Serializable][Documentation(Doc.Spawn, "ObjectsSpawnSystem")]
    public sealed class SpawnObjectsSystem : BaseSystem
    {
        [Required]
        public UpgradesGlobalHolderComponent UpgradesGlobalHolder;

        public async UniTask<Actor> SpawnBrawlerObject(Vector2 spawnPoint, int grade, Quaternion spawnRotation = default)
        {
            if (UpgradesGlobalHolder.TryGetContainerByID(EntityContainersMap._Shelly, out var container))
            {
                var targetGradeContainer = container.GetComponent<UpgradeComponent>().Upgrades[grade];
                var actor = await targetGradeContainer.GetActor(position: spawnPoint, rotation: spawnRotation);

                return actor;
            }

            throw new Exception("unknown container id");
        }

        public override void InitSystem()
        {
            
        }
    }
}
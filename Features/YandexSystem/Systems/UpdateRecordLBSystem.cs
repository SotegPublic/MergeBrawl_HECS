using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using Commands;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Xml;

namespace Systems
{
	[Serializable][Documentation(Doc.Yandex, Doc.Player, "This system checks the score record every time the score changes.")]
    public sealed class UpdateRecordLBSystem : BaseSystem, IGlobalStart, IUpdatable, IReactGlobalCommand<NewRecordScoreCommand>,
        IReactGlobalCommand<UpdateLBCommand>, IReactGlobalCommand<TransitionGameStateCommand>
    {
        [Required]
        private LBUpdateParametersComponent parameters;
        [Required]
        private LBDataHolderComponent dataHolder;

        [Single]
        private YandexReceiverSystem yandexSystem;

        private PlayerProgressComponent playerProgress;
        private AuthenticateStatusComponent authenticateStatus;
        private float currentLBUpdateTime;
        private float currentSetUpdateTime;
        private bool isActive;
        private bool isNeedUpdate;

        public void CommandGlobalReact(NewRecordScoreCommand command)
        {
            isNeedUpdate = true;
        }

        public void GlobalStart()
        {
            playerProgress = Owner.World.GetSingleComponent<PlayerProgressComponent>();
            authenticateStatus = Owner.World.GetSingleComponent<AuthenticateStatusComponent>();
        }

        public override void InitSystem()
        {
        }
        public void CommandGlobalReact(TransitionGameStateCommand command)
        {
            if (command.To == GameStateIdentifierMap.GameInProgressState)
            {
                isActive = true;
            }
            if(command.To == GameStateIdentifierMap.LoadLevelState)
            {
                yandexSystem.YandexReceiver.UpdateLeaderBoard(parameters.RecordLBName, parameters.TopQuantity, true, parameters.AroundQuantity);
            }
        }

        public void UpdateLocal()
        {
            if (!isActive)
                return;
            if (!authenticateStatus.IsAuthenticated)
                return;

            currentLBUpdateTime += Time.deltaTime;
            currentSetUpdateTime += Time.deltaTime;


            if (currentSetUpdateTime > parameters.SecondsBetweenSendRecord)
            {
                if (isNeedUpdate)
                {
                    yandexSystem.YandexReceiver.SetLeaderBoardValue(parameters.RecordLBName, (int)playerProgress.Record);
                    isNeedUpdate = false;
                }

                currentSetUpdateTime = 0;
            }

            if (currentLBUpdateTime > parameters.SecondsBetweenUpdate)
            {
                yandexSystem.YandexReceiver.UpdateLeaderBoard(parameters.RecordLBName, parameters.TopQuantity, true, parameters.AroundQuantity);
                currentLBUpdateTime = 0;
            }
        }

        public void CommandGlobalReact(UpdateLBCommand command)
        {
            LeaderboardData leaderboardData = JsonConvert.DeserializeObject<LeaderboardData>(command.LbJSON);
            var playerName = Owner.World.GetSingleComponent<AuthenticateStatusComponent>().PlayerName;
            var playerID = Owner.World.GetSingleComponent<AuthenticateStatusComponent>().PlayerUID;

            if (leaderboardData.UserRank == 0)
            {
                var rank = 0;
                var score = 0;

                dataHolder.UpdateData(new PlayerLBData(playerName, rank, score, true, true, playerID), dataHolder.ListLenth - 1);

                for (int i = 0; i < leaderboardData.Entries.Count; i++)
                {
                    var entry = leaderboardData.Entries[i];
                    var index = entry.Rank - 1;
                    var name = entry.Player.ScopePermissions.Public_Name == "allow" ? entry.Player.PublicName : "anonymous";
                    var isHasName = entry.Player.ScopePermissions.Public_Name == "allow" ? true : false;
                    var uniqueID = entry.Player.UniqueID;

                    dataHolder.UpdateData(new PlayerLBData(name, entry.Rank, entry.Score, false, isHasName, uniqueID), index);
                }
            }
            else if(leaderboardData.UserRank <= 3)
            {
                dataHolder.ClearLastElement();

                for (int i = 0; i < leaderboardData.Entries.Count; i++)
                {
                    var entry = leaderboardData.Entries[i];
                    var index = entry.Rank - 1;
                    var isPlayer = entry.Rank == leaderboardData.UserRank ? true : false;
                    var name = isPlayer ? playerName : entry.Player.ScopePermissions.Public_Name == "allow" ? entry.Player.PublicName : "anonymous";
                    var isHasName = isPlayer ? true : entry.Player.ScopePermissions.Public_Name == "allow" ? true : false;
                    var uniqueID = entry.Player.UniqueID;

                    dataHolder.UpdateData(new PlayerLBData(name, entry.Rank, entry.Score, isPlayer, isHasName, uniqueID), index);
                }
            }
            else
            {
                for (int i = 0; i < leaderboardData.Entries.Count; i++)
                {
                    var entry = leaderboardData.Entries[i];
                    var index = entry.Rank > dataHolder.ListLenth - 1 ? dataHolder.ListLenth - 1 : entry.Rank - 1;
                    var isPlayer = entry.Rank == leaderboardData.UserRank ? true : false;
                    var name = isPlayer ? playerName : entry.Player.ScopePermissions.Public_Name == "allow" ? entry.Player.PublicName : "anonymous";
                    var isHasName = isPlayer ? true : entry.Player.ScopePermissions.Public_Name == "allow" ? true : false;
                    var uniqueID = entry.Player.UniqueID;
                    
                    dataHolder.UpdateData(new PlayerLBData(name, entry.Rank, entry.Score, isPlayer, isHasName, uniqueID), index);
                }
            }

            Owner.World.Command(new UpdateLBListCommand { Players = dataHolder.Players });
        }
    }
}
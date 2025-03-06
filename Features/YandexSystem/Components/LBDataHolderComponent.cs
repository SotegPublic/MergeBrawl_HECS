using HECSFramework.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    [Serializable][Documentation(Doc.Yandex, "here we hold data from leaderboard response")]
    public sealed class LBDataHolderComponent : BaseComponent
    {
        [SerializeField] private List<PlayerLBData> players;
        [SerializeField] private int listLenth;

        public List<PlayerLBData> Players => players;
        public int ListLenth => listLenth;

        public void UpdateData(PlayerLBData data, int index)
        {
            players[index] = data;
        }

        public void ClearLastElement()
        {
            players[players.Count - 1].Clear();
        }

        public override void Init()
        {
            base.Init();
            players = new List<PlayerLBData>(listLenth);

            for(int i = 0; i < listLenth; i++)
            {
                players.Add(new PlayerLBData());
            }
        }
    }

    public class PlayerLBData
    {
        private string name;
        private int rank;
        private int record;
        private bool isPlayer;
        private bool isHasName;
        private bool isClear;
        private string unicID;

        public PlayerLBData (string playerName, int playerRank, int playerScore, bool isPlayer, bool isHasName, string unicID)
        {
            name = playerName;
            rank = playerRank;
            record = playerScore;
            this.isPlayer = isPlayer;
            this.isHasName = isHasName;
            isClear = false;
            this.unicID = unicID;
        }

        public PlayerLBData()
        {
            name = "??????";
            rank = 0;
            record = 0;
            isPlayer = false;
            isHasName = false;
            isClear = true;
            unicID = default;
        }

        public string Name => name;
        public int Rank => rank;
        public int Record => record;
        public bool IsPlayer => isPlayer;
        public bool IsHasName => isHasName;
        public bool IsClear => isClear;
        public string UnicID => unicID;

        public void Clear()
        {
            name = "??????";
            rank = 0;
            record = 0;
            isPlayer = false;
            isHasName = false;
            isClear = true;
            unicID = default;
        }
    }
}
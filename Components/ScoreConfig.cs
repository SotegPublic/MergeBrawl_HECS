using System;
using UnityEngine;
using Helpers;

namespace Components
{
    [Serializable]
    public class ScoreConfig
    {
        [SerializeField][IdentifierDropDown(nameof(SpawnObjectsIdentifier))] private int objectID;
        [SerializeField] private int scoreValue;

        public int ObjectID => objectID;
        public int ScoreValue => scoreValue;
    }
}
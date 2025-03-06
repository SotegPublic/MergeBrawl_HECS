using HECSFramework.Core;
using HECSFramework.Unity;
using System;
using UnityEngine;

namespace Components
{
    [Serializable][Documentation(Doc.Yandex, "here we hold parameters for LB update systems")]
    public sealed class LBUpdateParametersComponent : BaseComponent
    {
        [SerializeField] private int secondsBetweenUpdate;
        [SerializeField] private int secondsBetweenSendRecord;
        [SerializeField] private string scoreLBName;
        [SerializeField] private string recordLBName;
        [SerializeField] private int topQuantity;
        [SerializeField] private int aroundQuantity;

        public int SecondsBetweenUpdate => secondsBetweenUpdate;
        public int SecondsBetweenSendRecord => secondsBetweenSendRecord;
        public string ScoreLBName => scoreLBName;
        public string RecordLBName => recordLBName;
        public int TopQuantity => topQuantity;
        public int AroundQuantity => aroundQuantity;
    }
}
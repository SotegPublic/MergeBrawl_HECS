using HECSFramework.Core;
using HECSFramework.Unity;
using System;
using UnityEngine;

namespace Components
{
    [Serializable][Documentation(Doc.MergeLogic, "here we set max grage for merge system")]
    public sealed class MergeParametersComponent : BaseComponent, IWorldSingleComponent
    {
        [SerializeField] private int maxGrade;
        [SerializeField] private float horizontalForceMin;
        [SerializeField] private float horizontalForceMax;
        [SerializeField] private float verticalForce;
        [SerializeField] private float torqueForce;

        public int MaxGrade => maxGrade;
        public float HorizontalForceMin => horizontalForceMin;
        public float HorizontalForceMax => horizontalForceMax;
        public float VerticalForce => verticalForce;
        public float TorqueForce => torqueForce;
    }
}
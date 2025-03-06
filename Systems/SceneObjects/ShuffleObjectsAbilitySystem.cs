using System;
using HECSFramework.Unity;
using HECSFramework.Core;
using UnityEngine;
using Components;
using System.Collections.Generic;
using Commands;
using Random = System.Random;

namespace Systems
{
	[Serializable][Documentation(Doc.Abilities, "this system shuffle objects queue")]
    public sealed class ShuffleObjectsAbilitySystem : BaseSystem, IReactGlobalCommand<ShuffleAbilityCommand>
    {
        [Required]
        private ObjectsQueueComponent objectsQueue;

        private List<int> tmpList;
        private int lastElement;

        public void CommandGlobalReact(ShuffleAbilityCommand command)
        {
            for(int i = 0; i < objectsQueue.MaxQueueCount; i++)
            {
                tmpList[i] = objectsQueue.ObjectIDs.Dequeue();
                if(i == objectsQueue.MaxQueueCount - 1)
                {
                    lastElement = tmpList[i];
                }
            }

            Random rng = new Random();
            int n = tmpList.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = tmpList[k];
                tmpList[k] = tmpList[n];
                tmpList[n] = value;
            }

            //some more random, if last element doesn't change, shuffle it with penulte
            if (lastElement == tmpList[tmpList.Count - 1] && tmpList.Count > 1)
            {
                tmpList[tmpList.Count - 1] = tmpList[tmpList.Count - 2];
                tmpList[tmpList.Count - 2] = lastElement;
            }

            for(int i = 0; i < tmpList.Count; i++)
            {
                objectsQueue.ObjectIDs.Enqueue(tmpList[i]);
            }

            Owner.World.Command(new EndShuffleCommand());
            Owner.World.Command(new SaveCommand());
        }

        public override void InitSystem()
        {
            tmpList = new List<int>(objectsQueue.MaxQueueCount);

            for(int i = 0; i < tmpList.Capacity; i++)
            {
                tmpList.Add(0);
            }
        }
    }
}
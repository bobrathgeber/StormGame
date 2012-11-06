using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StormGame
{
    //Fort, Train Chase, Destroy All, Destroy as much as possible
    public enum ObjectiveType
    {
        TargetDestructible,
        MultipleTargets,
        ClearAll,
        MaximizeDestruction
    }

    class Objective
    {
        private ObjectiveType Type;
        private List<Destructible> targets;
        private float timeLimit;
        private float currentTime;

        public Objective(Destructible Target)
        {
            Type = ObjectiveType.TargetDestructible;
            targets = new List<Destructible>();
            targets.Add(Target);
        }

        public Objective(List<Destructible> Targets)
        {
            Type = ObjectiveType.MultipleTargets;
            this.targets = Targets;
        }

        public Objective(float TimeLimit)
        {
            Type = ObjectiveType.ClearAll;
            timeLimit = TimeLimit;
            currentTime = 0f;
        }

        public bool CheckCompletion()
        {
            if (targets != null)
            {
                foreach (Destructible d in targets)
                {
                    if (d.isAlive)
                        return false;
                }
                return true;
            }

            return false;
        }

        public bool CheckCompletion(float elapsedTime)
        {
            currentTime += elapsedTime;
            if (currentTime >= timeLimit)
                return true;
            else
                return false;
        }
    }
}

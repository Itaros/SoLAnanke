using System;
using System.Collections.Generic;
using System.Linq;

namespace Ananke.Attachment.Core.Phases
{
    public class PhaseController
    {

        public PhaseToken AcquirePhase(string name)
        {
            var activated = _tokenGroups.FirstOrDefault(tg => tg.Item1.Name == name)?.Item1;
            if (activated == null)
            {
                activated = new PhaseToken(name);
                _tokenGroups.Add(new Tuple<PhaseToken, List<PhaseAction>>(activated, new List<PhaseAction>()));
            }

            return activated;
        }
        
        /// <summary>
        /// Registers action for phase
        /// </summary>
        /// <param name="action"></param>
        /// <returns>True on success</returns>
        public bool RegisterPhaseAction(PhaseAction action)
        {
            var candidate = _tokenGroups.FirstOrDefault(tg => tg.Item1 == action.Phase);
            if (candidate == null)
                return false;
            candidate.Item2.Add(action);
            return true;
        }
        
        private readonly List<Tuple<PhaseToken, List<PhaseAction>>> _tokenGroups = new List<Tuple<PhaseToken,List<PhaseAction>>>();

        public bool RunPhase(PhaseToken phase, AnankeContext context)
        {
            var group = _tokenGroups.FirstOrDefault(tg => tg.Item1 == phase);
            if (group == null)
                return false;
            var actions = group.Item2;
            //TODO: Should have some sorting rules.
            foreach (var action in actions)
            {
                action.Do(context);
            }

            return true;
        }
    }
}
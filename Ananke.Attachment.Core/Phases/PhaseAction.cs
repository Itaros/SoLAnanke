using System;
using Ananke.Attachment.Core.Mod;

namespace Ananke.Attachment.Core.Phases
{
    public class PhaseAction
    {
        public delegate void OnPhaseStartedHandler(AnankeContext context);

        public PhaseAction(PhaseToken token, Type modType, OnPhaseStartedHandler handler)
        {
            Phase = token;
            ModType = modType;
            _handler = handler;
            if(!typeof(ISoLModV1).IsAssignableFrom(modType))
                throw new ArgumentException("Mod type is expected!");
        }
        
        public Type ModType { get; }
        
        public PhaseToken Phase { get; }

        private OnPhaseStartedHandler _handler;

        internal void Do(AnankeContext context)
        {
            _handler(context);
        }
    }
}
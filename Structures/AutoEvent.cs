using System.Collections.Generic;
using Exiled.API.Features;
using Toys = Exiled.API.Features.Toys;
using Exiled.API.Enums;
using Exiled.Events.EventArgs;
using UnityEngine;
using MEC;

namespace SmokyPlugin.Structures
{
    public abstract class AutoEvent
    {
        public abstract string name { get; }
        public abstract bool AllowLaterJoinSpawn { get; }
        public abstract bool AllowTeamsRespawn { get; }
        protected abstract CoroutineHandle EventCoroutine { get; set; }

        public virtual void PrepareRound()
        {
            
        }
        protected abstract IEnumerator<float> EventProgress();
        public abstract void OnRoundStarted();
        public virtual void OnVerified(VerifiedEventArgs ev)
        {

        }
        public virtual void OnEscaping(EscapingEventArgs ev)
        {

        }
        public virtual void OnDied(DiedEventArgs ev)
        {

        }
        public virtual void OnLeft(LeftEventArgs ev)
        {

        }
        public abstract void OnRestartingRound();
    }
}
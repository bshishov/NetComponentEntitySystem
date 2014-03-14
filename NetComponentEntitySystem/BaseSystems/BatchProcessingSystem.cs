#region

using System;
using System.Collections.Generic;
using NetComponentEntitySystem.Utilities;

#endregion

namespace NetComponentEntitySystem.BaseSystems
{
    public abstract class BatchProcessingSystem : EntitySystem
    {
        protected BatchProcessingSystem(Aspect<Type> availableTypes)
            : base(availableTypes)
        {
        }

        public override void Update()
        {
            lock (EntitiesLock)
            {
                Process(Entities);
            }
        }

        public abstract void Process(IEnumerable<Entity> entities);
    }
}
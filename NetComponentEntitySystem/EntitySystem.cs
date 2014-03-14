#region

using System;
using System.Collections.Generic;
using NetComponentEntitySystem.Utilities;

#endregion

namespace NetComponentEntitySystem
{
    public abstract class EntitySystem : IEntitySystem
    {
        protected readonly Aspect<Type> Aspect;
        protected IList<Entity> Entities;
        protected object EntitiesLock;

        internal EntitySystem(Aspect<Type> types)
        {
            Entities = new List<Entity>();
            Aspect = types;
            EntitiesLock = new object();
        }

        #region IEntitySystem Members

        public void TryAddEntity(Entity entity)
        {
            // Entity must have required components
            if (!Aspect.Query(entity.HasComponent))
                return;

            lock (EntitiesLock)
            {
                // No duplicated entities
                if (Entities.Contains(entity))
                    return;

                Entities.Add(entity);
            }
        }

        public void TryRemoveEntity(Entity entity)
        {
            lock (EntitiesLock)
            {
                if (Entities.Contains(entity))
                    Entities.Remove(entity);
            }
        }

        public abstract void Update();

        #endregion
    }
}
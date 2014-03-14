#region

using System;
using System.Collections.Generic;
using NetComponentEntitySystem.Utilities;

#endregion

namespace NetComponentEntitySystem
{
    /// <summary>
    ///     Complex entity processing system - system for processin entities of many components
    ///     For example renderer needs lights and meshes
    ///     so there will be 2 aspects {Transform, Model} and {Transform, Light}
    /// </summary>
    public abstract class ComplexEntitySystem : IEntitySystem
    {
        protected Dictionary<string, Aspect<Type>> Aspects;
        protected Dictionary<string, List<Entity>> Entities;
        protected object EntitiesLock;

        protected ComplexEntitySystem(Dictionary<string, Aspect<Type>> aspects)
        {
            Entities = new Dictionary<string, List<Entity>>();
            Aspects = aspects;
            EntitiesLock = new object();

            foreach (var aspect in Aspects)
            {
                Entities[aspect.Key] = new List<Entity>();
            }
        }

        #region IEntitySystem Members

        public void TryAddEntity(Entity entity)
        {
            foreach (var aspectkvp in Aspects)
            {
                if (!aspectkvp.Value.Query(entity.HasComponent))
                    continue;

                lock (EntitiesLock)
                {
                    Entities[aspectkvp.Key].Add(entity);
                }
            }
        }

        public void TryRemoveEntity(Entity entity)
        {
            foreach (var aspectkvp in Aspects)
            {
                lock (EntitiesLock)
                {
                    if (Entities[aspectkvp.Key].Contains(entity))
                        Entities[aspectkvp.Key].Remove(entity);
                }
            }
        }

        public abstract void Update();

        #endregion
    }
}
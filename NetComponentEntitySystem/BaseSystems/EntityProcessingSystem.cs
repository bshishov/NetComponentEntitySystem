#region

using System;
using NetComponentEntitySystem.Utilities;

#endregion

namespace NetComponentEntitySystem.BaseSystems
{
    public abstract class EntityProcessingSystem : EntitySystem
    {
        protected EntityProcessingSystem(Aspect<Type> availableTypes)
            : base(availableTypes)
        {
        }

        public override void Update()
        {
            lock (EntitiesLock)
            {
                Begin();
                foreach (var entity in Entities)
                {
                    Process(entity);
                }
                End();
            }
        }

        public abstract void Begin();
        public abstract void Process(Entity entity);
        public abstract void End();
    }

    public abstract class EntityProcessingSystem<T1> : EntitySystem
        where T1 : IComponent
    {
        protected EntityProcessingSystem()
            : base(new Aspect<Type>().AllOf(typeof (T1)))
        {
        }

        public override void Update()
        {
            lock (EntitiesLock)
            {
                Begin();
                foreach (var entity in Entities)
                {
                    Process(entity, entity.GetComponent<T1>());
                }
                End();
            }
        }

        public abstract void Begin();
        public abstract void Process(Entity entity, T1 component);
        public abstract void End();
    }

    public abstract class EntityProcessingSystem<T1, T2> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
    {
        protected EntityProcessingSystem()
            : base(new Aspect<Type>().AllOf(typeof (T1), typeof (T2)))
        {
        }

        public override void Update()
        {
            lock (EntitiesLock)
            {
                Begin();
                lock (Entities)
                {
                    foreach (var entity in Entities)
                    {
                        Process(entity, entity.GetComponent<T1>(), entity.GetComponent<T2>());
                    }
                }
                End();
            }
        }

        public abstract void Begin();
        public abstract void Process(Entity entity, T1 component1, T2 component2);
        public abstract void End();
    }
}
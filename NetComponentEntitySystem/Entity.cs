#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace NetComponentEntitySystem
{
    /// <summary>
    ///     Entity, gameobject class. Just a collection of scripts and components.
    /// </summary>
    public sealed class Entity
    {
        /// <summary>
        ///     Collection of entity components (data)
        /// </summary>
        private readonly IDictionary<Type, IComponent> _components;

        private readonly Object _lock;

        /// <summary>
        ///     Collection of entity scripts (logic)
        /// </summary>
        private readonly List<IScript> _scripts;

        /// <summary>
        ///     Reference to a world
        /// </summary>
        private readonly EntityWorld _world;

        public string Tag { get; set; }

        public Entity(EntityWorld world)
        {
            _components = new Dictionary<Type, IComponent>();
            _scripts = new List<IScript>();
            _world = world;
            _lock = new object();
        }

        public IEnumerable<IComponent> Components
        {
            get
            {
                lock (_lock)
                {
                    return _components.Values;
                }
            }
        }

        public bool HasComponent<T>()
            where T : IComponent
        {
            lock (_lock)
            {
                return _components.ContainsKey(typeof (T));
            }
        }

        public bool HasComponent(Type type)
        {
            lock (_lock)
            {
                return _components.ContainsKey(type);
            }
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            lock (_lock)
            {
                return (T) _components[typeof (T)];
            }
        }

        public void AddComponent<T>(T component)
            where T : IComponent
        {
            lock (_lock)
            {
                _components.Add(typeof (T), component);
            }
        }

        public void AddComponent<T>()
            where T : IComponent, new()
        {
            _components.Add(typeof (T), new T());
        }

        public void AddComponent(Type T, IComponent component)
        {
            lock (_lock)
            {
                _components.Add(T, component);
            }
        }

        public void AddScript<T>(T script)
            where T : Script
        {
            _scripts.Add(script);
            script.Initialize(this);
        }

        public void AddScript<T>()
            where T : Script, new()
        {
            _scripts.Add(CreateScript<T>());
        }

        public bool HasScript<T>()
            where T : Script
        {
            return _scripts.OfType<T>().Any();
        }

        public void RemoveScript<T>(T script)
            where T : Script
        {
            _scripts.Remove(script);
        }

        public void RemoveScript<T>()
            where T : Script
        {
            RemoveScript(_scripts.OfType<T>().First());
        }

        public T CreateScript<T>()
            where T : Script, new()
        {
            var script = new T();
            script.Initialize(this);
            return script;
        }

        /// <summary>
        ///     Destroy the entity on the next update
        /// </summary>
        public void Destroy()
        {
            _world.RemoveEntity(this);
        }

        public void OnUpdate(float elapsed)
        {
            foreach (var script in _scripts)
            {
                script.OnUpdate(elapsed);
            }
        }

        // TODO: Optimize! (Message masks)
        public void OnMessage(Message message)
        {
            foreach (var script in _scripts)
            {
                script.OnMessage(message);
            }
        }

    }
}
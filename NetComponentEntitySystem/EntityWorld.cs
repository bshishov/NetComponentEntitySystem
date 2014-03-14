#region

using System;
using System.Collections.Generic;

#endregion

namespace NetComponentEntitySystem
{
    /// <summary>
    ///     EntityWorld contains and manages all entities and systems
    /// </summary>
    public sealed class EntityWorld
    {
        private readonly List<Entity> _entities;

        /// <summary>
        ///     enitites that waiting delete
        /// </summary>
        private readonly List<Entity> _pending;

        private readonly SystemManager _systemManager;

        private int _entitiesCount;
        private int _i;

        /// <summary>
        ///     Creates a blank world
        /// </summary>
        public EntityWorld()
        {
            _entities = new List<Entity>();
            _pending = new List<Entity>();
            _systemManager = new SystemManager();
        }

        public SystemManager SystemManager
        {
            get { return _systemManager; }
        }

        private void UpdateCount()
        {
            _entitiesCount = _entities.Count;
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
            UpdateCount();
            _systemManager.AddEntity(entity);
        }

        /// <summary>
        ///     Removes entity from world (and systems)
        /// </summary>
        /// <param name="entity">The entity you want to remove</param>
        public void RemoveEntity(Entity entity)
        {
            _pending.Add(entity);
        }

        /// <summary>
        ///     Update gameloop
        /// </summary>
        /// <param name="elapsed">Milliseconds elapsed since last call</param>
        public void Update(float elapsed)
        {
            // Clear entities, that waits for delete
            for (_i = 0; _i < _pending.Count; _i++)
            {
                var entity = _pending[_i];
                _entities.Remove(entity);
                _systemManager.RemoveEntity(entity);
                UpdateCount();
            }
            _pending.Clear();

            _systemManager.Update(elapsed);

            for (_i = 0; _i < _entities.Count; _i++)
                _entities[_i].OnUpdate(elapsed);
        }

        /// <summary>
        ///     Draw gameloop
        /// </summary>
        /// <param name="elapsed">Milliseconds elapsed since last call</param>
        public void Draw(float elapsed)
        {
            _systemManager.Draw(elapsed);
        }

        /// <summary>
        ///     Adds system to a world system manager
        /// </summary>
        /// <typeparam name="T">Type of system</typeparam>
        /// <param name="system">Instance of system</param>
        /// <param name="parameters">System execution parameters</param>
        public void AddSystem<T>(T system, SystemParameters parameters)
            where T : IEntitySystem
        {
            _systemManager.AddSystem(system, parameters);

            // Subscribe new system to already created entities
            foreach (var entity in _entities)
                system.TryAddEntity(entity);
        }

        /// <summary>
        ///     Adds system to a world system manager
        /// </summary>
        /// <typeparam name="T">Type of system</typeparam>
        /// <param name="system">Instance of system</param>
        /// <param name="executionType">The way how it will be executed</param>
        /// <param name="gameLoopMode">The way in which gameloop wiil it be executed</param>
        /// <param name="layer">Order of execution if synchronous</param>
        public void AddSystem<T>(T system, ExecutionMode executionType, GameLoopType gameLoopMode, int layer = 0)
            where T : IEntitySystem
        {
            var parameters = new SystemParameters
            {
                Execution = executionType,
                GameLoop = gameLoopMode,
                Layer = layer
            };
            AddSystem(system, parameters);
        }

        /// <summary>
        ///     Removes system from system manager
        /// </summary>
        /// <typeparam name="T">Type of a system</typeparam>
        /// <param name="system">Instance of a system</param>
        public void RemoveSystem<T>(T system)
            where T : EntitySystem
        {
            _systemManager.RemoveSystem(system);
        }

        public void Destroy(object sender, EventArgs args)
        {
            _systemManager.Destroy();
        }
    }
}
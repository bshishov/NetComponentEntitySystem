#region

using System;
using System.Collections.Generic;
using System.Threading;

#endregion

namespace NetComponentEntitySystem
{
    public sealed class SystemManager
    {
        /// <summary>
        ///     Collection for all type of system, classified by
        ///     execution type and gameloop type
        /// </summary>
        private readonly ICollection<IEntitySystem> _asyncDraw;

        private readonly IList<IEntitySystem> _asyncUpdate;
        private readonly IList<IEntitySystem> _syncDraw;
        private readonly AutoResetEvent _syncEvent;
        private readonly IList<IEntitySystem> _syncUpdate;
        private readonly IDictionary<IEntitySystem, CancellationTokenSource> _threaded;
        private int _i;

        // Optimization
        // TODO: Remove this awful optimization?
        private int _syncDrawCount;
        private int _syncUpdateCount;

        //Threading

        public SystemManager()
        {
            _syncUpdate = new List<IEntitySystem>();
            _syncDraw = new List<IEntitySystem>();
            _asyncUpdate = new List<IEntitySystem>();
            _asyncDraw = new List<IEntitySystem>();
            _threaded = new Dictionary<IEntitySystem, CancellationTokenSource>();
            _syncEvent = new AutoResetEvent(false);
        }

        private void UpdateCount()
        {
            _syncDrawCount = _syncDraw.Count;
            _syncUpdateCount = _syncUpdate.Count;
        }

        public void AddSystem<T>(T system, SystemParameters parameters)
            where T : IEntitySystem
        {
            switch (parameters.Execution)
            {
                case ExecutionMode.Synchronous:
                    if (parameters.GameLoop == GameLoopType.UpdateLoop)
                        _syncUpdate.Insert(parameters.Layer, system);
                    if (parameters.GameLoop == GameLoopType.DrawLoop)
                        _syncDraw.Insert(parameters.Layer, system);
                    break;
                case ExecutionMode.Asynchronous:
                    if (parameters.GameLoop == GameLoopType.UpdateLoop)
                        _asyncUpdate.Add(system);
                    if (parameters.GameLoop == GameLoopType.DrawLoop)
                        _asyncDraw.Add(system);
                    break;
                case ExecutionMode.ThreadedFast:
                {
                    var cts = new CancellationTokenSource();
                    ThreadPool.QueueUserWorkItem(s => UpdateThreadFast(system, cts.Token));
                    _threaded.Add(system, cts);
                    break;
                }
                case ExecutionMode.ThreadedSync:
                {
                    var cts = new CancellationTokenSource();
                    ThreadPool.QueueUserWorkItem(s => UpdateThreadSync(system, cts.Token));
                    _threaded.Add(system, cts);
                    break;
                }
                default:
                    throw new Exception("Unknown execution type");
            }

            UpdateCount();
        }

        private void UpdateThreadFast(object system, CancellationToken cancellationToken)
        {
            if (!(system is IEntitySystem))
                throw new Exception("Invalid tread parameter, IEntitySystem expected");

            while (!cancellationToken.IsCancellationRequested)
            {
                (system as IEntitySystem).Update();
            }
        }

        private void UpdateThreadSync(object system, CancellationToken cancellationToken)
        {
            if (!(system is IEntitySystem))
                throw new Exception("Invalid tread parameter, IEntitySystem expected");

            while (_syncEvent.WaitOne() && !cancellationToken.IsCancellationRequested)
            {
                (system as IEntitySystem).Update();
            }
        }

        public void RemoveSystem<T>(T system)
            where T : IEntitySystem
        {
            if (_asyncDraw.Contains(system))
                _asyncDraw.Remove(system);
            if (_asyncUpdate.Contains(system))
                _asyncUpdate.Remove(system);
            if (_syncDraw.Contains(system))
                _syncDraw.Remove(system);
            if (_syncUpdate.Contains(system))
                _syncUpdate.Remove(system);

            UpdateCount();
        }

        public void AddEntity(Entity entity)
        {
            foreach (var s in _syncUpdate)
                s.TryAddEntity(entity);
            foreach (var s in _syncDraw)
                s.TryAddEntity(entity);
            foreach (var s in _asyncUpdate)
                s.TryAddEntity(entity);
            foreach (var s in _asyncDraw)
                s.TryAddEntity(entity);
            foreach (var s in _threaded.Keys)
                s.TryAddEntity(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            foreach (var s in _syncUpdate)
                s.TryRemoveEntity(entity);
            foreach (var s in _syncDraw)
                s.TryRemoveEntity(entity);
            foreach (var s in _asyncUpdate)
                s.TryRemoveEntity(entity);
            foreach (var s in _asyncDraw)
                s.TryRemoveEntity(entity);
            foreach (var s in _threaded.Keys)
                s.TryRemoveEntity(entity);
        }

        public void Update(float elapsed)
        {
            _syncEvent.Set();

            for (_i = 0; _i < _syncUpdateCount; _i++)
                _syncUpdate[_i].Update();

            // TODO: PERFORMANCE! and remove memory leaks
            // Use static method or not? http://www.dotnetperls.com/asparallel
            //Parallel.ForEach(_asyncUpdate, UpdateSystem);
        }

        public void Draw(float elapsed)
        {
            for (_i = 0; _i < _syncDrawCount; _i++)
                _syncDraw[_i].Update();

            // TODO: PERFORMANCE! and remove memory leaks
            // Use static method or not? http://www.dotnetperls.com/asparallel
            //Parallel.ForEach(_asyncDraw, UpdateSystem);
        }

        public void Destroy()
        {
            foreach (var cts in _threaded.Values)
            {
                cts.Cancel();
            }
        }
    }
}
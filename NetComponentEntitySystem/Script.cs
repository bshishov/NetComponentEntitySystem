namespace NetComponentEntitySystem
{
    /// <summary>
    ///     Base script. Contains logic
    /// </summary>
    public abstract class Script : IScript
    {
        protected Entity Entity { get; private set; }

        #region IScript Members

        public abstract void OnUpdate(float elapsed);
        public abstract void OnMessage(Message message);

        #endregion

        internal void Initialize(Entity entity)
        {
            Entity = entity;
            OnCreate();
        }

        protected abstract void OnCreate();
    }
}
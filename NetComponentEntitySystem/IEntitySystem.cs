namespace NetComponentEntitySystem
{
    public interface IEntitySystem
    {
        void TryAddEntity(Entity entity);
        void TryRemoveEntity(Entity entity);
        void Update();
    }
}
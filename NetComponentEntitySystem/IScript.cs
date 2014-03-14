namespace NetComponentEntitySystem
{
    public interface IScript
    {
        void OnUpdate(float elapsed);
        void OnMessage(Message message);
    }
}
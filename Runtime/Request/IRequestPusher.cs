namespace TanitakaTech.GodotProcessManager
{
    public interface IRequestPusher<TRequest>
    {
        public void PushRequest(TRequest requestValue);
    }
}
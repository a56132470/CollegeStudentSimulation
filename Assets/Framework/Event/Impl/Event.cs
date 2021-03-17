using Framework.Pool;

namespace Framework.Event
{
    public abstract class Event<T> : TObjectPool<T>,IEvent,IClear where T: IClear,new()
    {
        protected Event()
        {
            
        }
        public abstract void Clear();
    }
}
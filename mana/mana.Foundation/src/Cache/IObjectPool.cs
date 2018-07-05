namespace mana.Foundation
{
    public interface IObjectPool
    {
        object Get();
        bool Put(object item);
        void Clear();
    }
}
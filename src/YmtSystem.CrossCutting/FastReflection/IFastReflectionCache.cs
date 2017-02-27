
namespace YmtSystem.CrossCutting.FastReflection
{
    public interface IFastReflectionCache<TKey, TValue>
    {
        TValue Get(TKey key);
    }
}

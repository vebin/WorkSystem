
namespace YmtSystem.CrossCutting.FastReflection
{
    public interface IFastReflectionFactory<TKey, TValue>
    {
        TValue Create(TKey key);
    }
}

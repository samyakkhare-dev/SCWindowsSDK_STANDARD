using System.Threading.Tasks;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public abstract class InfoManager<T>
    {
        abstract public T make(T t);
        abstract public void updateCache(T t);
        abstract public Task accept(T t);
    }
}

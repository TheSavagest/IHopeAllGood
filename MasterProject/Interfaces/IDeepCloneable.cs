namespace MasterProject.Interfaces
{
    public interface IDeepCloneable<out T>
    {
        T DeepClone();
    }
}
using MasterProject.Interfaces;

namespace MasterProject.Solutions
{
    public interface ISolution<out TSolution>
        : IDeepCloneable<TSolution>
    {
        double Fitness { get; set; }
    }
}
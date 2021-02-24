namespace MasterProject.Solutions
{
    public interface IPoint<out TSolution>
        : ISolution<TSolution>
    {
        double Value { get; }
        double[] Coordinates { get; set; }
    }
}
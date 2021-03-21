namespace MasterProject.Solutions
{
    public interface IPoint<out TSolution>
        : ISolution<TSolution>
    {
        double Value { get; set; }
        double[] Coordinates { get; set; }
    }
}
namespace MasterProject.Statistics
{
    public sealed class RunStatistic
    {
        public const string Header = "RunIndex;IsSuccess;Distance;Value;Evaluation;TimeMs";

        public int RunIndex { get; set; }
        public bool IsSuccess { get; set; }
        public double Distance { get; set; }
        public double Value { get; set; }
        public int Evaluation { get; set; }
        public long TimeMs { get; set; }

        public RunStatistic()
        {
            RunIndex = -1;
            IsSuccess = false;
            Distance = double.NaN;
            Value = double.NaN;
            Evaluation = -1;
            TimeMs = -1;
        }

        public override string ToString()
        {
            return $"{RunIndex};{IsSuccess};{Distance};{Value};{Evaluation};{TimeMs}";
        }
    }
}
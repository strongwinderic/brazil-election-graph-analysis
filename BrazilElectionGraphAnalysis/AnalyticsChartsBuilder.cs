using LiveChartsCore.SkiaSharpView.SKCharts;

namespace BrazilElectionGraphAnalysis;

internal class AnalyticsChartsBuilder
{
    private readonly Dictionary<int, VotingInfo> _allVotingInfo;
    private readonly int _votingCountStep;

    public string GeneratedChartsDir { get; set; } = ".\\GeneratedCharts";

    public AnalyticsChartsBuilder(Dictionary<int, VotingInfo> allVotingInfo, int votingCountStep)
    {
        _allVotingInfo = allVotingInfo;
        _votingCountStep = votingCountStep;
    }

    public void GenerateSeveralRandomChartsAndSave(int quantity)
    {
        string generatedRandomChartDirSubDir = $"{DateTime.Now:s}".Replace(":", "-").Replace("T", "_"); ;
        IList<InMemorySkiaSharpChart> charts = GetSeveralRandomCharts(quantity).ToList();
        for (int i = 0; i < charts.Count(); i++)
        {
            string fileName = $"{GetSaveDir("RandomCharts")}\\{generatedRandomChartDirSubDir}\\chart_{i:D10}.png";
            ChartTools.SaveChart(fileName, charts[i]);
        }
    }

    public void GenerateTendencyChartAndSave()
    {
        InMemorySkiaSharpChart chart = GetTendencyChart();
        string fileName = $"{GetSaveDir("TendencyChart")}\\tendencyChart.png";
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        ChartTools.SaveChart(fileName, chart);
    }

    public IEnumerable<InMemorySkiaSharpChart> GetSeveralRandomCharts(int quantity)
    {
        Random random = new Random();
        for (int i = 1; i <= quantity; i++)
        {
            Dictionary<int, VotingInfo> randomVotingInfo = _allVotingInfo.OrderBy(_ => random.Next())
                .ToDictionary(item => item.Key, item => item.Value);
            InMemorySkiaSharpChart chart = ChartTools.GetVotingChart(randomVotingInfo, _votingCountStep);
            yield return chart;
        }
    }

    public InMemorySkiaSharpChart GetTendencyChart()
    {
        Dictionary<int, VotingInfo> tendencyVotingInfo = _allVotingInfo.OrderByDescending(x => x.Value.BolsonaroVotes)
                .ToDictionary(item => item.Key, item => item.Value);
        InMemorySkiaSharpChart chart = ChartTools.GetVotingChart(tendencyVotingInfo, _votingCountStep);
        return chart;
    }

    private string GetSaveDir(string subDirectory)
    {
        return $"{GeneratedChartsDir}\\{subDirectory}";
    }
}
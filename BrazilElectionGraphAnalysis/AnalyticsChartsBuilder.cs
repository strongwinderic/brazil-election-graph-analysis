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

    public async Task GenerateSeveralRandomChartsAndSave(int quantity, IProgress<string>? progress = default)
    {
        string generatedRandomChartDirSubDir = $"{DateTime.Now:s}".Replace(":", "-").Replace("T", "_");
        int count = 0;
        await foreach(var chart in GetSeveralRandomCharts(quantity, progress))
        {
            count++;
            progress?.Report($"Generating chart {count}");
            string fileName = $"{GetSaveDir("RandomCharts")}\\{generatedRandomChartDirSubDir}\\chart_{count:D5}.png";
            ChartTools.SaveChart(fileName, chart);
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

    public async IAsyncEnumerable<InMemorySkiaSharpChart> GetSeveralRandomCharts(int quantity, IProgress<string>? progress = default)
    {
        Random random = new Random();
        for (int i = 1; i <= quantity; i++)
        {
            Dictionary<int, VotingInfo> randomVotingInfo = _allVotingInfo.OrderBy(_ => random.Next())
                .ToDictionary(item => item.Key, item => item.Value);
            InMemorySkiaSharpChart chart = await Task.Run(() => ChartTools.GetVotingChart(randomVotingInfo, _votingCountStep, progress));
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
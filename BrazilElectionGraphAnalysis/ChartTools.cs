using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;

namespace BrazilElectionGraphAnalysis;

public static class ChartTools
{
    public static InMemorySkiaSharpChart GetVotingChart(Dictionary<int, VotingInfo> allVotingInfo, int votingCountStep, IProgress<string>? progress = default)
    {
        (List<double> lulaVotesInTime, List<double> bolsonaroVotesInTime) = GetChartDataFromVotingInfo(allVotingInfo, votingCountStep, progress);
        return GetVotingChart(lulaVotesInTime, bolsonaroVotesInTime);

    }

    public static void SaveChart(string fileName, InMemorySkiaSharpChart chart)
    {
        var dirName = Path.GetDirectoryName(fileName);
        if (dirName == null)
        {
            throw new Exception($"Cannot create dir for path {fileName}");
        }

        Directory.CreateDirectory(dirName);
        chart.SaveImage(fileName);
    }

    private static InMemorySkiaSharpChart GetVotingChart(List<double> lulaVotesInTime, List<double> bolsonaroVotesInTime)
    {
        var cartesianChart = new SKCartesianChart
        {
            Width = 900,
            Height = 600,
            Series = new ISeries[]
            {
                new LineSeries<double> { Name = "Lula" ,Fill = null, GeometryFill = null, GeometryStroke = null, Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 6 }, Values = lulaVotesInTime } ,
                new LineSeries<double> { Name = "Bolsonaro", Fill = null, GeometryFill = null, GeometryStroke = null, Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 6 }, Values = bolsonaroVotesInTime },
            },
            LegendPosition = LegendPosition.Bottom,
            YAxes = new Axis[] { new() { MinLimit = 0, MaxLimit = 100, Labeler = (x) => $"{x}%" } },
            XAxes = new Axis[] { new() { Labels = new List<string>(), } }
        };
        return cartesianChart;
    }

    private static (List<double> lulaVotesInTime, List<double> bolsonaroVotesInTime) GetChartDataFromVotingInfo(Dictionary<int, VotingInfo> allVotingInfo, int votingCountStep, IProgress<string>? progress = default)
    {
        int totalProcessed = 0;
        int totalVotesLula = 0;
        int totalVotesBolsonaro = 0;
        List<double> lulaVotesInTime = new();
        List<double> bolsonaroVotesInTime = new();

        foreach (KeyValuePair<int, VotingInfo> votingInfoPerBallot in allVotingInfo)
        {
            totalProcessed++;
            totalVotesLula += votingInfoPerBallot.Value.LulaVotes;
            totalVotesBolsonaro += votingInfoPerBallot.Value.BolsonaroVotes;
            int grandTotalVotes = totalVotesLula + totalVotesBolsonaro;

            if (totalProcessed % votingCountStep == 0 || totalProcessed == allVotingInfo.Keys.Count)
            {
                double currentLulaVotesInTime = (double)totalVotesLula * 100 / grandTotalVotes;
                double currentBolsonaroVotesInTime = (double)totalVotesBolsonaro * 100 / grandTotalVotes;
                progress?.Report($"Total ballots processed: {totalProcessed}/{allVotingInfo.Keys.Count} | Lula: {currentLulaVotesInTime} | Bolsonaro: {currentBolsonaroVotesInTime}");
                lulaVotesInTime.Add(currentLulaVotesInTime);
                bolsonaroVotesInTime.Add(currentBolsonaroVotesInTime);
            }
        }

        return (lulaVotesInTime, bolsonaroVotesInTime);
    }
}
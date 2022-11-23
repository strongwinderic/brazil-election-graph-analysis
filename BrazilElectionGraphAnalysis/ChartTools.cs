using System.Diagnostics;
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
    public static InMemorySkiaSharpChart GetStealingVotingChart(Dictionary<int, VotingInfo> allVotingInfo, int votingCountStep, IProgress<string>? progress = default)
    {
        (List<double> lulaVotesInTime, List<double> bolsonaroVotesInTime) = GetStealingVoteChartDataFromVotingInfo(allVotingInfo, votingCountStep, progress);
        return GetVotingChart(lulaVotesInTime, bolsonaroVotesInTime);
    }

    public static Task SaveChartAsync(string fileName, InMemorySkiaSharpChart chart)
    {
        return Task.Factory.StartNew(() => { SaveChart(fileName, chart); });
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
            YAxes = new Axis[] { new() { MinLimit = 44, MaxLimit = 54, Labeler = (x) => $"{x}%" } },
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

    private static (List<double> lulaVotesInTime, List<double> bolsonaroVotesInTime) GetStealingVoteChartDataFromVotingInfo(Dictionary<int, VotingInfo> allVotingInfo, int votingCountStep, IProgress<string>? progress = default)
    {
        int totalProcessed = 0;
        int totalVotesLula = 0;
        int totalVotesBolsonaro = 0;
        List<double> lulaVotesInTime = new();
        List<double> bolsonaroVotesInTime = new();
        int startStealingOnPercent = 20;

        foreach (KeyValuePair<int, VotingInfo> votingInfoPerBallot in allVotingInfo)
        {
            totalProcessed++;
            totalVotesLula += votingInfoPerBallot.Value.LulaVotes;
            totalVotesBolsonaro += votingInfoPerBallot.Value.BolsonaroVotes;
            int grandTotalVotes = totalVotesLula + totalVotesBolsonaro;
            double currentLulaVotesInTime = (double)totalVotesLula * 100 / grandTotalVotes;
            double currentBolsonaroVotesInTime = (double)totalVotesBolsonaro * 100 / grandTotalVotes;

            // steal votes needed to finish about 0.5 ahead of lula
            double processedPercent = (double)totalProcessed * 100 / allVotingInfo.Count;
            double remainingPercent = 100 - processedPercent;
            if (processedPercent >= startStealingOnPercent)
            {
                double stealVotesPercentPerBallot = (currentLulaVotesInTime - currentBolsonaroVotesInTime + .05) * (processedPercent / remainingPercent);
                int stealVotesQuantity = (int)Math.Ceiling(votingInfoPerBallot.Value.LulaVotes * stealVotesPercentPerBallot / 100);
                if (stealVotesQuantity < 0)
                {
                    stealVotesQuantity = 0;
                }

                if (stealVotesQuantity > votingInfoPerBallot.Value.LulaVotes)
                {
                    stealVotesQuantity = votingInfoPerBallot.Value.LulaVotes;
                }

                totalVotesLula-=stealVotesQuantity;
                totalVotesBolsonaro += stealVotesQuantity;
                currentLulaVotesInTime = (double)totalVotesLula * 100 / grandTotalVotes;
                currentBolsonaroVotesInTime = (double)totalVotesBolsonaro * 100 / grandTotalVotes;
            }

            if (totalProcessed % votingCountStep == 0 || totalProcessed == allVotingInfo.Keys.Count)
            {
                progress?.Report($"Total ballots processed: {totalProcessed}/{allVotingInfo.Keys.Count} | Lula: {currentLulaVotesInTime} | Bolsonaro: {currentBolsonaroVotesInTime}");
                lulaVotesInTime.Add(currentLulaVotesInTime);
                bolsonaroVotesInTime.Add(currentBolsonaroVotesInTime);
            }
        }

        return (lulaVotesInTime, bolsonaroVotesInTime);
    }
}
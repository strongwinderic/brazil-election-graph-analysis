using BrazilElectionGraphAnalysis;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using SkiaSharp;

namespace BrazilElectionGraphAnalysis;

internal class ChartPlotter
{
    private readonly Dictionary<int, VotingInfo> _allVotingInfo;

    public ChartPlotter(Dictionary<int, VotingInfo> allVotingInfo)
    {
        _allVotingInfo = allVotingInfo;
    }

    public void PlotAndSaveSeveral(int quantity)
    {
        string chartDirName = $"{DateTime.Now:s}".Replace(":", "-");
        for (int i = 1; i <= quantity; i++)
        {
            string fileName = $".\\GeneratedCharts\\{chartDirName}\\chart_{i:D10}.png";
            PlotAndSave(fileName);
        }
    }

    public void PlotAndSave(string fileName)
    {
        var cartesianChart = GetChartFromRandomReading();
        var dirName = Path.GetDirectoryName(fileName);
        if (dirName == null)
        {
            throw new Exception($"Cannot create dir for path {fileName}");
        }

        Directory.CreateDirectory(dirName);
        cartesianChart.SaveImage(fileName);
    }

    private SKCartesianChart GetChartFromRandomReading()
    {
        Console.WriteLine("Generating chart...");
        (List<double> lulaVotesInTime, List<double> bolsonaroVotesInTime) = GetChartDataFromRandomReading();

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
            YAxes =  new Axis[] { new() { MinLimit = 0, MaxLimit = 100, Labeler = (x) => $"{x}%"} },
            XAxes =  new Axis[] { new() { Labels = new List<string>(), }}
        };
        return cartesianChart;
    }

    private (List<double> lulaVotesInTime, List<double> bolsonaroVotesInTime) GetChartDataFromRandomReading()
    {
        int totalProcessed = 0;
        int totalVotesLula = 0;
        int totalVotesBolsonaro = 0;
        List<double> lulaVotesInTime = new();
        List<double> bolsonaroVotesInTime = new();

        Random random = new Random();
        var randomVotingInfo = _allVotingInfo.OrderBy(x => random.Next())
            .ToDictionary(item => item.Key, item => item.Value);

        foreach (KeyValuePair<int, VotingInfo> votingInfoPerBallot in randomVotingInfo)
        {
            totalProcessed++;
            totalVotesLula += votingInfoPerBallot.Value.LulaVotes;
            totalVotesBolsonaro += votingInfoPerBallot.Value.BolsonaroVotes;
            int grandTotalVotes = totalVotesLula + totalVotesBolsonaro;

            if (totalProcessed % 1 == 0 || totalProcessed == randomVotingInfo.Keys.Count)
            {
                double currentLulaVotesInTime = (double)totalVotesLula * 100 / grandTotalVotes;
                double currentBolsonaroVotesInTime = (double)totalVotesBolsonaro * 100 / grandTotalVotes;
                Console.WriteLine($"Total ballots processed: {totalProcessed}/{randomVotingInfo.Keys.Count} | Lula: {currentLulaVotesInTime} | Bolsonaro: {currentBolsonaroVotesInTime}");
                lulaVotesInTime.Add(currentLulaVotesInTime);
                bolsonaroVotesInTime.Add(currentBolsonaroVotesInTime);
            }
        }

        return (lulaVotesInTime, bolsonaroVotesInTime);
    }
}
using BrazilElectionGraphAnalysis;

Console.WriteLine("Brazil Election Graph Analysis");
Console.WriteLine("");

string zippedCsvDirectory = ".\\Data\\DumpFiles";
string unzippedCsvDirectory = ".\\Data\\DumpFiles\\unzipped";
string votingInfoFilePath = ".\\Data\\AggregatedData\\allVotingInfo.zip";

try
{
    var dataBuilder = new DataBuilder(zippedCsvDirectory, unzippedCsvDirectory, votingInfoFilePath);
    var votingInfoAggregator = new VotingInfoAggregator(dataBuilder);
    Dictionary<int, VotingInfo> allVotingInfo = votingInfoAggregator.GetVotingInfo();
    Console.WriteLine($"Total ballots: {allVotingInfo.Keys.Count}");
    Console.WriteLine($"Lula votes: {allVotingInfo.Values.Sum(x => x.LulaVotes)}");
    Console.WriteLine($"Bolsonaro votes: {allVotingInfo.Values.Sum(x => x.BolsonaroVotes)}");
    Console.WriteLine($"Invalid votes:  {allVotingInfo.Values.Sum(x => x.InvalidVotes)}");
    Console.WriteLine();
    Console.WriteLine("Enter voting count step. This is the amount of ballots will be processed to update the chart. The less the number, more accurate the chart is, but more time it takes to be processed. Default is 1");

    int votingCountStep;
    string? selection;
    do
    {
        selection = Console.ReadLine();
        if (selection?.Trim() == string.Empty)
        {
            selection = "1";
        }

    } while (!int.TryParse(selection, out votingCountStep));

    var chartPlotter = new AnalyticsChartsBuilder(allVotingInfo, votingCountStep);
    chartPlotter.GenerateTendencyChartAndSave();
    chartPlotter.GenerateStealingVotesChartAndSave();
    var progress = new Progress<string>(s => Console.WriteLine(s));
    await chartPlotter.GenerateSeveralRandomChartsAndSave(5, progress);


}
catch (Exception ex)
{
    Console.WriteLine("An error has occurred.");
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    Console.WriteLine("Finished. Press any key to exit.");
    Console.ReadLine();
}

// Instructions to run the software and generate the graphs:
// 1. Ensure you have the required .zip files from https://dados.gov.br/dataset/resultados-2022-boletim-de-urna and copy them to the `BrazilElectionGraphAnalysis/Data/DumpFiles` directory.
// 2. Build the project using Visual Studio or the .NET CLI.
// 3. Run the `BrazilElectionGraphAnalysis` project.
// 4. Follow the on-screen instructions to generate the charts.

﻿using BrazilElectionGraphAnalysis;

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

    var chartPlotter = new ChartPlotter(allVotingInfo);
    chartPlotter.PlotAndSaveSeveral(100);
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
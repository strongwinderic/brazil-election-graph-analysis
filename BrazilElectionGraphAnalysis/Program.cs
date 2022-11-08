// See https://aka.ms/new-console-template for more information

using BrazilElectionGraphAnalysis;
using System.Reflection;

Console.WriteLine("Brazil Election Graph Analysis");

string zippedCsvDirectory = "..\\..\\..\\DumpFiles";
string unzippedCsvDirectory = "..\\..\\..\\DumpFiles\\unzipped";

Console.WriteLine("Unzipping files...");
DumpFilesReader.Unzip(zippedCsvDirectory, unzippedCsvDirectory);

Console.WriteLine("Aggregating information...");
var allVotingInfo = DumpFilesReader.GetAllVotingInfo(unzippedCsvDirectory);
foreach (var votingInfo in allVotingInfo)
{
    Console.WriteLine($"Ballot Id: {votingInfo.Value.BallotId} | Lula votes: {votingInfo.Value.LulaVotes} | Bolsonaro votes: {votingInfo.Value.BolsonaroVotes}");
}

Console.ReadLine();

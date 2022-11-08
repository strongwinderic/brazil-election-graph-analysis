// See https://aka.ms/new-console-template for more information

using BrazilElectionGraphAnalysis;

Console.WriteLine("Brazil Election Graph Analysis");
Console.WriteLine("");

string zippedCsvDirectory = "..\\..\\..\\Data\\DumpFiles";
string unzippedCsvDirectory = "..\\..\\..\\Data\\DumpFiles\\unzipped";
string votingInfoFilePath = "..\\..\\..\\Data\\AggregatedData\\allVotingInfo.json";

var dataBuilder = new DataBuilder(zippedCsvDirectory, unzippedCsvDirectory, votingInfoFilePath);
try
{
    Dictionary<int, VotingInfo> allVotingInfo = GetVotingInfo();

    foreach (var votingInfo in allVotingInfo)
    {
        Console.WriteLine($"Ballot Id: {votingInfo.Value.BallotId} | Lula votes: {votingInfo.Value.LulaVotes} | Bolsonaro votes: {votingInfo.Value.BolsonaroVotes}");
    }
}
catch (Exception ex)
{   
    Console.WriteLine("An error has occurred.");
    Console.WriteLine($"Error: {ex.Message}");
}

Console.ReadLine();

Dictionary<int, VotingInfo> GetVotingInfo()
{
    bool useVotingInfoFile = false;
    if (dataBuilder.VotingInfoFileExists())
    {
        Console.WriteLine($"Voting file already exists on path {dataBuilder.VotingInfoFilePath}");
        ConsoleKeyInfo selectedOption;
        do
        {
            Console.WriteLine($"Use existent file? (Y/n)");
            selectedOption = Console.ReadKey();
        } while (selectedOption.Key is not (ConsoleKey.Enter or ConsoleKey.Y or ConsoleKey.N));

        useVotingInfoFile = selectedOption.Key is ConsoleKey.Enter or ConsoleKey.Y;
    }

    return useVotingInfoFile ? dataBuilder.LoadVotingInfo() : BuildVotingInfo();

    Dictionary<int, VotingInfo> BuildVotingInfo()
    {
        if (dataBuilder.AreFilesUnzipped())
        {
            Console.WriteLine($"TSE data unzipped file already found on directory {dataBuilder.UnzippedCsvDirectory}");
            Console.WriteLine("Using the unzipped data");
        }
        else
        {
            UnzipTseFiles();
        }

        var votingInfo = dataBuilder.GetAllVotingInfo();
        dataBuilder.SaveVotingInfo(votingInfo);
        return votingInfo;

        void UnzipTseFiles()
        {
            if (!dataBuilder.DirectoryHasAllTseZippedFiles())
            {
                throw new Exception($"TSE files could not be found. Please copy all {DataBuilder.TotalTseFiles} zipped TSE files to directory {dataBuilder.ZippedCsvDirectory}");
            }

            dataBuilder.UnzipCsvFiles();
        }
    }
}
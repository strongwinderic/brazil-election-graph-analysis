namespace BrazilElectionGraphAnalysis;

internal class VotingInfoAggregator
{
    private readonly DataBuilder _dataBuilder;

    public VotingInfoAggregator(DataBuilder dataBuilder)
    {
        _dataBuilder = dataBuilder;
    }

    internal Dictionary<int, VotingInfo> GetVotingInfo()
    {
        bool useVotingInfoFile = false;
        if (_dataBuilder.VotingInfoFileExists())
        {
            Console.WriteLine($"Voting file already exists on path {_dataBuilder.VotingInfoFilePath}");
            ConsoleKeyInfo selectedOption;
            do
            {
                Console.WriteLine($"Use existent file? (Y/n)");
                selectedOption = Console.ReadKey();
            } while (selectedOption.Key is not (ConsoleKey.Enter or ConsoleKey.Y or ConsoleKey.N));

            useVotingInfoFile = selectedOption.Key is ConsoleKey.Enter or ConsoleKey.Y;
        }

        return useVotingInfoFile ? _dataBuilder.LoadVotingInfo() : BuildVotingInfo();

    }
    private Dictionary<int, VotingInfo> BuildVotingInfo()
    {
        if (_dataBuilder.AreFilesUnzipped())
        {
            Console.WriteLine($"TSE data unzipped file already found on directory {_dataBuilder.UnzippedCsvDirectory}");
            Console.WriteLine("Using the unzipped data");
        }
        else
        {
            UnzipTseFiles();
        }

        var votingInfo = _dataBuilder.GetAllVotingInfo();
        _dataBuilder.SaveVotingInfo(votingInfo);
        return votingInfo;
    }

    private void UnzipTseFiles()
    {
        if (!_dataBuilder.DirectoryHasAllTseZippedFiles())
        {
            throw new Exception($"TSE files could not be found. Please copy all {DataBuilder.TotalTseFiles} zipped TSE files to directory {_dataBuilder.ZippedCsvDirectory}");
        }

        _dataBuilder.UnzipCsvFiles();
    }
}
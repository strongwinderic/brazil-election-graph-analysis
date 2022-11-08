using Newtonsoft.Json;
using System.IO.Compression;
using System.Text;

namespace BrazilElectionGraphAnalysis;

internal class DataBuilder
{
    public const int TotalTseFiles = 28; // 26 states + federal district + international ballots
    private const int ElectionYearIndex = 2;
    private const int BallotIdIndex = 32;
    private const int ZoneIndex = 13;
    private const int SectionIndex = 14;
    private const int CityIdIndex = 11;
    private const int CityIndex = 12;
    private const int VoteNumberIndex = 29;
    private const int VoteQuantityIndex = 31;

    public string ZippedCsvDirectory { get; set; }
    public string UnzippedCsvDirectory { get; set; }
    public string VotingInfoFilePath { get; set; }

    public DataBuilder(string zippedCsvDirectory, string unzippedCsvDirectory, string votingInfoFilePath)
    {
        ZippedCsvDirectory = zippedCsvDirectory;
        UnzippedCsvDirectory = unzippedCsvDirectory;
        VotingInfoFilePath = votingInfoFilePath;
    }

    internal bool VotingInfoFileExists()
    {
        return File.Exists(VotingInfoFilePath);
    }

    internal void SaveVotingInfo(Dictionary<int, VotingInfo> allVotingInfo)
    {
        string allVotingInfoJson = JsonConvert.SerializeObject(allVotingInfo);
        File.WriteAllText(VotingInfoFilePath, allVotingInfoJson);
    }

    internal Dictionary<int, VotingInfo> LoadVotingInfo()
    {
        string allVotingInfoJson = File.ReadAllText(VotingInfoFilePath);
        var allVotingInfo = JsonConvert.DeserializeObject<Dictionary<int, VotingInfo>>(allVotingInfoJson);

        if (allVotingInfo == null)
        {
            throw new Exception($"Could not load the voting info from the path {VotingInfoFilePath}");
        }

        return allVotingInfo;
    }

    internal bool AreFilesUnzipped()
    {
        return Directory.EnumerateFiles(UnzippedCsvDirectory, "*.csv").Count() == TotalTseFiles;
    }

    internal bool DirectoryHasAllTseZippedFiles()
    {
        return Directory.EnumerateFiles(ZippedCsvDirectory, "*.zip").Count() == TotalTseFiles;
    }

    internal void UnzipCsvFiles()
    {
        if (!Directory.Exists(UnzippedCsvDirectory))
        {
            Directory.CreateDirectory(UnzippedCsvDirectory);
        }

        // clear old csv files from directory
        foreach (string file in Directory.EnumerateFiles(UnzippedCsvDirectory, "*.csv"))
        {
            File.Delete(file);
        }

        // unzip csv files
        foreach (string file in Directory.EnumerateFiles(ZippedCsvDirectory, "*.zip"))
        {
            using ZipArchive archive = ZipFile.OpenRead(file);
            foreach (ZipArchiveEntry entry in archive.Entries.Where(e => e.FullName.EndsWith(".csv")))
            {
                entry.ExtractToFile(Path.Combine(UnzippedCsvDirectory, entry.FullName));
            }
        }
    }

    internal Dictionary<int, VotingInfo> GetAllVotingInfo()
    {
        Console.WriteLine("Aggregating information...");
        Dictionary<int, VotingInfo> votingInfoPerBallot = new();
        int fileCount = 0;
        var allCsvFiles = Directory.EnumerateFiles(UnzippedCsvDirectory, "*.csv").ToList();
        foreach (string file in allCsvFiles)
        {
            Console.WriteLine($"Processing file {++fileCount}/{allCsvFiles.Count}");
            using var reader = new StreamReader(file, Encoding.GetEncoding("ISO-8859-1"));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line is null)
                {
                    continue;
                }

                var values = line.Split(';').Select(x => x.Replace("\"", string.Empty)).ToList();

                // if it is not a valid int, it is a header and should be skipped
                if (!int.TryParse(values[ElectionYearIndex], out _))
                {
                    continue;
                }

                int ballotId = Convert.ToInt32(values[BallotIdIndex]);
                if (!votingInfoPerBallot.TryGetValue(ballotId, out var votingInfo))
                {
                    votingInfo = new VotingInfo()
                    {
                        BallotId = ballotId,
                        Zone = Convert.ToInt32(values[ZoneIndex]),
                        Section = Convert.ToInt32(values[SectionIndex]),
                        CityId = Convert.ToInt32(values[CityIdIndex]),
                        City = values[CityIndex],
                    };
                }

                int voteNumber = Convert.ToInt32(values[VoteNumberIndex]);
                int voteQuantity = Convert.ToInt32(values[VoteQuantityIndex]);
                switch (voteNumber)
                {
                    case 13:
                        votingInfo.LulaVotes += voteQuantity;
                        break;
                    case 22:
                        votingInfo.BolsonaroVotes += voteQuantity;
                        break;
                    default:
                        votingInfo.InvalidVotes += voteQuantity;
                        break;
                }

                votingInfoPerBallot[ballotId] = votingInfo;
            }
        }

        return votingInfoPerBallot;
    }
}
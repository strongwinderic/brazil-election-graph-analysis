using System.IO.Compression;
using System.Text;

namespace BrazilElectionGraphAnalysis;

internal static class DumpFilesReader
{
    private const int ElectionYearIndex = 2;
    private const int BallotIdIndex = 32;
    private const int ZoneIndex = 13;
    private const int SectionIndex = 14;
    private const int CityIdIndex = 11;
    private const int CityIndex = 12;
    private const int VoteNumberIndex = 29;
    private const int VoteQuantityIndex = 31;

    internal static void Unzip(string zippedCsvDirectory, string unzippedCsvDirectory)
    {
        if (!Directory.Exists(unzippedCsvDirectory))
        {
            Directory.CreateDirectory(unzippedCsvDirectory);
        }

        foreach (string file in Directory.EnumerateFiles(zippedCsvDirectory, "*.zip"))
        {
            using ZipArchive archive = ZipFile.OpenRead(file);
            foreach (ZipArchiveEntry entry in archive.Entries.Where(e => e.FullName.EndsWith(".csv")))
            {
                entry.ExtractToFile(Path.Combine(unzippedCsvDirectory, entry.FullName));
            }
        }
    }

    internal static Dictionary<int, VotingInfo> GetAllVotingInfo(string unzippedCsvDirectory)
    {
        Dictionary<int, VotingInfo> votingInfoPerBallot = new();
        foreach (string file in Directory.EnumerateFiles(unzippedCsvDirectory, "*.csv"))
        {
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
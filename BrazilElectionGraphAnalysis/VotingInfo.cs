namespace BrazilElectionGraphAnalysis;

public class VotingInfo
{
    public int BallotId { get; set; }
    public int Zone { get; set; }
    public int Section { get; set; }
    public int CityId { get; set; }
    public string? City { get; set; }
    public int BolsonaroVotes { get; set; }
    public int LulaVotes { get; set; }
    public int InvalidVotes { get; set; }
}
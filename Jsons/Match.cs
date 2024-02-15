/* Date: 15/02/2024
 * Author: Leonardo Trevisan Silio
 */
namespace RiotApi.Jsons;

/// <summary>
/// Represents a Match data.
/// </summary>
public class Match
{
    public MatchMetadata Metadata { get; set; }
    public MatchInfo Info { get; set; }
}

public class MatchMetadata
{
    public string MatchId { get; set; }
    public int DataVersion { get; set; }
    public string[] Participants { get; set; }
}

public class MatchInfo
{
    public string EndOfGameResult { get; set; }
    public string GameMode { get; set; }
    public string GameName { get; set; }
    public string GameType { get; set; }
    public string GameVersion { get; set; }
    public string PlataformId { get; set; }
    public string TournamentCode { get; set; }
    public long GameCreation { get; set; }
    public long GameDuration { get; set; }
    public long GameEndTimestamp { get; set; }
    public long GameStartTimestamp { get; set; }
    public long GameId { get; set; }
    public int QueueId { get; set; }
    public int MapId { get; set; }
    public ParticipantData[] Participants { get; set; }
    public TeamData[] Teams { get; set; }
}

public class ParticipantData
{
    public int Assists { get; set; }
    public int BaronKills { get; set; }
    public int BountyLevel { get; set; }
    public int ChampExperience { get; set; }
    public int ChampLevel { get; set; }
    public int ChampionId { get; set; }
    public string ChampionName { get; set; }
    public int ConsumablesPurchased { get; set; }
    public int DamageDealtToBuildings { get; set; }
    public int DamageDealtToObjectives { get; set; }
    public int DamageDealtToTurrets { get; set; }
    public int DamageSelfMitigated { get; set; }
    public int Deaths { get; set; }
    public int DragonKills { get; set; }
    public bool FirstBloodAssist { get; set; }
    public bool FirstBloodKill { get; set; }
    public bool FirstTowerAssist { get; set; }
    public bool FirstTowerKill { get; set; }
    public int DangerPings { get; set; }
    public int EnemyMissingPings { get; set; }
    public int EnemyVisionPings { get; set; }
    public int GetBackPings { get; set; }
    public int GoldEarned { get; set; }
    public int GoldSpent { get; set; }
    public int Item0 { get; set; }
    public int Item1 { get; set; }
    public int Item2 { get; set; }
    public int Item3 { get; set; }
    public int Item4 { get; set; }
    public int Item5 { get; set; }
    public int Item6 { get; set; }
    public int ItemsPurchased { get; set; }
    public int Kills { get; set; }
    public int LongestTimeSpentLiving { get; set; }
    public int MagicDamageDealt { get; set; }
    public int MagicDamageDealtToChampions { get; set; }
    public int MagicDamageTaken { get; set; }
    public int NeedVisionPings { get; set; }
    public int OnMyWayPings { get; set; }
    public int PhysicalDamageDealt { get; set; }
    public int PhysicalDamageDealtToChampions { get; set; }
    public int PhysicalDamageTaken { get; set; }
    public int PushPings { get; set; }
    public string Puuid { get; set; }
    public string RiotIdGameName { get; set; }
    public string RiotIdTagline { get; set; }
    public int Spell1Casts { get; set; }
    public int Spell2Casts { get; set; }
    public int Spell3Casts { get; set; }
    public int Spell4Casts { get; set; } 
    public int Summoner1Casts { get; set; }
    public int Summoner1Id { get; set; }
    public int Summoner2Casts { get; set; }
    public int Summoner2Id { get; set; }
    public int SummonerLevel { get; set; }
    public int TeamId { get; set; }
    public int TimeCCingOthers { get; set; }
    public int TimePlayed { get; set; }
    public int TotalAllyJungleMinionsKilled { get; set; }
    public int TotalDamageDealt { get; set; }
    public int TotalDamageDealtToChampions { get; set; }
    public int TotalDamageShieldedOnTeammates { get; set; }
    public int TotalDamageTaken { get; set; }
    public int TotalEnemyJungleMinionsKilled { get; set; }
    public int TotalHeal { get; set; }
    public int TotalHealsOnTeammates { get; set; }
    public int TotalMinionsKilled { get; set; }
    public int TotalTimeCCDealt { get; set; }
    public int TotalTimeSpentDead { get; set; }
    public int TotalUnitsHealed { get; set; }
    public int TripleKills { get; set; }
    public int TrueDamageDealt { get; set; }
    public int TrueDamageDealtToChampions { get; set; }
    public int TrueDamageTaken { get; set; }
    public int TurretKills { get; set; }
    public int TurretTakedowns { get; set; }
    public int TurretsLost { get; set; }
    public int UnrealKills { get; set; }
    public int VisionClearedPings { get; set; }
    public int VisionScore { get; set; }
    public int VisionWardsBoughtInGame { get; set; }
    public int WardsKilled { get; set; }
    public int WardsPlaced { get; set; }
    public bool Win { get; set; }
}

public class TeamData
{
    public BanData[] Bans { get; set; }
    public ObjectivesData Objectives { get; set; }
    public int TeamId { get; set; }
    public bool Win { get; set; }
}

public class BanData
{
    public int ChampionId { get; set; }
    public int PickTurn { get; set; }
}

public class ObjectivesData
{
    public ObjectiveData Baron { get; set; }
    public ObjectiveData Dragon { get; set; }
    public ObjectiveData Champion { get; set; }
    public ObjectiveData Horde { get; set; }
    public ObjectiveData Inhibitor { get; set; }
    public ObjectiveData RiftHerald { get; set; }
    public ObjectiveData Tower { get; set; }
}

public class ObjectiveData
{
    public bool First { get; set; }
    public int Kills { get; set; }
}
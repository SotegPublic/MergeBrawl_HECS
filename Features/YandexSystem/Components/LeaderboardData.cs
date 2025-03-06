using System.Collections.Generic;

public class LeaderboardData
{
    public List<Entry> Entries { get; set; } // entries: массив объектов
    public Leaderboard Leaderboard { get; set; } // leaderboard: объект
    public List<Range> Ranges { get; set; } // ranges: массив объектов
    public int UserRank { get; set; } // userRank: integer
}

public class Entry
{
    public string ExtraData { get; set; } // extraData: string
    public int Score { get; set; } // score: integer
    public int Rank { get; set; } // rank: integer
    public Player Player { get; set; } // player: объект
    public string FormattedScore { get; set; } // formattedScore: string
}

public class Player
{
    public string Lang { get; set; } // lang: string
    public string PublicName { get; set; } // publicName: string
    public ScopePermissions ScopePermissions { get; set; } // scopePermissions: объект
    public string UniqueID { get; set; } // uniqueID: string
}

public class ScopePermissions
{
    public string Avatar { get; set; } // avatar: string
    public string Public_Name { get; set; } // public_name: string
}

public class Leaderboard
{
    public string Name { get; set; } // name: string
    public int AppID { get; set; } // appID: integer
    public Title Title { get; set; } // title: объект
    public Description Description { get; set; } // description: объект
    public bool Default { get; set; } // default: boolean
}

public class Title
{
    public string En { get; set; } // en: string
    public string Ru { get; set; } // ru: string
    public string Tr { get; set; } // tr: string
}

public class Description
{
    public ScoreFormat ScoreFormat { get; set; } // score_format: объект
    public bool InvertSortOrder { get; set; } // invert_sort_order: boolean
}

public class ScoreFormat
{
    public string Type { get; set; } // type: string
    public ScoreFormatOptions Options { get; set; } // options: объект
}

public class ScoreFormatOptions
{
    public int DecimalOffset { get; set; } // decimal_offset: integer
}

public class Range
{
    public int Start { get; set; } // start: integer
    public int Size { get; set; } // size: integer
}
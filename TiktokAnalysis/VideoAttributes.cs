namespace TiktokAnalysis;

public class AuthorIdentity
{
    public ulong Id { get; set; }
    public string Tag { get; set; }
    public string Name { get; set; }
    public bool Verified { get; set; }
}

public class AuthorStats
{
    public int NumFollowers { get; set; }
    public int NumFollowing { get; set; }
    public int NumCummulativeLikes { get; set; }
    public int NumVideos { get; set; }
}

public class Author
{
    public AuthorIdentity Identity { get; set; }
    public AuthorStats Stats { get; set; }
}

public class Sound
{
    public ulong Id { get; set; }
    public string Title { get; set; }
    public string By { get; set; }
    public string Url { get; set; }
}

public class PostStats
{
    public int NumLikes { get; set; }
    public int NumComments { get; set; }
    public int NumShares { get; set; }
    public int NumPlays { get; set; }
}

public class VideoAttributes
{
    public ulong Id { get; set; }
    public Author PostedBy { get; set; }
    public DateTime PostedAt { get; set; }
    public DateTime WatchedAt { get; set; }
    public bool IsAd { get; set; }
    public string Caption { get; set; }
    public Sound Sound { get; set; }
    public PostStats Stats { get; set; }
    public List<string> Labels { get; set; }
    public List<string> ClosedCaptionURLs { get; set; }
}

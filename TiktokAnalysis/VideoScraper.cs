using System.Net;
using System.Text.Json.Nodes;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace TiktokAnalysis;

public class Video
{
    public string id { get; set; }
    public string downloadAddr { get; set; }
}

public class Identity
{
    public string id { get; set; }
    public string uniqueId { get; set; }
    public string nickname { get; set; }
    public bool verified { get; set; }
}


public class Music
{
    public string id { get; set; }
    public string title { get; set; }
    public string playUrl { get; set; }
    public string authorName { get; set; }
}

public class Stats
{
    public int diggCount { get; set; }
    public int shareCount { get; set; }
    public int commentCount { get; set; }
    public int playCount { get; set; }
}

public class DuetInfo
{
    public string duetFromId { get; set; }
}

public class AStats
{
    public int followerCount { get; set; }
    public int followingCount { get; set; }
    public int heart { get; set; }
    public int heartCount { get; set; }
    public int videoCount { get; set; }
}

public class ItemStruct
{
    public string id { get; set; }
    public string desc { get; set; }
    public int createTime { get; set; }
    public Video video { get; set; }
    public Identity author { get; set; }
    public Music music { get; set; }
    public Stats stats { get; set; }
    public DuetInfo duetInfo { get; set; }
    public bool digged { get; set; }
    public AStats authorStats { get; set; }
    public bool isAd { get; set; }
    public List<string> diversificationLabels { get; set; }
    public List<string> autoCaptionsURLs { get; set; }
}

public class ItemInfo
{
    public ItemStruct itemStruct { get; set; }
}

public class PageProps
{
    public ItemInfo itemInfo { get; set; }
}

public class Props
{
    public PageProps pageProps { get; set; }
}

public class TiktokResponse
{
    public Props props { get; set; }
}

public class VideoScraper
{
    public static VideoAttributes ScrapeVideo(string videoLink, DateTime watchedAt)
    {
        var web = new HtmlWeb();
        var htmlDocument = web.Load(videoLink);
        htmlDocument.OptionUseIdAttribute = true;

        string? pageDataString = htmlDocument.GetElementbyId("__NEXT_DATA__")?.InnerText;

        if (pageDataString == null) throw new Exception("");

        TiktokResponse? pageData = JsonConvert.DeserializeObject<TiktokResponse>(pageDataString);
        ItemStruct? item = pageData?
            .props
            .pageProps
            .itemInfo
            .itemStruct;
        if (item == null)
            throw new Exception("Failed to get data from tiktok");


        DateTime postedAt = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
        postedAt = postedAt.AddSeconds( item.createTime ).ToLocalTime();
        
        
        return new VideoAttributes
        {
            Id=ulong.Parse(item.id),
            PostedBy=new Author
            {
                Identity=new AuthorIdentity
                {
                    Id=ulong.Parse(item.author.id),
                    Name=item.author.nickname,
                    Verified=item.author.verified,
                    Tag=item.author.uniqueId,
                },
                Stats=new AuthorStats
                {
                    NumFollowers = item.authorStats.followerCount,
                    NumFollowing = item.authorStats.followingCount,
                    NumCummulativeLikes = item.authorStats.heartCount,
                    NumVideos = item.authorStats.videoCount,
                },
            },
            PostedAt=postedAt,
            WatchedAt = watchedAt,
            IsAd=item.isAd,
            Caption=item.desc,
            Sound=new Sound
            {
                Id = ulong.Parse(item.music.id),
                By = item.music.authorName,
                Title = item.music.title,
                Url = item.music.playUrl,
            },
            Stats=new PostStats
            {
                NumLikes=item.stats.diggCount,
                NumComments=item.stats.commentCount,
                NumShares=item.stats.shareCount,
                NumPlays=item.stats.playCount,
            },
            Labels=item.diversificationLabels,
            ClosedCaptionURLs= item.autoCaptionsURLs,
        };
    }
}

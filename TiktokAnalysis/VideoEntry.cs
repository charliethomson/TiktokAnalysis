namespace TiktokAnalysis;

// Date: 2021-12-28 23:28:47
// Video Link: https://www.tiktokv.com/share/video/7046825137659792686/

public class VideoEntry
{
    public DateTime WatchedAt { get; set; }
    public string Link { get; set; }

    public VideoEntry(string entry)
    {
        var parts = entry.Split('\n');
        WatchedAt = DateTime.Parse(parts[0].Split("Date: ")[1]);
        Link = parts[1].Split("Video Link: ")[1];
        

    }
}

public class VideoEntryParser
{
    public static IEnumerable<VideoEntry> Parse(string fileContents)
    {
        return fileContents.Split("\n\n").Where(entry => entry.Trim().Length != 0).Select(entry => new VideoEntry(entry));
    }
}

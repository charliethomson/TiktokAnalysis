using System.Net;
using TiktokAnalysis;
using System.Net.Http;

var watchHistoryContents = File.ReadAllText("/home/c/Desktop/TiktokData/Activity/Video Browsing.txt");

var videoEntries = VideoEntryParser.Parse(watchHistoryContents).Take(50).ToList();

int numEntries = videoEntries.Count;

(VideoEntry entry, string htmlContent)[] entryPages = new (VideoEntry entry, string htmlContent)[numEntries];

Task<HttpResponseMessage>[] tasks = new Task<HttpResponseMessage>[numEntries];


HttpClient client = new HttpClient();

for (int i = 0; i < numEntries; i++)
{
    Console.Write($"\r{i} / {numEntries} Requests started");
    tasks[i] = client.GetAsync(videoEntries[i].Link);
}

Console.WriteLine("\nWaiting for requests to complete..");
Task.WaitAll(tasks);

Task<string>[] contentTasks = new Task<string>[numEntries];

for (int i = 0; i < numEntries; i++)
{
    Console.Write($"\r{i} / {numEntries} Content tasks started");
    contentTasks[i] = tasks[i].Result.Content.ReadAsStringAsync();
}

Task.WaitAll(contentTasks);

List<string> contentStrings = contentTasks.Select(task => task.Result).ToList();

contentStrings.ForEach(Console.WriteLine);

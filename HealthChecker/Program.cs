using System.Diagnostics;
using Cron.NET;
using Telegram.Bot;

// Get params from env
var url = Environment.GetEnvironmentVariable("URL") ??
          throw new ArgumentNullException("url is null");

var botToken = Environment.GetEnvironmentVariable("BOT_TOKEN") ??
               throw new ArgumentNullException("BOT_TOKEN is null");

var chatId = long.Parse(Environment.GetEnvironmentVariable("CHAT_ID") ??
                        throw new ArgumentNullException("CHAT_ID is null"));

var cronSchedule = Environment.GetEnvironmentVariable("CRON_SCHEDULE") ??
                   throw new ArgumentNullException("CRON_SCHEDULE is null");

Console.WriteLine("Starting HealthChecker");

async Task<bool> CheckUrl(string url)
{
    try
    {
        var client = new HttpClient();
        var response = await client.GetAsync(url);

        return response.IsSuccessStatusCode;
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return false;
    }
}

async Task SendLog(string message)
{
    var bot = new TelegramBotClient(botToken);
    await bot.SendTextMessageAsync(chatId, message);
}

var logAction = new Action(async () => 
{
    var dateStr = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

    try
    {
        var watch = Stopwatch.StartNew();
        var isOk = await CheckUrl(url);
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;

        var statusStr = isOk ? "OK" : "NOT OK";
        
        if (!isOk)
        {
            await SendLog($"Health check: Not respond! ❌️️\n{url}");
        }

        Console.WriteLine($"{dateStr} (UTC) | Status: {statusStr} | Response time: {elapsedMs} ms");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{dateStr} (UTC)");
        Console.WriteLine(ex.Message);
    }
});

var daemon = new CronDaemon();

daemon.AddJob(cronSchedule, logAction);

daemon.Start();

// Wait and sleep forever. Let the cron daemon run.
await Task.Delay(Timeout.Infinite);

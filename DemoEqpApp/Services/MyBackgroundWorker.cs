using Serilog;

namespace DemoEqpApp.Services;

public class MyBackgroundWorker
{
    SecsGemManager SescGemManager;
    public MyBackgroundWorker(SecsGemManager sescGemManager) {

        this.SescGemManager = sescGemManager;

        var cts = new CancellationTokenSource();
        var token = cts.Token;
        _ = ExecuteAsync(token);
    }

    async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(5 * 1000, stoppingToken);// 這很重要
            Log.Logger.Warning("MyWorker still alive");

        }
    }
}

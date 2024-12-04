using Secs4Net;
using Secs4Net.Sml;
using Serilog;

namespace DemoEqpApp.Services;

public class SecsLogger : ISecsGemLogger
{
    public event Action<Task<string>>? OnHsmsMessage;
    public SecsLogger()
    {

    }

    public void MessageIn(SecsMessage msg, int id)
    {
        var msgStr = $"{DateTime.Now.ToString()}<-- [0x{id:X8}] {msg.ToSml()}\n";
        OnHsmsMessage?.Invoke(Task.FromResult(msgStr));
        Log.Logger.Information(msgStr);

    }

    public void MessageOut(SecsMessage msg, int id)
    {
        var msgStr = $"{DateTime.Now.ToString()}--> [0x{id:X8}] {msg.ToSml()}\n";
        OnHsmsMessage?.Invoke(Task.FromResult(msgStr));
        Log.Logger.Information(msgStr);

    }

    public void Info(string msg)
    {

        Log.Logger.Information($"{DateTime.Now.ToString()}{msg}\n");

    }

    public void Warning(string msg)
    {

        Log.Logger.Warning($"{DateTime.Now.ToString()}{msg}\n");

    }

    public void Error(string msg, SecsMessage? message, Exception? ex)
    {

        Log.Logger.Error($"{DateTime.Now.ToString()}{msg}\n" + $"{message?.ToSml()}\n" + $"{ex}\n");

    }

    public void Debug(string msg)
    {

        Log.Logger.Debug($"{DateTime.Now.ToString()}{msg}\n");

    }

}
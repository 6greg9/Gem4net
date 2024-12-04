using Gem4Net;
using Gem4NetRepository;
using Gem4NetRepository.Model;
using Serilog;
using System.Text.Json;

namespace DemoEqpApp.Services;

public class SecsGemManager
{
    GemEqpService GemEqpService { get; set; }
    GemRepository GemRepository { get; set; }
    public SecsGemManager(IConfiguration config, GemEqpService gemEqpService, GemRepository gemRepo)
    {


        try
        {
            GemEqpService = gemEqpService;
            GemRepository = gemRepo;
            GemEqpService.OnTerminalMessageReceived += (msg) =>
            {
                //WeakReferenceMessenger.Default.Send<S10Message>(
                //                            new S10Message { Text = msg.Item2 });
                return 0;
            };

            GemEqpService.OnRemoteCommandReceived += (commands) =>
            {
                var rtnParas = commands.Parameters.Select(para =>
                {
                    para.CPACK = 0;
                    return para;
                });
                var rtnCommands = commands;
                rtnCommands.Parameters = rtnParas.ToList();
                return rtnCommands;
            };

            GemEqpService.OnFormattedProcessProgramReceived += (fppSecs) =>
            {
                var pp = new FormattedProcessProgram();
                var result = GemRepository.PharseSecsItemToFormattedPP(fppSecs, out pp);
                var ppCmds = JsonSerializer.Deserialize<List<ProcessCommand>>(pp.PPBody);
                var paraA = ppCmds.FirstOrDefault().ProcessParameters.FirstOrDefault();
                var rtn = GemRepository.CreateFormattedProcessProgram(pp);
                return rtn.Result;
            };
            GemEqpService.OnProcessProgramDeleteReq += (ppLst) =>
            {

                if (ppLst.Count == 0)
                {
                    GemRepository.DeleteFormattedPPAll().Wait();
                }
                else
                {
                    GemRepository.DeleteFormattedProcessProgram(ppLst).Wait();
                }
                return 0;
            };
            GemEqpService.Enable();
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex.ToString());
        };

    }
}

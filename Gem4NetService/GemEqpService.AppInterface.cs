﻿using Microsoft.Extensions.Options;
using Secs4Net;
using Secs4Net.Extensions;
using Gem4NetRepository;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading.Channels;
using static Secs4Net.Item;
using System.Reflection.Metadata;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Gem4Net.Control;
using Gem4NetRepository.Model;
using System.Security;
using System.Collections.Generic;

namespace Gem4Net;
public partial class GemEqpService
{


    #region State Changed
    /// <summary> Connecting,Connected, Selected, Retry,</summary>
    public event Action<string>? OnConnectStatusChanged;

    /// <summary>(current,previous) DISABLED, WAIT_CR_FROM_HOST, WAIT_DELAY, WAIT_CRA, COMMUNICATING</summary>
    public event Action<string, string>? OnCommStateChanged;

    /// <summary>(current,previous) EQUIPMENT_OFF_LINE,HOST_OFF_LINE,ATTEMPT_ON_LINE, LOCAL,REMOTE</summary>
    public event Action<string, string>? OnControlStateChanged;
    #endregion

    #region Command Received

    /// <summary> 0 - ok, 1 - one or more constants does not exist, 2 - busy, 3 - one or more values out of range/// </summary>
    public Func<List<(int,Item)>, int>? OnEcRecieved;
    /// <summary> 0 - accepted for display , 1 - message will not be displayed , 2 - terminal not available</summary>
    public Func<(int,string), int>? OnTerminalMessageReceived;
    /// <summary> for S2F41  Input : ( RCMD, L( CPNAME, CPVAL ) ) , Output : ( HACK, L( CPNAME, CPVAL ) ) 
    /// HACK : 0 - ok, completed , 1 - invalid command , 2 - cannot do now , 3 - parameter error , 4 - initiated for asynchronous completion , 5 - rejected, already in desired condition , 6 - invalid object
    /// </summary>
    public Func<RemoteCommand, RemoteCommand>? OnRemoteCommandReceived;
    //public event Func<FormattedProcessProgram, int> OnFormattedProcessProgramReceived;

    /// <summary>
    /// ACKC7: 0 - Accepted, 1 - Permission not granted, 2 - length error, 3 - matrix overflow, 4 - PPID not found, 5 - unsupported mode, 6 - initiated for asynchronous completion, 7 - storage limit error
    /// </summary>
    public  Func<Item, int>? OnProcessProgramReceived;//還是應該自行解析處理, 只串接SF
    /// <summary>
    /// ACKC7: 0 - Accepted, 1 - Permission not granted, 2 - length error, 3 - matrix overflow, 4 - PPID not found, 5 - unsupported mode, 6 - initiated for asynchronous completion, 7 - storage limit error
    /// </summary>
    public  Func<Item, int>? OnFormattedProcessProgramReceived;//還是應該自行解析處理, 只串接SF

    
    /// <summary>
    /// 0 - Accepted, 1 - Permission not granted, 2 - length error, 3 - matrix overflow
    /// 4 - PPID not found, 5 - unsupported mode, 6 - initiated for asynchronous completion, 7 - storage limit error
    /// </summary>
    public Func<List<string>, int>? OnProcessProgramDeleteReq;
    /// <summary>
    /// 0 - ok, 1 - not done
    /// </summary>
    public Func<string, int>? OnDateTimeSetRequest;

    /// <summary>
    /// 0 - Ok, 1 - already have, 2 - no space, 3 - invalid PPID, 4 - busy, try later, 5 - will not accept, 6 - other error
    /// </summary>
    public Func<Item,int>? OnPPLoadInquire;
    //public event Action<SecsMessage>? OnSecsMessageSend;
    //public event Action? OnProcessProgramChanged;

    public event Action<PrimaryMessageWrapper>? OnUnhandledPrimaryMessage;
    
    #endregion


    #region Query States
    public ISecsGem? GetSecsWrapper     // 不在ON-LINE沒有辦法使用
        => (_ctrlStateManager.CurrentState is ControlState.LOCAL or ControlState.REMOTE)
                                        ? _secsGem : null;

    public async Task<Item?> GetVariableById(int VID)
    {
        return await _GemRepo.GetSv(VID); //這樣還有要?
    }
    public void GetVariableByName(string name)
    {
        // 
    }
    // 要包State Manager ?
    #endregion

    #region Commands

    public async Task<int> UpdateSV(int VID, object varValue)
    {
        return await _GemRepo.SetVarValue(VID, varValue);
    }
    public async Task<int> UpdateEC(List<(int, Item)> idValList)
    {
        return await _GemRepo.SetEcList(idValList);
    }

    //Report類
    /// <summary>S10F1</summary>
    /// <param name="terminalMessage"></param>
    /// <param name="terminalId"></param>
    /// <returns></returns>
    public async Task<int> SendTerminalMessageAsync(string terminalMessage, int terminalId, bool useWbit)
    {
        int ack10 = -1;
        try
        {
            using (var s10f1 = new SecsMessage(10, 1, useWbit)
            {
                SecsItem = L(
                B((byte)terminalId),
                A(terminalMessage))
            })
            {
                var rtns10f2 = await _secsGem.SendAsync(s10f1);
                // 應先資料驗證
                ack10 = rtns10f2 is null ? 0: rtns10f2.SecsItem.FirstValue<byte>();
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
        finally
        {

        }
        return ack10;

    }
    int dataID = 0;
    public async Task SendEventReport(int eventId, bool useWbit)
    {
        var reports = await _GemRepo.GetReportsByCeid(eventId);
        //Random random = new Random();
        dataID +=1;
        using var s6f11 = new SecsMessage(6, 11, useWbit)
        {
            SecsItem = L(
            U4((uint)dataID), //DATAID
            U4((uint)eventId), //CEID
            reports
            ),
        };
        _ = _secsGem?.SendAsync(s6f11);//射後不理
    }
    public async Task SendEventReport(int dataId,int eventId, bool useWbit)
    {
        var reports =await _GemRepo.GetReportsByCeid(eventId);
        
        using var s6f11 = new SecsMessage(6, 11, useWbit)
        {
            SecsItem = L(
            U4((uint)dataId), //DATAID
            U4((uint)eventId), //CEID
            reports
            ),
        };
        _ = _secsGem?.SendAsync(s6f11);//射後不理
    }
    /// <summary>
    /// 0:OK, 1:ALID not found, 2: not Enabled
    /// </summary>
    /// <param name="alrmSet"></param>
    /// <param name="alrmId"></param>
    /// <param name="alrmText"></param>
    /// <returns></returns>
    public Task<int> SendAlarmReport(int alrmSet, int alrmId,string alrmText, bool useWbit ) {

        return Task.Run(async () =>
        {
            var alrm = await _GemRepo.GetAlarm(alrmId);
            if (alrm == null)
                return 1;
            if (alrm.ALCD != alrmSet)
                _ =await _GemRepo.SetAlarmCode(alrmId, alrmSet);
            if(alrm.ALED!= true) return 2;
            var secsAlrmCode = alrmSet;//? 128 : 0;
            //var secsAlrmEnabled = alrm.ALED ? 128 : 0;
            using var s5f1 = new SecsMessage(5, 1, useWbit)
            {
                SecsItem = L(
                B((byte)secsAlrmCode),
                U4((uint)alrmId), //CEID
                A(alrmText)
                ),
            };
            _ = _secsGem?.SendAsync(s5f1);//射後不理
            return 0;
        });
        
    }
    public Task<int> SendAlarmReport(int alrmSet, int alrmId)
    {
        return Task.Run(async () =>
        {
            var alrm =await _GemRepo.GetAlarm(alrmId);
            if (alrm == null)
                return 1;
            if (alrm.ALED != true) return 2;
            if (alrm.ALCD != alrmSet)
                _ = await _GemRepo.SetAlarmCode(alrmId, alrmSet);
            var secsAlrmCode = alrmSet;//? 128 : 0;
            //var secsAlrmEnabled = alrm.ALED ? 128 : 0;
            using var s5f1 = new SecsMessage(5, 1,Convert.ToBoolean(EqpAppOptions.IsS5WbitUsed))
            {
                SecsItem = L(
                B((byte)secsAlrmCode),
                U4((uint)alrmId), //CEID
                A(alrm.ALTX)
                ),
            };
            _ = _secsGem?.SendAsync(s5f1);//射後不理
            return 0;
        });
    }

    //需要補上CommState, CtrlState的限制,
    //大部分語句在進入On-Line後才可使用, 理論上只須限制在ON-line

    public int EnableComm()
    {
        return _commStateManager.EnableComm();
    }
    public int DisableComm()
    {
        return _commStateManager.DisableComm();
    }

    public int RequestOnline()
    {
        return _ctrlStateManager.OnLineRequest();
    }
    public int GoOffline()
    {
        return _ctrlStateManager.OffLine();
    }
    public int GoOnlineLocal()
    {
        return _ctrlStateManager.OnLineLocal();
    }
    public int GoOnlineRemote()
    {
        return _ctrlStateManager.OnLineRemote();
    }

    /// <summary>
    /// Equip的主動Command ?
    /// </summary>
    /// <returns></returns>
    public int Command()
    {
        return 0;
    }

    public void StopCommunication()
    {
        this._commStateManager.GoNotCommunicating();
    }
    #endregion

}

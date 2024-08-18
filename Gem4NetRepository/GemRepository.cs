using AutoMapper;
using Dapper;
using Gem4NetRepository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Secs4Net;
using Secs4Net.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Secs4Net.Json;
using static Secs4Net.Item;
using Microsoft.Extensions.Options;
namespace Gem4NetRepository;
public partial class GemRepository
{
    /// <summary>
    /// The database
    /// </summary>
    private GemDbContext _context;
    private static object lockObject = new object();

    public int UseJsonSecsItem { get; private set; }

    IMapper Mapper;
    IConfiguration? _config;
    private int TimeFormat;

    public GemRepository(IConfiguration configaration)
    {
        
        _config = configaration;
        UseJsonSecsItem = Convert.ToInt32( _config["GemEqpRepoOptions:UseJsonSecsItem"] );
        using (_context = new GemDbContext(_config))
        {
            _ = _context.Variables.ToList();
            _ = _context.Events.ToList();
            _ = _context.Alarms.ToList();
            //_ = _context.Database.ExecuteSqlRaw("PRAGMA synchronous = ON;"); //sqlite加速?
        }

        UpdateTimeFormat();

        //EFcore加Dapper做成撒尿牛肉丸, 後來可以說是沒用到...
        SqlMapper.AddTypeHandler(new PPBodyHandler());
        SqlMapper.AddTypeHandler(new SqliteGuidTypeHandler());

        //AutoMapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            
            cfg.CreateMap<FormattedProcessProgram, FormattedProcessProgramLog>();
            
            cfg.CreateMap<ProcessProgram, ProcessProgramLog>(); 
        }
        ); // 註冊Model間的對映
        Mapper = mapperConfig.CreateMapper();
    }
    void UpdateTimeFormat()
    {
        var TimeFormatEC = GetEC(Convert.ToInt32(_config["GemEqpRepoOptions:UseJsonSecsItem"]));
        if (TimeFormatEC is not null)
            TimeFormat = TimeFormatEC.FirstValue<int>();
    }
    public class SqliteGuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid guid)
        {
            parameter.Value = guid.ToString();
        }

        public override Guid Parse(object value)
        {
            // Dapper may pass a Guid instead of a string
            if (value is Guid)
                return (Guid)value;

            return new Guid((string)value);
        }
    }
    /// <summary>for s1f3,s1f4</summary>
    /// <param name="vidList"></param>
    /// <returns></returns>
    public Item? GetSvList(IEnumerable<int> vidList)
    {
        UpdateTimeFormat();
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                return SubGetSvListByVidList(vidList);
            }
        }

    }
    Item? SubGetSvListByVidList(IEnumerable<int> vidList)
    {
        return Item.L(vidList.Select(vid => SubGetSvByVID(vid)).ToArray());
    }
    public Item? GetSv(int vid)
    {
        UpdateTimeFormat();

        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                return SubGetSvByVID(vid);
            }
        }

    }
    Item? SubGetSvByVID(int vid)
    {
        // Clock, 是要用Name還是Vid找
        if (vid == Convert.ToInt32(_config["ClockVID"]))
            return SubGetClock(TimeFormat);

        var Variable = _context.Variables
            .Where(v => v.VarType == "SV")
            .Where(v => v.VID == vid).FirstOrDefault();
        if (Variable == null)//找不到
            return A(); // ?

        return GemVariableToSecsItem(Variable);
    }
    public Item? GetEcList(IEnumerable<int> vidList)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                return SubGetEcListByVidList(vidList);
            }
        }

    }
    Item? SubGetEcListByVidList(IEnumerable<int> vidList)
    {
        return Item.L(vidList.Select(vid => SubGetEcByVID(vid)).ToArray());
    }
    public Item? GetEC(int vid)
    {
        lock (lockObject)
        {
            lock (lockObject)
            {
                using (_context = new GemDbContext(_config))
                {
                    return SubGetEcByVID(vid);
                }
            }
        }
    }
    Item? SubGetEcByVID(int vid)
    {

        var Variable = _context.Variables
            .Where(v => v.VarType == "EC")
            .Where(v => v.VID == vid).FirstOrDefault();
        if (Variable == null)//找不到
            return A(); // ?

        return GemVariableToSecsItem(Variable);
    }
    public Item? GetVariable(string name)
    {
        using (_context = new GemDbContext(_config))
        {
            return SubGetVarByName(name);
        }
    }
    public Item? GetVariable(int vid)
    {
        using (_context = new GemDbContext(_config))
        {
            return SubGetVarByVid(vid);
        }
    }
    Item? SubGetVarByName(string name)
    {
        var Variable = _context.Variables
            //.Where(v=>v.VarType=="SV")
            .Where(v => v.Name == name).FirstOrDefault();
        if (Variable == null)//找不到
            return A(); // ?

        return GemVariableToSecsItem(Variable);
    }
    Item? SubGetVarByVid(int vid)
    {
        var Variable = _context.Variables
            //.Where(v=>v.VarType=="SV")
            .Where(v => v.VID == vid).FirstOrDefault();
        if (Variable == null)//找不到
            return A(); // ?
        return GemVariableToSecsItem(Variable);
    }
    Item? SubGetClock(int timeFormatcode)
    {
        
        //var timeFormat = GetEC(Convert.ToInt32(_config["GemEqpAppOptions:TimeFormatVID"]))
        //                        ?? U4((uint)Convert.ToInt32(_config["GemEqpAppOptions:ClockFormatCode"]));

        //timeFormatcode = timeFormat.FirstValue<int>();
        if (timeFormatcode == 0)
            return A(DateTime.Now.ToString("yyMMddHHmmss"));
        if (timeFormatcode == 1)
            return A(DateTime.Now.ToString("yyyyMMddHHmmssff"));
        if (timeFormatcode == 2)
            return A(DateTime.UtcNow.ToString("yyyy-MM-dd") + "T" +  //UTC
                DateTime.UtcNow.ToString("HH:mm:ss.fff") + "Z");
        return A(DateTime.Now.ToString("yyyyMMddHHmmssff"));

    }
    Item? GemVariableToSecsItem(GemVariable variable)
    {
        try
        {
            if (UseJsonSecsItem == 1)
            {
                return JsonDocument.Parse(variable.Value).RootElement.ToItem();
            }
            return VarStringToItem(variable.DataType, variable.Value);
            //if (variable.DataType != "LIST")
            //{
            //}
            //else
            //{
            //    var subItem = _context.Variables.Where(v => v.VarType == "SV").Where(v => v.ListSVID == variable.VID).ToList();
            //    return Item.L(subItem.Select(v =>
            //    {
            //        return v.DataType == "LIST" ? GetSv(v.VID) : GemVariableToSecsItem(v);
            //    }).ToArray());
            //}
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return null;
        }
    }
    public Item VarStringToItem(string dataType, string varStr)
    {
        switch (dataType)
        {
            case "BINARY":

                return Item.B(Convert.FromHexString(varStr));
            case "BOOL":
                var BOOL = Convert.ToBoolean(varStr);
                return Item.Boolean(BOOL);
            case "ASCII":
                var ASCII = varStr;
                return Item.A(ASCII);
            case "UINT_1":
                var UINT_1 = Convert.ToByte(varStr);
                return Item.U1(UINT_1);
            case "UINT_2":
                var UINT_2 = Convert.ToUInt16(varStr);
                return Item.U2(UINT_2);
            case "UINT_4":
                var UINT_4 = Convert.ToUInt32(varStr);
                return Item.U4(UINT_4);
            case "UINT_8":
                var UINT_8 = Convert.ToUInt64(varStr);
                return Item.U8(UINT_8);
            case "INT_1":
                var INT_1 = Convert.ToSByte(varStr);
                return Item.I1(INT_1);
            case "INT_2":
                var INT_2 = Convert.ToInt16(varStr);
                return Item.I2(INT_2);
            case "INT_4":
                var INT_4 = Convert.ToInt32(varStr);
                return Item.I4(INT_4);
            case "INT_8":
                var INT_8 = Convert.ToInt64(varStr);
                return Item.I8(INT_8);
            case "FLOAT_4":
                var FLOAT_4 = Convert.ToSingle(varStr);
                return Item.F4(FLOAT_4);
            case "FLOAT_8":
                var FLOAT_8 = Convert.ToDouble(varStr);
                return Item.F8(FLOAT_8);
            case "LIST":
                var LIST = JsonDocument.Parse(varStr).RootElement;
                return LIST.ToItem();
            default:
                return Item.J(); // !?
        }
    }
    /// <summary>
    /// SVID, SVNAMES, UNITS
    /// </summary>
    /// <returns></returns>
    public Item? GetVariableAll()
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                return SubGetVariableAll();
            }
        }
    }
    public Item? SubGetVariableAll()
    {
        var VarLst = _context.Variables.ToList().Select(v => GemVariableToSecsItem(v)).ToArray();
        return Item.L(VarLst);
    }
    /// <summary>
    /// for s1f11,s1f12
    /// </summary>
    /// <param name="vidList"></param>
    /// <returns></returns>
    public Item? GetSvNameList(IEnumerable<int> vidList)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                var svNameList = vidList.Select(vid =>
                {   //這種寫法有一天要改
                    return _context.Variables.Where(v => v.VarType == "SV") //混到EC, EV可嗎?
                   .Where(v => v.VID == vid).FirstOrDefault();
                }).Select(v =>  // Comment: A:0 for SVNAME and UNITS indicates unknown SVID
                {
                    if (v is null)
                    {
                        return A();// ?
                    }
                    return Item.L(U4((uint)v.VID), A(v.Name), A(v.Unit));
                });
                return Item.L(svNameList.ToArray());
            }
        }

    }
    /// <summary>
    /// For S1F11
    /// </summary>
    /// <returns></returns>
    public Item? GetSvNameListAll()
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                var itemList = _context.Variables.Where(v => v.VarType == "SV")
                .Select(v => Item.L(U4((uint)v.VID), A(v.Name), A(v.Unit)));
                return Item.L(itemList.ToArray());
            }
        }

    }


    /// <summary>
    /// for s2f14
    /// </summary>
    /// <returns></returns>
    public Item? GetEcValueList(IEnumerable<int> vidList)
    {
        if (vidList.Count() == 0)
        {
            return GetEcValueListAll();
        }
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                //IQueryable<GemVariable?> rtnGemVar = null;
                List<GemVariable?> rtnGemVar = new();
                foreach (var vid in vidList)
                {
                    var rtnEc = _context.Variables.Where(v => v.VarType == "EC").Where(v => v.VID == vid).FirstOrDefault();
                    //rtnGemVar = rtnGemVar == null ? rtnEc : rtnGemVar.Concat(rtnEc);
                    //rtnGemVar =  rtnGemVar.Concat(rtnEc);
                    rtnGemVar.Add(rtnEc); // 這樣很違反EF的原則..., 有空改成raw sql
                }
                //var svNameList = vidList.Select(vid =>
                //{
                //    return _context.Variables.Where(v => v.VarType == "EC")
                //   .Where(v => v.VID == vid).FirstOrDefault();
                //}); //EF 查詢到此為止
                var rtnGemVarLst = rtnGemVar.ToList();
                var itemLst = rtnGemVarLst.Select(v =>
                {
                    if (v is null)
                    {
                        return A();// ?
                    }
                    return GemVariableToSecsItem(v);
                });
                return Item.L(itemLst);
            }
        }

    }
    public Item? GetEcValueListAll()
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                var ecLst = _context.Variables.Where(v => v.VarType == "EC");
                var itemList = ecLst.ToList().Select(v => GemVariableToSecsItem(v)); // 這裡有EF的坑
                var itemArry = itemList.ToArray();
                return Item.L(itemArry);
            }
        }

    }

    /// <summary>
    /// for S2F29,( ECID, ECNAME, ECMIN, ECMAX, ECDEF, UNITS), 沒有value
    /// </summary>
    /// <param name="vidList"></param>
    /// <returns></returns>
    public Item? GetEcDetailList(IEnumerable<int> vidList)
    {
        if (vidList.Count() == 0)
        {
            return GetEcDetailListAll();
        }
        lock (lockObject)
        {

            using (_context = new GemDbContext(_config))
            {
                var ecDetailLst = _context.Variables.Where(v=> vidList.Contains(v.VID) )
                    //.Where(v => v.VarType == "EC") //我決定不管都給,頂多是空的
                    .ToList().Select(v => GemEcDetailToSecsItem(v)); 
                return L(ecDetailLst.ToArray());
            }
        }

    }
    public Item? GetEcDetailListAll()
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                var ecLst = _context.Variables.Where(v => v.VarType == "EC");
                var itemList = ecLst.ToList().Select(v => GemEcDetailToSecsItem(v)); // 這裡有EF的坑
                var itemArry = itemList.ToArray();
                return Item.L(itemArry);
            }
        }

    }
    /// <summary>
    /// 假定進來的都已經確定是EC
    /// </summary>
    /// <param name="variable"></param>
    /// <returns></returns>
    Item? GemEcDetailToSecsItem(GemVariable variable)
    {
        try
        {
            var ecid = U4( Convert.ToUInt32(variable.VID) );
            var ecname = A(variable.Name );
            var ecmin = A(variable.MinValue );
            var ecmax = A(variable.MaxValue );
            var ecdef = A(variable.Definition);
            var ecunit = A(variable.Unit);
            var ecDetail = L(ecid,ecname,ecmin,ecmax,ecdef,ecunit);
            return ecDetail;

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return null;
        }
    }
    /// <summary>
    /// for app update var,  return: 1 - not found, 2 - also EC
    /// </summary>
    /// <param name="vid"></param>
    /// <param name="updateValue"></param>
    /// <returns></returns>
    public int SetVarValue(int vid, object updateValue)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                var variable = _context.Variables.FirstOrDefault(v => v.VID == vid);
                if (variable is null)
                    return 1;//NotFound
                try
                {
                    if (variable.DataType != "LIST")
                    {
                        switch (variable.DataType)
                        {
                            
                            case "BINARY":
                                var HexString = Convert.ToHexString((byte[])updateValue);
                                variable.Value = HexString;
                                break;
                            case "BOOL":
                                var BOOL = Convert.ToBoolean(updateValue);
                                variable.Value = BOOL.ToString();
                                break;
                            case "ASCII":
                                var ASCII = Convert.ToString(updateValue);
                                variable.Value = ASCII.ToString();
                                break;
                            case "UINT_1":  ///數值類注意OutOfRange
                                var UINT_1 = Convert.ToByte(updateValue);
                                variable.Value = UINT_1.ToString();
                                break;
                            case "UINT_2":
                                var UINT_2 = Convert.ToUInt16(updateValue);
                                variable.Value = UINT_2.ToString();
                                break;
                            case "UINT_4":
                                var UINT_4 = Convert.ToUInt32(updateValue);
                                variable.Value = UINT_4.ToString();
                                break;
                            case "UINT_8":
                                var UINT_8 = Convert.ToUInt64(updateValue);
                                variable.Value = UINT_8.ToString();
                                break;
                            case "INT_1":
                                var INT_1 = Convert.ToSByte(updateValue);
                                variable.Value = INT_1.ToString();
                                break;
                            case "INT_2":
                                var INT_2 = Convert.ToInt16(updateValue);
                                variable.Value = INT_2.ToString();
                                break;
                            case "INT_4":
                                var INT_4 = Convert.ToInt32(updateValue);
                                variable.Value = INT_4.ToString();
                                break;
                            case "INT_8":
                                var INT_8 = Convert.ToInt64(updateValue);
                                variable.Value = INT_8.ToString();
                                break;
                            case "FLOAT_4":
                                var FLOAT_4 = Convert.ToSingle(updateValue);
                                variable.Value = FLOAT_4.ToString();
                                break;
                            case "FLOAT_8":
                                var FLOAT_8 = Convert.ToDouble(updateValue);
                                variable.Value = FLOAT_8.ToString();
                                break;
                            case "LIST":
                                var LIST = updateValue as Item;
                                variable.Value = LIST.ToJson();
                                break;
                            default:
                                return 2;
                        }
                        _context.SaveChanges();
                        if (variable.VarType == "EC")
                            return 2;
                        return 0; //SV,DV
                    }
                }
                catch (Exception) { throw; }
                return 3;//ListSV ?!
            }
        }

    }

    #region for s2f15
    /// <summary>
    /// for s2f15, EAC 0 - ok, 1 - one or more constants does not exist, 2 - busy, 3 - one or more values out of range
    /// </summary>
    /// <param name="idValLst"></param>
    /// <returns></returns>
    public int SetEcList(IEnumerable<(int, Item)> idValLst)
    {
        int EAC = -1;
        var idLst = idValLst.Select(pair => pair.Item1).ToList();
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                var ECs = _context.Variables.Where(v => v.VarType == "EC");
                if (ECs.Where(v => idLst.Contains(v.VID)).Count() != idLst.Count)
                {
                    EAC = 1;
                    return EAC;
                }
                foreach (var idVal in idValLst)
                {
                    var EC = ECs.Where(ec => ec.VID == idVal.Item1).First();
                    var SetEcResult = SubSetVarById(EC, idVal);
                    if (SetEcResult != 0)
                    { return SetEcResult; }
                }
                _context.SaveChanges();
                return 0; //all success
            }
        }

    }
    int SubSetVarById(GemVariable variable, (int, Item) idVal)
    {
        try
        {
            if (variable.DataType != "LIST")
            {

                switch (variable.DataType)
                {
                    case "BINARY"://應該只會有1個byte?
                        //Memory<byte> bytes = idVal.Item2.GetMemory<byte>();
                        var BINARY = idVal.Item2.FirstValue<byte>();
                        variable.Value = Convert.ToString((int)BINARY);
                        
                        break;
                    case "BOOL":
                        var BOOL = idVal.Item2.FirstValue<bool>;
                        variable.Value = BOOL.ToString();
                        break;
                    case "ASCII":
                        var ASCII = idVal.Item2.GetString();
                        variable.Value = ASCII.ToString();
                        break;
                    case "UINT_1":  ///數值類注意OutOfRange
                        var UINT_1 = idVal.Item2.FirstValue<byte>();
                        if (IsOutOfValueRange(UINT_1, variable))
                            return 3;
                        variable.Value = UINT_1.ToString();
                        break;
                    case "UINT_2":
                        var UINT_2 = idVal.Item2.FirstValue<UInt16>();
                        if (IsOutOfValueRange(UINT_2, variable))
                            return 3;
                        variable.Value = UINT_2.ToString();
                        break;
                    case "UINT_4":
                        var UINT_4 = idVal.Item2.FirstValue<UInt32>();
                        if (IsOutOfValueRange(UINT_4, variable))
                            return 3;
                        variable.Value = UINT_4.ToString();
                        break;
                    case "UINT_8":
                        var UINT_8 = idVal.Item2.FirstValue<UInt64>();
                        if (IsOutOfValueRange(UINT_8, variable))
                            return 3;
                        variable.Value = UINT_8.ToString();
                        break;
                    case "INT_1":
                        var INT_1 = idVal.Item2.FirstValue<SByte>();
                        if (IsOutOfValueRange(INT_1, variable))
                            return 3;
                        variable.Value = INT_1.ToString();
                        break;
                    case "INT_2":
                        var INT_2 = idVal.Item2.FirstValue<Int16>();
                        if (IsOutOfValueRange(INT_2, variable))
                            return 3;
                        variable.Value = INT_2.ToString();
                        break;
                    case "INT_4":
                        var INT_4 = idVal.Item2.FirstValue<Int32>();
                        if (IsOutOfValueRange(INT_4, variable))
                            return 3;
                        variable.Value = INT_4.ToString();
                        break;
                    case "INT_8":
                        var INT_8 = idVal.Item2.FirstValue<Int64>();
                        variable.Value = INT_8.ToString();
                        break;
                    case "FLOAT_4":
                        var FLOAT_4 = idVal.Item2.FirstValue<float>;
                        if (IsOutOfValueRange(FLOAT_4, variable))
                            return 3;
                        variable.Value = FLOAT_4.ToString();
                        break;
                    case "FLOAT_8":
                        var FLOAT_8 = idVal.Item2.FirstValue<Double>();
                        if (IsOutOfValueRange(FLOAT_8, variable))
                            return 3;
                        variable.Value = FLOAT_8.ToString();
                        break;
                    default:
                        return 2;
                }
                //_context.SaveChanges();
                return 0; //SV,DV
            }
        }
        catch (Exception) { return 4; }
        return 3;//ListSV ?!

        return 1;
    }
    bool IsOutOfValueRange(object val, GemVariable variable)
    {
        try
        {
            var inputVal = Convert.ToDouble(val);
            if (variable.MaxValue != string.Empty)
            {
                var max = Convert.ToDouble(variable.MaxValue);
                if (inputVal > max)
                    return true;
            }
            if (variable.MinValue != string.Empty)
            {
                var min = Convert.ToDouble(variable.MinValue);
                if (inputVal < min)
                    return true;
            }
        }
        catch (Exception) { return true; }


        return false;
    }
    #endregion

    public Item? GetReportsByCeid(int ceid)
    {
        UpdateTimeFormat();
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                List<(int, Item)> rtnRptItems = new();
                var reports = _context.EventReportLinks.Where(link => link.ECID == ceid)
                    .Select(link => link.Report);
                var reportVars = reports
                    .Join(_context.ReportVariableLinks,
                    rpt => rpt.RPTID,
                    link => link.RPTID,
                    (rpt, link) =>
                    new
                    {
                        RptId = rpt.RPTID,
                        Variable = _context.Variables.Where(v => v.VID == link.VID).First()
                    }).ToList(); //.GroupBy( v=>v.RptId ).Select(v=> v.)
                                 //.Select(pair=> {RptId = pair.Key, Datas =  pair} );
                var rtnReports = reportVars
                    .GroupBy(d => d.RptId)
                    .Select(pair => (pair.Key, pair.ToList())).ToList();
                foreach (var groupData in rtnReports)
                {
                    var vids = groupData.Item2.Select(v => v.Variable.VID);
                    var datas = SubGetSvListByVidList(vids);
                    rtnRptItems.Add((groupData.Item1, datas));
                }
                return L(rtnRptItems.Select(p => L(U4((uint)p.Item1), L(p.Item2))));
            }
        }

    }
    public Item? GetReportByRpid(int rpid)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                var rptVarLink = _context.ReportVariableLinks.Where(rpt => rpt.RPTID == rpid);
                if (!rptVarLink.Any())
                {
                    return L();
                }
                var reportVars = rptVarLink
                    .Join(_context.Variables,
                        link => link.VID,
                        variable => variable.VID,
                        (link, variable) => variable
                    ).ToList()
                    .Select(v => GemVariableToSecsItem(v)).ToArray();
                return L(reportVars);

            }
        }

    }
    #region DynamicEventReport
    /// <summary>
    /// 0 - ok, 1 - out of spac, 2 - invalid format, 3 - 1 or more RPTID already defined, 4 - 1 or more invalid VID
    /// </summary>
    /// <returns></returns>
    public int DefineReport(IEnumerable<(int RPTID, int[] VID)> rptLst)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                if (rptLst.Count() == 0)//清光, 也許應該寫在其他地方...
                {
                    //_context.Database.ExecuteSqlRaw("TRUNCATE TABLE Reports "); sqlite 居然沒有Truncate
                    _context.Database.ExecuteSqlRaw("DELETE FROM Reports");
                    _context.SaveChanges();
                    return 0;
                }

                var newRptIds = rptLst.Where(rpt => rpt.VID.Length > 0).Select(rpt => rpt.RPTID).ToList(); //不是刪除的個數
                if (_context.Reports.Where(rpt => newRptIds.Contains(rpt.RPTID)).Count() > 0)
                    return 3;

                foreach (var rptDefine in rptLst)
                {
                    if (rptDefine.VID.Length == 0)//Delete
                    {
                        var rpt = _context.Reports.Where(rpt => rpt.RPTID == rptDefine.RPTID);//.First();
                        _context.RemoveRange(rpt);
                        //_context.Remove(rpt);
                        continue;
                    }
                    //Create
                    var vidLst = rptDefine.VID.ToList();
                    if (_context.Variables.Where(v => vidLst.Contains(v.VID)).Count() != vidLst.Count())//有不存在的VID
                        return 4;
                    var newRpt = new GemReport { RPTID = rptDefine.RPTID }; // Define,Remark 就算了...
                    _context.Reports.Add(newRpt);
                    var newRptVarLinks = new List<(int, int)>();
                    foreach (var vid in rptDefine.VID)
                    {
                        newRptVarLinks.Add((rptDefine.RPTID, vid));
                    }
                    var newAddRptVarLinks = newRptVarLinks.Select(t => new ReportVariableLink { RPTID = t.Item1, VID = t.Item2 }).ToList();
                    _context.ReportVariableLinks.AddRange(newAddRptVarLinks);

                }
                _context.SaveChanges();
                return 0;
            }
        }

    }

    /// <summary>
    /// 0 - ok, 1 - out of space, 2 - invalid format, 3 - 1 or more CEID links already defined, 4 - 1 or more CEID invalid, 5 - 1 or more RPTID invalid
    /// </summary>
    /// <param name="evntRptLinks"></param>
    /// <returns></returns>
    public int LinkEvent(IEnumerable<(int CEID, int[] RPTIDs)> evntRptLinks)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                if (evntRptLinks.Count() == 0)//清光, 也許應該寫在其他地方...
                {
                    _context.Database.ExecuteSqlRaw("DELETE FROM Events");
                    _context.SaveChanges();
                    return 0;
                }
                var newLinkECIDs = evntRptLinks.Where(rpt => rpt.RPTIDs.Length > 0).Select(rpt => rpt.CEID).ToList(); //不是刪除的個數
                if (_context.EventReportLinks.Where(link => newLinkECIDs.Contains(link.ECID)).Count() > 0)
                    return 3;
                foreach (var linkDefine in evntRptLinks)
                {
                    if (linkDefine.RPTIDs.Length == 0)//Delete
                    {
                        var deleteLinks = _context.EventReportLinks.Where(rpt => rpt.ECID == linkDefine.CEID);
                        _context.RemoveRange(deleteLinks);
                        continue;
                    }
                    //Create
                    if (_context.Events.Where(evnt => evnt.ECID == linkDefine.CEID).Count() == 0)//CEID 不存在
                        return 4;
                    var vidLst = linkDefine.RPTIDs.ToList(); // RPTID 存在
                    if (_context.Reports.Where(rpt => vidLst.Contains(rpt.RPTID)).Count() == 0)//有不存在的RPTID
                        return 5;

                    var newEvntRptLinks = new List<(int, int)>();// ( CEID, RPTID )
                    foreach (var rptid in linkDefine.RPTIDs)
                    {
                        newEvntRptLinks.Add((linkDefine.CEID, rptid));
                    }
                    var newAddEvntRptLinks = newEvntRptLinks
                        .Select(lnk => new EventReportLink { ECID = lnk.Item1, RPTID = lnk.Item2 }).ToList();
                    _context.EventReportLinks.AddRange(newAddEvntRptLinks);

                }
                _context.SaveChanges();
                return 0;
            }
        }

    }
    /// <summary>
    /// 0 - ok, 1 - denied
    /// </summary>
    /// <param name="isEnable"></param>
    /// <param name="ecids"></param>
    /// <returns></returns>
    public int EnableEvent(bool isEnable, IEnumerable<int> ecids)
    {
        lock (lockObject)
        {
            using (_context = new GemDbContext(_config))
            {
                if (_context.Events.Where(evnt => ecids.Contains(evnt.ECID)).Count() != ecids.Count()) //ECID有不存在
                    return 1;
                if (ecids.Count() == 0) // 全部
                {
                    foreach (var evnt in _context.Events)
                    {
                        evnt.Enabled = isEnable;
                        //if (evnt.EnabledVid != null)
                        //{
                        //    SetVarValue((int)evnt.EnabledVid, isEnable);
                        //}
                    }
                }
                else
                {
                    foreach (var evnt in _context.Events.Where(evnt => ecids.Contains(evnt.ECID)))
                    {
                        evnt.Enabled = isEnable;
                        //另外還要把對應SV改變
                        //if (evnt.EnabledVid != null)
                        //{
                        //    SetVarValue((int)evnt.EnabledVid, isEnable);
                        //}
                    }
                }

                _context.SaveChanges();
                return 0;
            }
        }

    }
    #endregion


}

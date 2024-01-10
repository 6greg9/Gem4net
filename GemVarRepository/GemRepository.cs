using GemVarRepository.Model;
using Microsoft.EntityFrameworkCore;
using Secs4Net;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Secs4Net.Item;
namespace GemVarRepository;
public partial class GemRepository
{
    /// <summary>
    /// The database
    /// </summary>
    private GemVarContext _context;

    public GemRepository()
    {

    }
    /// <summary>for s1f3,s1f4</summary>
    /// <param name="vidList"></param>
    /// <returns></returns>
    public Item? GetSvList(IEnumerable<int> vidList)
    {
        using (_context = new GemVarContext())
        {
            return SubGetSvListByVidList(vidList);
        }
    }
    Item? SubGetSvListByVidList(IEnumerable<int> vidList)
    {
        return Item.L(vidList.Select(vid => SubGetVarByVID(vid)).ToArray());
    }
    public Item? GetSv(int vid)
    {
        using (_context = new GemVarContext())
        {
            return SubGetVarByVID(vid);
        }
    }
    Item? SubGetVarByVID(int vid)
    {
        //在這特別處理CLOCK...
        //但是還是每秒更新1次CLOCK?
        var Variable = _context.Variables
            //.Where(v=>v.VarType=="SV")
            .Where(v => v.VID == vid).FirstOrDefault();
        if (Variable == null)//找不到
            return A(); // ?

        return GemVariableToSecsItem(Variable);
    }
    Item? GemVariableToSecsItem(GemVariable variable)
    {
        try
        {
            if (variable.DataType != "LIST")
            {
                switch (variable.DataType)
                {
                    case "BINARY":
                        
                        return Item.B(Convert.FromHexString(variable.Value));
                    case "BOOL":
                        var BOOL = Convert.ToBoolean(variable.Value);
                        return Item.Boolean(BOOL);
                    case "ASCII":
                        var ASCII = variable.Value;
                        return Item.A(ASCII);
                    case "UINT_1":
                        var UINT_1 = Convert.ToByte(variable.Value);
                        return Item.U1(UINT_1);
                    case "UINT_2":
                        var UINT_2 = Convert.ToUInt16(variable.Value);
                        return Item.U2(UINT_2);
                    case "UINT_4":
                        var UINT_4 = Convert.ToUInt32(variable.Value);
                        return Item.U4(UINT_4);
                    case "UINT_8":
                        var UINT_8 = Convert.ToUInt64(variable.Value);
                        return Item.U8(UINT_8);
                    case "INT_1":
                        var INT_1 = Convert.ToSByte(variable.Value);
                        return Item.I1(INT_1);
                    case "INT_2":
                        var INT_2 = Convert.ToInt16(variable.Value);
                        return Item.I2(INT_2);
                    case "INT_4":
                        var INT_4 = Convert.ToInt32(variable.Value);
                        return Item.I4(INT_4);
                    case "INT_8":
                        var INT_8 = Convert.ToInt64(variable.Value);
                        return Item.I8(INT_8);
                    case "FLOAT_4":
                        var FLOAT_4 = Convert.ToSingle(variable.Value);
                        return Item.F4(FLOAT_4);
                    case "FLOAT_8":
                        var FLOAT_8 = Convert.ToDouble(variable.Value);
                        return Item.F8(FLOAT_8);
                    default:
                        return Item.J(); // !?
                }

            }
            else
            {
                var subItem = _context.Variables.Where(v => v.VarType == "SV").Where(v => v.ListSVID == variable.VID).ToList();
                return Item.L(subItem.Select(v =>
                {
                    return v.DataType == "LIST" ? GetSv(v.VID) : GemVariableToSecsItem(v);
                }).ToArray());
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// for s1f11,s1f12
    /// </summary>
    /// <param name="vidList"></param>
    /// <returns></returns>
    public Item? GetSvNameList(IEnumerable<int> vidList)
    {
        using (_context = new GemVarContext())
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
    public Item? GetSvNameListAll()
    {
        using (_context = new GemVarContext())
        {
            var itemList = _context.Variables.Where(v => v.VarType == "SV")
            .Select(v => Item.L(U4((uint)v.VID), A(v.Name), A(v.Unit)));
            return Item.L(itemList.ToArray());
        }
    }
    /// <summary>
    /// for s2f14
    /// </summary>
    /// <returns></returns>
    public Item? GetEcNameList(IEnumerable<int> vidList)
    {
        using (_context = new GemVarContext())
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
    public Item? GetEcNameListAll()
    {
        using (_context = new GemVarContext())
        {
            var ecLst = _context.Variables.Where(v => v.VarType == "EC");
            var itemList = ecLst.ToList().Select(v => GemVariableToSecsItem(v)); // 這裡有EF的坑
            var itemArry = itemList.ToArray();
            return Item.L(itemArry);
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
        using (_context = new GemVarContext())
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
                            var INT_1 = Convert.ToSByte(variable.Value);
                            variable.Value = INT_1.ToString();
                            break;
                        case "INT_2":
                            var INT_2 = Convert.ToInt16(variable.Value);
                            variable.Value = INT_2.ToString();
                            break;
                        case "INT_4":
                            var INT_4 = Convert.ToInt32(variable.Value);
                            variable.Value = INT_4.ToString();
                            break;
                        case "INT_8":
                            var INT_8 = Convert.ToInt64(variable.Value);
                            variable.Value = INT_8.ToString();
                            break;
                        case "FLOAT_4":
                            var FLOAT_4 = Convert.ToSingle(variable.Value);
                            variable.Value = FLOAT_4.ToString();
                            break;
                        case "FLOAT_8":
                            var FLOAT_8 = Convert.ToDouble(variable.Value);
                            variable.Value = FLOAT_8.ToString();
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
        using (_context = new GemVarContext())
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
    int SubSetVarById(GemVariable variable, (int, Item) idVal)
    {
        try
        {
            if (variable.DataType != "LIST")
            {
                switch (variable.DataType)
                {
                    //case "BINARY":
                    //    return Item.B
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

    public Item? GetReport(int ceid)
    {
        using (_context = new GemVarContext())
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

    #region DynamicEventReport
    /// <summary>
    /// 0 - ok, 1 - out of spac, 2 - invalid format, 3 - 1 or more RPTID already defined, 4 - 1 or more invalid VID
    /// </summary>
    /// <returns></returns>
    public int DefineReport(IEnumerable<(int RPTID, int[] VID)> rptLst)
    {
        using (_context = new GemVarContext())
        {
            if (rptLst.Count() == 0)//清光, 也許應該寫在其他地方...
            {
                _context.Database.ExecuteSqlRaw("TRUNCATE TABLE Reports ");
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
                    var rpt = _context.Reports.Where(rpt => rpt.RPTID == rptDefine.RPTID);
                    _context.Remove(rpt);
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

    /// <summary>
    /// 0 - ok, 1 - out of space, 2 - invalid format, 3 - 1 or more CEID links already defined, 4 - 1 or more CEID invalid, 5 - 1 or more RPTID invalid
    /// </summary>
    /// <param name="evntRptLinks"></param>
    /// <returns></returns>
    public int LinkEvent(IEnumerable<(int CEID, int[] RPTIDs)> evntRptLinks)
    {
        using (_context = new GemVarContext())
        {
            if (evntRptLinks.Count() == 0)//清光, 也許應該寫在其他地方...
            {
                _context.Database.ExecuteSqlRaw("TRUNCATE TABLE Reports ");
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
                    _context.Remove(deleteLinks);
                    continue;
                }
                //Create
                if (_context.Events.Where(evnt => evnt.ECID == linkDefine.CEID).Count() == 0)//CEID 不存在
                    return 4;
                var vidLst = linkDefine.RPTIDs.ToList(); // RPTID 存在
                if (_context.Reports.Where(rpt => vidLst.Contains(rpt.RPTID)).Count() == 0)//有不存在的RPTID
                    return 5;

                var newEvntRptLinks = new List<(int, int)>();
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
    /// <summary>
    /// 0 - ok, 1 - denied
    /// </summary>
    /// <param name="isEnable"></param>
    /// <param name="ecids"></param>
    /// <returns></returns>
    public int EnableEvent(bool isEnable, IEnumerable<int> ecids)
    {
        using (_context = new GemVarContext())
        {
            if (_context.Events.Where(evnt => ecids.Contains(evnt.ECID)).Count() != ecids.Count()) //ECID有不存在
                return 1;
            foreach (var evnt in _context.Events.Where(evnt => ecids.Contains(evnt.ECID)))
            {
                evnt.Enabled = isEnable;
                //另外還要把對應SV改變
            }
            _context.SaveChanges();
            return 0;
        }
    }
    #endregion
}

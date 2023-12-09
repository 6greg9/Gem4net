using GemVarRepository.Model;
using Secs4Net;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Secs4Net.Item;
namespace GemVarRepository;
public class GemRepository
{
    /// <summary>
    /// The database
    /// </summary>
    private GemVarContext _context;

    public GemRepository()
    {

    }
    /// <summary>
    /// for s1f3,s1f4
    /// </summary>
    /// <param name="vidList"></param>
    /// <returns></returns>
    public Item? GetSvListByVidList(IEnumerable<int> vidList)
    {
        using (_context = new GemVarContext())
        {
            return SubGetSvListByVidList(vidList);
        }
    }
    Item? SubGetSvListByVidList(IEnumerable<int> vidList)
    {
        return Item.L(vidList.Select(vid => GetSvByVID(vid)).ToArray());
    }

    public Item? GetSvByVID(int vid)
    {
        using (_context = new GemVarContext())
        {
            return SubGetSvByVID(vid);
        }

    }
    Item? SubGetSvByVID(int vid)
    {
        var Variable = _context.Variables
            .Where(v=>v.VarType=="SV")
            .Where(v=>v.VID==vid).FirstOrDefault();
        if (Variable == null)//找不到
            return A(); // ?

        return ConvertDataToSecsItem(Variable);
    }
    Item ConvertDataToSecsItem(GemVariable variable)
    {
        if (variable.DataType != "LIST")
        {
            switch (variable.DataType)
            {
                //case "BINARY":
                //    return Item.B
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
                    return Item.J();
            }

        }
        else
        {
            var subItem = _context.Variables.Where(v=>v.VarType=="SV").Where(v => v.ListSVID == variable.VID).ToList();
            return Item.L(subItem.Select(v =>
            {
                if (v.DataType == "LIST")
                    return GetSvByVID(v.VID);
                return ConvertDataToSecsItem(v);

            }).ToArray());
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
            var svNameList = vidList.Select(vid=>
        {
            return  _context.Variables.Where(v=>v.VarType=="SV")
           .Where(v=>v.VID== vid).FirstOrDefault();
        }).Select(v=>
       {
           if(v is null)
           {
               return A();// ?
           }
           return Item.L( U4((uint)v.VID), A(v.Name),A(v.Unit));
       });
            return Item.L(svNameList.ToArray());
        }
    }
    public Item? GetSvNameListAll()
    {
        using (_context = new GemVarContext())
        {
            var itemList = _context.Variables.Where(v=> v.VarType=="SV")
            .Select(v=> Item.L( U4((uint)v.VID), A(v.Name),A(v.Unit)));
            return Item.L(itemList.ToArray());
        }
    }

    /// <summary>
    /// for app update var,  return: 1 - not found, 2 - also EC
    /// </summary>
    /// <param name="vid"></param>
    /// <param name="updateValue"></param>
    /// <returns></returns>
    public int SetVarValueById(int vid, object updateValue)
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
                        //case "BINARY":
                        //    return Item.B
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
    public int SetECByIdLst(List<(int, object)> idValLst)
    {
        int EAC = -1;
        var idLst = idValLst.Select( pair=> pair.Item1 ).ToList();
        using (_context = new GemVarContext())
        {
            var ECs = _context.Variables.Where(v=>v.VarType== "EC");
            if (ECs.Where(v => idLst.Contains(v.VID)).Count() != idValLst.Count)
            {
                EAC = 1; 
                return EAC;
            }
            foreach( var idVal in  idValLst )
            {
                var EC = ECs.Where( ec=> ec.VID== idVal.Item1 ).First();
                var SetEcResult = SubSetVarById(EC, idVal);
                if(SetEcResult!=0)
                { return SetEcResult; }
            }
            _context.SaveChanges();
            return 0; //all success
        }
    }
    /// <summary>
    /// 這裡面沒SaveChange
    /// </summary>
    /// <returns></returns>
    int SubSetVarById(GemVariable variable, (int, object) idVal)
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
                        var BOOL = Convert.ToBoolean(idVal.Item2);
                        variable.Value = BOOL.ToString();
                        break;
                    case "ASCII":
                        var ASCII = Convert.ToString(idVal.Item2);
                        variable.Value = ASCII.ToString();
                        break;
                    case "UINT_1":  ///數值類注意OutOfRange
                        var UINT_1 = Convert.ToByte(idVal.Item2);
                        if (IsOutOfValueRange(UINT_1, variable))
                            return 3;
                        variable.Value = UINT_1.ToString();
                        break;
                    case "UINT_2":
                        var UINT_2 = Convert.ToUInt16(idVal.Item2);
                        if (IsOutOfValueRange(UINT_2, variable))
                            return 3;
                        variable.Value = UINT_2.ToString();
                        break;
                    case "UINT_4":
                        var UINT_4 = Convert.ToUInt32(idVal.Item2);
                        if (IsOutOfValueRange(UINT_4, variable))
                            return 3;
                        variable.Value = UINT_4.ToString();
                        break;
                    case "UINT_8":
                        var UINT_8 = Convert.ToUInt64(idVal.Item2);
                        if (IsOutOfValueRange(UINT_8, variable))
                            return 3;
                        variable.Value = UINT_8.ToString();
                        break;
                    case "INT_1":
                        var INT_1 = Convert.ToSByte(idVal.Item2);
                        if (IsOutOfValueRange(INT_1, variable))
                            return 3;
                        variable.Value = INT_1.ToString();
                        break;
                    case "INT_2":
                        var INT_2 = Convert.ToInt16(idVal.Item2);
                        if (IsOutOfValueRange(INT_2, variable))
                            return 3;
                        variable.Value = INT_2.ToString();
                        break;
                    case "INT_4":
                        var INT_4 = Convert.ToInt32(idVal.Item2);
                        if (IsOutOfValueRange(INT_4, variable))
                            return 3;
                        variable.Value = INT_4.ToString();
                        break;
                    case "INT_8":
                        var INT_8 = Convert.ToInt64(idVal.Item2);
                        variable.Value = INT_8.ToString();
                        break;
                    case "FLOAT_4":
                        var FLOAT_4 = Convert.ToSingle(idVal.Item2);
                        if (IsOutOfValueRange(FLOAT_4, variable))
                            return 3;
                        variable.Value = FLOAT_4.ToString();
                        break;
                    case "FLOAT_8":
                        var FLOAT_8 = Convert.ToDouble(idVal.Item2);
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

    public Item? GetReportByEventId(int ceid)
    {
        using (_context = new GemVarContext())
        {
            List<(int,Item)> rtnRptItems = new(); 
            var reports = _context.EventReportLinks.Where(link => link.ECID == ceid)
                .Select(link => link.Report);
            var reportVars = reports
                .Join(_context.ReportVariableLinks,
                rpt => rpt.RPTID,
                link => link.RPTID,
                (rpt,link)=>
                new
                {
                    RptId = rpt.RPTID,
                    Variable = _context.Variables.Where(v=>v.VID==link.VID).First()
                }).ToList(); //.GroupBy( v=>v.RptId ).Select(v=> v.)
            //.Select(pair=> {RptId = pair.Key, Datas =  pair} );
            var rtnReports = reportVars.GroupBy( d=> d.RptId).Select(pair=>(pair.Key,pair.ToList())).ToList();
            foreach(var groupData in rtnReports)
            {
                var vids = groupData.Item2.Select(v=> v.Variable.VID);
                var datas = SubGetSvListByVidList(vids);
                rtnRptItems.Add((groupData.Item1, datas));
            }
            return  L(rtnRptItems.Select( p=>L(U4((uint) p.Item1),L(p.Item2 ) ) ));
        }

        
    }
}

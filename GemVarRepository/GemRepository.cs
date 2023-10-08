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

    public GemRepository(GemVarContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// for s1f3,s1f4
    /// </summary>
    /// <param name="vidList"></param>
    /// <returns></returns>
    public Item? GetSvListByVidList(IEnumerable<int> vidList)
    {
        _context = new GemVarContext();

        return Item.L(vidList.Select(vid=> GetSvByVID(vid)).ToArray());
    }
    public Item? GetSvByVID(int vid)
    {
        _context = new GemVarContext();

        var Variable = _context.Variables
            .Where(v=>v.VarType=="SV")
            .Where(v=>v.VID==vid).FirstOrDefault();
        if (Variable == null)//找不到
            return A(); // ?
        
        return ConvertDataToSecsItem(Variable);

    }
    Item ConvertDataToSecsItem(GemVariable variable)
    {
        if(variable.DataType != "LIST")
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
            return Item.L(subItem.Select(v => {
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
    public Item? GetSvNameListAll()
    {
        var itemList = _context.Variables.Where(v=> v.VarType=="SV")
            .Select(v=> Item.L( U4((uint)v.VID), A(v.Name),A(v.Unit)));
        return Item.L(itemList.ToArray());
    }
}

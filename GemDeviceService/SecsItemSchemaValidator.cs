using Secs4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemDeviceService;
public static class SecsItemSchemaValidator
{
    public static Func<SecsMessage,bool> IsValid = ( Msg ) =>
    {
        switch (Msg.S,Msg.F) // Dic< (int,int), Func > ?
        {
            case (1,1):
                return IsS1F1(Msg.SecsItem);
            case (1,2):
                return IsS1F2(Msg.SecsItem);
            case (1,3):
                return IsS1F3(Msg.SecsItem);
            case (1,4):
                return IsS1F4(Msg.SecsItem);
            case (1,11):
                return IsS1F11(Msg.SecsItem);
            case (1,12):
                return IsS1F12(Msg.SecsItem);
            case (1,13):
                return IsS1F13(Msg.SecsItem);
            case (1,15):
                return IsS1F15(Msg.SecsItem);
            case (1, 17):
                return IsS1F17(Msg.SecsItem);
            case (2, 13):
                return IsS2F13(Msg.SecsItem);
            case (2,15):
                return IsS2F15(Msg.SecsItem);
            case (2,25):
                return IsS2F25(Msg.SecsItem);
            case (10,3):
                return IsS10F3(Msg.SecsItem);
            default:
                return true;
        }
        return false;
    };

    #region Stream 1

    static Func<Item?, bool> IsS1F1=(item)=>
    {
        if ( item != null )
            return false;

        return true;
    };

    static Func<Item?, bool> IsS1F2=(item)=>
    {
        if ( item == null )
            return false;
        if(item.Format != SecsFormat.List)
            return false;
        if(item.Count != 2)
            return false;

        if(item.Items[0].Format != SecsFormat.ASCII
        || item.Items[1].Format != SecsFormat.ASCII)
            return false;

        return true;
    };

    static Func<Item?, bool> IsS1F3=(item)=>
    {
        if ( item == null )
            return false;
        if(item.Format != SecsFormat.List)
            return false;

        if(item.Items.Where(item=>item.Format != SecsFormat.U4).Count()>0)
            return false;

        return true;
    };

    static Func<Item?, bool> IsS1F4=(item)=>
    {
        if ( item == null )
            return false;
        if(item.Format != SecsFormat.List)
            return false;

        return true;
    };

    static Func<Item?, bool> IsS1F11=(item)=>
    {
        if ( item == null )
            return false;
        if(item.Format != SecsFormat.List)
            return false;

        //第一層
        foreach( var i in item.Items )
        {
            if( i.Format != SecsFormat.U4 )
                return false;
        }
        return true;
    };

    static Func<Item?, bool> IsS1F12 = (item)=>
    {
        if ( item == null
        ||   item.Format != SecsFormat.List)
            return false;

        //第一層
        foreach( var vData in item.Items )
        {
            if( vData.Format != SecsFormat.List
            ||  vData.Count != 3 )
                return false;
            //第二層
            
            if(vData[0].Format != SecsFormat.U4
            || vData[1].Format != SecsFormat.ASCII
            || vData[2].Format != SecsFormat.ASCII)
                return false;


            return true;
        }

        return true;
    };

    static Func<Item?,bool > IsS1F13 = ( item ) =>
    {
        if ( item == null )
            return false;
        if(item.Format != SecsFormat.List)
            return false;
        //第一層
        var level1 = item.Items;
        if ( level1 == null 
        ||   level1.Length != 2
        ||   level1[0].Format != SecsFormat.ASCII
        ||   level1[1].Format != SecsFormat.ASCII)
            return false;

        return true;
    };

    static Func<Item?, bool> IsS1F15 = (item) =>
    {
        if (item == null)
            return true;
       
        return false;
    };

    static Func<Item?, bool> IsS1F17 = (item) =>
    {
        if (item == null)
            return true;

        return false;
    };

    #endregion

    #region Stream 2
    static Func<Item?, bool> IsS2F13 = (itemRoot) =>
    {
        if (itemRoot == null)
            return false;
        if (itemRoot.Format != SecsFormat.List)
            return false;
        //第一層
        if (itemRoot.Items.Where(item => item.Format != SecsFormat.U4).Count() > 0)
            return false;
        return true;

    };
    static Func<Item?, bool> IsS2F15 = (itemRoot) =>
    {
        if (itemRoot == null)
            return false;
        if(itemRoot.Format != SecsFormat.List)
            return false;
        //第一層
        if(itemRoot.Items.Where(item=>item.Format != SecsFormat.List).Count()>0)
            return false;
        var item1s = itemRoot.Items;
        foreach(var item in item1s)
        {
            if( item.Items.Count() != 2)
                return false;
            if(item.Items[0].Format != SecsFormat.U4)
                return false;
        }
        return true;

    };
    static Func<Item?, bool> IsS2F25 = (itemRoot) =>
    {
        if (itemRoot == null)
            return false;
        if(itemRoot.Format != SecsFormat.Binary)
            return false;
        return true;

    };
    static Func<Item?, bool> IsS2F41 = (item) =>
    {
        if (item == null)
            return false;

        return false;
    };


    #endregion

    #region　Stream 10
    static Func<Item?, bool> IsS10F3 = (item) =>
    {
        if (item == null)
            return false;
        if (item.Format != SecsFormat.List)
            return false;
        //第一層
        var level1 = item.Items;
        if (level1 == null
        || level1.Length != 2
        || level1[0].Format != SecsFormat.Binary
        || level1[1].Format != SecsFormat.ASCII)
            return false;

        return true;
    };

    #endregion
}

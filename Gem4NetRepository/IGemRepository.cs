using Gem4NetRepository.Model;
using Secs4Net;

namespace Gem4NetRepository
{
    public interface IGemRepository
    {
        int UseJsonSecsItem { get; }


        Task<int> DefineReport(IEnumerable<(int RPTID, int[] VID)> rptLst);

        Task<int> EnableAlarm(int alid, bool enable);
        Task<int> EnableEvent(bool isEnable, IEnumerable<int> ecids);

        Task<IEnumerable<GemAlarm?>> GetAlarm(IEnumerable<int> alarmIds);
        Task<GemAlarm?> GetAlarm(int alarmId);
        Task<IEnumerable<GemAlarm>> GetAlarmAll();
        Task<Item?> GetEC(int vid);
        Task<Item?> GetEcDetailList(IEnumerable<int> vidList);
        Task<Item?> GetEcDetailListAll();
        Task<Item?> GetEcList(IEnumerable<int> vidList);
        Task<Item?> GetEcValueList(IEnumerable<int> vidList);
        Task<Item?> GetEcValueListAll();

        Task<Item?> GetReportByRpid(int rpid);
        Task<Item?> GetReportsByCeid(int ceid);
        Task<Item?> GetSv(int vid);
        Task<Item?> GetSvAll();
        Task<Item?> GetSvListByVidList(IEnumerable<int> vidList);
        Task<Item?> GetSvNameList(IEnumerable<int> vidList);
        Task<Item?> GetSvNameListAll();
        Task<Item?> GetVariable(int vid);
        Task<Item?> GetVariable(string name);
        Task<Item?> GetVariableAll();
        string ItemToVarString(Item item);
        Task<int> LinkEvent(IEnumerable<(int CEID, int[] RPTIDs)> evntRptLinks);

        Task<int> SetAlarmCode(int alid, int alcd);
        Task<int> SetEcList(IEnumerable<(int, Item)> idValLst);
        Task<int> SetVarValue(int vid, object updateValue);
        Item? SubGetVariableAll();
        Item VarStringToItem(string dataType, string varStr);
    }
}
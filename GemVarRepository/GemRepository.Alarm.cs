using GemVarRepository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemVarRepository;
public partial class GemRepository
{
    // S5系列
    public int GetAlarm(int alarmId)
    {
        using (_context = new GemDbContext())
        {

            return 0;
        }
    }
    public int GetAlarm(IEnumerable<int> alarmIds)
    {
        return 0;
    }
    public int GetAlarmAll() { return 0; }

    public int SetAlarm(GemAlarm alarm) { return 0; }
}

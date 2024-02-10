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
    public GemAlarm? GetAlarm(int alarmId)
    {
        using (_context = new GemDbContext())
        {
            return _context.Alarms.Where(alrm => alrm.ALID == alarmId).First();
        }
    }
    public IEnumerable<GemAlarm?> GetAlarm(IEnumerable<int> alarmIds)
    {
        using (_context = new GemDbContext())
        {
            var tempAlarm = new List<GemAlarm?>();
            foreach (var alid in alarmIds)
            {
                tempAlarm.Add(_context.Alarms.Where(alrm => alrm.ALID == alid).FirstOrDefault());
            }
            return tempAlarm;
        }
    }
    public IEnumerable<GemAlarm> GetAlarmAll()
    {
        using (_context = new GemDbContext())
        {
            return _context.Alarms.ToList();
        }
    }
    /// <summary>
    /// 0:success ,1:notfound, 2:
    /// </summary>
    /// <param name="alarm"></param>
    /// <returns></returns>
    public int SetAlarmCode(int alid, bool alcd)
    {
        using (_context = new GemDbContext())
        {
            var targetAlarm = _context.Alarms
                .Where(alrm => alrm.ALID == alid).Take(1);
            if (!targetAlarm.Any())
                return 1;
            foreach (var item in targetAlarm)//應該只有1個...
            {
                item.ALCD = alcd;
            }

            _context.SaveChanges();
            return 0;
        }
    }
    public int EnableAlarm(int alid, bool enable)
    {
        using (_context = new GemDbContext())
        {
            var targetAlarm = _context.Alarms
                .Where(alrm => alrm.ALID == alid).Take(1);
            if (!targetAlarm.Any())
                return 1;
            foreach (var item in targetAlarm)//應該只有1個...
            {
                item.ALED = enable;
            }

            _context.SaveChanges();
            return 0;
        }
    }
}

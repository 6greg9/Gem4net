using Gem4NetRepository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository;
public partial class GemRepository
{
    // S5系列
    public async Task<GemAlarm?> GetAlarm(int alarmId)
    {
        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
            {
                return _context.Alarms.Where(alrm => alrm.ALID == alarmId).FirstOrDefault();
            }
        }

        finally { semSlim.Release(); }
    }
    public async Task<IEnumerable<GemAlarm?>> GetAlarm(IEnumerable<int> alarmIds)
    {
        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
            {
                var tempAlarm = new List<GemAlarm?>();
                foreach (var alid in alarmIds)
                {
                    tempAlarm.Add(_context.Alarms.Where(alrm => alrm.ALID == alid).FirstOrDefault());
                }
                return tempAlarm;
            }
        }

        finally { semSlim.Release(); }
    }
    public async Task<IEnumerable<GemAlarm>> GetAlarmAll()
    {
        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
            {
                return _context.Alarms.ToList();
            }
        }

        finally { semSlim.Release(); }
        
    }
    /// <summary>
    /// 0:success ,1:notfound, 2:
    /// </summary>
    /// <param name="alarm"></param>
    /// <returns></returns>
    public async Task<int> SetAlarmCode(int alid, int alcd)
    {
        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
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
        finally { semSlim.Release(); }
        
    }
    public async Task<int> EnableAlarm(int alid, bool enable)
    {
        await semSlim.WaitAsync();
        try
        {
            using (_context = new GemDbContext(_config))
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
        finally { semSlim.Release(); }
        
    }
}

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.Model
{
    public static class GemDbContextExtension
    {
        public static IQueryable<GemVariable> GetVariableByVid(
        this IQueryable<GemVariable> dbVariables, 
        int vid)
        {
            return dbVariables.Where(v=> v.VID == vid).Take(1);
        }

        public static IQueryable<GemVariable> GetVariableByVidList(
        this IQueryable<GemVariable> dbVariables,
        IEnumerable<int> vidList)
        {
            return dbVariables.Where(v => vidList.Contains( v.VID ) );
        }
    }
}

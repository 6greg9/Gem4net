using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem4NetRepository.Model
{

    [Mapper(UseDeepCloning = true)]
    public static partial  class Mappers
    {
        public static partial FormattedProcessProgramLog FormattedProcessProgramToFormattedProcessProgramLog(FormattedProcessProgram fpp);

        public static partial ProcessProgramLog ProcessProgramToProcessProgramLog(ProcessProgram pp);
    }
}

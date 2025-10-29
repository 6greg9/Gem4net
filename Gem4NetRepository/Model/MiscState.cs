using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gem4NetRepository.Model
{
    public class MiscState
    {
        public int ID { get; set; }
        public string Name { get; set; }

        // 也可以同時放一個 schema-less 的 Data 欄（示範在第 2/3 節）
        public JsonDocument? Info { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Loot3Framework.Types.Classes.Algorithms.Looting;

namespace HTTPLoot.Models
{
    [Serializable]
    class LootWrapper
    {
        public string LootItem { get; set; }
        public PR_PartionLoot<string, PartitionLoot<string>> Looter { get; set; }
    }
}

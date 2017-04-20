using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPLoot.Models
{
    [Serializable]
    class Config
    {
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
        public string RarName { get; set; }
    }
}

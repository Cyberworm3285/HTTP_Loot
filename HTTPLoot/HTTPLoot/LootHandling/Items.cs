using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Loot3Framework.Types.Classes.BaseClasses;
using Loot3Framework.Interfaces;

namespace HTTPLoot.LootHandling
{
    class Items : DefaultObjectFetcher<string>
    {
        public Items() 
            : base(new ILootable<string>[] {
                new PP_Function(() => "jo1").SetProps("bla", 10, "bla"),
                new PP_Function(() => "jo2").SetProps("bla", 40, "bla"),
                new PP_Function(() => "jo3").SetProps("bla", 300, "bla"),
                new PP_Function(() => "jo4").SetProps("bla", 800, "bla"),
                new PP_Function(() => "bla1").SetProps("jo", 5, "jo"),
                new PP_Function(() => "bla2").SetProps("jo", 50, "jo"),
                new PP_Function(() => "bla3").SetProps("jo", 250, "jo"),
                new PP_Function(() => "bla4").SetProps("jo", 1000, "jo"),
            })
        {}
    }
}

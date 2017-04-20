using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Loot3Framework.Types.Classes.BaseClasses;
using Loot3Framework.Interfaces;
using Loot3Framework.Types.Classes.Algorithms.TypeFetching;
using Loot3Framework.ExtensionMethods.CollectionOperations;
using Loot3Framework.Types.Classes.Algorithms.Looting;
using Loot3Framework.Types.Classes.Algorithms.Filter;
using Loot3Framework.Types.Classes.RarityTables;
using Loot3Framework.Types.Exceptions;
using Newtonsoft.Json;

using HTTPLoot.Models;

namespace HTTPLoot.LootHandling
{
    class LootHandler : BaseLootHolder<string>
    {
        private static LootHandler instance;
        public static LootHandler Instance => instance ?? (instance = new LootHandler());
        public static Config Config { get; set; }

        public LootHandler() 
            : base(new FetchByInheritance<string>(typeof(ILootable<string>)))
        {
            AddAllLootObjects();
            Config = new Config() { LowerBound = 0, RarName = null, UpperBound = 1000 };
        }

        public override ILootable<string> GetLoot(ILootingAlgorithm<string> algo)
        {
            return base.GetLoot(
                algo,
                new ConfigurableFilter(
                            _rarityLowerBound: Config.LowerBound,
                            _rarityUpperBound: Config.UpperBound,
                            _rarityName: Config.RarName
                        )
                );
        }

        public string GetLootJSON()
        {
            PR_PartionLoot<string, PartitionLoot<string>> looter;
            if (Config.LowerBound != 0 || Config.UpperBound != 1000)
            {
                List<string> allowedRars = new List<string>();
                DefaultRarityTable.SharedInstance.Chain.Intervalls.Fuse(DefaultRarityTable.SharedInstance.Values).DoAction(r =>
                {
                    if (r.Item1.X >= Config.LowerBound && r.Item1.Y <= Config.UpperBound)
                        allowedRars.Add(r.Item2);
                });
                looter = new PR_PartionLoot<string, PartitionLoot<string>>(DefaultRarityTable.SharedInstance, PartitionLoot<string>.SharedInstance, allowedRars.ToArray());
            }
            else
                looter = new PR_PartionLoot<string, PartitionLoot<string>>(DefaultRarityTable.SharedInstance, PartitionLoot<string>.SharedInstance);
            return JsonConvert.SerializeObject(
                new LootWrapper()
                {
                    LootItem = GetLoot(
                        looter
                    ).Item,
                    Looter = looter
                },
                Formatting.Indented
            );
        }
    }
}

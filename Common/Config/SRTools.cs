using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace KoishiServer.Common.Config
{
    public class SRToolsConfigLoader
    {
        private const string SRToolsConfigFilePath = "freesr-data.json";

        public static async Task<SRToolData> LoadConfigAsync()
        {
            return await ConfigLoader.FromFileAsync<SRToolData>(SRToolsConfigFilePath);
        }

        public static bool HasFileChanged()
        {
            return ConfigLoader.HasFileChanged(SRToolsConfigFilePath);
        }

        public static async Task SaveToFileAsync(SRToolData newData)
        {
            await ConfigLoader.SaveToFileAsync(SRToolsConfigFilePath, newData);
        }
    }

    public class SRToolData
    {
        public class RelicSubAffixData
        {
            [JsonProperty("sub_affix_id")]
            public uint SubAffixId { get; set; }

            [JsonProperty("count")]
            public uint Count { get; set; }

            [JsonProperty("step")]
            public uint Step { get; set; }
        }

        public class RelicData
        {
            [JsonProperty("level")]
            public uint Level { get; set; }

            [JsonProperty("relic_id")]
            public uint RelicID { get; set; }

            [JsonProperty("relic_set_id")]
            public uint RelicSetID { get; set; }

            [JsonProperty("main_affix_id")]
            public uint MainAffixID { get; set; }

            [JsonProperty("sub_affixes")]
            public List<RelicSubAffixData>? SubAffixes { get; set; }

            [JsonProperty("internal_uid")]
            public uint InternalUID { get; set; }

            [JsonProperty("equip_avatar")]
            public uint EquipAvatar { get; set; }
        }

        public class LightconeData
        {
            [JsonProperty("level")]
            public uint Level { get; set; }

            [JsonProperty("internal_uid")]
            public uint InternalUID { get; set; }

            [JsonProperty("equip_avatar")]
            public uint EquipAvatar { get; set; }

            [JsonProperty("item_id")]
            public uint ItemID { get; set; }

            [JsonProperty("rank")]
            public uint Rank { get; set; }

            [JsonProperty("promotion")]
            public uint Promotion { get; set; }
        }

        public class AvatarData
        {
            [JsonProperty("owner_uid")]
            public uint OwnerUID { get; set; }

            [JsonProperty("avatar_id")]
            public uint AvatarID { get; set; }

            [JsonProperty("data")]
            public AvatarInnerData? Data { get; set; }

            [JsonProperty("level")]
            public uint Level { get; set; }

            [JsonProperty("promotion")]
            public uint Promotion { get; set; }

            [JsonProperty("techniques")]
            public List<uint>? Techniques { get; set; }

            [JsonProperty("sp_value")]
            public uint SpValue { get; set; }

            [JsonProperty("sp_max")]
            public uint SpMax { get; set; }
        }

        public class AvatarInnerData
        {
            [JsonProperty("rank")]
            public uint Rank { get; set; }

            [JsonProperty("skills")]
            public Dictionary<uint, uint>? Skills { get; set; }
        }

        public class DynamicKey
        {
            [JsonProperty("key")]
            public string? Key { get; set; }

            [JsonProperty("value")]
            public uint Value { get; set; }
        }

        public class BlessingData
        {

            [JsonProperty("level")]
            public uint Level { get; set; }

            [JsonProperty("id")]
            public uint ID { get; set; }

            [JsonProperty("dynamic_key")]
            public DynamicKey? DynamicKey { get; set; }
        }

        public class MonsterData
        {
            [JsonProperty("level")]
            public uint Level { get; set; }

            [JsonProperty("monster_id")]
            public uint MonsterID { get; set; }

            [JsonProperty("max_hp")]
            public uint MaxHP { get; set; }
        }

        public class BattleConfigData
        {
            [JsonProperty("battle_type")]
            public string? BattleType { get; set; }

            [JsonProperty("blessings")]
            public BlessingData[]? Blessings { get; set; }

            [JsonProperty("custom_stats")]
            public RelicSubAffixData[]? CustomStats { get; set; }

            [JsonProperty("cycle_count")]
            public uint CycleCount { get; set; }

            [JsonProperty("monsters")]
            public MonsterData[][]? Monsters { get; set; }

            [JsonProperty("path_resonance_id")]
            public uint PathResonanceID { get; set; }

            [JsonProperty("stage_id")]
            public uint StageID { get; set; }
        }

        [JsonProperty("relics")]
        public List<RelicData>? Relics { get; set; }

        [JsonProperty("lightcones")]
        public List<LightconeData>? Lightcones { get; set; }

        [JsonProperty("avatars")]
        public Dictionary<uint, AvatarData>? Avatars { get; set; }
    }

    public class SRToolDataReq
    {
        [JsonProperty("data")]
        public SRToolData? Data { get; set; }
    }

    public class SRToolDataRsp
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}

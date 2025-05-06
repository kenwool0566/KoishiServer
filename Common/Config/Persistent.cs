using Newtonsoft.Json;

namespace KoishiServer.Common.Config
{
    public class PersistentLoader
    {
        private const string PersistentFilePath = "Persistent.json";

        public static async Task<Persistent> LoadConfigAsync()
        {
            return await ConfigLoader.FromFileAsync<Persistent>(PersistentFilePath);
        }

        public static async Task SaveToFileAsync(Persistent newData)
        {
            await ConfigLoader.SaveToFileAsync(PersistentFilePath, newData);
        }
    }

    public class Persistent
    {
        [JsonProperty("Lineup")]
        public Dictionary<string, LineupEntry?> Lineup { get; set; } = new()
        {
            ["0"] = new LineupEntry { Id = 1403, Leader = false },
            ["1"] = new LineupEntry { Id = 1407, Leader = false },
            ["2"] = new LineupEntry { Id = 8001, Leader = true },
            ["3"] = new LineupEntry { Id = 1001, Leader = false },
        };

        [JsonProperty("Position")]
        public Vector3 Position { get; set; } = new Vector3 { X = -26968, Y = 78953, Z = 14457 };

        [JsonProperty("Rotation")]
        public Vector3 Rotation { get; set; } = new Vector3 { X = 0, Y = 11858, Z = 0 };

        [JsonProperty("MapLayer")]
        public uint MapLayer { get; set; } = 2;

        [JsonProperty("Scene")]
        public SceneData Scene { get; set; } = new SceneData
        {
            PlaneId = 20411,
            FloorId = 20411001,
            EntryId = 2041101
        };

        [JsonProperty("Trailblazer")]
        public Trailblazer Trailblazer { get; set; } = new Trailblazer
        {
            Gender = "Boy",
            Path = "Remembrance"
        };

        [JsonProperty("MarchPath")]
        public string MarchPath { get; set; } = "TheHunt";
    }

    public class LineupEntry
    {
        [JsonProperty("Id")]
        public uint Id { get; set; }

        [JsonProperty("Leader")]
        public bool Leader { get; set; }
    }

    public class Trailblazer
    {
        [JsonProperty("Gender")]
        public string Gender { get; set; } = "Boy";

        [JsonProperty("Path")]
        public string Path { get; set; } = "Remembrance";
    }

    public class Vector3
    {
        [JsonProperty("X")]
        public int X { get; set; }

        [JsonProperty("Y")]
        public int Y { get; set; }

        [JsonProperty("Z")]
        public int Z { get; set; }
    }

    public class SceneData
    {
        [JsonProperty("PlaneId")]
        public uint PlaneId { get; set; }

        [JsonProperty("FloorId")]
        public uint FloorId { get; set; }

        [JsonProperty("EntryId")]
        public uint EntryId { get; set; }
    }
}

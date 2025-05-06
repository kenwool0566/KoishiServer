using KoishiServer.Common.Resource.Proto;
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

    public partial class Persistent
    {
        [JsonProperty("Lineup")]
        public Dictionary<byte, LineupEntry?> Lineup { get; set; } = new()
        {
            [0] = new LineupEntry { Id = 1403, Leader = false },
            [1] = new LineupEntry { Id = 1407, Leader = false },
            [2] = new LineupEntry { Id = 8001, Leader = true },
            [3] = new LineupEntry { Id = 1001, Leader = false },
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

    public partial class Persistent
    {
        private static readonly Dictionary<string, MultiPathAvatarType> MarchPathMap = new()
        {
            ["Preservation"] = MultiPathAvatarType.Mar7ThKnightType,
            ["TheHunt"] = MultiPathAvatarType.Mar7ThRogueType
        };

        private static readonly Dictionary<(string gender, string path), MultiPathAvatarType> McPathMap = new()
        {
            [("Boy", "Destruction")] = MultiPathAvatarType.BoyWarriorType,
            [("Boy", "Preservation")] = MultiPathAvatarType.BoyKnightType,
            [("Boy", "Harmony")] = MultiPathAvatarType.BoyShamanType,
            [("Boy", "Remembrance")] = MultiPathAvatarType.BoyMemoryType,

            [("Girl", "Destruction")] = MultiPathAvatarType.GirlWarriorType,
            [("Girl", "Preservation")] = MultiPathAvatarType.GirlKnightType,
            [("Girl", "Harmony")] = MultiPathAvatarType.GirlShamanType,
            [("Girl", "Remembrance")] = MultiPathAvatarType.GirlMemoryType,
        };

        public Gender GetTrailblazerGender()
        {
            return this.Trailblazer.Gender == "Girl" ? Gender.Woman : Gender.Man;
        }

        public MultiPathAvatarType GetTrailblazerType()
        {
            return McPathMap.GetValueOrDefault((this.Trailblazer.Gender, this.Trailblazer.Path), MultiPathAvatarType.BoyWarriorType);
        }

        public MultiPathAvatarType GetMarchType()
        {
            return MarchPathMap.GetValueOrDefault(this.MarchPath, MultiPathAvatarType.Mar7ThKnightType);
        }

        public List<uint> GetTrailblazerIds()
        {
            if (this.Trailblazer.Gender == "Girl") return new List<uint> { 8002, 8004, 8006, 8008 };
            else return new List<uint> { 8001, 8003, 8005, 8007 };
        }

        public uint GetLineupLeader()
        {
            return this.Lineup
                .Where(kvp => kvp.Value != null && kvp.Value.Leader)
                .Select(kvp => kvp.Value!.Id)
                .FirstOrDefault();
        }

        public void SetLineupLeader(uint slot)
        {
            byte slotByte = (byte)slot;

            if (!Lineup.ContainsKey(slotByte) || Lineup[slotByte] == null)
            {
                throw new ArgumentException($"Slot {slot} is invalid or uninitialized.");
            }

            foreach (KeyValuePair<byte, LineupEntry?> kvp in Lineup)
            {
                if (kvp.Value != null)
                {
                    kvp.Value.Leader = false;
                }
            }

            Lineup[slotByte]!.Leader = true;
        }

        private void EnsureLeaderExists()
        {
            bool hasLeader = this.Lineup.Values.Any(v => v != null && v.Leader);
            if (!hasLeader)
            {
                foreach (KeyValuePair<byte, LineupEntry?> kvp in this.Lineup)
                {
                    if (kvp.Value != null)
                    {
                        kvp.Value.Leader = true;
                        break;
                    }
                }
            }
        }

        public void UpdateLineupSlot(byte slot, uint newAvatarId)
        {
            if (this.Lineup.ContainsKey(slot))
            {
                LineupEntry entry = this.Lineup[slot]!;

                if (entry.Leader)
                {
                    entry.Leader = (newAvatarId == entry.Id);
                }

                entry.Id = newAvatarId;

                EnsureLeaderExists();
            }

            else
            {
                throw new ArgumentException($"Slot {slot} does not exist in the lineup.");
            }
        }

        public void FullUpdateLineup(Dictionary<byte, uint> newLineup)
        {
            uint currentLeaderId = GetLineupLeader();

            this.Lineup.Clear();

            foreach (KeyValuePair<byte, uint> kvp in newLineup)
            {
                byte slot = kvp.Key;
                uint id = kvp.Value;

                this.Lineup[slot] = new LineupEntry
                {
                    Id = id,
                    Leader = (id == currentLeaderId)
                };
            }

            EnsureLeaderExists();
        }

        public void SetPosRot((int posX, int posY, int posZ) position, (int rotX, int rotY, int rotZ) rotation)
        {
            this.Position.X = position.posX;
            this.Position.Y = position.posY;
            this.Position.Z = position.posZ;

            this.Rotation.X = rotation.rotX;
            this.Rotation.Y = rotation.rotY;
            this.Rotation.Z = rotation.rotZ;
        }

        public void SetMapLayer(uint layer)
        {
            this.MapLayer = layer;
        }

        public void SetSceneData(uint planeId, uint floorId, uint entryId)
        {
            this.Scene.PlaneId = planeId;
            this.Scene.FloorId = floorId;
            this.Scene.EntryId = entryId;
        }

        public void SetTrailblazerType(string gender, string path)
        {
            this.Trailblazer.Gender = gender;
            this.Trailblazer.Path = path;
        }

        public void SetMarchPath(string path)
        {
            this.MarchPath = path;
        }
    }
}

using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KoishiServer.Common.Config
{
    public static class ConfigLoader
    {
        private static readonly Dictionary<string, DateTime?> _fileTimestamps = new();

        public static bool HasFileChanged(string filePath)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(filePath);
            _fileTimestamps.TryGetValue(filePath, out DateTime? previousWriteTime);

            if (previousWriteTime != null)
            {
                if (lastWriteTime > previousWriteTime)
                {
                    _fileTimestamps[filePath] = lastWriteTime;
                    return true;
                }
                return false;
            }

            _fileTimestamps[filePath] = lastWriteTime;
            return true;
        }

        public static T FromFile<T>(string filePath) where T : new()
        {
            try
            {
                Log.Information("Loading {JsonFile}.", filePath);
                
                string json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json)) throw new InvalidDataException($"{filePath} is empty.");

                T? result = JsonConvert.DeserializeObject<T>(json);
                if (result == null) throw new InvalidDataException($"{filePath} deserialization failed.");

                Log.Information("Loaded {JsonFile} successfully.", filePath);

                return result;
            }
            
            catch (Exception ex)
            {
                Log.Error("Failed loading {JsonFile}: {Exception}", filePath, ex);
                Log.Warning("{JsonFile} loader caught an exception. Fallbacking.", filePath);
                return new T();
            }
        }

        public static async Task<T> FromFileAsync<T>(string filePath) where T : new()
        {
            try
            {
                Log.Information("Loading {JsonFile}.", filePath);

                string json = await File.ReadAllTextAsync(filePath);
                if (string.IsNullOrWhiteSpace(json)) throw new InvalidDataException($"{filePath} is empty.");

                T? result = JsonConvert.DeserializeObject<T>(json);
                if (result == null) throw new InvalidDataException($"{filePath} deserialization failed.");

                Log.Information("Loaded {JsonFile} successfully.", filePath);

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Failed loading {JsonFile}: {Exception}", filePath, ex);
                Log.Warning("{JsonFile} loader caught an exception. Fallbacking.", filePath);
                return new T();
            }
        }

        // public static async Task SaveToFileAsync(string filePath, string json)
        // {
        //     try
        //     {
        //         Log.Information("Saving {JsonFile}.", filePath);
        //         await File.WriteAllTextAsync(filePath, json);
        //         Log.Information("{JsonFile} saved successfully.", filePath);
        //     }
        //     catch (Exception ex)
        //     {
        //         Log.Error("Failed saving {JsonFile}: {Exception}", filePath, ex);
        //     }
        // }

        public static async Task SaveToFileAsync<T>(string filePath, T obj)
        {
            try
            {
                Log.Information("Saving {JsonFile}.", filePath);
                string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, json);
                Log.Information("{JsonFile} saved successfully.", filePath);
            }
            catch (Exception ex)
            {
                Log.Error("Failed saving {JsonFile}: {Exception}", filePath, ex);
            }
        }

        public static T FromString<T>(string json) where T : new()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json)) throw new InvalidDataException("JSON string is empty.");

                T? result = JsonConvert.DeserializeObject<T>(json);
                if (result == null) throw new InvalidDataException("Deserialization returned null.");

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to deserialize {Json}: {Exception}", json, ex);
                return new T();
            }
        }
    }
}

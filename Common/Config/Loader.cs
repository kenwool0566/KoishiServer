using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;

namespace KoishiServer.Common.Config
{
    public static class ConfigLoader
    {
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
    }
}

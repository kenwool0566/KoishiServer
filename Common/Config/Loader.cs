using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;

namespace KoishiServer.Common.Config
{
    public static class ConfigLoader
    {
        public static T LoadConfig<T>(string filePath) where T : new()
        {
            try
            {
                Log.Information("Loading {JsonFile}.", filePath);
                string json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    Log.Warning("{JsonFile} is empty. Fallbacking.", filePath);
                    return new T();
                }

                T? result = JsonConvert.DeserializeObject<T>(json);

                if (result == null)
                {
                    Log.Warning("{JsonFile} deserialization failed. Fallbacking.", filePath);
                    return new T();
                }

                Log.Information("Loaded {JsonFile} successfully.", filePath);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Failed loading {JsonFile}: {Exception}", filePath, ex);
                Log.Warning("{JsonFile} loader catched an exception. Fallbacking.", filePath);
                return new T();
            }
        }
    }
}

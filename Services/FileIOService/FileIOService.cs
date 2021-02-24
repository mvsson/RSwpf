using System.IO;
using Newtonsoft.Json;

namespace RateShopperWPF.Services.FileIOService
{
    internal class FileIOService<T> where T: new()
    {
        private readonly string PATH;
        public FileIOService(string path)
        {
            PATH = path;
        }
        public T LoadSettings()
        {
            var fileExists = File.Exists(PATH);
            if (!fileExists)
            {
                File.CreateText(PATH).Dispose();
                return new T();
            }
            using (StreamReader reader = File.OpenText(PATH))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileText);
            }
        }
        public void SaveData(T file)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(file);
                writer.Write(output);
            }
        }
    }
}

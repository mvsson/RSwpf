using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using RateShopperWPF.ViewModels.UserSettings;

namespace RateShopperWPF.Services.FileIO
{
    internal class FileIOService
    {
        private readonly string PATH;
        public FileIOService(string path)
        {
            PATH = path;
        }
        public UserSettings LoadSettings()
        {
            var fileExists = File.Exists(PATH);
            if (!fileExists)
            {
                File.CreateText(PATH).Dispose();
                return new UserSettings()
                {
                    IsUseList = false,
                    IsShowChartLabels = true,
                    IsSoundOn = true,
                    ListLink = new ObservableCollection<HotelLinkSetter>()
                };
            }
            using (StreamReader reader = File.OpenText(PATH))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<UserSettings>(fileText);
            }
        }
        public void SaveData(UserSettings userSettings)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(userSettings);
                writer.Write(output);
            }
        }
    }
}

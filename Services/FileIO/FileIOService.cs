using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using RateShopperWPF.Models.InputModels;
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
        public UserSettingsViewModel LoadSettings()
        {
            var fileExists = File.Exists(PATH);
            if (!fileExists)
            {
                File.CreateText(PATH).Dispose();
                return new UserSettingsViewModel()
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
                return JsonConvert.DeserializeObject<UserSettingsViewModel>(fileText);
            }
        }
        public void SaveData(UserSettingsViewModel userSettings)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(userSettings);
                writer.Write(output);
            }
        }
    }
}

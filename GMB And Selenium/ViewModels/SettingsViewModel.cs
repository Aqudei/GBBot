using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using GMB_And_Selenium.Interfaces;

namespace GMB_And_Selenium.ViewModels
{
    class SettingsViewModel : Screen
    {
        private IPasswordProvider _passwordProvider;
        private string _username;

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }

        protected override void OnViewReady(object view)
        {
            _passwordProvider = view as IPasswordProvider;

            Username = Properties.Settings.Default.USERNAME;
        }

        public void OpenLogs()
        {
            if (Directory.Exists("logs"))
                Process.Start("explorer", "logs");
        }

        public void SaveSettings()
        {
            var password = _passwordProvider?.GetPassword();
            Properties.Settings.Default.PASSWORD = password ?? Properties.Settings.Default.PASSWORD;
            Properties.Settings.Default.USERNAME = Username;
            Properties.Settings.Default.Save();
        }


    }
}

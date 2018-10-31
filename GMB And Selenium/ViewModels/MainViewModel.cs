using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using GMB_And_Selenium.Views;

namespace GMB_And_Selenium.ViewModels
{
    class MainViewModel
    {
        private readonly IWindowManager _windowManager;

        public ProjectListViewModel ProjectList => IoC.Get<ProjectListViewModel>();

        public MainViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }
        public void OpenSettings()
        {
            _windowManager.ShowDialog(IoC.Get<SettingsViewModel>());
        }
    }
}

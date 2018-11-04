using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using GMB_And_Selenium.Bot;
using GMB_And_Selenium.Utilities;
using Microsoft.Win32;

namespace GMB_And_Selenium.ViewModels
{
    class ProjectListViewModel : Screen
    {
        private readonly CsvUtil _csvUtil;
        private readonly IMapper _mapper;
        public BindableCollection<ProjectItemViewModel> Projects { get; set; } = new BindableCollection<ProjectItemViewModel>();

        public ProjectListViewModel(CsvUtil csvUtil, IMapper _mapper)
        {
            _csvUtil = csvUtil;
            this._mapper = _mapper;
        }

        public async void ImportProjects()
        {
            var dlg = new OpenFileDialog { Multiselect = true, Filter = "CSV and Text files only|*.csv;*.txt" };
            var rslt = dlg.ShowDialog();
            if (!rslt.HasValue || !rslt.Value)
                return;
            foreach (var filename in dlg.FileNames)
            {
                var projectData = await _csvUtil.ReadAllAsync(filename);
                foreach (var data in projectData)
                {
                    var item = _mapper.Map<ProjectItemViewModel>(data);
                    item.ProjectData = data;
                    Projects.Add(item);
                }
            }

        }

        public void StopBot()
        {
        }

        public void StartBot()
        {
            foreach (var projectItemViewModel in Projects)
            {
                if (!projectItemViewModel.IsFitForBot())
                    continue;

                var bot = new ProjectBuilderBot(projectItemViewModel.ProjectData);
                bot.StartAsync();
                break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using Caliburn.Micro;
using GMB_And_Selenium.Bot;
using GMB_And_Selenium.Models;
using GMB_And_Selenium.ViewModels;
using Unity;

namespace GMB_And_Selenium
{
    class Bootstrapper : BootstrapperBase
    {
        private readonly IUnityContainer _container = new UnityContainer();
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void Configure()
        {
            _container.RegisterSingleton<IEventAggregator, EventAggregator>();
            _container.RegisterSingleton<IWindowManager, WindowManager>();
            _container.RegisterSingleton<PhoneNumberProvider>();
            
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProjectData, ProjectItemViewModel>().ReverseMap());
            _container.RegisterInstance(config.CreateMapper());

            base.Configure();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.ResolveAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.Resolve(service, key);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}


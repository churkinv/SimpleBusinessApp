﻿using Autofac;
using Prism.Events;
using SimpleBusinessApp.Data;
using SimpleBusinessApp.Data.Lookups;
using SimpleBusinessApp.Data.Repositories;
using SimpleBusinessApp.Data.Repository;
using SimpleBusinessApp.DataAccess;
using SimpleBusinessApp.View.Services;
using SimpleBusinessApp.ViewModel;

namespace SimpleBusinessApp.Startup
{
    /// <summary>
    /// Class is responsible for creating Autofac container 
    /// and used for creating instances
    /// </summary>
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance(); // with this we can add as IEventAggregator as Constructor parametr to our viewmodels to use EventAggregator

            builder.RegisterType<ClientOrganizerDbContext>().AsSelf(); // in this case context will be created every time it is requested. Registration of ClientDataServiseDbContext from UI

            builder.RegisterType<MainWindow>().AsSelf();

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<ClientDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(ClientDetailViewModel));
            builder.RegisterType<MeetingDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));
            builder.RegisterType<CompanyDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(CompanyDetailViewModel));

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();// we also could use IClientLookupDataService, but now LookupDataService will be injected all the func for that are implemented by the LookupDataService class
            builder.RegisterType<ClientRepository>().As<IClientRepository>(); // Container knows now when IClientDataService is required it will just create an instance of ClientDataService class
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();
            builder.RegisterType<CompanyRepository>().As<ICompanyRepository>();

            return builder.Build();
        }
    }
}

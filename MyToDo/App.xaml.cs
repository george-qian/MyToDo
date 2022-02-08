using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyToDo.Common;
using MyToDo.Context;
using MyToDo.Context.Repository;
using MyToDo.Service;
using MyToDo.ViewModels;
using MyToDo.ViewModels.Dialogs;
using MyToDo.Views;
using MyToDo.Views.Dialogs;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }
        public static void LoginOut(IContainerProvider containerProvider)
        {
            Current.MainWindow.Hide();
            var dialog = containerProvider.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result == ButtonResult.No)
                {
                    Environment.Exit(0);
                }
                Current.MainWindow.Show();
            });
        }
        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result == ButtonResult.No)
                {
                    Environment.Exit(0);
                }
            });
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if(service != null)
                service.Configure();
            base.OnInitialized();
        }
        protected override IContainerExtension CreateContainerExtension()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<MyToDoContext>(options =>
            {
                options.UseSqlite("Data Source=to.db");
            })
                .AddUnitOfWork<MyToDoContext>()
                .AddCustomRepository<ToDo, ToDoRepository>()
                .AddCustomRepository<Memo, MemoRepository>()
                .AddCustomRepository<User, UserRepository>();

            serviceCollection.AddTransient<IToDoService, ToDoService>();
            serviceCollection.AddTransient<IMemoService, MemoService>();
            serviceCollection.AddTransient<ILoginService, LoginService>();

            serviceCollection.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            return new DryIocContainerExtension(new Container(CreateContainerRules())
                .WithDependencyInjectionAdapter(serviceCollection));

        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.GetContainer()
            //    .Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            //containerRegistry.GetContainer()
            //    .RegisterInstance(@"http://localhost:5000/", serviceKey: "webUrl");
            //    //.RegisterInstance(@"http://localhost:5155/", serviceKey: "webUrl");
            //containerRegistry.Register<IToDoService,ToDoService>();
            //containerRegistry.Register<IMemoService, MemoService>();
            //containerRegistry.Register<ILoginService, LoginService>();

            containerRegistry.Register<IDialogHostService, DialogHostService>();

            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();

            containerRegistry.RegisterForNavigation<AddToDoView,AddToDoViewModel>();
            containerRegistry.RegisterForNavigation<AddMemoView,AddMemoViewModel>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
            containerRegistry.RegisterForNavigation<AboutView>();
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            containerRegistry.RegisterForNavigation<IndexView,IndexViewModel>();
            containerRegistry.RegisterForNavigation<MemoView,MemoViewModel>();
            containerRegistry.RegisterForNavigation<ToDoView,ToDoViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView,SettingsViewModel>();
        }
    }
}

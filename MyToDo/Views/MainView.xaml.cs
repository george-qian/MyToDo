using MyToDo.Common;
using MyToDo.Extensions;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyToDo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private readonly IDialogHostService dialogHostService;

        public MainView(IEventAggregator aggregator, IDialogHostService dialogHostService)
        {
            InitializeComponent();
            aggregator.SubscribeMessage(arg =>
            {
                Snackbar?.MessageQueue?.Enqueue(arg);
            });
            this.dialogHostService = dialogHostService;
            aggregator.Register(ArgIterator => 
            {
                DialogHost.IsOpen = ArgIterator.IsOpen;
                if (DialogHost.IsOpen)
                    DialogHost.DialogContent = new ProgressView();
            });

            menuBar.SelectionChanged += (o, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };
            btnMin.Click += (o, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (o, e) => {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };
            btnClose.Click += async (o, e) => {
                var dialogResult = await dialogHostService.Qusetion("温馨提示", "确认退出系统？");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                this.Close(); 
            };
            ColorZone.MouseDoubleClick += (o, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Maximized;
            };
            ColorZone.MouseMove += (o, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };

        }
    }
}

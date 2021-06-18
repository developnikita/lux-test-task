using DataConsumer.Helpers;
using DataConsumer.ViewModel;
using System;
using DataConsumer;
using System.Windows;

namespace DataConsumer.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView(MainWindowViewModel dataContext)
        {
            InitializeComponent();
            this.DataContext = dataContext;
            this.Show();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }
    }
}

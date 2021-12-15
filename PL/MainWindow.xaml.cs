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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL mybl;
        public MainWindow()
        {
            mybl = BlApi.BlFactory.GetBL();
            InitializeComponent();
        }

        private void btSkimmerListView_Click(object sender, RoutedEventArgs e)
        {
            new SkimmerListWindow(mybl).Show();
        }

        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btClintListView_Click(object sender, RoutedEventArgs e)
        {
            //new ClintListWindow(mybl).Show();
        }

        private void BaseStation_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationListWindow(mybl).Show();
        }
    }
}

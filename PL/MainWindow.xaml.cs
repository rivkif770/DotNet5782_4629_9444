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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL.IBL mybl;
        public MainWindow()
        {
           // mybl = new BL.BL();
            InitializeComponent();
        }

        private void btnShowListDrones_Click(object sender, RoutedEventArgs e)
        {
            DronesListWindow wnd = new DronesListWindow(mybl);
            wnd.Show();
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow wnd = new DroneWindow();
            bool? result = wnd.ShowDialog();
            if(result != null)
            {
            MessageBox.Show( wnd.Skimmer.ToString());
            }
           
        }
    }
}

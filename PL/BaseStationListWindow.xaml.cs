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

namespace PL
{
    /// <summary>
    /// Interaction logic for BaseStationListWindow.xaml
    /// </summary>
    public partial class BaseStationListWindow : Window
    {
        BlApi.IBL bL;
        BaseStationWindow baseStationWindow;
        public BaseStationListWindow()
        {
            bL = BlApi.BlFactory.GetBL();
            InitializeComponent();

            //BaseStationListView.ItemsSource = bl.GetBaseStationList();
            foreach (BO.BaseStationToList baseStationToList in bL.GetBaseStationList())
            {
                BaseStationListView.Items.Add(baseStationToList);
            }

            // DataContext = this;
        }
        private void RefreshListView(object ob, EventArgs ev)
        {
            BaseStationListView.Items.Refresh();
            BaseStationListView.Items.Clear();
            foreach (BO.BaseStationToList baseStationToList in bL.GetBaseStationList())
            {
                BaseStationListView.Items.Add(baseStationToList);
            }
        }
        private void btnAddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            baseStationWindow = new BaseStationWindow();
            baseStationWindow.Closed += RefreshListView;
            baseStationWindow.Show();
        }
        private void BaseStationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((BO.BaseStationToList)BaseStationListView.SelectedItem != null)
            {
                baseStationWindow = new BaseStationWindow((BO.BaseStationToList)BaseStationListView.SelectedItem, this);
                baseStationWindow.Closed += RefreshListView;
                baseStationWindow.Show();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (BaseStationListView.Items != null)
                BaseStationListView.Items.Clear();
            BaseStationListView.ItemsSource = bL.GetBaseStationList();
        }

        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void freeCharging_Click(object sender, RoutedEventArgs e)
        {
            BaseStationListView.ItemsSource = bL.GetBaseStationFreeCharging();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (BaseStationListView.Items != null)
                BaseStationListView.Items.Clear();
            var baseStations = bL.GetBaseStationList().GroupBy(b => b.FreeChargingstations);
            foreach (var group in baseStations)
            {
                foreach (BO.BaseStationToList baseStation in group)
                {
                    BaseStationListView.Items.Add(baseStation);
                }
            }
        }
    }
}

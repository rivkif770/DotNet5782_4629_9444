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
using System.Runtime.CompilerServices;

namespace PL
{
    /// <summary>
    /// Interaction logic for BaseStationListWindow.xaml
    /// </summary>
    public partial class BaseStationListWindow : Window
    {
        BlApi.IBL bL;
        BaseStationWindow baseStationWindow;
        /// <summary>
        /// constructor
        /// </summary>
        public BaseStationListWindow()
        {
            bL = BlApi.BlFactory.GetBL();
            InitializeComponent();

            foreach (BO.BaseStationToList baseStationToList in bL.GetBaseStationList())
            {
                BaseStationListView.Items.Add(baseStationToList);
            }
        }
        /// <summary>
        /// Refreshes the list whenever there is a change
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="ev"></param>
        private void RefreshListView(object ob, EventArgs ev)
        {
            BaseStationListView.Items.Refresh();
            BaseStationListView.Items.Clear();
            foreach (BO.BaseStationToList baseStationToList in bL.GetBaseStationList())
            {
                BaseStationListView.Items.Add(baseStationToList);
            }
        }
        /// <summary>
        /// Add button, opens a new window for adding a base station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            baseStationWindow = new BaseStationWindow();
            baseStationWindow.Closed += RefreshListView;
            baseStationWindow.Show();
        }
        /// <summary>
        /// An update window opens from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseStationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((BO.BaseStationToList)BaseStationListView.SelectedItem != null)
            {
                baseStationWindow = new BaseStationWindow((BO.BaseStationToList)BaseStationListView.SelectedItem, this);
                baseStationWindow.Closed += RefreshListView;
                baseStationWindow.Show();
            }
        }
        /// <summary>
        /// Cleans changes, returns the window to its normal state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (BaseStationListView.Items != null)
                BaseStationListView.Items.Clear();
            foreach (BO.BaseStationToList baseStationToList in bL.GetBaseStationList())
            {
                BaseStationListView.Items.Add(baseStationToList);
            }
        }
        /// <summary>
        /// Exit button from the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Displays the list of skimmers with available charging stations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void freeCharging_Click(object sender, RoutedEventArgs e)
        {
            if (BaseStationListView.Items != null)
                BaseStationListView.Items.Clear();
            foreach (BO.BaseStationToList baseStationToList in bL.GetBaseStationFreeCharging())
            {
                BaseStationListView.Items.Add(baseStationToList);
            }
        }
        /// <summary>
        /// List of groups by number of available charging stations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AvailableChargingStations_Checked(object sender, RoutedEventArgs e)
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
        /// <summary>
        /// When CheckBox is off, the list returns to normal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AvailableChargingStations_Unchecked(object sender, RoutedEventArgs e)
        {
            BaseStationListView.Items.Refresh();
            BaseStationListView.Items.Clear();
            foreach (BO.BaseStationToList baseStationToList in bL.GetBaseStationList())
            {
                BaseStationListView.Items.Add(baseStationToList);
            }
        }
    }
}

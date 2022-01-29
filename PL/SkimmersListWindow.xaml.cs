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
using BlApi;
using BO;
using System.Runtime.CompilerServices;

namespace PL
{
    /// <summary>
    /// Interaction logic for skimmerListWindow.xaml
    /// </summary>
    public partial class SkimmerListWindow : Window
    {
        BlApi.IBL bL;
        SkimmerWindow skimmerWindow;
        static Weight? weightFilter;
        /// <summary>
        /// constructor
        /// </summary>
        public SkimmerListWindow()
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBL();
            foreach (SkimmerToList skimmer in bL.GetSkimmerList())
            {
                SkimmerListView.Items.Add(skimmer);}
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.Weight));
        }
        /// <summary>
        /// Refreshes the list whenever there is a change
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="ev"></param>
        private void RefreshListView(object ob,EventArgs ev)
        {
            SkimmerListView.Items.Refresh();
            if (WeightSelector.SelectedItem == null)
                SkimmerListView.Items.Clear();
            foreach (SkimmerToList skimmer in bL.GetSkimmerList())
            {
                SkimmerListView.Items.Add(skimmer);
            }
            if (WeightSelector.SelectedItem != null) WeightSelector_SelectionChanged(this, null);
        }
        /// <summary>
        /// Sorts a list by weight selected by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SkimmerListView.Items.Clear();
            if (WeightSelector.SelectedIndex != -1)
            {
                weightFilter = (Weight)WeightSelector.SelectedItem;
                foreach (SkimmerToList skimmer in bL.GetSkimmerList())
                {
                    if (skimmer.WeightCategory == weightFilter)
                        SkimmerListView.Items.Add(skimmer);
                }
            }
            else
            {
               WeightSelector.SelectedIndex = -1;
               weightFilter = null;
            }
        }
        /// <summary>
        /// Sending by double-tapping a skimmer from the skimmer list to the Updates window and updating the change in the list afterwards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkimmerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if((BO.SkimmerToList)SkimmerListView.SelectedItem != null)
            {
                skimmerWindow = new SkimmerWindow((BO.SkimmerToList)SkimmerListView.SelectedItem);
                skimmerWindow.Closed += RefreshListView;
                skimmerWindow.backgroundWorker.ProgressChanged += RefreshListView;
                skimmerWindow.Show();             
            }
        }
        /// <summary>
        /// Submit by tapping the Add Skimmer button to the Add and Update the New Skimmer window in the list afterwards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSkimmer_Click(object sender, RoutedEventArgs e)
        {
            skimmerWindow = new SkimmerWindow();
            skimmerWindow.Closed += RefreshListView;
            skimmerWindow.Show();
        }
        /// <summary>
        /// Option to clear the filter of the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            WeightSelector.SelectedIndex = -1;
            if (SkimmerListView.Items != null)
                SkimmerListView.Items.Clear();
            foreach (SkimmerToList skimmer in bL.GetSkimmerList())
            {
                SkimmerListView.Items.Add(skimmer);
            }
            if (status.IsChecked == true) status.IsChecked = false;
        }
        /// <summary>
        /// Close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// List of groups by number of Skimmer Status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Status_Checked(object sender, RoutedEventArgs e)
        {
            if(SkimmerListView.Items != null)
                SkimmerListView.Items.Clear();
            var skimmers = bL.GetSkimmerList().GroupBy(s => s.SkimmerStatus);
            foreach (var group in skimmers)
            {
                foreach (SkimmerToList skimmer in group)
                {
                    SkimmerListView.Items.Add(skimmer);
                }
            }
        }
        /// <summary>
        /// When CheckBox of status is off, the list returns to normal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void status_Unchecked(object sender, RoutedEventArgs e)
        {
            SkimmerListView.Items.Clear();
            foreach (SkimmerToList skimmer in bL.GetSkimmerList())
            {
                SkimmerListView.Items.Add(skimmer);
            }
        }
    }
}

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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class SkimmerListWindow : Window
    {
        IBL.IBL bL;
        SkimmerWindow skimmerWindow;
        public SkimmerListWindow(IBL.IBL bl)
        {
            InitializeComponent();
            SkimmerListView.ItemsSource = bl.GetSkimmerList();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.SkimmerStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.Weight));
            bL = bl;
        }

        private void RefreshListView(object ob)
        {
            SkimmerListView.Items.Refresh();
            if (WeightSelector.SelectedItem == null && StatusSelector.SelectedItem == null) SkimmerListView.ItemsSource = bL.GetSkimmerList();
            if (WeightSelector.SelectedItem != null) WeightSelector_SelectionChanged(this, null);
            if (StatusSelector.SelectedItem != null) SkimmerListView_MouseDoubleClick(this, null);
        }
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SkimmerStatuses status = (SkimmerStatuses)StatusSelector.SelectedItem;
            SkimmerListView.ItemsSource = bL.GetSkimmerList(x => x.SkimmerStatus == status);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Weight weight = (Weight)WeightSelector.SelectedItem;
            SkimmerListView.ItemsSource = bL.GetSkimmerList(x => x.WeightCategory == weight);
        }

        private void SkimmerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if((IBL.BO.SkimmerToList)SkimmerListView.SelectedItem!=null)
            {
                skimmerWindow = new SkimmerWindow(bL, (IBL.BO.SkimmerToList)SkimmerListView.SelectedItem, this);
                skimmerWindow.CloseWindowEvent += RefreshListView;
                skimmerWindow.Show();             
            }
            //SkimmerListView.SelectedItem.Clear();
        }
        private void btnAddSkimmer_Click(object sender, RoutedEventArgs e)
        {
            skimmerWindow = new SkimmerWindow(bL);
            skimmerWindow.CloseWindowEvent += RefreshListView;
            skimmerWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SkimmerListView.ItemsSource = bL.GetSkimmerList();            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

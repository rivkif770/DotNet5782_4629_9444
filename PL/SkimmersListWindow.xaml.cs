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
        public SkimmerListWindow(IBL.IBL bl)
        {
            InitializeComponent();
            SkimmerListView.ItemsSource = bl.GetSkimmerList();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.SkimmerStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.Weight));
            bL = bl;
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
            SkimmerToList skimmer = new SkimmerToList();
            skimmer = (SkimmerToList)SkimmerListView.SelectedItem;
            new SkimmerWindow(bL, skimmer).ShowDialog();
           
        }
        private void btnAddSkimmer_Click(object sender, RoutedEventArgs e)
        {
            new SkimmerWindow(bL).Show();
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

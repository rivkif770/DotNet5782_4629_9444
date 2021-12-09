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
            //StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.SkimmerStatuses));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SkimmerStatuses status = (SkimmerStatuses)StatusSelector.SelectedItem;
            //this.txtTBD.Text = StatusSelector.SelectedItem.ToString();
            SkimmerListView.ItemsSource = bL.GetSkimmerList(x => x.SkimmerStatus == status);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Weight weight = (Weight)WeightSelector.SelectedItem;
            //this.txtTBd.Text = WeightSelector.SelectedItem.ToString();
            SkimmerListView.ItemsSource = bL.GetSkimmerList(x => x.WeightCategory == weight);
        }

        private void SkimmerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SkimmerToList skimmerToList = new SkimmerToList();
            skimmerToList = (SkimmerToList)SkimmerListView.SelectedItem;
            new SkimmerView(bL, skimmerToList).ShowDialog();
           
        }
        private void btnAddSkimmer_Click(object sender, RoutedEventArgs e)
        {
            new SkimmerWindow(bL).Show();
        }

        private void SkimmerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtTBD_TextChanged(object sender, TextChangedEventArgs e)
        {

        }




        //private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    SkimmerStatuses status = (SkimmerStatuses)comboStatusSelector.SelectedItem;
        //   // this.DronesListView.ItemsSource = fakelist.Where(x => x.SkimmerStatus == status);
        //}
    }
}

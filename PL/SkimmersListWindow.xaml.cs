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
            //this.SkimmerListView.ItemsSource = bL.GetSkimmerList();
            this.StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.SkimmerStatuses));
            this.WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.Weight));
            this.bL = bl;
            //StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.SkimmerStatuses));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SkimmerStatuses status = (SkimmerStatuses)StatusSelector.SelectedItem;
            this.txtTBD.Text = StatusSelector.SelectedItem.ToString();
            this.SkimmerListView.ItemsSource = bL.GetSkimmerList(x => x.SkimmerStatus == status);
        }

        private void SkimmerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.txtTBD.Text = "All skimmers";
            this.SkimmerListView.ItemsSource = bL.GetSkimmerList();
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Weight weight = (Weight)WeightSelector.SelectedItem;
            this.txtTBd.Text = WeightSelector.SelectedItem.ToString();
            this.SkimmerListView.ItemsSource = bL.GetSkimmerList(x => x.WeightCategory == weight);
        }

     

        //private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    SkimmerStatuses status = (SkimmerStatuses)comboStatusSelector.SelectedItem;
        //   // this.DronesListView.ItemsSource = fakelist.Where(x => x.SkimmerStatus == status);
        //}
    }
}

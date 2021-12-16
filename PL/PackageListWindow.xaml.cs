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
    /// Interaction logic for PackageListWindow.xaml
    /// </summary>
    public partial class PackageListWindow : Window
    {
        BlApi.IBL bL;
        PackageWindow packageWindow;
        public PackageListWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            PackageListView.ItemsSource = bl.GetPackageList();
            bL = bl;
            ComboBoxItem newItem = new ComboBoxItem();
            comboSelectorCustomer.Items.Add(newItem.Content = "Customer sends");
            comboSelectorCustomer.Items.Add(newItem.Content = "Customer receives");

            //foreach (BO.PackageToList item in bl.GetPackageList())
            //{
            //    ComboBoxItem newItem = new ComboBoxItem();
            //    newItem.Content = item.CustomerNameGets;
            //    GettingSelector.Items.Add(newItem);
            //}
            //foreach (BO.PackageToList item in bl.GetPackageList())
            //{
            //    ComboBoxItem newItem = new ComboBoxItem();
            //    newItem.Content = item.CustomerNameSends;
            //    SenderSelector.Items.Add(newItem);
            //}
        }
        private void RefreshListView(object ob, EventArgs ev)
        {
            PackageListView.Items.Refresh();
        }
        private void PackageListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((BO.PackageToList)PackageListView.SelectedItem != null)
            {
                //packageWindow = new PackageWindow(bL, (BO.PackageToList)PackageListView.SelectedItem, this);
                packageWindow.Closed += RefreshListView;
                packageWindow.Show();
            }
        }

        private void textSelectorCustomer_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textSelectorCustomer.Text;
            var bc = new BrushConverter();
            if (text != "" && char.IsLetter(text.ElementAt(0)))
            {
                textSelectorCustomer.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else
                textSelectorCustomer.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void btnSelectorCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (comboSelectorCustomer.SelectedItem == null)
            {
                MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (comboSelectorCustomer.SelectedItem.ToString() == "Customer sends")
                {
                    foreach (BO.PackageToList item in bL.GetPackageList())
                    {
                        if (item.CustomerNameSends == textSelectorCustomer.Text)
                        {
                            ListViewItem newItem = new ListViewItem();
                            newItem.Content = item;
                            PackageListView.Items.Add(newItem);
                        }
                    }
                }
                else
                {
                    foreach (BO.PackageToList item in bL.GetPackageList())
                    {
                        if (item.CustomerNameGets == textSelectorCustomer.Text)
                        {
                            ListViewItem newItem = new ListViewItem();
                            newItem.Content = item;
                            PackageListView.Items.Add(newItem);
                        }
                    }
                }
            }
        }

        private void butEXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void butClear_Click(object sender, RoutedEventArgs e)
        {
            PackageListView.ItemsSource = bL.GetPackageList();
        }
    }
}

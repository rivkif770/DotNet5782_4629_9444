using BO;
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
        static Weight? weightFilter;
        static Priority? PriorityFilter;
        static ParcelStatus? statusesFilter;
        public PackageListWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            PackageListView.ItemsSource = bl.GetPackageList();
            bL = bl;
            ComboBoxItem newItem = new ComboBoxItem();
            comboSelectorCustomer.Items.Add(newItem.Content = "Customer sends");
            comboSelectorCustomer.Items.Add(newItem.Content = "Customer receives");
            comboWeight.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            comboPriority.ItemsSource = Enum.GetValues(typeof(BO.Priority));
            comboStatus.ItemsSource = Enum.GetValues(typeof(BO.ParcelStatus));
        }
        private void RefreshListView(object ob, EventArgs ev)
        {
            PackageListView.Items.Refresh();
        }
        private void PackageListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((BO.PackageToList)PackageListView.SelectedItem != null)
            {
                packageWindow = new PackageWindow(bL, (BO.PackageToList)PackageListView.SelectedItem, this);
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
            comboWeight.SelectedIndex = -1;
            comboStatus.SelectedIndex = -1;
            comboPriority.SelectedIndex = -1;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            packageWindow = new PackageWindow(bL);
            packageWindow.Closed += RefreshListView;
            packageWindow.Show();
        }

        private void comboWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboWeight.SelectedIndex != -1)
            {
                weightFilter = (Weight)comboWeight.SelectedItem;
                if (statusesFilter == null && PriorityFilter == null)
                {
                    PackageListView.ItemsSource = bL.GetPackageList(x => x.WeightCategory == weightFilter);
                }
                else
                {
                    if(statusesFilter == null && PriorityFilter != null)
                    {
                        PackageListView.ItemsSource = bL.GetPackageList(x => x.WeightCategory == weightFilter && x.priority == PriorityFilter);
                    }
                    else
                    {
                        if (statusesFilter != null && PriorityFilter == null)
                        {
                            PackageListView.ItemsSource = bL.GetPackageList(x => x.WeightCategory == weightFilter && x.PackageMode == statusesFilter);
                        }
                        else
                            PackageListView.ItemsSource = bL.GetPackageList(x => x.WeightCategory == weightFilter && x.PackageMode == statusesFilter && x.priority == PriorityFilter);

                    }
                }
            }
            else
            {
                comboWeight.SelectedIndex = -1;
                weightFilter = null;
                comboWeight.Text = "Choose a Weight";
            }
        }

        private void comboPriority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboPriority.SelectedIndex != -1)
            {
                PriorityFilter = (Priority)comboPriority.SelectedItem;
                if (statusesFilter == null && weightFilter == null)
                {
                    PackageListView.ItemsSource = bL.GetPackageList(x => x.priority == PriorityFilter);
                }
                else
                {
                    if (statusesFilter == null && weightFilter != null)
                    {
                        PackageListView.ItemsSource = bL.GetPackageList(x => x.priority == PriorityFilter && x.WeightCategory == weightFilter);
                    }
                    else
                    {
                        if (statusesFilter != null && weightFilter == null)
                        {
                            PackageListView.ItemsSource = bL.GetPackageList(x => x.priority == PriorityFilter && x.PackageMode == statusesFilter);
                        }
                        else
                            PackageListView.ItemsSource = bL.GetPackageList(x => x.priority == PriorityFilter && x.PackageMode == statusesFilter && x.WeightCategory == weightFilter);

                    }
                }
            }
            else
            {
                comboPriority.SelectedIndex = -1;
                PriorityFilter = null;
                comboPriority.Text = "Choose a Priority";
            }
        }

        private void comboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboStatus.SelectedIndex != -1)
            {
                statusesFilter = (ParcelStatus)comboStatus.SelectedItem;
                if (PriorityFilter == null && weightFilter == null)
                {
                    PackageListView.ItemsSource = bL.GetPackageList(x => x.PackageMode == statusesFilter);
                }
                else
                {
                    if (PriorityFilter == null && weightFilter != null)
                    {
                        PackageListView.ItemsSource = bL.GetPackageList(x => x.PackageMode == statusesFilter && x.WeightCategory == weightFilter);
                    }
                    else
                    {
                        if (PriorityFilter != null && weightFilter == null)
                        {
                            PackageListView.ItemsSource = bL.GetPackageList(x => x.PackageMode == statusesFilter && x.priority == PriorityFilter);
                        }
                        else
                            PackageListView.ItemsSource = bL.GetPackageList(x => x.PackageMode == statusesFilter && x.priority == PriorityFilter && x.WeightCategory == weightFilter);

                    }
                }
            }
            else
            {
                comboPriority.SelectedIndex = -1;
                PriorityFilter = null;
                comboPriority.Text = "Choose a Priority";
            }
        }
    }
}

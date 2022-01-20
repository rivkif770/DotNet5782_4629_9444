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
using System.Runtime.CompilerServices;

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
        public PackageListWindow()
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBL();
            foreach (BO.PackageToList packageToList in bL.GetPackageList())
            {
                PackageListView.Items.Add(packageToList);
            }
            ComboBoxItem newItem = new ComboBoxItem();
            comboSelectorCustomer.Items.Add(newItem.Content = "Customer sends");
            comboSelectorCustomer.Items.Add(newItem.Content = "Customer receives");
            comboWeight.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            comboPriority.ItemsSource = Enum.GetValues(typeof(BO.Priority));
            comboStatus.ItemsSource = Enum.GetValues(typeof(BO.ParcelStatus));
            DataContext = this;
        }
        private void RefreshListView(object ob, EventArgs ev)
        {
            PackageListView.Items.Refresh();
            if (comboWeight.SelectedItem == null && comboStatus.SelectedItem == null && comboPriority.SelectedItem == null)
            {
                PackageListView.Items.Clear();
                foreach (BO.PackageToList packageToList in bL.GetPackageList())
                {
                    PackageListView.Items.Add(packageToList);
                }
            }
            if (comboWeight.SelectedItem != null) comboWeight_SelectionChanged(this, null);
            if (comboStatus.SelectedItem != null) comboStatus_SelectionChanged(this, null);
            if (comboPriority.SelectedItem != null) comboPriority_SelectionChanged(this, null);
        }

        private void PackageListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string text = textSelectorCustomer.Text;
            var bc = new BrushConverter();
            if (text != "" && char.IsLetter(text.ElementAt(0)))
            {
                textSelectorCustomer.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else
                textSelectorCustomer.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
            if ((BO.PackageToList)PackageListView.SelectedItem != null)
            {
                packageWindow = new PackageWindow((BO.PackageToList)PackageListView.SelectedItem);
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
                if (geter.IsChecked == true) geter.IsChecked = false;
                else send.IsChecked = false;
                if (comboSelectorCustomer.SelectedItem.ToString() == "Customer sends")
                {
                    PackageListView.Items.Clear();
                    foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.CustomerNameSends == textSelectorCustomer.Text))
                    {
                        PackageListView.Items.Add(packageToList);
                    }
                    //PackageListView.ItemsSource = bL.GetPackageList(x => x.CustomerNameSends == textSelectorCustomer.Text);
                }
                else
                {
                    PackageListView.Items.Clear();
                    foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.CustomerNameGets == textSelectorCustomer.Text))
                    {
                        PackageListView.Items.Add(packageToList);
                    }
                    //PackageListView.ItemsSource = bL.GetPackageList(x => x.CustomerNameGets == textSelectorCustomer.Text);
                }
            }
        }

        private void butEXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void butClear_Click(object sender, RoutedEventArgs e)
        {
            comboWeight.SelectedIndex = -1;
            comboStatus.SelectedIndex = -1;
            comboPriority.SelectedIndex = -1;
            PackageListView.Items.Clear();
            foreach (BO.PackageToList packageToList in bL.GetPackageList())
            {
                PackageListView.Items.Add(packageToList);
            }
            comboSelectorCustomer.SelectedIndex = -1;
            if (geter.IsChecked == true) geter.IsChecked = false;
            else send.IsChecked = false;
            textSelectorCustomer.Clear();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            packageWindow = new PackageWindow();
            packageWindow.Closed += RefreshListView;
            packageWindow.Show();
        }

        private void comboWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (geter.IsChecked == true) geter.IsChecked = false;
            else send.IsChecked = false;
            if (PackageListView.Items.Count != 0)
            {
                PackageListView.Items.Clear();
            }
            if (comboWeight.SelectedIndex != -1)
            {
                weightFilter = (Weight)comboWeight.SelectedItem;
                if (statusesFilter == null && PriorityFilter == null)
                {
                    foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.WeightCategory == weightFilter))
                    {
                        PackageListView.Items.Add(packageToList);
                    }
                   // PackageListView.ItemsSource = bL.GetPackageList(x => x.WeightCategory == weightFilter);
                }
                else
                {
                    if(statusesFilter == null && PriorityFilter != null)
                    {
                        foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.WeightCategory == weightFilter && x.priority == PriorityFilter))
                        {
                            PackageListView.Items.Add(packageToList);
                        }
                        //PackageListView.ItemsSource = bL.GetPackageList(x => x.WeightCategory == weightFilter && x.priority == PriorityFilter);
                    }
                    else
                    {
                        if (statusesFilter != null && PriorityFilter == null)
                        {
                            foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.WeightCategory == weightFilter && x.PackageMode == statusesFilter))
                            {
                                PackageListView.Items.Add(packageToList);
                            }
                            //PackageListView.ItemsSource = bL.GetPackageList(x => x.WeightCategory == weightFilter && x.PackageMode == statusesFilter);
                        }
                        else
                        {
                            foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.WeightCategory == weightFilter && x.PackageMode == statusesFilter && x.priority == PriorityFilter))
                            {
                                PackageListView.Items.Add(packageToList);
                            }
                        }
                        //PackageListView.ItemsSource = bL.GetPackageList(x => x.WeightCategory == weightFilter && x.PackageMode == statusesFilter && x.priority == PriorityFilter);

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
            if (geter.IsChecked == true) geter.IsChecked = false;
            else send.IsChecked = false;
            if (PackageListView.Items!=null)
               PackageListView.Items.Clear();
            if (comboPriority.SelectedIndex != -1)
            {
                PriorityFilter = (Priority)comboPriority.SelectedItem;
                if (statusesFilter == null && weightFilter == null)
                {
                    foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.priority == PriorityFilter))
                    {
                        PackageListView.Items.Add(packageToList);
                    }
                    //PackageListView.ItemsSource = bL.GetPackageList(x => x.priority == PriorityFilter);
                }
                else
                {
                    if (statusesFilter == null && weightFilter != null)
                    {
                        foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.priority == PriorityFilter && x.WeightCategory == weightFilter))
                        {
                            PackageListView.Items.Add(packageToList);
                        }
                        //PackageListView.ItemsSource = bL.GetPackageList(x => x.priority == PriorityFilter && x.WeightCategory == weightFilter);
                    }
                    else
                    {
                        if (statusesFilter != null && weightFilter == null)
                        {
                            foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.priority == PriorityFilter && x.PackageMode == statusesFilter))
                            {
                                PackageListView.Items.Add(packageToList);
                            }
                            //PackageListView.ItemsSource = bL.GetPackageList(x => x.priority == PriorityFilter && x.PackageMode == statusesFilter);
                        }
                        else
                        {
                            foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.priority == PriorityFilter && x.PackageMode == statusesFilter && x.WeightCategory == weightFilter))
                            {
                                PackageListView.Items.Add(packageToList);
                            }
                        }
                            //PackageListView.ItemsSource = bL.GetPackageList(x => x.priority == PriorityFilter && x.PackageMode == statusesFilter && x.WeightCategory == weightFilter);

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
            if (geter.IsChecked == true) geter.IsChecked = false;
            else send.IsChecked = false;
            if (PackageListView.Items != null)
                PackageListView.Items.Clear();
            if (comboStatus.SelectedIndex != -1)
            {
                statusesFilter = (ParcelStatus)comboStatus.SelectedItem;
                if (PriorityFilter == null && weightFilter == null)
                {
                    foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.PackageMode == statusesFilter))
                    {
                        PackageListView.Items.Add(packageToList);
                    }
                    //PackageListView.ItemsSource = bL.GetPackageList(x => x.PackageMode == statusesFilter);
                }
                else
                {
                    if (PriorityFilter == null && weightFilter != null)
                    {
                        foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.PackageMode == statusesFilter && x.WeightCategory == weightFilter))
                        {
                            PackageListView.Items.Add(packageToList);
                        }
                        //PackageListView.ItemsSource = bL.GetPackageList(x => x.PackageMode == statusesFilter && x.WeightCategory == weightFilter);
                    }
                    else
                    {
                        if (PriorityFilter != null && weightFilter == null)
                        {
                            foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.PackageMode == statusesFilter && x.priority == PriorityFilter))
                            {
                                PackageListView.Items.Add(packageToList);
                            }
                            //PackageListView.ItemsSource = bL.GetPackageList(x => x.PackageMode == statusesFilter && x.priority == PriorityFilter);
                        }
                        else
                        {
                            foreach (BO.PackageToList packageToList in bL.GetPackageList(x => x.PackageMode == statusesFilter && x.priority == PriorityFilter && x.WeightCategory == weightFilter))
                            {
                                PackageListView.Items.Add(packageToList);
                            }
                        }
                            //PackageListView.ItemsSource = bL.GetPackageList(x => x.PackageMode == statusesFilter && x.priority == PriorityFilter && x.WeightCategory == weightFilter);

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

        private void comboSelectorCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboSelectorCustomer.SelectedIndex == -1)
            {
                comboSelectorCustomer.SelectedIndex = -1;
                comboSelectorCustomer.Text = "Choose a Priority";
            }
        }

        private void Geter_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (PackageListView.Items != null)
                PackageListView.Items.Clear();
            var Packages = bL.GetPackageList().GroupBy(b => b.CustomerNameGets);
            foreach (var group in Packages)
            {
                foreach (BO.PackageToList Package in group)
                {
                    PackageListView.Items.Add(Package);
                }
            }
        }

        private void Sender_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (PackageListView.Items != null)
                PackageListView.Items.Clear();
            var Packages = bL.GetPackageList().GroupBy(b => b.CustomerNameSends);
            foreach (var group in Packages)
            {
                foreach (BO.PackageToList Package in group)
                {
                    PackageListView.Items.Add(Package);
                }
            }
        }

        private void GeterAndSender_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PackageListView.Items.Refresh();
            PackageListView.Items.Clear();
            foreach (BO.PackageToList packageToList in bL.GetPackageList())
            {
                PackageListView.Items.Add(packageToList);
            }
        }
    }
}

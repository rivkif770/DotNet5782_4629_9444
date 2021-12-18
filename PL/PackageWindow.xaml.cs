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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for PackageWindow.xaml
    /// </summary>
    public partial class PackageWindow : Window
    {
        PackageToList newPackage;
        BlApi.IBL bL;
        public delegate void CloseWindow(object ob);
        //public event CloseWindow CloseWindowEvent;
        /// <summary>
        /// Builder to add
        /// </summary>
        /// <param name="bl"></param>
        public PackageWindow(BlApi.IBL bl)
        {

            bL = bl;
            InitializeComponent();
            add.Visibility = Visibility.Visible;
            ComboWeightCategory.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            ComboPrioritys.ItemsSource = Enum.GetValues(typeof(BO.Priority));
        }
        /// <summary>
        /// Builder for update
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="skimmerToList"></param>
        /// <param name="skimmerListWindow"></param>
        public PackageWindow(BlApi.IBL bl, PackageToList packageToList, PackageListWindow packageListWindow)
        {
            bL = bl;
            InitializeComponent();
            Updates.Visibility = Visibility.Visible;
            deletePackage.Visibility = Visibility.Visible;
            newPackage = new PackageToList();
            newPackage = packageToList;
            showPackage.Text = bl.GetPackage(newPackage.Id).ToString();
        }
        public PackageWindow(BlApi.IBL bl, PackageToList packageToList)
        {
            bL = bl;
            InitializeComponent();
            Updates.Visibility = Visibility.Visible;
            newPackage = new PackageToList();
            newPackage = packageToList;
            showPackage.Text = bl.GetPackage(newPackage.Id).ToString();
        }
        /// <summary>
        /// Button attempt to add skimmer-checks whether all the required fields are filled correctly and sends to try to add in bl, updates the new skimmer, sends a suitable message and closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPackage_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (ComboWeightCategory.SelectedItem == null || ComboPrioritys.SelectedItem == null || SolidColorBrush.Equals(((SolidColorBrush)textIdSender.BorderBrush).Color, red.Color) || SolidColorBrush.Equals(((SolidColorBrush)textIdGet.BorderBrush).Color, red.Color))
            {
                MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    BO.Package package = new Package();
                    package.SendPackage = bL.GetCustomerInParcel(Int32.Parse(textIdSender.Text));
                    package.ReceivesPackage = bL.GetCustomerInParcel(Int32.Parse(textIdGet.Text));
                    package.priority = (Priority)(ComboPrioritys.SelectedItem);
                    package.WeightCategory = (Weight)(ComboWeightCategory.SelectedItem);
                    bL.AddPackage(package);
                    MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                    //CloseWindowEvent(this);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        /// <summary>
        /// Input the id from the user and color the field according to the correctness of the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textIdSender_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textIdSender.Text.All(char.IsDigit) && textIdSender.Text.Length ==9)
            {

                textIdSender.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textIdSender.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

        }
        private void textIdGet_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textIdGet.Text.All(char.IsDigit) && textIdGet.Text.Length == 9)
            {

                textIdGet.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textIdGet.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

        }
        /// <summary>
        /// Input the Skimmer Model from the user and color the field according to the correctness of the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        /// <summary>
        /// Button for closing the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_UpdateDelete(object sender, RoutedEventArgs e)
        {
            bL.DeletePackage(newPackage.Id);
            MessageBox.Show("The deletion was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

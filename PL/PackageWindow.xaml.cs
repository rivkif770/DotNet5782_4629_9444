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
using System.Runtime.CompilerServices;

namespace PL
{
    /// <summary>
    /// Interaction logic for PackageWindow.xaml
    /// </summary>
    public partial class PackageWindow : Window
    {
        PackageToList newPackage;
        Package package1 = new Package();
        BlApi.IBL bL;
        private SkimmerWindow skimmerWindow;
        /// <summary>
        /// Builder to add
        /// </summary>
        /// <param name="bl"></param>
        public PackageWindow()
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBL();
            ComboWeightCategory.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            ComboPrioritys.ItemsSource = Enum.GetValues(typeof(BO.Priority));
            DataContext = package1;
        }
        /// <summary>
        /// Builder for update
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="skimmerToList"></param>
        /// <param name="skimmerListWindow"></param>
        public PackageWindow(PackageToList packageToList)
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBL();
            ComboWeightCategory.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            ComboPrioritys.ItemsSource = Enum.GetValues(typeof(BO.Priority));
            newPackage = new PackageToList();
            package1.ReceivesPackage = new CustomerInParcel();
            package1.SendPackage = new CustomerInParcel();
            DataContext = bL.GetPackage(packageToList.Id);
            package1 = bL.GetPackage(packageToList.Id);
            help.IsChecked = true;
        }
        /// <summary>
        /// Button for adding a package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPackage_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (ComboWeightCategory.SelectedItem == null || ComboPrioritys.SelectedItem == null || SolidColorBrush.Equals(((SolidColorBrush)textIdSender.BorderBrush).Color, red.Color) || SolidColorBrush.Equals(((SolidColorBrush)textIdGet.BorderBrush).Color, red.Color) || SolidColorBrush.Equals(((SolidColorBrush)textIdSender.BorderBrush).Color, red.Color))
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        /// <summary>
        /// To test and color the TextBox of the IdSender
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
        /// <summary>
        /// To test and color the TextBox of the IdGet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Button for closing the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Deleting a package from the repository
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeletePackage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (package1.AssignmentTime == null)
                {
                    bL.DeletePackage(package1.Id);
                    MessageBox.Show("The deletion was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show($"This package could not be deleted", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        /// <summary>
        /// Displays a skimmer that sends the package (if the package is associated)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkimmerInPackage_Click(object sender, RoutedEventArgs e)
        {
            if (package1.SkimmerInPackage != null)
            {
                SkimmerToList skimmer = new SkimmerToList();
                skimmer.Id = package1.SkimmerInPackage.Id;
                skimmerWindow = new SkimmerWindow(skimmer);
                skimmerWindow.Show();
            }
            else
            {
                MessageBox.Show("There is no associated skimmer", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// To test and color the TextBox of the id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textId_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textId.Text !="" && textId.Text.All(char.IsDigit) && textId.Text.Length == 4)
            {

                textId.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textId.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        /// <summary>
        /// To test and color the TextBox of the Creation Time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textPackageCreationTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textPackageCreationTime.Text != "")
            {

                textPackageCreationTime.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textPackageCreationTime.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        /// <summary>
        /// To test and color the TextBox of the Assignment Time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textAssignmentTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textAssignmentTime.Text != "")
            {

                textAssignmentTime.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textAssignmentTime.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        /// <summary>
        /// To test and color the TextBox of the Collection Time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textCollectionTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textCollectionTime.Text != "")
            {

                textCollectionTime.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textCollectionTime.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        /// <summary>
        /// To test and color the TextBox of the Supply Time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textSupplyTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textSupplyTime.Text != "")
            {

                textSupplyTime.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textSupplyTime.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
    }
}

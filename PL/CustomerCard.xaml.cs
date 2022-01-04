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
    /// Interaction logic for CustomerCard.xaml
    /// </summary>
    public partial class CustomerCard : Window
    {
        Package package1 = new Package();
        
        CustomerToList newCustomer = new CustomerToList();
        BlApi.IBL bL;
        BO.Customer customer = new BO.Customer();
        public CustomerCard( Customer customer1)
        {
            bL = BlApi.BlFactory.GetBL();
            InitializeComponent();
            customer = customer1;
            //DataContext = customer;
            package1.SendPackage = new CustomerInParcel();
            package1.ReceivesPackage = new CustomerInParcel();

            ComboWeightCategory.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            ComboPrioritys.ItemsSource = Enum.GetValues(typeof(BO.Priority));
            //DataContext = customer;
            PackageShippedListView.ItemsSource = bL.GetListOfPackageShipped(customer);
            PackageReceivedListView.ItemsSource = bL.GetListOfPackageReceived(customer);
            help.IsChecked = true;
            //DataContext = this;
           
            package1.SendPackage.Id = customer.Id;
            package1.SendPackage.Name = customer.Name;
            DataContext = package1;
        }
        private void btnAddPackage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.AddPackage(package1);
                MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                package1 = new Package();

            }
            catch (Exception ex)
            { 
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //    private void buttonDeletPackage_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        private void textIdGet_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textIdGet.Text.All(char.IsDigit) && textIdGet.Text.Length == 9)
            {

                textIdGet.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textIdGet.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonDeletPackage_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.DeletePackage(((BO.Package)PackageShippedListView.SelectedItems).Id);
                MessageBox.Show("package successfully deleted !", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex )
            {
                MessageBox.Show($"Can not delete package {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //private void buttonDeletPackage_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        help.
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //private void buttonDeletPackage_Click(object sender, RoutedEventArgs e)
        //{
        //    BO.Package package = bL.GetPackage(newPackage.Id);
        //    if (package.AssignmentTime == null)
        //    {
        //        bL.DeletePackage(newPackage.Id);
        //        MessageBox.Show("The deletion was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
        //        this.Close();
        //    }
        //    else
        //        MessageBox.Show("The package was associated", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
    }
}

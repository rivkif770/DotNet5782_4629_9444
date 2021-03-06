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
    /// Interaction logic for CustomerCard.xaml
    /// </summary>
    public partial class CustomerCard : Window
    {
        Package package1 = new Package();    
        CustomerToList newCustomer = new CustomerToList();
        BlApi.IBL bL;
        BO.Customer customer = new BO.Customer();
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="customer1"></param>
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
            PackageShippedListView.ItemsSource = bL.GetListOfPackageShipped(customer.Id);
            PackageReceivedListView.ItemsSource = bL.GetListOfPackageReceived(customer.Id);
            help.IsChecked = true;
            //DataContext = this;
           
            package1.SendPackage.Id = customer.Id;
            package1.SendPackage.Name = customer.Name;
            DataContext = package1;
        }
        /// <summary>
        /// Button for adding a new package to this customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPackage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.AddPackage(package1);
                MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                package1 = new Package();
                PackageShippedListView.ItemsSource = bL.GetListOfPackageShipped(customer.Id);
            }
            catch (Exception ex)
            { 
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Checking and coloring the textbox of the ID of the receiving customer
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
        /// Button for deleting a package (only if this customer sent it and it is not yet associated with the glider)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeletPackage_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.DeletePackage(((BO.PackageToList)PackageShippedListView.SelectedItem).Id);
                MessageBox.Show("package successfully deleted !", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                PackageShippedListView.ItemsSource= bL.GetListOfPackageShipped(customer.Id);
            }
            catch (Exception ex )
            {
                MessageBox.Show($"Can not delete package {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Package collection confirmation (only packages sent from this customer and they were associated)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfimCollection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = bL.GetPackage(((BO.PackageToList)PackageShippedListView.SelectedItem).Id).SkimmerInPackage.Id;
                bL.CollectingPackageBySkimmer(id);
                MessageBox.Show("The package collection confirmation was successful !", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                PackageShippedListView.ItemsSource = bL.GetListOfPackageShipped(customer.Id);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Can not For confirmation of package collection {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ;
            }
        }
        /// <summary>
        /// Confirmation of delivery of the package (only packages received by this customer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfimDelivery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = bL.GetPackage(((BO.PackageToList)PackageReceivedListView.SelectedItem).Id).SkimmerInPackage.Id;
                //bL.DeliveryOfPackageBySkimmer(((BO.PackageToList)PackageShippedListView.SelectedItem).Id);
                bL.DeliveryOfPackageBySkimmer(id);
                MessageBox.Show("The package was delivered successfully !", "S  uccess", MessageBoxButton.OK, MessageBoxImage.Information);
                PackageReceivedListView.ItemsSource = bL.GetListOfPackageReceived(customer.Id);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Can no Unable to deliver package {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ;
            }
        }
    }
}

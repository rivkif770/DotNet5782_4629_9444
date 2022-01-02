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
    /// Interaction logic for CustomerCard.xaml
    /// </summary>
    public partial class CustomerCard : Window
    {
        BlApi.IBL bL;
        BO.Customer customer = new BO.Customer();
        public CustomerCard()
        {
            InitializeComponent();
            customer = (BO.Customer)DataContext;
            PackageShippedListView.ItemsSource = bL.GetListOfPackageShipped(customer);
            PackageReceivedListView.ItemsSource = bL.GetListOfPackageReceived(customer);
        }

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

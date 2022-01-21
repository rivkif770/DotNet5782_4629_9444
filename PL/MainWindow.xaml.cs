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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
using BO;
using System.Runtime.CompilerServices;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlApi.IBL mybl;
        /// <summary>
        /// A builder who opens an entrance window
        /// </summary>
        public MainWindow()
        {
            mybl = BlApi.BlFactory.GetBL();
            InitializeComponent();
        }
        /// <summary>
        /// A builder that opens an administrator window
        /// </summary>
        /// <param name="mybL"></param>
        public MainWindow(BlApi.IBL mybL)
        {
            mybl = mybL;
            InitializeComponent();
            help.IsChecked = true;
        }
        /// <summary>
        /// Opening a Skimmer window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSkimmerListView_Click(object sender, RoutedEventArgs e)
        {
            new SkimmerListWindow().Show();
        }
        /// <summary>
        /// Exit this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Opening a Customer window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCustomerListView_Click(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow().Show();
        }
        /// <summary>
        /// Opening a base station window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseStation_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationListWindow().Show();
        }
        /// <summary>
        /// Opening a Package window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Package_Click(object sender, RoutedEventArgs e)
        {
            new PackageListWindow().Show();
        }
        /// <summary>
        /// Open an administrator window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void manager_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(mybl).Show();
        }
        /// <summary>
        /// Opening a New Customer window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewCustomer_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow().Show();
        }
        /// <summary>
        /// Login as an existing customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer customer = mybl.GetCustomerListID(Int32.Parse(textPassword.Text), textName.Text);
                new CustomerCard( customer).Show();
                textName.Text = "";
                textPassword.Text = "";
                return;
            }
            catch (Exception )
            {

                 MessageBox.Show("The password is incorrect", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Testing and coloring the TextBox of the Password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textPassword.Text.All(char.IsDigit) && textPassword.Text.Length == 9)
            {
                textPassword.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textPassword.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        /// <summary>
        /// Testing and coloring the TextBox of the Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textName.Text;
            var bc = new BrushConverter();
            if (text != "" && char.IsLetter(text.ElementAt(0)))
            {
                textName.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else
                textName.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
    }
}

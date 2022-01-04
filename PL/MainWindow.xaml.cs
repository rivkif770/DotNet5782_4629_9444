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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlApi.IBL mybl;
        public MainWindow()
        {
            mybl = BlApi.BlFactory.GetBL();
            InitializeComponent();
        }
        public MainWindow(BlApi.IBL mybL)
        {
            mybl = mybL;
            InitializeComponent();
            help.IsChecked = true;
        }

        private void btSkimmerListView_Click(object sender, RoutedEventArgs e)
        {
            new SkimmerListWindow(mybl).Show();
        }

        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btCustomerListView_Click(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(mybl).Show();
        }

        private void BaseStation_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationListWindow(mybl).Show();
        }

        private void Package_Click(object sender, RoutedEventArgs e)
        {
            new PackageListWindow(mybl).Show();
        }

        private void manager_Click(object sender, RoutedEventArgs e)
        {
            if(textPasswordM.Text== "123456789")
            {
                new MainWindow(mybl).Show();
            }
            else MessageBox.Show("The password is incorrect", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void textPasswordM_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textPasswordM.Text.All(char.IsDigit) && textPasswordM.Text.Length == 9)
            {

                textPasswordM.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textPasswordM.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void NewCustomer_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(mybl).Show();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer customer = mybl.GetCustomerListID(Int32.Parse(textPassword.Text), textName.Text);
                new CustomerCard( customer).Show();
                return;
            }
            catch (Exception ex)
            {

                 MessageBox.Show("The password is incorrect", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //private void textName_TextChanged(object sender, RoutedEventArgs e)
        //{
        //    string text = textName.Text;
        //    var bc = new BrushConverter();
        //    if (text != "" && char.IsLetter(text.ElementAt(0)))
        //    {
        //        textName.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
        //    }
        //    else
        //        textName.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        //}
        private void textPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textPassword.Text.All(char.IsDigit) && textPassword.Text.Length == 9)
            {

                textPassword.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textPassword.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

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

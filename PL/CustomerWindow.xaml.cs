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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        CustomerToList newCustomer = new CustomerToList();
        Customer customer = new Customer();
        BlApi.IBL bL;
        private PackageWindow PackageWindow;

        public delegate void CloseWindow(object ob);
        //public event CloseWindow CloseWindowEvent;
        //CustomerWindow customerWindow;
        /// <summary>
        /// Builder to add
        /// </summary>
        /// <param name="bl"></param>
        public CustomerWindow()
        {
            bL = BlApi.BlFactory.GetBL();
            InitializeComponent();
            //add.Visibility = Visibility.Visible;
            // customer = (Customer)DataContext;
            customer.Location = new Location();
            DataContext = customer;

        }
        /// <summary>
        /// Builder for update
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="CustomerToList"></param>
        /// <param name="CustomerListWindow"></param>
        public CustomerWindow(CustomerToList customerToList)
        {
            bL = BlApi.BlFactory.GetBL();
            InitializeComponent();
            DataContext = bL.GetCustomer(customerToList.Id);
            help.IsChecked = true;
            newCustomer = customerToList;
            customer = bL.GetCustomer(newCustomer.Id);
            PackgeListOfSenderView.ItemsSource = bL.GetSentParcels(customer);
            PackgeListOfGetView.ItemsSource = bL.GetReceiveParcels(customer);

        }
        /// <summary>
        /// Button attempt to add skimmer-checks whether all the required fields are filled correctly and sends to try to add in bl, updates the new skimmer, sends a suitable message and closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.AddCustomer(customer);
                MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                customer = new Customer();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            //if (SolidColorBrush.Equals(((SolidColorBrush)textlongitude.BorderBrush).Color, red.Color) || SolidColorBrush.Equals(((SolidColorBrush)textLatitude.BorderBrush).Color, red.Color) || SolidColorBrush.Equals(((SolidColorBrush)textCustomerPhone.BorderBrush).Color, red.Color) || SolidColorBrush.Equals(((SolidColorBrush)textId.BorderBrush).Color, red.Color) || SolidColorBrush.Equals(((SolidColorBrush)textCustomerName.BorderBrush).Color, red.Color))
            //{
            //    MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //else
            //{
            //    BO.Customer customer = new Customer();
            //    customer.Id = Int32.Parse(textId.Text);
            //    customer.Name = textCustomerName.Text;
            //    customer.Phone = textCustomerPhone.Text;
            //    customer.Location.Longitude = Int32.Parse(textlongitude.Text);
            //    customer.Location.Latitude = Int32.Parse(textLatitude.Text);
            //    try
            //    {
            //        bL.AddCustomer(customer);
            //        MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            //        this.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
        }
        /// <summary>
        /// Input the id from the user and color the field according to the correctness of the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textId_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textId.Text.All(char.IsDigit) && textId.Text.Length == 9)
            {

                textId.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textId.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

        }
        /// <summary>
        /// Input the Customer Name from the user and color the field according to the correctness of the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textCustomerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textCustomerName.Text;
            var bc = new BrushConverter();
            if (text != "" && char.IsLetter(text.ElementAt(0)))
            {
                textCustomerName.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else
                textCustomerName.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        private void textCustomerPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textCustomerPhone.Text.All(char.IsDigit) && textCustomerPhone.Text.Length == 10)
            {

                textCustomerPhone.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textCustomerPhone.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

        }
        private void textlongitude_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textlongitude.Text != "" && textlongitude.Text.All(char.IsDigit) && Int32.Parse(textlongitude.Text) < 50 && Int32.Parse(textlongitude.Text) > (-50) && Int32.Parse(textlongitude.Text) != 0)
            {
                textlongitude.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textlongitude.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

        }
        private void textLatitude_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textLatitude.Text != "" && textLatitude.Text.All(char.IsDigit) && Int32.Parse(textlongitude.Text) < 50 && Int32.Parse(textlongitude.Text) > (-50) && Int32.Parse(textlongitude.Text) != 0)
            {

                textLatitude.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textLatitude.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

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
        /// /// Attempt to update skimmer name checks if all required fields are filled in correctly and sends to try to update in bl, updates the new skimmer, sends a suitable message and closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Update(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (SolidColorBrush.Equals(((SolidColorBrush)textCustomerName.BorderBrush).Color, red.Color) && SolidColorBrush.Equals(((SolidColorBrush)textCustomerPhone.BorderBrush).Color, red.Color))
                MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                string name = textCustomerName.Text;
                string phon = textCustomerPhone.Text;
                try
                {
                    bL.UpdateCustomerData(newCustomer.Id, name, phon);
                    MessageBox.Show("The update was successful", "Updated a Customer Data", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }
        private void textUpdateName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textUpdateName.Text;
            var bc = new BrushConverter();
            if (text != "" && char.IsLetter(text.ElementAt(0)))
            {
                textUpdateName.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else
                textUpdateName.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        private void textUpdatePhon_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textUpdatePhon.Text;
            var bc = new BrushConverter();
            if (text.All(char.IsDigit) && textUpdatePhon.Text.Length == 10)
            {
                textUpdatePhon.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else
                textUpdatePhon.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        private void RefreshListSendView(object ob, EventArgs ev)
        {
            PackgeListOfSenderView.ItemsSource = bL.GetSentParcels(customer);
        }
        
        private void PackgeListOfSenderView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PackgeListOfSenderView.SelectedItem != null)
            {

                PackageToList package = new PackageToList();
                package.Id = ((PackageAtCustomer)PackgeListOfSenderView.SelectedItem).Id;
                PackageWindow = new PackageWindow( package);
                PackageWindow.Closed += RefreshListSendView;
                PackageWindow.Show();
            }
        }
        private void RefreshListGetView(object ob, EventArgs ev)
        {
            PackgeListOfGetView.ItemsSource = bL.GetReceiveParcels(customer);
        }

        private void PackgeListOfGetView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((BO.PackageAtCustomer)PackgeListOfGetView.SelectedItem != null)
            {

                PackageToList package = new PackageToList();
                package.Id = ((PackageAtCustomer)PackgeListOfGetView.SelectedItem).Id;
                PackageWindow = new PackageWindow(package);
                PackageWindow.Closed += RefreshListGetView;
                PackageWindow.Show();
            }
        }
        //private void Button_UpdateDelete(object sender, RoutedEventArgs e)
        //{
        //    bL.DeleteCustomer(newCustomer.Id);
        //    MessageBox.Show("The deletion was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
        //    this.Close();
        //}
    }
} 

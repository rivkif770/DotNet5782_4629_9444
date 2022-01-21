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
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        BlApi.IBL bL;
        CustomerWindow customerWindow;
        /// <summary>
        /// constructor
        /// </summary>
        public CustomerListWindow()
        {
            InitializeComponent();
            bL = BlApi.BlFactory.GetBL();
            CustomerListView.ItemsSource = bL.GetCustomerList();
            DataContext = this;
        }
        /// <summary>
        /// Refreshing the customer list
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="ev"></param>
        private void RefreshListView(object ob, EventArgs ev)
        {
            CustomerListView.Items.Refresh();
            CustomerListView.ItemsSource = bL.GetCustomerList();
        }
        /// <summary>
        /// Submit by tapping the Add Customer button to the Add and Update the New Customer window in the list afterwards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            customerWindow = new CustomerWindow();
            customerWindow.Closed += RefreshListView;
            customerWindow.Show();
        }
        /// <summary>
        /// Option to clear the filter of the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            CustomerListView.ItemsSource = bL.GetCustomerList();
        }
        ///// <summary>
        ///// Close the window
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void EXIT_Click(object sender, RoutedEventArgs e)
        //{
        //    this.Close();
        //}
        /// <summary>
        /// Displays a specific client window by double-clicking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((BO.CustomerToList)CustomerListView.SelectedItem != null)
            {
                customerWindow = new CustomerWindow((BO.CustomerToList)CustomerListView.SelectedItem);
                customerWindow.Closed += RefreshListView;
                customerWindow.Show();
            }
        }
        /// <summary>
        /// Close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EXIT_Click2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Close the window
        /// </summary>
        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

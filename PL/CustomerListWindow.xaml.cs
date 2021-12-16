﻿using System;
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
    public partial class CustomerListWindow : Window
    {
        BlApi.IBL bL;
        CustomerWindow customerWindow;
        public CustomerListWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            CustomerListView.ItemsSource = bl.GetCustomerList();
            bL = bl;
        }
        private void RefreshListView(object ob, EventArgs ev)
        {
            CustomerListView.Items.Refresh();
        }
        /// <summary>
        /// Filter by skimmer status (checks whether there is filter by weight and combines them)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        /// Submit by tapping the Add Customer button to the Add and Update the New Customer window in the list afterwards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            customerWindow = new CustomerWindow(bL);
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
        /// <summary>
        /// Close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((BO.CustomerToList)CustomerListView.SelectedItem != null)
            {
                customerWindow = new CustomerWindow(bL, (BO.CustomerToList)CustomerListView.SelectedItem, this);
                customerWindow.Closed += RefreshListView;
                customerWindow.Show();
            }
        }

        private void EXIT_Click2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
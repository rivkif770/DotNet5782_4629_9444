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
using BlApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for skimmerListWindow.xaml
    /// </summary>
    public partial class SkimmerListWindow : Window
    {
        BlApi.IBL bL;
        SkimmerWindow skimmerWindow;
        static Weight? weightFilter;
        static SkimmerStatuses? statusesFilter;
        public SkimmerListWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            SkimmerListView.ItemsSource = bl.GetSkimmerList();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.SkimmerStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            bL = bl;
            DataContext = this;
        }

        private void RefreshListView(object ob,EventArgs ev)
        {
            SkimmerListView.Items.Refresh();
            if (WeightSelector.SelectedItem == null && StatusSelector.SelectedItem == null) SkimmerListView.ItemsSource = bL.GetSkimmerList();
            if (WeightSelector.SelectedItem != null) WeightSelector_SelectionChanged(this, null);
            if (StatusSelector.SelectedItem != null) SkimmerListView_MouseDoubleClick(this, null);
        }
        /// <summary>
        /// Filter by skimmer status (checks whether there is filter by weight and combines them)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedIndex != -1)
            {
                statusesFilter = (SkimmerStatuses)StatusSelector.SelectedItem;
                if (weightFilter == null)
                {
                    SkimmerListView.ItemsSource = bL.GetSkimmerList(x => x.SkimmerStatus == statusesFilter);
                }
                else
                {
                    SkimmerListView.ItemsSource = bL.GetSkimmerList(x => x.SkimmerStatus == statusesFilter && x.WeightCategory == weightFilter);
                }
            }
            else
            {
                StatusSelector.SelectedIndex = -1;
                statusesFilter = null;
                StatusSelector.Text = "Choose a Status";
            }
        }
        /// <summary>
        /// Filter by weight of skimmer (checks whether there is a filter by status and combines them)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(WeightSelector.SelectedIndex != -1)
            {
                weightFilter = (Weight)WeightSelector.SelectedItem;
                if (statusesFilter == null)
                {
                    SkimmerListView.ItemsSource = bL.GetSkimmerList(x => x.WeightCategory == weightFilter);
                }
                else
                {
                    SkimmerListView.ItemsSource = bL.GetSkimmerList(x => x.WeightCategory == weightFilter && x.SkimmerStatus == statusesFilter);
                }
            }
            else
            {
                WeightSelector.SelectedIndex = -1;
                weightFilter = null;
                WeightSelector.Text = "Choose a Weight";
            }
        }
        /// <summary>
        /// Sending by double-tapping a skimmer from the skimmer list to the Updates window and updating the change in the list afterwards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkimmerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if((BO.SkimmerToList)SkimmerListView.SelectedItem != null)
            {
                skimmerWindow = new SkimmerWindow(bL, (BO.SkimmerToList)SkimmerListView.SelectedItem, this);
                skimmerWindow.Closed += RefreshListView;
                skimmerWindow.Show();             
            }
        }
        /// <summary>
        /// Submit by tapping the Add Skimmer button to the Add and Update the New Skimmer window in the list afterwards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSkimmer_Click(object sender, RoutedEventArgs e)
        {
            skimmerWindow = new SkimmerWindow(bL);
            skimmerWindow.Closed += RefreshListView;
            skimmerWindow.Show();
        }
        /// <summary>
        /// Option to clear the filter of the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            SkimmerListView.ItemsSource = bL.GetSkimmerList();
            StatusSelector.SelectedIndex = -1;
            WeightSelector.SelectedIndex = -1;
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
    }
}

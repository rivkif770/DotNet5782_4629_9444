﻿using BO;
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
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        BaseStationToList newBaseStation = new BaseStationToList();
        BaseStation baseStation1 = new BaseStation();
        BlApi.IBL bL;
        private SkimmerWindow skimmerWindow;

        public delegate void CloseWindow(object ob);
        public BaseStationWindow()
        {
            bL = BlApi.BlFactory.GetBL();
            InitializeComponent();
            baseStation1.Location = new Location();
            //baseStation1.ListOfSkimmersCharge = new List<SkimmerInCharging>();
            DataContext = baseStation1;
        }
        public BaseStationWindow(BaseStationToList baseStationToList, BaseStationListWindow baseStationListWindow)
        {
            bL = BlApi.BlFactory.GetBL();
            InitializeComponent();
            help.IsChecked = true;
            newBaseStation = baseStationToList;
            baseStation1 = bL.GetBeseStation(newBaseStation.Id);
            DataContext = baseStation1;
            SkimmerListOfChargeView.ItemsSource = bL.GetListOfSkimmersCharge(baseStation1);
        }

        private void textId_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textId.Text.All(char.IsDigit) && textId.Text.Length < 5 && textId.Text.Length > 3)
            {
                textId.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textId.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        private void textName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textName.Text;
            var bc = new BrushConverter();
            if (text != "")
            {
                textName.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else
                textName.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        private void textLatitude_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textLatitude.Text != "" && textLatitude.Text.All(char.IsDigit) && Convert.ToInt32(textLatitude.Text) <= 32 && Convert.ToInt32(textLatitude.Text) >= 31 && Convert.ToInt32(textLatitude.Text) != 0)
            {
                textLatitude.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textLatitude.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        private void textLongitude_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textLongitude.Text != "" && textLongitude.Text.All(char.IsDigit) && Convert.ToInt32(textLongitude.Text) <= 36 && Convert.ToInt32(textLongitude.Text) >= 35 && Convert.ToInt32(textLongitude.Text) != 0)
            {
                textLongitude.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textLongitude.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        private void textCharging_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textCharging.Text != "" && textCharging.Text.All(char.IsDigit) && Int32.Parse(textCharging.Text) > 0)
            {
                textCharging.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textCharging.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        private void btnAddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.AddBaseStation(baseStation1);
                MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btnUpdat_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (SolidColorBrush.Equals(((SolidColorBrush)textName.BorderBrush).Color, red.Color) && SolidColorBrush.Equals(((SolidColorBrush)textCharging.BorderBrush).Color, red.Color))
            {
                MessageBox.Show($"Insert input", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                bL.UpdateBaseStation(newBaseStation.Id,textName.Text, textCharging.Text);
                MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshListView(object ob, EventArgs ev)
        {
            SkimmerListOfChargeView.ItemsSource = bL.GetListOfSkimmersCharge(baseStation1);
        }
        private void SkimmerListOfChargeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((BO.SkimmerInCharging)SkimmerListOfChargeView.SelectedItem != null)
            {
                
                SkimmerToList skimmer = new SkimmerToList();
                skimmer.Id = ((SkimmerInCharging)SkimmerListOfChargeView.SelectedItem).Id;
                skimmerWindow = new SkimmerWindow(skimmer);
                skimmerWindow.Closed += RefreshListView;
                skimmerWindow.Show();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.DeleteBaseStation(newBaseStation.Id);
                MessageBox.Show("The Delete was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }
}

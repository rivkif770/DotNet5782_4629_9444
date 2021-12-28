using BO;
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
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        BaseStationToList newBaseStation;
        BlApi.IBL BL;
        public delegate void CloseWindow(object ob);
        public BaseStationWindow(BlApi.IBL bL)
        {
            bL = BL;
            InitializeComponent();
            add.Visibility = Visibility.Visible;
        }
        public BaseStationWindow(BlApi.IBL bl, BaseStationToList baseStationToList, BaseStationListWindow baseStationListWindow)
        {
            BL = bl;
            InitializeComponent();
            Updates.Visibility = Visibility.Visible;
            newBaseStation = new BaseStationToList();
            newBaseStation = baseStationToList;
            showBaseStation.Text = bl.GetBeseStation(newBaseStation.Id).ToString();
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
            if (textLatitude.Text.All(char.IsDigit) && Int32.Parse(textLatitude.Text) < 50 && Int32.Parse(textLatitude.Text) > -50 && Int32.Parse(textLatitude.Text) != 0)
            {
                textLatitude.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textLatitude.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void textLongitude_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textLongitude.Text.All(char.IsDigit) && Int32.Parse(textLongitude.Text) < 50 && Int32.Parse(textLongitude.Text) > -50 && Int32.Parse(textLongitude.Text) != 0)
            {

                textLongitude.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textLongitude.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void textCharging_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textCharging.Text.All(char.IsDigit) && Int32.Parse(textCharging.Text) > 0)
            {

                textCharging.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textCharging.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void btnAddBaseStation_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (SolidColorBrush.Equals(((SolidColorBrush)textId.BorderBrush).Color, red.Color) || SolidColorBrush.Equals(((SolidColorBrush)textName.BorderBrush).Color, red.Color)
                || SolidColorBrush.Equals(((SolidColorBrush)textLatitude.BorderBrush).Color, red.Color) || SolidColorBrush.Equals(((SolidColorBrush)textLongitude.BorderBrush).Color, red.Color)
                || SolidColorBrush.Equals(((SolidColorBrush)textCharging.BorderBrush).Color, red.Color))
            {
                MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                BO.BaseStation baseStation = new BaseStation();
                baseStation.Id = Int32.Parse(textId.Text);
                baseStation.Name = textName.Text;
                baseStation.Location.Latitude = double.Parse(textLatitude.Text);
                baseStation.Location.Longitude = double.Parse(textLongitude.Text);
                baseStation.SeveralClaimPositionsVacant = Int32.Parse(textCharging.Text);
                try
                {
                    BL.AddBaseStation(baseStation);
                    MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private void btnDeletBaseStation_Click(object sender, RoutedEventArgs e)
        //{
        //    BL.DeleteBaseStation(newBaseStation.Id);
        //    MessageBox.Show("The deletion was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
        //    this.Close();
        //}

        private void btnUpdat_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (SolidColorBrush.Equals(((SolidColorBrush)textUpdateName.BorderBrush).Color, red.Color) && SolidColorBrush.Equals(((SolidColorBrush)textUpdateCharging.BorderBrush).Color, red.Color))
            {
                MessageBox.Show($"Insert input", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                BL.UpdateBaseStation(newBaseStation.Id, textUpdateName.Text, textUpdateCharging.Text);
                MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void textUpdateName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textName.Text;
            var bc = new BrushConverter();
            if (text != "")
            {
                textUpdateName.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else
                textUpdateName.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void textUpdateCharging_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textUpdateCharging.Text.All(char.IsDigit) && Int32.Parse(textUpdateCharging.Text) > 0)
            {

                textUpdateCharging.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textUpdateCharging.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void btnEXIT2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

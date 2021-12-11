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
using System.Drawing;
using System.Collections;
using IBL.BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for SkimmerWindow.xaml
    /// </summary>
    public partial class SkimmerWindow : Window
    {
        SkimmerToList newSkimmer;
        IBL.IBL bL;
        public delegate void CloseWindow(object ob);
        public event CloseWindow CloseWindowEvent;
        SkimmerWindow skimmerWindow;
        public SkimmerWindow(IBL.IBL bl)
        {
            
            bL = bl;
            //newSkimmer = new Skimmer();           
            InitializeComponent();
            add.Visibility = Visibility.Visible;
            ComboWeightCategory.ItemsSource = Enum.GetValues(typeof(IBL.BO.Weight));
            foreach (BaseStationToList item in bl.GetBaseStationList())
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = item.Id;
                ComboStationID.Items.Add(newItem);
            }
        }

        public SkimmerWindow(IBL.IBL bl,SkimmerToList skimmerToList,SkimmerListWindow skimmerListWindow)
        {
            bL = bl;
            InitializeComponent();
            Updates.Visibility = Visibility.Visible;           
            newSkimmer = new SkimmerToList();
            newSkimmer = skimmerToList;
        }
        private void btnAddSkimmer_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (ComboWeightCategory.SelectedItem==null || ComboStationID.SelectedItem==null || SolidColorBrush.Equals(((SolidColorBrush)textSkimmerModel.BorderBrush).Color, red.Color) || SolidColorBrush.Equals(((SolidColorBrush)textId.BorderBrush).Color, red.Color))
            {
                MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                IBL.BO.Skimmer skimmer = new Skimmer();
                skimmer.Id = Int32.Parse(textId.Text);
                skimmer.SkimmerModel = textSkimmerModel.Text;
                skimmer.WeightCategory = (Weight)(ComboWeightCategory.SelectedItem);
                try
                {
                    bL.AddSkimmer(skimmer, Int32.Parse(ComboStationID.Text));
                    MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseWindowEvent(this);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }      
        }

        private void textId_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textId.Text.All(char.IsDigit) && textId.Text.Length < 4 && textId.Text.Length > 2)
            {
               
                textId.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textId.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

        }

        private void textSkimmerModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textSkimmerModel.Text;
            var bc = new BrushConverter();
            if (text != "" && char.IsLetter(text.ElementAt(0)) && textSkimmerModel.Text.Length < 4)
            {
                textSkimmerModel.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else
                textSkimmerModel.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Update(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (SolidColorBrush.Equals(((SolidColorBrush)textUpdate.BorderBrush).Color, red.Color))
                MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                string name = textUpdate.Text;
                try
                {
                    bL.UpdateSkimmerName(newSkimmer.Id, name);
                    MessageBox.Show("The update was successful", "Updated a skimmer", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseWindowEvent(this);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void Button_Sending_Loading(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.SendingSkimmerForCharging(newSkimmer.Id);
                MessageBox.Show("The skimmer was successfully shipped for loading", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseWindowEvent(this);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Release(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (SolidColorBrush.Equals(((SolidColorBrush)textRelease.BorderBrush).Color, red.Color))
                MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                int Time = Int32.Parse(textRelease.Text);
                try
                {
                    bL.ReleaseSkimmerFromCharging(newSkimmer.Id, Time);
                    MessageBox.Show("The skimmer was successfully released for loading", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseWindowEvent(this);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void textUpdate_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textUpdate.Text;
            var bc = new BrushConverter();
            if (text != "" && char.IsLetter(text.ElementAt(0)))
            {
                textUpdate.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else
                textUpdate.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void textRelease_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textRelease.Text.All(char.IsDigit))
            {

                textRelease.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textRelease.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void Button_Sending_Skimmer_For_Delivery(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.AssigningPackageToSkimmer(newSkimmer.Id);
                MessageBox.Show("The glider was successfully shipped", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseWindowEvent(this);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Collect_The_Package(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.CollectingPackageBySkimmer(newSkimmer.Id);
                MessageBox.Show("The package was successfully collected", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseWindowEvent(this);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Package_Delivery(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.DeliveryOfPackageBySkimmer(newSkimmer.Id);
                MessageBox.Show("The shipment reached its destination successfully", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseWindowEvent(this);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
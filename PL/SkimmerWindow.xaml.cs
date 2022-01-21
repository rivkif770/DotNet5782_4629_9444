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
using BO;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace PL
{
    public partial class SkimmerWindow : Window
    {
        SkimmerToList newSkimmer;
        Skimmer skimmer1 = new Skimmer();
        BlApi.IBL bL;
        internal BackgroundWorker backgroundWorker;
        public delegate void CloseWindow(object ob);
        /// <summary>
        /// Builder of insert
        /// </summary>
        /// <param name="bl"></param>
        public SkimmerWindow()
        {           
            bL = BlApi.BlFactory.GetBL();                      
            InitializeComponent();
            ComboWeightCategory.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            foreach (BaseStationToList item in bL.GetBaseStationFreeCharging())
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = item.Id;
                newSkimmer = new SkimmerToList();
                ComboStationID.Items.Add(newItem);
            }
            DataContext = skimmer1;
        }
        /// <summary>
        /// Builder of update
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="skimmerToList"></param>
        /// <param name="skimmerListWindow"></param>
        public SkimmerWindow(SkimmerToList skimmerToList)
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += SimulatorDoWork;
            backgroundWorker.ProgressChanged += SimulatorProgressChanged;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;


            bL = BlApi.BlFactory.GetBL();
            InitializeComponent();
            ComboWeightCategory.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            ComboSkimmerStatus.ItemsSource = Enum.GetValues(typeof(BO.SkimmerStatuses));
            help.IsChecked = true;
            DataContext = bL.GetSkimmerr(skimmerToList.Id);
            newSkimmer = new SkimmerToList();
            newSkimmer = skimmerToList;
            skimmer1 = bL.GetSkimmerr(skimmerToList.Id);
        }
        /// <summary>
        /// Skimmer page update while the simulator is running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SimulatorProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DataContext = bL.GetSkimmerr(skimmer1.Id);
        }
        /// <summary>
        /// Runs the simulator according to the values it receives from reporting changes and according to the Boolean value for stopping the simulator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SimulatorDoWork(object sender, DoWorkEventArgs e)
        {
            bL.SimulatorActive(skimmer1.Id, update, stop);
        }
        /// <summary>
        /// Reports changes
        /// </summary>
        void update()
        {
            backgroundWorker.ReportProgress(0);
        }
        /// <summary>
        /// Boolean value returns if the stop button is pressed and the simulator needs to be stopped
        /// </summary>
        /// <returns></returns>
        bool stop()
        {
            return backgroundWorker.CancellationPending;
        }
        /// <summary>
        /// Button for adding a skimmer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSkimmer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int baseStation = Convert.ToInt32(((ComboBoxItem)ComboStationID.SelectedItem).Content);

                bL.AddSkimmer(skimmer1, baseStation);
                MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                skimmer1 = new Skimmer();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// To test and color the TextBox of the Id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textId_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textId.Text.All(char.IsDigit) && textId.Text.Length < 4 && textId.Text.Length > 2)
            {
                textId.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textId.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

        }
        /// <summary>
        /// To test and color the TextBox of the Skimmer Model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// Button updating name skimmer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Update(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (SolidColorBrush.Equals(((SolidColorBrush)textSkimmerModel.BorderBrush).Color, red.Color))
                MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                string name = textSkimmerModel.Text;
                try
                {
                    bL.UpdateSkimmerName(newSkimmer.Id, name);
                    MessageBox.Show("The update was successful", "Updated a skimmer", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }
        /// <summary>
        /// Button that sends a skimmer for charging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Sending_Loading(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.SendingSkimmerForCharging(newSkimmer.Id);
                MessageBox.Show("The skimmer was successfully shipped for loading", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                ComboSkimmerStatus.SelectedItem = SkimmerStatuses.maintenance;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// A button that releases a skimmer from a charger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Release(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.ReleaseSkimmerFromCharging(newSkimmer.Id);
                MessageBox.Show("The skimmer was successfully released for loading", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                ComboSkimmerStatus.SelectedItem = SkimmerStatuses.free;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        /// <summary>
        /// A button assigns a skimmer to the package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Sending_Skimmer_For_Delivery(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.AssigningPackageToSkimmer(newSkimmer.Id);
                MessageBox.Show("The glider was successfully shipped", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                ComboSkimmerStatus.SelectedItem = SkimmerStatuses.shipping;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Delivery of the package to the customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Package_Delivery(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.DeliveryOfPackageBySkimmer(newSkimmer.Id);
                MessageBox.Show("The shipment reached its destination successfully", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                ComboSkimmerStatus.SelectedItem = SkimmerStatuses.free;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Package collection by skimmer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Package_Collect(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.CollectingPackageBySkimmer(newSkimmer.Id);
                MessageBox.Show("Package collection was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Button that activates the simulator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSimulator_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker.IsBusy != true)
            {
                backgroundWorker.RunWorkerAsync();
                Simulator.IsChecked = true;
            }

        }
        /// <summary>
        /// Button that stops the simulator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync();
            Simulator.IsChecked = false;
        }
        /// <summary>
        /// Displays the package being sent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
             if (skimmer1.PackageInTransfer != null)
            {
                PackageToList packageToList = new PackageToList
                {
                    Id = skimmer1.PackageInTransfer.Id
                };
                new PackageWindow(packageToList).Show();
            }
        }
        /// <summary>
        /// To test and color the TextBox of the Longitude
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textLongitude_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            string Longitude = textLongitude.Text;
            if (Longitude.Count() > 3)
            {
                if (textLongitude.Text != "" && Longitude.Substring(0, 2) == "35" && Longitude.Substring(2, 1) == "." && Longitude.Substring(3).All(char.IsDigit))
                {
                    textLongitude.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
                }
                else textLongitude.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
            }
            else
            {
                if (textLongitude.Text != "" && textLongitude.Text.All(char.IsDigit) && Convert.ToInt32(textLongitude.Text) <= 36 && Convert.ToInt32(textLongitude.Text) >= 35 && Convert.ToInt32(textLongitude.Text) != 0)
                {
                    textLongitude.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
                }
                else textLongitude.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
            }
        }
        /// <summary>
        /// To test and color the TextBox of the Latitude
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textLatitude_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            string Latitude = textLatitude.Text;
            if (Latitude.Count() > 3)
            {
                if (textLatitude.Text != "" && Latitude.Substring(0, 2) == "31" && Latitude.Substring(2, 1) == "." && Latitude.Substring(3).All(char.IsDigit))
                {
                    textLatitude.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
                }
                else textLatitude.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
            }
            else
            {
                if (textLatitude.Text != "" && textLatitude.Text.All(char.IsDigit) && Convert.ToInt32(textLatitude.Text) <= 32 && Convert.ToInt32(textLatitude.Text) >= 31 && Convert.ToInt32(textLatitude.Text) != 0)
                {
                    textLatitude.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
                }
                else textLatitude.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
            }
        }
    }
}
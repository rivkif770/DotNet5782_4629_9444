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


namespace PL
{
    public partial class SkimmerWindow : Window
    {
        PackageWindow packageWindow;
        SkimmerToList newSkimmer;
        Skimmer skimmer1 = new Skimmer();
        BlApi.IBL bL;
        public delegate void CloseWindow(object ob);
        //public event CloseWindow CloseWindowEvent;
        /// <summary>
        /// Builder to add
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
            //DataContext = this;
        }
        /// <summary>
        /// Builder for update
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="skimmerToList"></param>
        /// <param name="skimmerListWindow"></param>
        public SkimmerWindow(SkimmerToList skimmerToList)
        {
            bL = BlApi.BlFactory.GetBL();
            InitializeComponent();
            ComboWeightCategory.ItemsSource = Enum.GetValues(typeof(BO.Weight));
            help.IsChecked = true;
            DataContext = bL.GetSkimmerr(skimmerToList.Id);
            newSkimmer = new SkimmerToList();
            newSkimmer = skimmerToList;
            //showSkimmer.Text = bl.GetSkimmerr(newSkimmer.Id).ToString();
        }
        /// <summary>
        /// Button attempt to add skimmer-checks whether all the required fields are filled correctly and sends to try to add in bl, updates the new skimmer, sends a suitable message and closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSkimmer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //bL.AddSkimmer(skimmer, Int32.Parse(ComboStationID.Text));
                BaseStation baseStation = (BaseStation)ComboStationID.SelectedItem;
                bL.AddSkimmer(skimmer1, baseStation.Id);
                MessageBox.Show("The addition was successful", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Input the id from the user and color the field according to the correctness of the input
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
        /// Input the Skimmer Model from the user and color the field according to the correctness of the input
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
        /// /// Attempt to update skimmer name checks if all required fields are filled in correctly and sends to try to update in bl, updates the new skimmer, sends a suitable message and closes the window
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
                    //CloseWindowEvent(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }
        /// <summary>
        /// /// Attempt button to send skimmer for charging sends to try to update in bl, updates the new skimmer, sends a suitable message and closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Sending_Loading(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.SendingSkimmerForCharging(newSkimmer.Id);
                MessageBox.Show("The skimmer was successfully shipped for loading", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                //CloseWindowEvent(this);
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// /// Attempt to release skimmer charger checks if all required fields are filled in correctly and sends to try to update in bl, updates the new skimmer, sends a suitable message and closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    //CloseWindowEvent(this);
                    //this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        /// <summary>
        /// /// Enter the new name from the user and color the field according to the correctness of the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// /// Enter the number of hours of charging from the user and color the field according to the correctness of the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textRelease_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textRelease.Text.All(char.IsDigit))
            {

                textRelease.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textRelease.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
        /// <summary>
        /// /// /// Attempt to associate a package with a glider Sender Try to update on bl, update the new glider, send an appropriate message and close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Sending_Skimmer_For_Delivery(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.AssigningPackageToSkimmer(newSkimmer.Id);
                MessageBox.Show("The glider was successfully shipped", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                //CloseWindowEvent(this);
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// /// /// Attempt to collect a package, sends to try to update on bl, updates the new glider, sends an appropriate message and closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Collect_The_Package(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.CollectingPackageBySkimmer(newSkimmer.Id);
                MessageBox.Show("The package was successfully collected", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                //CloseWindowEvent(this);
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// /// /// Attempt to deliver a package, sends to try to update on bl, updates the new glider, sends an appropriate message and closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Package_Delivery(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.DeliveryOfPackageBySkimmer(newSkimmer.Id);
                MessageBox.Show("The shipment reached its destination successfully", "Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                //CloseWindowEvent(this);
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// A button that displays the package in transit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowPackage_Click(object sender, RoutedEventArgs e)
        {
            if (newSkimmer.PackageNumberTransferred != 0)
            {
                //skimmerWindow.Closed += RefreshListView;
                //skimmerWindow.Show();
                Package package = bL.GetPackage(newSkimmer.PackageNumberTransferred);
                PackageToList packageToList = new PackageToList
                {
                    Id = package.Id,
                    CustomerNameSends = package.SendPackage.Name,
                    CustomerNameGets = package.ReceivesPackage.Name,
                    WeightCategory = package.WeightCategory,
                    priority = package.priority,
                    PackageMode = (ParcelStatus)bL.PackageMode(package)
                };
                packageWindow = new PackageWindow(packageToList);
                packageWindow.Show();
            }
            else
            {
                MessageBox.Show($"No package in transfer", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
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
    /// Interaction logic for AddSkimmerWindow.xaml
    /// </summary>
    public partial class SkimmerWindow : Window
    {
        Skimmer newSkimmer;
        IBL.IBL bL;
        public SkimmerWindow(IBL.IBL bl)
        {
            bL = bl;
            newSkimmer = new Skimmer();
            DataContext = new Skimmer();
            InitializeComponent();
            ComboWeightCategory.ItemsSource = Enum.GetValues(typeof(IBL.BO.Weight));
            foreach (var item in bl.GetBaseStationList())
            {
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = item.Id;
                ComboStationID.Items.Add(newItem);
            }
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (ComboWeightCategory.SelectedItem==null || ComboStationID.SelectedItem==null || SolidColorBrush.Equals(((SolidColorBrush)textSkimmerModel.BorderBrush).Color, red.Color) || SolidColorBrush.Equals(((SolidColorBrush)textId.BorderBrush).Color, red.Color))
            {
                MessageBox.Show("Please enter correct input", "Error input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                newSkimmer = new Skimmer
                {
                    Id = Int32.Parse(textId.Text),
                    SkimmerModel = textSkimmerModel.Text,
                    WeightCategory = (Weight)(ComboWeightCategory.SelectedValue)
                };
                int station = (int)(ComboStationID.SelectedValue);
                bL.AddSkimmer(newSkimmer, station);
            }      
        }

        private void textId_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (textId.Text.All(char.IsDigit))
            {
               
                textId.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else textId.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

        }

        private void textSkimmerModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textSkimmerModel.Text;
            var bc = new BrushConverter();
            if (text != "" && char.IsLetter(text.ElementAt(0)))
            {
                textSkimmerModel.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else
                textSkimmerModel.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
    }
}








/*<!--<Window x:Class="PL.SkimmerWindow123"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:PL"
        mc:Ignorable="d"
        Title="DroneWindow" Height="450" Width="800">
    <Grid>
        <Grid x:Name="grid1" VerticalAlignment="Top" Margin="84,32,0,0" HorizontalAlignment="Left" Height="206" Width="302">
            <Grid.RowDefinitions>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="Auto"/>
            </Grid.RowDefinitions>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="Auto"/>
                <ColumnDefinition Width="Auto"/>
            </Grid.ColumnDefinitions>
            <Label VerticalAlignment="Center" Grid.Row="0" Margin="3" HorizontalAlignment="Left" Grid.Column="0" Content="Battery Status:"/>
            <TextBox x:Name="batteryStatusTextBox" Width="120" VerticalAlignment="Center" Text="{Binding BatteryStatus, Mode=TwoWay, NotifyOnValidationError=true, ValidatesOnExceptions=true}" Grid.Row="0" Margin="3" Height="23" HorizontalAlignment="Left" Grid.Column="1"/>
            <Label VerticalAlignment="Center" Grid.Row="1" Margin="3" HorizontalAlignment="Left" Grid.Column="0" Content="Id:"/>
            <TextBox x:Name="idTextBox" Width="120" VerticalAlignment="Center" Text="{Binding Id, Mode=TwoWay, NotifyOnValidationError=true, ValidatesOnExceptions=true}" Grid.Row="1" Margin="3" Height="23" HorizontalAlignment="Left" Grid.Column="1"/>
            <Label VerticalAlignment="Center" Grid.Row="3" Margin="3" HorizontalAlignment="Left" Grid.Column="0" Content="Skimmer Model:"/>
            <TextBox x:Name="skimmerModelTextBox" Width="120" VerticalAlignment="Center" Text="{Binding SkimmerModel, Mode=TwoWay, NotifyOnValidationError=true, ValidatesOnExceptions=true}" Grid.Row="3" Margin="3" Height="23" HorizontalAlignment="Left" Grid.Column="1"/>
            <Label VerticalAlignment="Center" Grid.Row="4" Margin="3" HorizontalAlignment="Left" Grid.Column="0" Content="Skimmer Status:"/>
            <ComboBox x:Name="skimmerStatusComboBox" Width="120" Text="Choose a Status" IsEditable="True" IsReadOnly="True" VerticalAlignment="Center" Grid.Row="4" Margin="3"  Height="Auto" HorizontalAlignment="Left"
                      DisplayMemberPath="SkimmerStatus" Grid.Column="1">
                <ComboBox.ItemsPanel>
                    <ItemsPanelTemplate>
                        <VirtualizingStackPanel/>
                    </ItemsPanelTemplate>
                </ComboBox.ItemsPanel>
            </ComboBox>
            <Label VerticalAlignment="Center" Grid.Row="5" Margin="3" HorizontalAlignment="Left" Grid.Column="0" Content="Weight Category:"/>
            --><!--<TextBox x:Name="weightCategoryTextBox" Width="120" VerticalAlignment="Center" Text="{Binding WeightCategory, Mode=TwoWay, NotifyOnValidationError=true, ValidatesOnExceptions=true}" Grid.Row="5" Margin="3" Height="23" HorizontalAlignment="Left" Grid.Column="1"/>--><!--
            <ComboBox Grid.Row="5" Grid.Column="1" x:Name="ComboWeightCategory" Text="Choose a Weight"  Height="23"
                            IsEditable="True" IsReadOnly="True" Width="120" VerticalAlignment="Center" HorizontalAlignment="Left"
                      SelectedItem="{Binding WeightCategory,Mode=TwoWay, NotifyOnValidationError=true, ValidatesOnExceptions=true}" Margin="4,0,0,0" />
            <Label VerticalAlignment="Center" Grid.Row="6" Margin="6,0,0,0" HorizontalAlignment="Left" Content="station:"/>
            --><!--<TextBox x:Name="latitudeTextBox" Width="120" VerticalAlignment="Center" Text="{Binding Location.Latitude, Mode=TwoWay, NotifyOnValidationError=true, ValidatesOnExceptions=true}" Grid.Row="6" Height="23" HorizontalAlignment="Center" Grid.Column="1"/>--><!--
            <ComboBox Grid.Row="6" x:Name="ComboStationID" Text="Choose a Station"  
                            IsEditable="True" IsReadOnly="True" Width="120"
                      SelectedItem="{Binding StationID}" Height="23" HorizontalAlignment="Center" Grid.Column="1"  />
        </Grid>
        <Button x:Name="btnShow" Content="ADD" HorizontalAlignment="Left" Margin="169,286,0,0" VerticalAlignment="Top" Width="75" Click="btnShow_Click" RenderTransformOrigin="-3.728,7.236"/>

    </Grid>
</Window>-->*/

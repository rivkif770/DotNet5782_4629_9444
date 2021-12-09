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
    public partial class AddSkimmerWindow : Window
    {
        Skimmer newSkimmer;
        IBL.IBL bL;
        public AddSkimmerWindow(IBL.IBL bl)
        {
            this.bL = bl;
            newSkimmer = new Skimmer();
            //this.DataContext = station;
            this.DataContext = new Skimmer();
            InitializeComponent();
            this.textWeightCategory.ItemsSource = Enum.GetValues(typeof(IBL.BO.Weight));
            textStationID.ItemsSource = BaseStationToList(typeof(BaseStationToList));  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            newSkimmer = new Skimmer
            {
                Id = Int32.Parse(textId.Text),
                SkimmerModel = textSkimmerModel.Text,
                WeightCategory = (Weight)(textWeightCategory.SelectedValue)
            };
            int station = Int32.Parse((string)textStationID.SelectedValue);          
        }
    }
}

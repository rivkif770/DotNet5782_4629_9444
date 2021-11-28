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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class DronesListWindow : Window
    {
        List<IBL.BO.SkimmerToList> fakelist = new List<SkimmerToList>()
        {
            new SkimmerToList
            {
                 Id =3000,
                  SkimmerModel= "BBTT67H",
                   BatteryStatus= 0.50,
                    SkimmerStatus= SkimmerStatuses.free,
                     PackageNumberTransferred=0,
                      CurrentLocation= new Location{ Latitude=32,  Longitude=31},
                       WeightCategory= Weight.Heavy
            },
         new SkimmerToList
            {
                 Id =45000,
                  SkimmerModel= "ISBN86678",
                   BatteryStatus= 0.50,
                    SkimmerStatus= SkimmerStatuses.maintenance,
                     PackageNumberTransferred=0,
                      CurrentLocation= new Location{ Latitude=32,  Longitude=31},
                       WeightCategory= Weight.Light
            },
         new SkimmerToList
            {
                 Id =3000,
                  SkimmerModel= "YYY7U6765",
                   BatteryStatus= 0.50,
                    SkimmerStatus= SkimmerStatuses.free,
                     PackageNumberTransferred=0,
                      CurrentLocation= new Location{ Latitude=32,  Longitude=31},
                       WeightCategory= Weight.Heavy
            },
         new SkimmerToList
            {
                 Id =3000,
                  SkimmerModel= "BBTT67H",
                   BatteryStatus= 0.50,
                    SkimmerStatus= SkimmerStatuses.free,
                     PackageNumberTransferred=0,
                      CurrentLocation= new Location{ Latitude=32,  Longitude=31},
                       WeightCategory= Weight.Heavy
            },
         new SkimmerToList
            {
                 Id =3000,
                  SkimmerModel= "BBTT67H",
                   BatteryStatus= 0.50,
                    SkimmerStatus= SkimmerStatuses.free,
                     PackageNumberTransferred=0,
                      CurrentLocation= new Location{ Latitude=32,  Longitude=31},
                       WeightCategory= Weight.Heavy
            }
  
        };
        IBL.IBL bL;
        public DronesListWindow(IBL.IBL bl)
        {
            this.bL = bl;
            InitializeComponent();
            comboStatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.SkimmerStatuses));
        }

        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SkimmerStatuses status = (SkimmerStatuses) comboStatusSelector.SelectedItem;
            this.DronesListView.ItemsSource = fakelist.Where(x => x.SkimmerStatus == status);
        }
    }
}

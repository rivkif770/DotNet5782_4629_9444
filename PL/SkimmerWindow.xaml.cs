using IBL.BO;
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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.BO.Skimmer skimmer;
        public DroneWindow()
        {
            skimmer = new IBL.BO.Skimmer();
            DataContext = skimmer;
            InitializeComponent();
   //         skimmerStatusComboBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.SkimmerStatuses));
        }

        public DroneWindow(IBL.BO.Skimmer skimmer)
        {
            this.skimmer = skimmer;
            DataContext = skimmer;
            InitializeComponent();
        }

        public Skimmer Skimmer { get => skimmer; }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            String msg = skimmer.ToString();
            MessageBox.Show(msg);
        }
    }
}

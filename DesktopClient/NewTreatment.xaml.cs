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

namespace DesktopClient
{
    /// <summary>
    /// Interaction logic for NewReservation.xaml
    /// </summary>
    public partial class NewReservation : Window
    {
        private List<int> durations { get; }
        public NewReservation()
        {
            InitializeComponent();
            durations = new List<int> { 30, 60, 90, 120, 150, 180 };
            DurationCombo.ItemsSource = durations;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

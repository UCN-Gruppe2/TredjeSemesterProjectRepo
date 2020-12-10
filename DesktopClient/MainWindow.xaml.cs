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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;
using RestSharp;

namespace DesktopClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestClient _client;
        public MainWindow(RestClient client)
        {
            _client = client;
            InitializeComponent();
        }

        private void NewTreatment_Click(object sender, RoutedEventArgs e)
        {
            NewTreatment newTreat = new NewTreatment(_client);
            newTreat.Show();
        }

        private void NewEmployee_Click(object sender, RoutedEventArgs e)
        {
            FunctionNotImplemented_Click(sender, e);
        }

        private void NewReservation_Click(object sender, RoutedEventArgs e)
        {
            NewReservation newRes = new NewReservation(_client);
            newRes.Show();
        }

        private void FunctionNotImplemented_Click(object sender, RoutedEventArgs e)
        {
            NotImplemented notImplemented = new NotImplemented();
            notImplemented.Show();
        }
    }
}

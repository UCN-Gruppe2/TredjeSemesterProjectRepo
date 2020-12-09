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
        private RestClient _restClient = new RestClient("https://localhost:44388");
        private List<Reservation> _reservations = new List<Reservation>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task TabItem_MouseDoubleClickAsync(object sender, MouseButtonEventArgs e)
        {
            IRestRequest request = new RestRequest("reservation/GetReservationsByEmployeeID/{id}")
                .AddUrlSegment("id", 1);
            _reservations = (List<Reservation>)await _restClient.ExecuteAsync<List<Reservation>>(request);
            dataGrid.DataContext = _reservations;
        }

        private void NewTreatment_Click(object sender, RoutedEventArgs e)
        {
            NewTreatment newTreat = new NewTreatment();
            newTreat.Show();
        }

        private void NewEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FunctionNotImplemented_Click(object sender, RoutedEventArgs e)
        {
            NotImplemented notImplemented = new NotImplemented();
            notImplemented.Show();
        }
    }
}

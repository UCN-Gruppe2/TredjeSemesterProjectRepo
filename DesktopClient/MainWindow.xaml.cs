using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace DesktopClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestClient _client;
        private ObservableCollection<Reservation> reservationsOfEmployee { get; set; }
        public MainWindow(RestClient client)
        {
            _client = client;
            InitializeComponent();
        }

        private void NewTreatment_Click(object sender, RoutedEventArgs e)
        {
            CreateTreatment newTreat = new CreateTreatment(_client);
            newTreat.Show();
        }

        private void NewEmployee_Click(object sender, RoutedEventArgs e)
        {
            FunctionNotImplemented_Click(sender, e);
        }

        private void NewReservation_Click(object sender, RoutedEventArgs e)
        {
            CreateReservation newRes = new CreateReservation(_client);
            newRes.Show();
        }

        private void SearchEmployee_Click(object sender, RoutedEventArgs e)
        {
            bool state = UpdateDataGridEmployeeReservations();
            if (state)
            {
                EmployeeIDLbl.Content = SearchEmployee.Text;
            }
            else
            {
                EmployeeIDLbl.Content = "Der var ingenting i listen";
            }
            SearchEmployee.Text = "";
        }

        private bool UpdateDataGridEmployeeReservations()
        {
            bool state = false;
            ObservableCollection<Reservation> reservationsOfEmployee = new ObservableCollection<Reservation>(getReservations());
            if (reservationsOfEmployee.Count > 0)
            {
                state = true;
            }
            //dataGrid.DataContext = reservationsOfEmployee;
            dataGrid.ItemsSource = reservationsOfEmployee;
            return state;
        }

        private List<Reservation> getReservations()
        {
            RestRequest addRequest = new RestRequest("api/Employee/GetReservations", Method.GET);
            addRequest.AddParameter("employeeID", Int32.Parse(SearchEmployee.Text));

            var response = _client.Execute(addRequest);

            string theJson = response.Content;
            List<Reservation> reservations = JsonConvert.DeserializeObject<List<Reservation>>(theJson);
            //JObject anObject = JObject.Parse(response.Content);
            //List<Reservation> reservations = anObject.ToObject<List<Reservation>>();
            return reservations;
        }

        private void FunctionNotImplemented_Click(object sender, RoutedEventArgs e)
        {
            NotImplemented notImplemented = new NotImplemented();
            notImplemented.Show();
        }


    }
}

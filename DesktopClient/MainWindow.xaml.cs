using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");
            InitializeComponent();
            StartUp();
        }

        private void StartUp()
        {
            TreatmentStartUp();
            EmployeeStartUp();
            ReservationStartUp();
        }

        //Treatment Tab
        private void NewTreatment_Click(object sender, RoutedEventArgs e)
        {
            CreateTreatment newTreat = new CreateTreatment(this, _client);
            newTreat.Show();
        }

        private void TreatmentStartUp()
        {
            TreatmentCompanyLbl.Content = "";
            TreatmentNameLbl.Content = "";
            TreatmentDescriptionLbl.Text = "";
            TreatmentDurationLbl.Content = "";
            TreatmentPriceLbl.Content = "";
        }

        public void ShowCreatedTreatment(Treatment treatment)
        {
            TreatmentCompanyLbl.Content = treatment.CompanyID;
            TreatmentNameLbl.Content = treatment.Name;
            TreatmentDescriptionLbl.Text = treatment.Description;
            TreatmentDurationLbl.Content = treatment.Duration + " Minutter";
            TreatmentPriceLbl.Content = treatment.Price + " DKK";
        }


        //Employee Tab
        private void NewEmployee_Click(object sender, RoutedEventArgs e)
        {
            FunctionNotImplemented_Click(sender, e);
        }

        private void EmployeeStartUp()
        {

        }

        private void SearchEmployee_Click(object sender, RoutedEventArgs e)
        {
            bool state = UpdateDataGridEmployeeReservation();
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

        private bool UpdateDataGridEmployeeReservation()
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
            RestRequest addRequest = new RestRequest("api/Reservation/GetReservationsByEmployeeID", Method.GET);
            addRequest.AddParameter("id", Int32.Parse(SearchEmployee.Text));

            var response = _client.Execute(addRequest);

            string theJson = response.Content;
            List<Reservation> reservations = JsonConvert.DeserializeObject<List<Reservation>>(theJson);
            //JObject anObject = JObject.Parse(response.Content);
            //List<Reservation> reservations = anObject.ToObject<List<Reservation>>();
            return reservations;
        }

        
        //Reservation Tab
        private void NewReservation_Click(object sender, RoutedEventArgs e)
        {
            CreateReservation newRes = new CreateReservation(this, _client);
            newRes.Show();
        }

        private void ReservationStartUp()
        {
            ReservationDateLbl.Content = "";
            ReservationTreatmentLbl.Content = "";
            ReservationEmployeeLbl.Content = "";
            ReservationCustomerLbl.Content = "";
        }

        public void ShowCreatedReservation(Reservation reservation)
        {
            ReservationDateLbl.Content = reservation.StartTime;
            ReservationTreatmentLbl.Content = reservation.TreatmentID;
            ReservationEmployeeLbl.Content = reservation.EmployeeID;
            ReservationCustomerLbl.Content = reservation.CustomerID;
        }


        //Not Implementet function
        private void FunctionNotImplemented_Click(object sender, RoutedEventArgs e)
        {
            NotImplemented notImplemented = new NotImplemented();
            notImplemented.Show();
        }


    }
}

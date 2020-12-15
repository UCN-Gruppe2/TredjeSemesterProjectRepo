using Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for CreateReservation.xaml
    /// </summary>
    public partial class CreateReservation : Window
    {
        private readonly List<string> Times;
        private RestClient _client;
        private MainWindow main;

        public CreateReservation(MainWindow main, RestClient client)
        {
            _client = client;
            this.main = main;
            InitializeComponent();
            FailLbl.Opacity = 0;
            //Making a list of possible starttimes
            Times = new List<string> { "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00",
                "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30",
                "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30" };
            TimeCombo.ItemsSource = Times;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            string time = TimeCombo.Text;

            string[] clock = time.Split(':');
            int hour = Int32.Parse(clock[0]);
            int minutes = Int32.Parse(clock[1]);
            TimeSpan timeSpan = new TimeSpan(hour, minutes, 0);
            DateTime startTime = (DateTime)DateSelector.SelectedDate;
            startTime = startTime.Add(timeSpan);

            Reservation_DTO reservationToAdd = new Reservation_DTO(Int32.Parse(TreatmentIDBox.Text),
                Int32.Parse(CustomerIDBox.Text), Int32.Parse(EmployeeIDBox.Text),
                startTime);

            RestRequest addRequest = new RestRequest("api/Reservation", Method.POST);
            addRequest.AddJsonBody(reservationToAdd);

            var response = _client.Execute(addRequest);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                FailLbl.Content = FailLbl.Content + "//n " + response.StatusCode + ", " + response.ErrorException.Message;
                FailLbl.Opacity = 100;
                string message = response.ErrorException.Message;
                if (message.Contains("TreatmentID"))
                {
                    TreatmentIDBox.BorderBrush = Brushes.Red;
                    TreatmentIDBox.BorderThickness = new Thickness(1, 1, 1, 1);
                }
                if (message.Contains("CustomerID"))
                {
                    CustomerIDBox.BorderBrush = Brushes.Red;
                    CustomerIDBox.BorderThickness = new Thickness(1, 1, 1, 1);
                }
                if (message.Contains("EmployeeID"))
                {
                    EmployeeIDBox.BorderBrush = Brushes.Red;
                    EmployeeIDBox.BorderThickness = new Thickness(1, 1, 1, 1);
                }
                if(message.Contains("occurrred a conflict"))
                {
                    FailLbl.Content = FailLbl.Content + "//n " + response.StatusCode + ", " + response.ErrorException.Message;
                    FailLbl.Opacity = 100;
                    TimeCombo.BorderBrush = Brushes.Red;
                    TimeCombo.BorderThickness = new Thickness(1, 1, 1, 1);
                }
            }
            else if(response.StatusCode == HttpStatusCode.NotFound)
            {
                TreatmentIDBox.BorderBrush = Brushes.Red;
                TreatmentIDBox.BorderThickness = new Thickness(1, 1, 1, 1);
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                CreateButton.BorderBrush = Brushes.Red;
                CreateButton.BorderThickness = new Thickness(1, 1, 1, 1);
            }
            else
            {
                string theJson = response.Content;
                Reservation reservation = JsonConvert.DeserializeObject<Reservation>(theJson);
                reservation.StartTime = reservation.StartTime.ToLocalTime();
                reservation.EndTime = reservation.EndTime.ToLocalTime();

                main.ShowCreatedReservation(reservation);

                this.Close();
            }
        }
    }
}

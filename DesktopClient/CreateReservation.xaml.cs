using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        private async void CreateBtn_Click_Improved(object sender, RoutedEventArgs e)
        {
            if (TreatmentIDBox.Text.Trim().Length == 0)
            {
                setFailText(TreatmentIDBox, "Indtast venligst behandlingsID.");
                return;
            }

            if (CustomerIDBox.Text.Trim().Length == 0)
            {
                setFailText(CustomerIDBox, "Indtast venligst kunde ID.");
                return;
            }

            if (EmployeeIDBox.Text.Trim().Length == 0)
            {
                setFailText(EmployeeIDBox, "Indtast venligst medarbejder ID.");
                return;
            }

            if (TimeCombo.Text.Trim().Length == 0)
            {
                setFailText(TimeCombo, "Indsæt venligst gyldigt tidspunkt.");
                return;
            }

            if (!DateSelector.SelectedDate.HasValue) //Hvis der ikke er blevet valgt en dato.
            {
                setFailText(DateSelector, "Vælg venligst en dato.");
                return;
            }

            string selectedTimeAsString = TimeCombo.Text;
            TimeSpan timeSlotStart = TimeSpan.Parse(selectedTimeAsString);
            DateTime selectedDate = DateSelector.SelectedDate.Value.Add(timeSlotStart); //Bedre? Eller ny linje?
            //selectedDate = selectedDate.Add(timeSlotStart); //Adds selected time slot start to the selected day.

            Reservation_DTO reservationToAdd = new Reservation_DTO(
                treatmentID: int.Parse(TreatmentIDBox.Text),
                customerID: int.Parse(CustomerIDBox.Text),
                employeeID: int.Parse(EmployeeIDBox.Text),
                startTime: selectedDate
            );

            RestRequest addRequest = new RestRequest("api/Reservation", Method.POST);
            addRequest.AddJsonBody(reservationToAdd);
            var response = await _client.ExecuteAsync(addRequest);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    showResult(response.Content);
                    break;

                case HttpStatusCode.Conflict:
                    Control elementToRedBorder = findControlFromMessage(response.Content);
                    setFailText(elementToRedBorder, response);
                    break;

                case HttpStatusCode.InternalServerError:
                    setFailText(CreateButton, response);
                    break;

                case HttpStatusCode.NotFound:
                    setFailText(TreatmentIDBox, response);
                    break;
            }
        }

        private void showResult(string jsonResponseAsString)
        {
            Reservation reservation = JsonConvert.DeserializeObject<Reservation>(jsonResponseAsString);
            reservation.StartTime = reservation.StartTime.ToLocalTime();
            reservation.EndTime = reservation.EndTime.ToLocalTime();

            main.ShowCreatedReservation(reservation);

            this.Close();
        }

        private Control findControlFromMessage(string message)
        {
            Control elementToReturn = null;

            if (message.Contains("TreatmentID"))
            {
                elementToReturn = TreatmentIDBox;
            }
            else if (message.Contains("CustomerID"))
            {
                elementToReturn = CustomerIDBox;
            }
            else if (message.Contains("EmployeeID"))
            {
                elementToReturn = EmployeeIDBox;
            }
            else if (message.Contains("occurred a conflict"))
            {
                elementToReturn = TimeCombo;
            }

            return elementToReturn;
        }

        private void setCommonFailText(Control elementBehindFail = null)
        {
            FailLbl.Content = "Der skete en fejl. Indsæt venligst alle data.";
            FailLbl.Opacity = 100;

            if (elementBehindFail != null)
            {
                giveElementRedBorderResetOthers(elementBehindFail);
            }
        }

        private void giveElementRedBorderResetOthers(Control element)
        {
            Control[] controlElementsToReset = new Control[]
            {
                TreatmentIDBox, CreateButton, CustomerIDBox, EmployeeIDBox, TimeCombo
            };

            foreach (Control controlElementToReset in controlElementsToReset)
            {
                controlElementToReset.BorderBrush = Brushes.Gray;
                controlElementToReset.BorderThickness = new Thickness(1, 1, 1, 1);
            }

            element.BorderBrush = Brushes.Red;
            element.BorderThickness = new Thickness(1, 1, 1, 1);
        }

        private void setFailText(Control elementBehindFail, string message)
        {
            FailLbl.Content = $"Der skete en fejl:\n{message}";
            FailLbl.Opacity = 100;

            giveElementRedBorderResetOthers(elementBehindFail);
        }

        private void setFailText(Control elementBehindFail, IRestResponse errorResponse)
        {
            FailLbl.Content = $"Der skete en fejl!\n({errorResponse.StatusCode}): {errorResponse.Content}";
            FailLbl.Opacity = 100;
            giveElementRedBorderResetOthers(elementBehindFail);
        }

        private async void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TreatmentIDBox.Text.Trim().Length == 0 && CustomerIDBox.Text.Trim().Length == 0 && EmployeeIDBox.Text.Trim().Length == 0)
            {
                TreatmentIDBox.Text = "0";
                CustomerIDBox.Text = "0";
                EmployeeIDBox.Text = "0";
                DateSelector.SelectedDate = DateTime.Now;
                TimeCombo.Text = "19:30";
            }

            string time = TimeCombo.Text;
            FailLbl.Content = "Der skete en fejl!";
            FailLbl.Opacity = 0;
            TreatmentIDBox.BorderBrush = Brushes.Gray;
            TreatmentIDBox.BorderThickness = new Thickness(1, 1, 1, 1);
            CreateButton.BorderBrush = Brushes.Gray;
            CreateButton.BorderThickness = new Thickness(1, 1, 1, 1);

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

            var response = await _client.ExecuteAsync(addRequest);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                string message = response.Content;
                FailLbl.Content = FailLbl.Content + "\n" + response.StatusCode + ": " + message;
                FailLbl.Opacity = 100;

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
                if (message.Contains("occurred a conflict"))
                {
                    TimeCombo.BorderBrush = Brushes.Red;
                    TimeCombo.BorderThickness = new Thickness(1, 1, 1, 1);
                }
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string message = response.Content;
                FailLbl.Content = FailLbl.Content + "\n" + response.StatusCode + ": " + message;
                FailLbl.Opacity = 100;
                TreatmentIDBox.BorderBrush = Brushes.Red;
                TreatmentIDBox.BorderThickness = new Thickness(1, 1, 1, 1);
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                string message = response.Content;
                FailLbl.Content = FailLbl.Content + "\n" + response.StatusCode + ": " + message;
                FailLbl.Opacity = 100;
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

﻿using System;
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
using RestSharp;
using Model;

namespace DesktopClient
{
    /// <summary>
    /// Interaction logic for NewReservation.xaml
    /// </summary>
    public partial class NewReservation : Window
    {
        private readonly List<string> Times;
        private RestClient _client;
        public NewReservation(RestClient client)
        {
            _client = client;
            InitializeComponent();
            //Making a list of possible starttimes
            Times = new List<string> { "06:00", "06:30", "07:00", "07:30", "08:00", "08:30", "09:00", "09:30", "10:00", 
                "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", 
                "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00", "19:30" };
            TimeCombo.ItemsSource = Times;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            Reservation_DTO reservationToAdd = new Reservation_DTO(Int32.Parse(TreatmentIDBox.Text), 
                Int32.Parse(CustomerIDBox.Text), Int32.Parse(EmployeeIDBox.Text), 
                DateTime.Parse(DateSelector.SelectedDate + " " + TimeCombo.Text));

            RestRequest addRequest = new RestRequest("api/Reservation", Method.POST);
            addRequest.AddJsonBody(reservationToAdd);

            _client.Execute(addRequest);
            this.Close();
        }
    }
}
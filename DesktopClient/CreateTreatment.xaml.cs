using Model;
using Newtonsoft.Json;
using RestSharp;
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
    /// Interaction logic for CreateTreatment.xaml
    /// </summary>
    public partial class CreateTreatment : Window
    {
        private List<int> durations { get; }
        private RestClient _client;
        private MainWindow main;
        public CreateTreatment(MainWindow main, RestClient client)
        {
            _client = client;
            this.main = main;
            InitializeComponent();
            durations = new List<int> { 30, 60, 90, 120, 150, 180 };
            DurationCombo.ItemsSource = durations;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            Int32 duration = (int)DurationCombo.SelectedItem;
            Treatment_DTO treatmentToAdd = new Treatment_DTO(Int32.Parse(CompanyIDBox.Text),
                NameBox.Text, DescriptionBox.Text, duration, Decimal.Parse(PriceBox.Text));

            RestRequest addRequest = new RestRequest("api/Treatment", Method.POST);
            addRequest.AddJsonBody(treatmentToAdd);

            var response = _client.Execute(addRequest);

            string theJson = response.Content;
            Treatment treatment = JsonConvert.DeserializeObject<Treatment>(theJson);

            main.ShowCreatedTreatment(treatment);

            this.Close();
        }
    }
}

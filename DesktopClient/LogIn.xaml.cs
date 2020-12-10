using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        private RestClient _client = new RestClient("https://localhost:44388");
        public LogIn()
        {
            InitializeComponent();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            RestRequest request = new RestRequest("/Token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("userName", mailTextBox.Text);
            request.AddParameter("password", PasswordBox.Password);
            var response = _client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string accessToken = JObject.Parse(response.Content)["access_token"].ToString();
                _client.AddDefaultHeader("Authorization", $"Bearer { accessToken }");
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            } 
            else
            {
                mailTextBox.Text = "";
                PasswordBox.Password = "";
                BadSignIn.Content = "Forkert email/password kombination. Prøv igen.";
                BadSignIn.Visibility = Visibility.Visible;
            }
        }
    }
}

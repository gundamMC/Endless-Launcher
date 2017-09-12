using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net.Http;
using LitJson;

namespace EndlessLauncher
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void HeaderBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(UsernameInputBox.Text) || String.IsNullOrWhiteSpace(PasswordInputBox.Password))
            {
                MessageBoxOK form = new MessageBoxOK("You must input your username and password", "OK");
                form.ShowDialog();
            }

            Login();
        }

        Boolean POSTSuccess = false;

        public async void Login()
        {

            Guid ClientToken = new Guid();                                                    // Generate new random (g)uuid

            string sendData =                                                                 // According to http://wiki.vg/Authentication
                  "{" + "\n" +
                      "\"agent\": {" + "\n" +
                          "\"name\": \"Minecraft\"," + "\n" +
                          "\"version\": 1" + "\n" +
                      "}," + "\n" +
                      "\"username\": \"" + UsernameInputBox.Text + "\"," + "\n" +             // Email address or username for legacy accounts
                      "\"password\": \"" + PasswordInputBox.Password + "\"," + "\n" +
                      "\"clientToken\": \"" + ClientToken.ToString("D") + "\"," + "\n" +      // ToString("D") to add the hyphens as used in the official launcher
                      "\"requestUser\": true" + "\n" +                                        // To get the username / user info
                  @"}";

            string results = await LoginPOST("");

            

            if (POSTSuccess)
            {
                ResponseClass Response = JsonMapper.ToObject<ResponseClass>(results.Replace("selectedProfile", "Profiles").Replace("availableProfiles", "Profiles"));

                App.Config.Username = UsernameInputBox.Text;                    // Email / account username
                App.Config.DisplayName = Response.Profiles[0].Name;             // In game name
                App.Config.UUID = Response.Profiles[0].Id;                      // UUID
                App.Config.AccessToken = Guid.Parse(Response.AccessToken);      // Access Token
                App.Config.ClientToken = Guid.Parse(Response.ClientToken);      // Client Token
            }
            else
            {
                if (String.IsNullOrWhiteSpace(results))
                    return;                                 // Connection error messagebox has already been shown

                MessageBoxOK form = new MessageBoxOK("Error : " + (string)JsonMapper.ToObject<JsonData>(results)["error"], "OK");
                form.ShowDialog();   
            }
            
        }

        private async Task<string> LoginPOST(string sendData)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://authserver.mojang.com");
                    client.Timeout = new TimeSpan(0, 0, 20);                                        //20 Seconds timeout
                    var content = new StringContent(sendData, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("/authenticate", content);
                    var response = await result.Content.ReadAsStringAsync();

                    POSTSuccess = result.IsSuccessStatusCode;
                    return response;
                }
            }
            catch (HttpRequestException ex)     //connection errors (note: auth errors are handled in Login())
            {
                MessageBoxOK form = new MessageBoxOK("There is a connection error to the Mojang servers\nError: " + ex.Message, "OK");
                form.ShowDialog();
                return null;
            }
            
        }

        //Json maps - this only contains what we need from the response

        public class ProfilesList
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class Usersid
        {
            public string Id { get; set; }
            
        }

        public class ResponseClass
        {
            public string AccessToken { get; set; }
            public string ClientToken { get; set; }
            public List<ProfilesList> Profiles { get; set; }
            public List<Usersid> User { get; set; }
        }
    }
}

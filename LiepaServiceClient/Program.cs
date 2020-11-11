using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LiepaService.Models.Views;
using System.Text;
using System.Net.Http.Formatting;
using Microsoft.Extensions.Configuration;

namespace LiepaServiceClient
{
    class Program
    {
        private static HttpClient Client = new HttpClient();
        static async Task Main(string[] args)
        {
            ConfigureClient();

            switch(args[0]) {
                case "--create":
                case "-c": {
                    Console.WriteLine(await Create(int.Parse(args[1]), args[2], args[3]));
                    break;
                }     
                case "--read":               
                case "-r": {
                    Console.WriteLine(await Read(int.Parse(args[1])));
                    break;
                }
                case "--update":
                case "-u": {
                    Console.WriteLine(await Update(int.Parse(args[1]), args[2]));
                    break;
                }
                case "--delete":
                case "-d": {
                    Console.WriteLine(await Delete(int.Parse(args[1])));
                    break;
                }
                default : {
                    Console.WriteLine("Usage: \n" +
                                    "-c, --create <id> <name> <status>: Create a new user, using  provided info" +
                                    "-r, --read <int>: Find user by id" +
                                    "-u, --update <id> <status>: Set new status to a user" +
                                    "-d, --delete <id>: Delete user by id");
                    return;
                }
            }
        }

        private static void ConfigureClient() {

            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
                .Build();

            Client.BaseAddress = new Uri(Configuration["serverAddress"]);
            
            var plainTextBytes = Encoding.UTF8.GetBytes(Configuration["basicAuthCredentials"]);
            var base64Credentials = Convert.ToBase64String(plainTextBytes);
            Client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Basic {base64Credentials}");
        }

        private static async Task<string> Create(int id, string name, string status) {
            var request = new RequestView {
                User = new UserView {
                    Id = id,
                    Name = name,
                    Status = status
                }
            };

            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var response = await Client.PostAsync("auth/createuser", request, 
                                    new XmlMediaTypeFormatter { UseXmlSerializer = true });
            return await response.Content.ReadAsStringAsync();
        }

        private static async Task<string> Read(int id) {
            var response = await Client.GetAsync($"public/userinfo?id={id}");
            return await response.Content.ReadAsStringAsync();
        }

        private static async Task<string> Update(int id, string newStatus) {
            
            Client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestForm = $"id={id}&newStatus={newStatus}"; 

            var response = await Client.PostAsync("auth/setstatus", 
                                        new StringContent(requestForm, Encoding.Unicode, "application/x-www-form-urlencoded"));
            return await response.Content.ReadAsStringAsync();
        }

        private static async Task<string> Delete(int id) {
            var request = new DeleteRequestView {
                RemoveUser = new RemoveUserView {
                    Id = id
                }
            };
            
            var response = await Client.PostAsJsonAsync("auth/removeuser", request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}


using RestSharp;
using System.Net;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace RestSharpAPITests
{
    public class RestSharpAPI_Tests
    {
        private RestClient restClient;
        private const string myBaseUrl = "https://petyasshorturl.petyazh29.repl.co/";
        [SetUp]
        public void Setup()
        {
            restClient = new RestClient(myBaseUrl);
        }

        [Test]
        public void Test_List_All_Short_URLs()
        {
            var request = new RestRequest("api/urls", Method.Get);
            var response = restClient.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.Not.Empty);          
        }


        [Test]
        public void Test_Search_URL_by_ShortCode()
        {
            var request = new RestRequest("api/urls/nak", Method.Get);
            var response = restClient.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var urlObject = JsonSerializer.Deserialize<URL>(response.Content!);
            Assert.That(urlObject.url, Is.EqualTo("https://nakov.com"));


        }

        [Test]
        public void Test_Search_URL_by_Invalid_ShortCode()
        {
            var request = new RestRequest("api/urls/petya", Method.Get);
            var response = restClient.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));        
        }

        [Test]
        public void Test_Create_New_Short_URL()
        {
            var request = new RestRequest("api/urls", Method.Post);
            var createNewUrl = new
            {
                url = "https://alex.com" + DateTime.Now.Ticks,
                shortCode= "alex" +DateTime.Now.Ticks   
            };
            request.AddBody(createNewUrl);
            var response = restClient.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var urlS = JsonSerializer.Deserialize<NewURL>(response.Content!);

            Assert.That(urlS.msg, Is.EqualTo("Short code added."));

        }

        [Test]
        public void Test_Try_Create_New_Short_URL()
        {
            var request = new RestRequest("api/urls", Method.Post);
            var createNewUrl = new
            {
                url = "https://nakov.com",
                shortCode = "nak"
            };
            request.AddBody(createNewUrl);
            var response = restClient.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));           
        }
    }
}
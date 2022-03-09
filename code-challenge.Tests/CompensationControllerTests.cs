using challenge.Models;
using code_challenge.Tests.Integration.Extensions;
using code_challenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Text;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var compensation = new Compensation()
            {
                Salary = "Salary",
                EffectiveDate = "EffectiveDate",
                Employee = new Employee()
                {
                    EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f"
                }
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.CompensationId);
            Assert.IsNotNull(newCompensation.Employee.FirstName);
            Assert.IsNotNull(newCompensation.Employee.LastName);
            Assert.IsNotNull(newCompensation.Employee.Department);
            Assert.IsNotNull(newCompensation.Employee.Position);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
        }

        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            // Arrange
            var compensation = new Compensation()
            {
                Salary = "Salary",
                EffectiveDate = "EffectiveDate",
                Employee = new Employee()
                {
                    EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f"
                }
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask1 = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response1 = postRequestTask1.Result;

            // Execute
            var getRequestTask2 = _httpClient.GetAsync($"api/compensation/{compensation.Employee.EmployeeId}");
            var response2 = getRequestTask2.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
            var newCompensation = response2.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.CompensationId);
            Assert.IsNotNull(newCompensation.Employee.FirstName);
            Assert.IsNotNull(newCompensation.Employee.LastName);
            Assert.IsNotNull(newCompensation.Employee.Department);
            Assert.IsNotNull(newCompensation.Employee.Position);
            Assert.AreEqual(compensation.EffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(compensation.Salary, newCompensation.Salary);
        }

        [TestMethod]
        public void GetCompensationById_Returns_NotFound()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "Invalid_Id",
                Department = "Music",
                FirstName = "Sunny",
                LastName = "Bono",
                Position = "Singer/Song Writer",
            };
            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var postRequestTask = _httpClient.PutAsync($"api/compensation/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

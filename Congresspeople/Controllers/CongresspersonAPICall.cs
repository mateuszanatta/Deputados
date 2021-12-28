using Congressperson.Controllers.Interfaces;
using System.Threading.Tasks;
using System.Text.Json;
using Congressperson.HttpClients.Interfaces;
using Congressperson.Models.DTO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Threading;
using System;

namespace Congressperson.Controllers
{
    public class CongresspersonAPICall : ICongresspersonAPICall
    {
        public IHttpClients Client { get; set; }

        const string BaseUrl = "https://dadosabertos.camara.leg.br/api/v2/deputados";
        const int InitialYear = 2003; //Year data started becoming public
        public async Task<DTOCongressperson> GetCongresspeopleAsync()
        {
            return await ProcessCongresspeople();
        }

        private async Task<DTOCongressperson> ProcessCongresspeople()
        {
            using (var client = Client)
            {
                client.Clear();
                client.Add(mediaType: "application/json");
                DTOCongressperson congresspersonData = await JsonSerializer.DeserializeAsync<DTOCongressperson>(await client.GetStreamAsync(BaseUrl));
                await ExpensesCongressperson(client, congresspersonData);
                return congresspersonData;
            }
        }

        private async Task ExpensesCongressperson(IHttpClients client, DTOCongressperson congresspersonData)
        {
            foreach (var congressperson in congresspersonData.Congressperson)
            {
                DTOExpenses expenses = new();
                List<Expenses> expensesList = new List<Expenses>();
                string years = GenerateYearArrayParameter();
                string expensesUrl = BaseUrl + $"/{congressperson.IdCongressperson}/despesas?{years}";
                HttpResponseMessage response;
                do
                {
                    response = await client.GetAsync(expensesUrl);
                    if(response.IsSuccessStatusCode)
                    {
                        expenses = await JsonSerializer.DeserializeAsync<DTOExpenses>(await response.Content.ReadAsStreamAsync());
                        expensesList.AddRange(expenses.Expenses);
                        expensesUrl = expenses.Links.FirstOrDefault(link => link.Rel == "next")?.Href;
                    }
                    else if(HasTooManyRequests(response))
                    {
                        Thread.Sleep(response.Headers.RetryAfter.Delta ?? new TimeSpan(5000));
                        expensesUrl = expenses.Links.FirstOrDefault(link => link.Rel == "self")?.Href;
                    }
                } while (expenses.Links.Any(_ => _.Rel == "next"));

                congressperson.Expenses = expensesList;
            }
        }

        private static string GenerateYearArrayParameter()
        {
            var yearRange = Enumerable.Range(InitialYear, System.DateTime.Now.Year - InitialYear + 1);
            StringBuilder yearArrayParameter = new StringBuilder();
            foreach (int year in yearRange)
            {
                yearArrayParameter.AppendFormat("ano={0}&", year);
            }
            return yearArrayParameter.ToString();
        }

        private bool HasTooManyRequests(HttpResponseMessage response) => response.StatusCode == HttpStatusCode.TooManyRequests;
        
    }
}

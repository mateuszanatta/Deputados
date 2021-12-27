using Deputados.Controllers.Interfaces;
using System.Threading.Tasks;
using System.Text.Json;
using Deputados.Models;
using Deputados.HttpClients.Interfaces;
using Deputados.Models.DTO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Threading;
using System;

namespace Deputados.Controllers
{
    public class DeputadosAPICall : IDeputadosAPICall
    {
        public IHttpClients Client { get; set; }

        const string BaseUrl = "https://dadosabertos.camara.leg.br/api/v2/deputados";
        const int InitialYear = 2003; //Year data started becoming public
        public async Task<Dados> GetDadosAsync()
        {
            return await ProcessDeputados();
        }

        private async Task<Dados> ProcessDeputados()
        {
            Client.Clear();
            Client.Add(mediaType: "application/json");
            Dados dados = await JsonSerializer.DeserializeAsync<Dados>(await Client.GetStreamAsync(BaseUrl));
            await ExpensesDeputados(dados);
            return dados;
        }

        private async Task ExpensesDeputados(Dados dados)
        {
            foreach (var deputado in dados.Deputados)
            {
                DTOExpenses expenses = new();
                List<Expenses> expensesList = new List<Expenses>();
                string years = GenerateYearArrayParameter();
                string expensesUrl = BaseUrl + $"/{deputado.IdDeputado}/despesas?{years}";
                HttpResponseMessage response;
                do
                {
                    response = await Client.GetAsync(expensesUrl);
                    if(response.IsSuccessStatusCode)
                    {
                        expenses = await JsonSerializer.DeserializeAsync<DTOExpenses>(await response.Content.ReadAsStreamAsync());
                        expensesList.AddRange(expenses.Expenses);
                        expensesUrl = expenses.Links.FirstOrDefault(link => link.Rel == "next")?.Href;
                    }
                    else if(response.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        Thread.Sleep(response.Headers.RetryAfter.Delta ?? new TimeSpan(5000));
                        expensesUrl = expenses.Links.FirstOrDefault(link => link.Rel == "self")?.Href;
                    }
                } while (expenses.Links.Any(_ => _.Rel == "next"));

                deputado.Expenses = expensesList;
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
    }
}

using Deputados.Controllers.Interfaces;
using System.Threading.Tasks;
using System.Text.Json;
using Deputados.Models;
using Deputados.HttpClients.Interfaces;
using Deputados.Models.DTO;
using System.Linq;
using System.Collections.Generic;

namespace Deputados.Controllers
{
    public class DeputadosAPICall : IDeputadosAPICall
    {
        public IHttpClients Client { get; set; }

        const string BaseUrl = "https://dadosabertos.camara.leg.br/api/v2/deputados";

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
            //return await JsonSerializer.DeserializeAsync<Dados>(await Client.GetStreamAsync(BaseUrl));

        }

        private async Task ExpensesDeputados(Dados dados)
        {
            foreach (var deputado in dados.Deputados)
            {
                string expensesUrl = BaseUrl + $"/{deputado.IdDeputado}/despesas";
                DTOExpenses expenses = new();
                List<Expenses> expensesList = new List<Expenses>();

                do
                {
                    /// TODO: Figure out how to retrieve all expenses from preivous years
                    /// Now it only retrieve the expenses of the last 6 months
                    expenses = await JsonSerializer.DeserializeAsync<DTOExpenses>(await Client.GetStreamAsync(expensesUrl));
                    expensesList.AddRange(expenses.Expenses);
                    expensesUrl = expenses.Links.FirstOrDefault(link => link.Rel == "next")?.Href;

                } while (expenses.Links.Any(_ => _.Rel == "next"));
                deputado.Expenses = expensesList;
            }
        }
    }
}

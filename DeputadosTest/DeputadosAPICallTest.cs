using Deputados.Controllers;
using Deputados.Controllers.Interfaces;
using Deputados.HttpClients.Interfaces;
using Deputados.Models;
using FakeItEasy;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Deputados.Models.DTO;

namespace DeputadosTest
{
    public class DeputadosAPICallTest
    {
        IDeputadosAPICall _deputadosCall;
        IHttpClients _httpClient;

        [OneTimeSetUp]
        public void Setup()
        {
            _httpClient = A.Fake<IHttpClients>();
            _deputadosCall = new DeputadosAPICall();
            _deputadosCall.Client = _httpClient;
        }

        [Test]
        public async Task GetDeputados_ShouldReturn_ListOfDeputadosAsync()
        {
            var mediaType = "application/json";
            var baseURL = "https://dadosabertos.camara.leg.br/api/v2/deputados";
            var dados = CreateDados();
            var expenses = CreateExpenses();
            string expensesUrl = baseURL + $"/{dados.Deputados.ToList()[0].IdDeputado}/despesas";

            Task<Stream> streamDados = CreateJsonStream(dados);
            Task<Stream> streamExpenses = CreateJsonStream(expenses);

            A.CallTo(() => _httpClient.Clear());
            A.CallTo(() => _httpClient.Add(A.Dummy<string>()));
            A.CallTo(() => _httpClient.GetStreamAsync(baseURL)).Returns(streamDados).Once();
            A.CallTo(() => _httpClient.GetStreamAsync(expensesUrl)).Returns(streamExpenses).Once();

            var _deputadosReturn = await _deputadosCall.GetDadosAsync();

            Assert.AreEqual(dados.Deputados.ToList()[0].IdDeputado, _deputadosReturn.Deputados.ToList()[0].IdDeputado);
            Assert.AreEqual(dados.Deputados.ToList()[0].Expenses.ToList()[0].Ano, _deputadosReturn.Deputados.ToList()[0].Expenses.ToList()[0].Ano);
            A.CallTo(() => _httpClient.Clear()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _httpClient.Add(mediaType)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _httpClient.GetStreamAsync(baseURL)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _httpClient.GetStreamAsync(expensesUrl)).MustHaveHappenedOnceExactly();

        }

        private Dados CreateDados()
        {
            return new Dados()
            {
                Deputados = new List<Deputado>()
                {
                    new Deputado
                    {
                        IdDeputado = 204554,
                        URI = "https://dadosabertos.camara.leg.br/api/v2/deputados/204554",
                        Nome = "Abílio Santana",
                        SiglaPartido = "PL",
                        URIPartido = "https://dadosabertos.camara.leg.br/api/v2/partidos/37906",
                        UF = "BA",
                        IdLegislatura = 56,
                        URLFoto = "https://www.camara.leg.br/internet/deputado/bandep/204554.jpg",
                        Email = "dep.abiliosantana@camara.leg.br",
                        Expenses = new List<Expenses>
                        {
                            new Expenses
                            {
                                Ano = 0,
                                Mes = 0,
                                TipoDespesa = "",
                                TipoDocumento = "",
                                ValorDocumento = 0,
                                ValorGlosa = 0,
                                valorLiquido = 0
                            }
                        }
                    }
                },
                Links = new List<Links>()
                {
                    new Links
                    {
                        Rel = "",
                        Href = ""
                    }
                }
            };
        }

        private static DTOExpenses CreateExpenses() => new DTOExpenses()
        {
            Expenses = new List<Expenses>
            {
                new Expenses
                {
                    Ano = 0,
                    Mes = 0,
                    TipoDespesa = "",
                    TipoDocumento = "",
                    ValorDocumento = 0,
                    ValorGlosa = 0,
                    valorLiquido = 0
                }
            },
            Links = new List<Links>
            {
                new Links
                {
                    Rel = "",
                    Href = ""
                }
            }
        };


        private static async Task<Stream> CreateJsonStream<T>(T dados)
        {
            Stream streamDados = new MemoryStream();
            await JsonSerializer.SerializeAsync(streamDados, dados, typeof(T));
            streamDados.Position = 0;
            return streamDados;
        }
    }
}
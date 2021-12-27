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
using System.Net.Http;
using System.Net;

namespace DeputadosTest
{
    public class DeputadosAPICallTest
    {
        IDeputadosAPICall _deputadosCall;

        [OneTimeSetUp]
        public void Setup()
        {
            IHttpClients _httpClient = A.Fake<IHttpClients>();
            _deputadosCall = new DeputadosAPICall();
            _deputadosCall.Client = _httpClient;
        }

        [Test]
        public async Task GetDeputados_ShouldReturn_ListOfDeputadosAsync()
        {
            var dados = CreateDados();
            var expenses = CreateExpenses();

            Stream dataStream = await CreateJsonStream(dados);
            var responseMessage = await CreateHttpResponseMessageStream(expenses);

            A.CallTo(() => _deputadosCall.Client.Clear());
            A.CallTo(() => _deputadosCall.Client.Add(A<string>.Ignored));
            A.CallTo(() => _deputadosCall.Client.GetStreamAsync(A<string>.Ignored))
                .Returns(dataStream).Once();
            A.CallTo(() => _deputadosCall.Client.GetAsync(A<string>.Ignored))
                .Returns(responseMessage).Once();

            var _deputadosReturn = await _deputadosCall.GetDadosAsync();

            Assert.AreEqual(dados.Deputados.ToList()[0].IdDeputado, _deputadosReturn.Deputados.ToList()[0].IdDeputado);
            Assert.AreEqual(dados.Deputados.ToList()[0].Expenses.ToList()[0].Ano, _deputadosReturn.Deputados.ToList()[0].Expenses.ToList()[0].Ano);
            A.CallTo(() => _deputadosCall.Client.Clear()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _deputadosCall.Client.Add(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _deputadosCall.Client.GetStreamAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _deputadosCall.Client.GetAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
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
                        Nome = "Ab�lio Santana",
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
                                Ano = 2021,
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
                    Ano = 2021,
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

        private static async Task<Stream> CreateJsonStream<T>(T data)
        {
            Stream dataStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(dataStream, data, typeof(T));
            dataStream.Position = 0;
            return dataStream;
        }

        private async Task<HttpResponseMessage> CreateHttpResponseMessageStream<T>(T data)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(await CreateJsonStream(data))
            };
        }
    }
}
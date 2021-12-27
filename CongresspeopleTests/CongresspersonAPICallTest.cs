using Congressperson.Controllers;
using Congressperson.Controllers.Interfaces;
using Congressperson.HttpClients.Interfaces;
using Congressperson.Models;
using FakeItEasy;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Congressperson.Models.DTO;
using System.Net.Http;
using System.Net;

namespace CongresspersonTest
{
    public class CongresspersonAPICallTest
    {
        ICongresspersonAPICall _congresspersonCall;

        [OneTimeSetUp]
        public void Setup()
        {
            IHttpClients _httpClient = A.Fake<IHttpClients>();
            _congresspersonCall = new CongresspersonAPICall();
            _congresspersonCall.Client = _httpClient;
        }

        [Test]
        public async Task GetCongresspeople_ShouldReturn_ListOfCongresspeopleAsync()
        {
            var dados = CreateDados();
            var expenses = CreateExpenses();

            Stream dataStream = await CreateJsonStream(dados);
            var responseMessage = await CreateHttpResponseMessageStream(expenses);

            A.CallTo(() => _congresspersonCall.Client.Clear());
            A.CallTo(() => _congresspersonCall.Client.Add(A<string>.Ignored));
            A.CallTo(() => _congresspersonCall.Client.GetStreamAsync(A<string>.Ignored))
                .Returns(dataStream).Once();
            A.CallTo(() => _congresspersonCall.Client.GetAsync(A<string>.Ignored))
                .Returns(responseMessage).Once();

            var _congresspeopleReturn = await _congresspersonCall.GetCongresspeopleAsync();

            Assert.AreEqual(dados.Congressperson.ToList()[0].IdCongressperson, _congresspeopleReturn.Congressperson.ToList()[0].IdCongressperson);
            Assert.AreEqual(dados.Congressperson.ToList()[0].Expenses.ToList()[0].Year, _congresspeopleReturn.Congressperson.ToList()[0].Expenses.ToList()[0].Year);
            A.CallTo(() => _congresspersonCall.Client.Clear()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _congresspersonCall.Client.Add(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _congresspersonCall.Client.GetStreamAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _congresspersonCall.Client.GetAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
        }

        private DTOCongressperson CreateDados()
        {
            return new DTOCongressperson()
            {
                Congressperson = new List<Congressperson.Models.Congressperson>()
                {
                    new Congressperson.Models.Congressperson
                    {
                        IdCongressperson = 204554,
                        URI = "https://dadosabertos.camara.leg.br/api/v2/deputados/204554",
                        Name = "Ab�lio Santana",
                        PartyAcronym = "PL",
                        URIParty = "https://dadosabertos.camara.leg.br/api/v2/partidos/37906",
                        State = "BA",
                        IdLegislature = 56,
                        PictureURL = "https://www.camara.leg.br/internet/deputado/bandep/204554.jpg",
                        Email = "dep.abiliosantana@camara.leg.br",
                        Expenses = new List<Expenses>
                        {
                            new Expenses
                            {
                                Year = 2021,
                                Month = 0,
                                ExpenseType = "",
                                TypeDocument = "",
                                GrossAmount = 0,
                                NonRefundableAmount = 0,
                                NetAmount = 0
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
                    Year = 2021,
                    Month = 0,
                    ExpenseType = "",
                    TypeDocument = "",
                    GrossAmount = 0,
                    NonRefundableAmount = 0,
                    NetAmount = 0
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
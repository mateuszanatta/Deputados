using Deputados.Controllers.Interfaces;
using Deputados.HttpClients.Interfaces;
using Deputados.Models;
using Deputados.Models.DTO;
using Deputados.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Deputados.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeputadosController : ControllerBase
    {
        private readonly IHttpClients _client;
        private readonly IDeputadosAPICall _deputadosAPI;
        private readonly IDeputadosService _deputadoService;
        public DeputadosController(IHttpClients httpClient, IDeputadosAPICall deputadosAPI, IDeputadosService deputadosService)
        {
            _client = httpClient;
            _deputadosAPI = deputadosAPI;
            _deputadosAPI.Client = _client;
            _deputadoService = deputadosService;
        }
        [HttpGet("~/GetDadosFromAPI")]
        public async Task<ActionResult<Dados>> GetDadosFromAPI()
        {
            try
            {
                return await _deputadosAPI.GetDadosAsync();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("~/GetDeputados")]
        public IEnumerable GetDeputados() => _deputadoService.Get();

        [HttpGet("~/GetDeputado/{idDeputado}")]
        public Deputado GetDeputado(int idDeputado) => _deputadoService.GetByIdDeputado(idDeputado);

        [HttpGet("~/GetDeputadosStatistics/{idDeputado}")]
        public DeputadoStatistics GetDeputadosStatistics(int idDeputado) => _deputadoService.GetDeputadoStatistics(idDeputado);

        [HttpPost]
        public async Task<ActionResult> InsertDeputadosFromAPIToDatabase()
        {
            try
            {
                var dados = await _deputadosAPI.GetDadosAsync();
                _deputadoService.InsertMany(dados.Deputados);
                return StatusCode(201, "Internal server error");
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }

        }
    }
}

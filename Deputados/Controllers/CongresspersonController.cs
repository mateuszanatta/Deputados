using Congressperson.Controllers.Interfaces;
using Congressperson.HttpClients.Interfaces;
using Congressperson.Models.DTO;
using Congressperson.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace Congressperson.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CongresspersonController : ControllerBase
    {
        private readonly IHttpClients _client;
        private readonly ICongresspersonAPICall _deputadosAPI;
        private readonly ICongresspersonService _deputadoService;
        public CongresspersonController(IHttpClients httpClient, ICongresspersonAPICall deputadosAPI, ICongresspersonService deputadosService)
        {
            _client = httpClient;
            _deputadosAPI = deputadosAPI;
            _deputadosAPI.Client = _client;
            _deputadoService = deputadosService;
        }
        [HttpGet("~/GetDadosFromAPI")]
        public async Task<ActionResult<DTOCongressperson>> GetDadosFromAPI()
        {
            try
            {
                return await _deputadosAPI.GetCongresspeopleAsync();
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("~/GetDeputados")]
        public IEnumerable GetDeputados() => _deputadoService.Get();

        [HttpGet("~/GetDeputado/{idDeputado}")]
        public Models.Congressperson GetDeputado(int idDeputado) => _deputadoService.GetByIdDeputado(idDeputado);

        [HttpGet("~/GetDeputadosStatistics/{idDeputado}")]
        public CongresspersonStatistics GetDeputadosStatistics(int idDeputado) => _deputadoService.GetDeputadoStatistics(idDeputado);

        [HttpPost]
        public async Task<ActionResult> InsertDeputadosFromAPIToDatabase()
        {
            try
            {
                var dados = await _deputadosAPI.GetCongresspeopleAsync();
                _deputadoService.InsertMany(dados.Congressperson);
                return StatusCode(201, "Internal server error");
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }

        }
    }
}

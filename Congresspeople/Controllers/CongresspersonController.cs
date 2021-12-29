using System;
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
        private readonly ICongresspersonAPICall _congresspersonAPI;
        private readonly ICongresspersonService _congresspersonService;
        public CongresspersonController(IHttpClients httpClient, ICongresspersonAPICall congresspersonAPI, ICongresspersonService congresspersonService)
        {
            _congresspersonAPI = congresspersonAPI;
            _congresspersonAPI.Client = httpClient;
            _congresspersonService = congresspersonService;
        }
        [HttpGet("~/GetCongresspersonDataFromAPI")]
        public async Task<ActionResult<DTOCongressperson>> GetCongresspersonDataFromAPI()
        {
            try
            {
                return await _congresspersonAPI.GetCongresspeopleAsync();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("~/GetCongressperson")]
        public IEnumerable GetCongressperson() => _congresspersonService.Get();

        [HttpGet("~/GetCongressperson/{idCongressperson}")]
        public Models.Congressperson GetCongressperson(int idCongressperson) => _congresspersonService.GetByIdCongressperson(idCongressperson);

        [HttpGet("~/GetCongresspersonStatistics/{idCongressperson}")]
        public CongresspersonStatistics GetCongresspersonStatistics(int idCongressperson) => _congresspersonService.GetCongresspersonStatistics(idCongressperson);

        [HttpPost]
        public async Task<ActionResult> InsertCongresspersonFromAPIToDatabase()
        {
            try
            {
                var dados = await _congresspersonAPI.GetCongresspeopleAsync();
                _congresspersonService.InsertMany(dados.Congressperson);
                return StatusCode(201, "Internal server error");
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }

        }
    }
}

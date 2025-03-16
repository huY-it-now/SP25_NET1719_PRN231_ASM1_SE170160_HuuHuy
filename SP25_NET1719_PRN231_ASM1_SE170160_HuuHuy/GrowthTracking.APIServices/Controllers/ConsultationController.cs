using GrowthTracking.Repository.Models;
using GrowthTracking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GrowthTracking.APIServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConsultationController : ControllerBase
    {
        private readonly IConsultationService _consultationService;

        public ConsultationController(IConsultationService consultationService)
        {
            _consultationService = consultationService;
        }

        // GET: api/<ConsultationController>
        [HttpGet]
        [Authorize(Roles = "1,2")]
        [EnableQuery]
        public async Task<IEnumerable<Consultation>> Get()
        {
            //return new string[] { "value1", "value2" };
            return await _consultationService.GetAll();
        }
        [HttpGet("{consultationId}/{userId}")]
        public async Task<IEnumerable<Consultation>> Get(Guid consultationId, Guid userId)
        {
            return await _consultationService.Search(consultationId, userId);
        }


        // GET api/<ConsultationController>/5
        [HttpGet("{id}")]
        public async Task<Consultation> Get(Guid id)
        {
            return await _consultationService.GetById(id);
        }

        // POST api/<ConsultationController>
        [HttpPost]
        public async Task<Guid> Post(Consultation consultation)
        {
            return await _consultationService.Create(consultation);
        }

        // PUT api/<ConsultationController>/5
        [HttpPut("{id}")]
        public async Task<int> Put(Consultation consultation)
        {
            return await _consultationService.Update(consultation);
        }

        // DELETE api/<ConsultationController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await _consultationService.Delete(id);
        }
    }
}

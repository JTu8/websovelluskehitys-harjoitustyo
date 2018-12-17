using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SAmiHelper.Models;

namespace SAmiHelper.Controllers
{
    [Produces("application/json")]
    [Route("api/SearchData")]
    public class SearchDataController : Controller
    {
        private SamiService _service;

        public SearchDataController(SamiService service)
        {
            this._service = service;
        }

        // GET: api/SearchData
        [HttpGet]
        public async Task<List<SamiMeasurementModel>> Get()
        {
            string jsonData = HttpContext.Session.GetString("search-model");
            SearchModel model = JsonConvert.DeserializeObject<SearchModel>(jsonData);

            var data = await _service.GetSamiMeasurements(model);
            return data;
        }

        
    }
}

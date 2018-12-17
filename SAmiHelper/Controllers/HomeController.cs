using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAmiHelper.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using SAmiHelper.Data;
using System.Web;
using Microsoft.AspNetCore.Identity;

namespace SAmiHelper.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private SamiService _service;

        public HomeController(SamiService service, UserManager<ApplicationUser> userManager)
        {
            this._service = service;
            this._userManager = userManager;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult SAmIHaku()
        {
            SearchModel model = null;
            string jsonData = HttpContext.Session.GetString("search-model");
            if(false == string.IsNullOrEmpty(jsonData))
            {
                model = JsonConvert.DeserializeObject<SearchModel>(jsonData);
            }
            else
            {
                model = new SearchModel
                {
                    From = new DateTime(2018, 2, 6)
                };
            }

            
            return View(model); 
        }

        

        [HttpPost]
        public async Task<IActionResult> Search(SearchModel search)
        {
            string json = JsonConvert.SerializeObject(search);
            HttpContext.Session.SetString("search-model", json);

            var userID = _userManager.GetUserId(HttpContext.User);
            ApplicationUser user = _userManager.FindByIdAsync(userID).Result;

            Debug.WriteLine(user);

            var sensors = await _service.GetSensors(search);
            
            //Lisätään avain tietokantaan
            using (var connection = new SqliteConnection("" +
                new SqliteConnectionStringBuilder
                {
                    DataSource = "SAmiHelper"
                }))

            {
                connection.Open();

                using (var t = connection.BeginTransaction())
                {
                    try
                    {
                        var insertCommant = connection.CreateCommand();
                        insertCommant.Transaction = t;
                        insertCommant.CommandText = "INSERT INTO Profiili (UserID, avain) VALUES ($id, $key)";
                        insertCommant.Parameters.AddWithValue("$id", userID);
                        insertCommant.Parameters.AddWithValue("$key", search.Key.ToString());
                        insertCommant.ExecuteNonQuery();

                        t.Commit();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }

                connection.Close();
            }
            return View(new SensorSelectViewModel()
            {
                SearchModel = search,
                Sensors = sensors
            });


        }

        [HttpPost]
        public IActionResult SearchData(SearchModel model)
        {
            string jsonData = HttpContext.Session.GetString("search-model");
            SearchModel searchModel = JsonConvert.DeserializeObject<SearchModel>(jsonData);
            searchModel.To = model.From.HasValue ? model.From.Value.AddDays(1) : default(DateTime?);
            searchModel.Sensors = model.Sensors;

            string json = JsonConvert.SerializeObject(searchModel);
            HttpContext.Session.SetString("search-model", json);

            return View("SensorGraph");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

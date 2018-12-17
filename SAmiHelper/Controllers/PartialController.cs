using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SAmiHelper.Models;

namespace SAmiHelper.Controllers
{
    public class PartialController : Controller
    {
       

        public PartialController(SearchCounter counter)
        {
           
            if (null == counter)
            {
                counter = new SearchCounter();
            }
            counter.SearchCount++;
            counter.SearchTime = DateTime.Now;
        }
    }
}
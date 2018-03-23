using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Web.Sys.Admin
{
    public class IndexModel : PageModel
    {
        public IConfiguration Configuration;

        public IndexModel()
        {
               
        }

        public void OnGet()
        {

        }

        public async Task<JsonResult> OnPostGetMenuAsync()
        {
            return new JsonResult("{ 'name':'1','age':10}");
        }
    }
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}

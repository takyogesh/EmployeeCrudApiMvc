using ConsumeEmployeeWebApiWebApp_29_07.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConsumeEmployeeWebApiWebApp_29_07.Controllers
{
    public class EmployeeController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7157/");

        HttpClient httpClient = new HttpClient();
        public EmployeeController()
        {
            httpClient.BaseAddress = baseAddress;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEmployee(Employee employee)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage();
                var data = JsonConvert.SerializeObject(employee);
                var contentData = new StringContent(data, System.Text.Encoding.UTF8,"application/json");

                HttpResponseMessage response = httpClient.PostAsync(baseAddress+"Employees/Create", contentData).Result;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return Content("ooh");
            }
        }

        //[HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage Response = httpClient.GetAsync(baseAddress + "Employees/Index").Result;
            if (Response.IsSuccessStatusCode)
            {
                string data = Response.Content.ReadAsStringAsync().Result;
                List<Employee> list = new List<Employee>();
                list = JsonConvert.DeserializeObject<List<Employee>>(data);
                return View(list);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string SearchQuery)
        {
            // string SearchQuery = Request.Form["SearchQuery"];
            if (SearchQuery == null)
            {
                return View();
            }
            HttpResponseMessage response = httpClient.GetAsync(baseAddress + "Employees/Details/search?search="+SearchQuery).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;

            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(stringData);
            if (employees == null)
            {
                return NotFound(SearchQuery);
            }
            return View(employees);
        }

        public IActionResult CreateNewEmployee()
        {
            return View();
        }
        [HttpGet("id")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                HttpResponseMessage response = httpClient.DeleteAsync(baseAddress + "Employees/Delete/id?id=" + id).Result;
                if(response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch
            {
                return RedirectToAction(nameof(Index));


            }
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public IActionResult UpdateEmployee(int Id)
        {
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(baseAddress + "Employees/GetEmployee/id?Id=" + Id).Result;
                string stringData = response.Content.ReadAsStringAsync().Result;
                Employee data = JsonConvert.DeserializeObject<Employee>(stringData);
                if (data != null)
                {
                    return View(data);
                }

            }
            catch
            {
                return RedirectToAction(nameof(UpdateEmployee));
            }

            return RedirectToAction(nameof(UpdateEmployee));
        }
        [HttpPost]
        public IActionResult UpdateEmployee(Employee employee)
        {
            try
            {

                string stringData = JsonConvert.SerializeObject(employee);
                var contentData = new StringContent(stringData,System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PutAsync(baseAddress+"Employees/Edit",contentData).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    return RedirectToAction(nameof(UpdateEmployee));
                }
                

            }
            catch
            {
                return RedirectToAction(nameof(UpdateEmployee));

            }
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            HttpResponseMessage response = httpClient.GetAsync(baseAddress + "Employees/GetEmployee/id?Id=" + id).Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
            Employee data = JsonConvert.DeserializeObject<Employee>(stringData);

            return View(data);
        }
    }
}

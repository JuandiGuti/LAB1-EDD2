using LAB1_EDD2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LAB1_EDD2.Controllers
{
    public class HomeController : Controller
    {
        public static List<string> instruccionsList = new List<string>();
        public static List<person> jsonListString = new List<person>();


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public bool insert(person personJson, bool canBeInserted)
        {
			if (jsonListString.Count() > 0)
			{
				foreach (person foreachPerson in jsonListString)
				{
					if (foreachPerson.dpi == personJson.dpi)
					{
						canBeInserted = false;
					}
				}
				if (canBeInserted)
				{
					jsonListString.Add(personJson);

				}
			}
			else if (canBeInserted)
			{
				jsonListString.Add(personJson);
			}
			else
			{
				ViewBag.Error = "Error al ingresar el Json";
			}
			return true;
        }

        [HttpPost("load-interface")]
        public IActionResult load(IFormFile file)
        {
            if (file != null)
            {
                try
                {
                    string path = Path.Combine(Path.GetTempPath(), file.Name);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    string fullFile = System.IO.File.ReadAllText(path);

                    foreach (string line in fullFile.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] actualRow = line.Split(';');
                            bool canBeInserted = true;
                            person personJson = JsonSerializer.Deserialize<person>(actualRow[1]);
                            if (actualRow[0] == "INSERT")
                            {
                                insert(personJson, canBeInserted);
							}
                            else if (actualRow[0] == "PATCH")
                            {
                                foreach (person foreachPerson in jsonListString)
                                {
                                    if (foreachPerson == personJson)
                                    {
                                        foreachPerson.datebirth = personJson.datebirth;
										foreachPerson.address = personJson.address;
									}
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Error = "Error al leer el archivo" + e.Message;
                }
            }
            else
            {
                ViewBag.Error = "No se ha ingresado la ruta de archivo";
            }
            return View(/*Mandar lista con elementos*/);
        }
        [Route("load-interface")]
        public IActionResult load()
        {
            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace LAB1_EDD2.Controllers
{
    [Route("[controller]")]
    public class FileLoaderController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
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
							string[] actualRow = line.Split(',');
							//Logica de ingreso de datos
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
	}
}

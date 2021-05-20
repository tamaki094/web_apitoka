
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebToka.Models;

namespace WebToka.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index(int pagina = 1)
        {
            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync("http://localhost:8080/api/personasfisicas");

            var personas = JsonConvert.DeserializeObject<List<PersonaFisica>>(json);

            var cantidadregistrosporpagina = 2;

            var personas_mostrar = personas.OrderBy(x => x.IdPersonaFisica)
                .Skip((pagina - 1) * cantidadregistrosporpagina) //luego de saltar registros
                .Take(cantidadregistrosporpagina).ToList(); //tomar la cantidad de registros por pagina

            var totalRegistros = personas.Count();



            var modelo = new IndexViewModel();
            modelo.PersonasFisicas = personas_mostrar;
            modelo.PaginaActual = pagina;
            modelo.TotalRegistros = totalRegistros;
            modelo.RegistroPorPagina = cantidadregistrosporpagina;


            return View(modelo);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

       public ActionResult CrearPersonaFisica()
        {
            return View();
        }
    }

}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebToka.Models;
using System.Net.Http.Json;
using System.IO;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace WebToka.Controllers
{
    public class HomeController : Controller
    {
        string base_url = "http://localhost:8080/";

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

        public async Task<ActionResult> putPersona()
        {
            
            Console.WriteLine(Request.Params);

            string nombre = Request.Params["txtNombre"];
            string apellidop = Request.Params["txtApellidoP"];
            string apellidom = Request.Params["txtApellidoM"];
            string fechan = Request.Params["txtFechaNac"];
            DateTime fecha_nac = Convert.ToDateTime(fechan);
            string rfc = Request.Params["txtRfc"];

            PersonaFisica persona = new PersonaFisica()
            {
                Nombre = nombre,
                ApellidoPaterno = apellidom,
                ApellidoMaterno= apellidom,
                FechaNacimiento = fecha_nac,
                RFC = rfc,
                UsuarioAgrega = 1
            };
            using (var httpClinte = new HttpClient())
            {
                var respuesta = await httpClinte.PostAsJsonAsync(base_url + "api/personafisica", persona);

                if(respuesta.IsSuccessStatusCode)
                {
                    string mensaje = await respuesta.Content.ReadAsStringAsync();
                    dynamic msj_json = JObject.Parse(mensaje);
                    ViewData["Mensaje"] = msj_json.MENSAJEERROR;
                    ViewData["Codigo"] = msj_json.ERROR;
                }
            }
            return View("CrearPersonaFisica");
        }

       public ActionResult CrearPersonaFisica()
        {
            return View();
        }
    }

}
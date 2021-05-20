
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

            var json = await httpClient.GetStringAsync(base_url + "api/personasfisicas");

            var personas = JsonConvert.DeserializeObject<List<PersonaFisica>>(json);

            var cantidadregistrosporpagina = 5;

            var personas_mostrar = personas.OrderBy(x => x.IdPersonaFisica)
                .Skip((pagina - 1) * cantidadregistrosporpagina) //luego de saltar registros
                .Take(cantidadregistrosporpagina).ToList(); //tomar la cantidad de registros por pagina

            var totalRegistros = personas.Count();



            var modelo = new IndexViewModel();
            modelo.PersonasFisicas = personas_mostrar;
            modelo.PaginaActual = pagina;
            modelo.TotalRegistros = totalRegistros;
            modelo.RegistroPorPagina = cantidadregistrosporpagina;


            return View("Index",modelo);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public async Task<ActionResult> Contact()
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
            var path = Request.QueryString;
            string accion = "";
            if(path.Count >= 1)
            {
                var idPersonaFisica = Request["idPersonaFisica"];
                var httpClient = new HttpClient();
                var json = await httpClient.GetStringAsync(string.Format("{0}api/personafisica/{1}", base_url, idPersonaFisica));
                var personas = JsonConvert.DeserializeObject<List<PersonaFisica>>(json);

                var modelo = new IndexViewModel();
                modelo.PersonasFisicas = personas;
                modelo.PaginaActual = 1;
                modelo.TotalRegistros = 1;
                modelo.RegistroPorPagina = 1;

                return View("CrearPersonaFisica", modelo);
            }
            else //-------------------POST
            {
                PersonaFisica persona;
                if (!string.IsNullOrEmpty(Request.Params["IdPersonaFisica"]))
                {
                    accion = string.Format("api/personafisica/{0}", Request.Params["IdPersonaFisica"]);

                    string nombre = Request.Params["txtNombre"];
                    string apellidop = Request.Params["txtApellidoP"];
                    string apellidom = Request.Params["txtApellidoM"];
                    string fechan = Request.Params["txtFechaNac"];
                    DateTime fecha_nac = Convert.ToDateTime(fechan);
                    string rfc = Request.Params["txtRfc"];

                    persona = new PersonaFisica()
                    {
                        IdPersonaFisica = Convert.ToInt32(Request.Params["IdPersonaFisica"]),
                        Nombre = nombre,
                        ApellidoPaterno = apellidop,
                        ApellidoMaterno = apellidom,
                        FechaNacimiento = fecha_nac,
                        RFC = rfc,
                        UsuarioAgrega = 1,
                        Activo= true
                    };

                    using (var httpClinte = new HttpClient())
                    {
                        var respuesta = await httpClinte.PutAsJsonAsync(string.Format("{0}{1}", base_url, accion), persona);

                        if (respuesta.IsSuccessStatusCode)
                        {
                            string mensaje = await respuesta.Content.ReadAsStringAsync();
                            dynamic msj_json = JObject.Parse(mensaje);
                            ViewData["Mensaje"] = msj_json.MENSAJEERROR;
                            ViewData["Codigo"] = msj_json.ERROR;
                        }
                    }
                    return await Index(1);
                }
                else
                {
                    accion = "api/personafisica";

                    string nombre = Request.Params["txtNombre"];
                    string apellidop = Request.Params["txtApellidoP"];
                    string apellidom = Request.Params["txtApellidoM"];
                    string fechan = Request.Params["txtFechaNac"];
                    DateTime fecha_nac = Convert.ToDateTime(fechan);
                    string rfc = Request.Params["txtRfc"];
                    
                    persona = new PersonaFisica()
                    {
                        Nombre = nombre,
                        ApellidoPaterno = apellidom,
                        ApellidoMaterno = apellidom,
                        FechaNacimiento = fecha_nac,
                        RFC = rfc,
                        UsuarioAgrega = 1
                    };

                    using (var httpClinte = new HttpClient())
                    {
                        var respuesta = await httpClinte.PostAsJsonAsync(string.Format("{0}{1}", base_url, accion), persona);

                        if (respuesta.IsSuccessStatusCode)
                        {
                            string mensaje = await respuesta.Content.ReadAsStringAsync();
                            dynamic msj_json = JObject.Parse(mensaje);
                            ViewData["Mensaje"] = msj_json.MENSAJEERROR;
                            ViewData["Codigo"] = msj_json.ERROR;
                        }
                    }
                    return View("CrearPersonaFisica");
                }                          
            }         
        }


        public async Task<ActionResult> deletePersona()
        {
            if (!string.IsNullOrEmpty(Request.Params["IdPersonaFisica"]))
            {
                var IdPersonaFisica = Request.Params["IdPersonaFisica"];
                var Activar = Request.Params["Activar"];
                string accion = string.Format("api/personafisica/{0}", IdPersonaFisica);

                using (var httpClinte = new HttpClient())
                {
                    var respuesta = await httpClinte.DeleteAsync(string.Format("{0}{1}", base_url, accion));

                    if (respuesta.IsSuccessStatusCode)
                    {
                        string mensaje = await respuesta.Content.ReadAsStringAsync();
                        dynamic msj_json = JObject.Parse(mensaje);
                        ViewData["Mensaje"] = msj_json.MENSAJEERROR;
                        ViewData["Codigo"] = msj_json.ERROR;
                    }
                }


            }
            return await Index(1);
        }
        public ActionResult CrearPersonaFisica()
        {
            return View();
        }
    }

}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjetoIntegradoFront.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProjetoIntegradoFront.Controllers
{
    public class HomeController : Controller
    {
        private string tokenUsuario = "";
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(Usuario usuario)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:58693/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"autenticacao/Login/login?name={usuario.Email}&password={usuario.Senha}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var dicionarioToken = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    if (dicionarioToken != null && dicionarioToken.Count > 0)
                    {
                        if (dicionarioToken.Keys.Any(x => x == "access_token"))
                            tokenUsuario = dicionarioToken.Values.FirstOrDefault();
                    }
                }

                if (string.IsNullOrEmpty(tokenUsuario))
                {
                    ViewData["Message"] = "Dados invalidos";
                    tokenUsuario = "";
                    return View("Index");
                }
                else { ViewData["Message"] = ""; }

                SalvaTokenSessao(tokenUsuario);
            }
            return RedirectToAction("Index", "OrdemServico");
        }


        public void SalvaTokenSessao(string tokenUsuario)
        {
            HttpContext.Session.SetString("token", tokenUsuario);
        }

        public string ObterTokenSessao()
        {
            return HttpContext.Session.GetString("token");
        }

    }
}

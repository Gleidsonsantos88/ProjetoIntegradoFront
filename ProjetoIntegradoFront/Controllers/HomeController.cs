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
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetoIntegradoFront.Controllers
{
    public class HomeController : Controller
    {
        private string tokenUsuario = "";
        private readonly string urlBase = "http://localhost:58693/";
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("role") != null && HttpContext.Session.GetString("role") == "Tecnico")
                return RedirectToAction("Listar", "OrdemServico");
            else if (HttpContext.Session.GetString("role") == null)
                    return View("Index");
                else
                return View();
        }

        public async Task<IActionResult> Login(Usuario usuario)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(urlBase);
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

            if(HttpContext.Session.GetString("role") == "Tecnico")
                return RedirectToAction("Listar", "OrdemServico");
            else
                return RedirectToAction("Index", "OrdemServico");
        }

        public IActionResult Sair()
        {
            HttpContext.Session.SetString("role", "");
            HttpContext.Session.SetString("token", "");
            HttpContext.Session.SetString("idUsuario", "");
            return View("Index");
        }

        public void SalvaTokenSessao(string tokenUsuario)
        {
            string roleUsuario = ObterJWTTokenClaim(tokenUsuario);
            string idUsuarioLogado = ObterIdUsuarioLogado(tokenUsuario);
            
            HttpContext.Session.SetString("role", roleUsuario);
            HttpContext.Session.SetString("token", tokenUsuario);
            HttpContext.Session.SetString("idUsuario", idUsuarioLogado);
        }

        public string ObterTokenSessao()
        {
            return HttpContext.Session.GetString("token");
        }

        public string ObterJWTTokenClaim(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                return claimValue;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string ObterIdUsuarioLogado(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                var IdUsuarioLogado = securityToken.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;

                return IdUsuarioLogado;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

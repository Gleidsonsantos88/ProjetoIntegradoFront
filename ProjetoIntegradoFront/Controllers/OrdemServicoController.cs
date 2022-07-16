using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoIntegradoFront.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProjetoIntegradoFront.Controllers
{
    public class OrdemServicoController : Controller
    {
        private string token = "";
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Listar()
        {
            List<OrdemServicoViewModel> ordensServico = null;
            using (var client = new HttpClient())
            {
                token = HttpContext.Session.GetString("token");
                client.BaseAddress = new System.Uri("http://localhost:58693/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync("ordemServico/api/ordemservico/listar-portecnico/716a5085-7246-4e3f-af47-f576ff208c7f");
               
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    ordensServico = JsonConvert.DeserializeObject<List<OrdemServicoViewModel>>(json);
                }
            }
            return View(ordensServico);
        }
    }
}

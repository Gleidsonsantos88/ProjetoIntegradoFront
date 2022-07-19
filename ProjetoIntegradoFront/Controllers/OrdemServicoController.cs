using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoIntegradoFront.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoIntegradoFront.Controllers
{
    public class OrdemServicoController : Controller
    {
        private string token = "";
        private readonly string urlBase = "http://localhost:58693/";
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("role") != null && HttpContext.Session.GetString("role") == "Tecnico")
                return RedirectToAction("Listar", "OrdemServico");
            else if (string.IsNullOrEmpty(HttpContext.Session.GetString("role")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        public async Task<IActionResult> Listar()
        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("role")))
                return RedirectToAction("Index", "Home");

            List<OrdemServicoViewModel> ordensServico = null;

            ordensServico = ObterOrdemServico().Result;

            return View(ordensServico);
        }

        public async Task<IActionResult> CriarOrdemServico(OrdemServicoViewModel ordemServico)
        {
            ordemServico.ItemOrdemServicos.FirstOrDefault().DescricaoServico = ObterDescricaoServico(ordemServico.ItemOrdemServicos.FirstOrDefault().ServicoId);
            ordemServico.NomeTecnico = ObterNomeTecnico(ordemServico.TecnicoId);
            ordemServico.UsuarioCriacaoId = Guid.Parse("716a5085-7246-4e3f-af47-f576ff208c7f");

            using (var client = new HttpClient())
            {
                token = HttpContext.Session.GetString("token");
                client.BaseAddress = new System.Uri(urlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(ordemServico);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("ordemServico/api/ordemservico/criar", data);

                if (response.IsSuccessStatusCode)
                {
                    ViewData["MessageSuccess"] = "Ordem de serviço cadastrada com sucesso.";
                    List<OrdemServicoViewModel> ordensServico = null;
                    ordensServico = ObterOrdemServico().Result;
                    
                    return View("Listar", ordensServico);
                }
            }

            ViewData["MessageError"] = "Erro ao tentar cadastrar ordem de serviço.";
            return View("Index", ordemServico);
        }

        private string ObterDescricaoServico(Guid servicoId)
        {
            var dicServico = new Dictionary<Guid, string>();

            dicServico.Add(Guid.Parse("ee3996e2-4c69-46e2-8b38-5907db569039"), "Instalação de rede");
            dicServico.Add(Guid.Parse("508a1ed2-a700-4364-b54c-58a614c78aea"), "Instalação de interfone");
            dicServico.Add(Guid.Parse("26707827-0e39-4090-8c2e-dd51cb39887b"), "Instalação de ar condicionado");
            dicServico.Add(Guid.Parse("6791c0b3-436e-4701-91a8-454743e9b653"), "Instalação de cameras de segurança");
            dicServico.Add(Guid.Parse("d09085d3-7a27-43f0-9b61-68b44c252489"), "Configurar de rede wifi");
            dicServico.Add(Guid.Parse("e55d8659-bfde-4d29-a5d9-ad90c679d052"), "Instalação de alarme");

            foreach (var item in dicServico)
            {
                if (item.Key == servicoId)
                    return item.Value;
            }
            return "";
        }


        private string ObterNomeTecnico(Guid tecnicoId)
        {
            var dicTecnico = new Dictionary<Guid, string>();

            dicTecnico.Add(Guid.Parse("f98f5fd2-407a-49dd-ab31-ee2603e78256"), "Carlos Eduardo");
            dicTecnico.Add(Guid.Parse("ea532ad1-0fc8-4638-b8eb-4d3b3936c695"), "Gleidson Jeferson");

            foreach (var item in dicTecnico)
            {
                if (item.Key == tecnicoId)
                    return item.Value;
            }
            return "";
        }

        public async Task<List<OrdemServicoViewModel>> ObterOrdemServico()
        {
            List<OrdemServicoViewModel> ordensServico = null;
            using (var client = new HttpClient())
            {
                token = HttpContext.Session.GetString("token");
                client.BaseAddress = new System.Uri(urlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = null;

                if (HttpContext.Session.GetString("role") == "Tecnico")
                    response = await client.GetAsync($"ordemServico/api/ordemservico/listar-portecnico/{HttpContext.Session.GetString("idUsuario")}");
                else
                    response = await client.GetAsync($"ordemServico/api/ordemservico/listar");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    ordensServico = JsonConvert.DeserializeObject<List<OrdemServicoViewModel>>(json);
                }
            }
            return ordensServico;
        }
    }
}

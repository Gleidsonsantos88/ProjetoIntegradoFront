using System;

namespace ProjetoIntegradoFront.Models
{
    public class EnderecoViewModel
    {
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Complemento { get; set; }

        public Guid OrdemServicoId { get; set; }
        public OrdemServicoViewModel OrdemServico { get; set; }
    }
}

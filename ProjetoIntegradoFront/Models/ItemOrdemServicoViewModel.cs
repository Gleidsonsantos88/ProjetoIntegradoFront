using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetoIntegradoFront.Models
{
    public class ItemOrdemServicoViewModel
    {
        [Required(ErrorMessage = "Informe o serviço")]
        public Guid ServicoId { get; set; }

        [Required(ErrorMessage = "Informe o valor do serviço")]
        public Decimal Valor { get; set; }

        [Required(ErrorMessage = "Informe a descrição do serviço")]
        public string DescricaoServico { get; set; }
        public OrdemServicoViewModel OrdemServico { get; set; }
    }
}

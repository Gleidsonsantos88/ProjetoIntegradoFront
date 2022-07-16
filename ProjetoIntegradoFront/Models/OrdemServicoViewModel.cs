using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoIntegradoFront.Models
{
    public class OrdemServicoViewModel
    {
        [Required(ErrorMessage = "Informe o técnico")]
        public Guid TecnicoId { get; set; }

        [Required(ErrorMessage = "Informe o usuário de criação")]
        public Guid UsuarioCriacaoId { get; set; }

        [Required(ErrorMessage = "Informe a data de inicio do serviço")]
        public DateTime DataInicioServico { get; set; }

        [Required(ErrorMessage = "Informe o nome técnico")]
        public string NomeTecnico { get; set; }

        [Required(ErrorMessage = "Informe o nome do cliente")]
        public string NomeCliente { get; set; }

        [MaxLength(500, ErrorMessage = "Observação não pode ter mais de 500 caracteres")]
        public String Observacao { get; set; }

        public decimal ValorTotal { get; set; }

        public EnderecoViewModel Endereco { get; set; }
        public virtual List<ItemOrdemServicoViewModel> ItemOrdemServicos { get; set; }
        public virtual List<OrdemServicoSituacaoViewModel> OrdemServicoSituacoes { get; set; }
    }
}

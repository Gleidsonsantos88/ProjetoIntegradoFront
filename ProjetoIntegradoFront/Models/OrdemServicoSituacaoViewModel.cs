using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoIntegradoFront.Models
{
    public class OrdemServicoSituacaoViewModel
    {
        [Required(ErrorMessage = "Informe o usuário de criação")]
        public Guid UsuarioCriacaoId { get; set; }

        public Guid OrdemServicoId { get; set; }
        public OrdemServicoViewModel OrdemServico { get; set; }
        public OrdemServicoStatusViewModel OrdemServicoStatus { get; set; }
    }
}

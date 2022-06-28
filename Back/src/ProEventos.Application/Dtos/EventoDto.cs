using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {


        public int Id { get; set; }

        [Required(ErrorMessage = "O local do evento é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O campo local deve ter no máximo {1} caracteres")]
        public string Local { get; set; }

        
        public string DataEvento { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
        public string Tema { get; set; }

        [Display(Name = "Participantes")]
        [Range(1, 120000, ErrorMessage = "O número de {0} deve ser entre {1} e {2}")]
        public int QtdPessoas { get; set; }

        [RegularExpression(@"(.*\.)(jpg|JPG|gif|GIF|png|PNG)$", 
        ErrorMessage = 
        "Formato de imagem inválido (jpg|JPG|gif|GIF|png|PNG)")]
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Phone(ErrorMessage = "O campo {0} não é um número válido")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório."), Display(Name = "e-mail")]
        [EmailAddress(ErrorMessage = "O campo {0} não é um endereço válido")]
        public string Email { get; set; }

        public IEnumerable<PalestranteDto> PalestrantesEventos { get; set; }
        public IEnumerable<LoteDto> Lotes { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace projetoDM106.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Color { get; set; }
        [Required(ErrorMessage = "O campo Modelo é obrigatório")]
        public string Modelo { get; set; }
        [Required(ErrorMessage = "O campo Codigo é obrigatório")]
        public string Codigo { get; set; }
        [Range(10, 999, ErrorMessage = "O preço deverá ser entre 10 e 999.")]
        public float Preco { get; set; }
        public float Peso { get; set; }
        public float Altura { get; set; }
        public float Largura { get; set; }
        public float Comprimento { get; set; }
        public string URLimagem { get; set; }
        public string Fabricado { get; set; }
    }
}

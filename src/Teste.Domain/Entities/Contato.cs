using System;
using System.Collections.Generic;
using System.Text;

namespace Teste.Domain.Entities
{
    public class Contato : Entity
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; }
        public string Sexo { get; set; }
        public int? Idade { get; set; }
    }
}

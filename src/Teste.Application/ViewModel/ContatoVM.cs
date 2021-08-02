using System;
using System.Collections.Generic;
using System.Text;

namespace Teste.Application.ViewModel
{
    public class ContatoVM
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; }
        public string Sexo { get; set; }
        public int Idade { get; set; }

        public string MsgErro { get; set; }
        public bool Valido { get; set; }
    }
}

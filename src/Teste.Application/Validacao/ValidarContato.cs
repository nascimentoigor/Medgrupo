using System;
using System.Collections.Generic;
using System.Text;
using Teste.Application.ViewModel;

namespace Teste.Application.Validacao
{
    public class ValidarContato
    {
        public static ErrorMessage ValidarDados(ContatoVM obj)
        {
            var validacao = new ErrorMessage() { Valido = true};

            if (obj.DataNascimento > DateTime.Now)
                validacao = new ErrorMessage() { Valido = false, Erro = "Data de nascimento é maior que a data atual" };

            if (obj.Idade < 18)
                validacao = new ErrorMessage() { Valido = false, Erro = "O contato tem que ser maior de idade" };

            return validacao;
        }

        public static int CalcularIdade(ContatoVM obj)
        {
            var dataNascimento = obj.DataNascimento;
            int idade = DateTime.Now.Year - dataNascimento.Year;
            if (DateTime.Now.DayOfYear < dataNascimento.DayOfYear)
            {
                idade = idade - 1;
            }
            return idade;
        }
    }
}

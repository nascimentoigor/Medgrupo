using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Teste.Application.ViewModel;

namespace Teste.Application.Service
{
    public interface IContatoService
    {
        Task<IEnumerable<ContatoVM>> GetAll();
        Task<ContatoVM> RetornaPorId(int id);
        Task<ContatoVM> Incluir(ContatoVM obj);
        Task<ContatoVM> Alterar(ContatoVM obj);
        Task<ContatoVM> Excluir(int id);
    }

}

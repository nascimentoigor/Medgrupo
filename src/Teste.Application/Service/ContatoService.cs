using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste.Application.ViewModel;
using Teste.Domain.Entities;
using Teste.Infrastructure.Data.Repositories;

namespace Teste.Application.Service
{
    public class ContatoService : IContatoService
    {
        private readonly IContatoRepository _contatoRepository;
        private readonly ILogger<IContatoService> _logger;
        private readonly IMapper _mapper;

        public ContatoService(ILogger<IContatoService> logger, IMapper mapper, IContatoRepository cadastroTesteRepository)
        {
            _contatoRepository = cadastroTesteRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContatoVM>> GetAll()
        {
            return _mapper.Map<IEnumerable<ContatoVM>>(_contatoRepository.AsQueryable().Where(p => p.Ativo == true));
        }

        public async Task<ContatoVM> RetornaPorId(int id)
        {
            var retorno = _contatoRepository.AsQueryable().Where(p => p.Id == id).FirstOrDefault();
            return _mapper.Map<ContatoVM>(retorno);
        }

        public async Task<ContatoVM> Incluir(ContatoVM obj)
        {
            var objRetorno = new ContatoVM();

            obj.Idade = Validacao.ValidarContato.CalcularIdade(obj);
            var validacaoDados = Validacao.ValidarContato.ValidarDados(obj);            

            if (!validacaoDados.Valido)
                objRetorno.MsgErro = "Erro ao incluir dados! " + validacaoDados.Erro;
            else
            {
                try
                {
                    var objInc = _mapper.Map<Contato>(obj);
                    var testeInc = await _contatoRepository.Insert(objInc);
                    objRetorno = _mapper.Map<ContatoVM>(objInc);
                }
                catch (Exception ex)
                {
                    objRetorno.MsgErro = "Erro ao incluir dados! " + ex.Message;
                }
            }
            
            return objRetorno;
        }

        public async Task<ContatoVM> Excluir(int id)
        {
            var retorno = new ContatoVM();

            try
            {
                var obj = _contatoRepository.SelectById(id).Result;

                if(obj == null)
                {
                    retorno.Valido = false;
                    retorno.MsgErro = "Contato não encontrado!";
                }
                else
                {
                    _contatoRepository.Delete(obj);
                    retorno.Valido = true;
                    await _contatoRepository.Commit();
                }
            }
            catch (Exception ex)
            {
                retorno.Valido = false;
                retorno.MsgErro = "Erro ao excluir Contato! " + ex.Message; 

            }
            
            return retorno;
        }

        public async Task<ContatoVM> Alterar(ContatoVM obj)
        {
            try
            {
                var objAlt = _mapper.Map<Contato>(obj);
                await _contatoRepository.Update(objAlt);
                await _contatoRepository.Commit();
                obj.Valido = true;
            }
            catch (Exception ex)
            {
                obj.Valido = false;
                obj.MsgErro = "Erro ao incluir dados! " + ex.Message;
            }

            return obj;
        }
    }
}

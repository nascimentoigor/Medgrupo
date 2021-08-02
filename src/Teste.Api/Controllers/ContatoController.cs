using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Teste.Application.Service;
using Teste.Application.ViewModel;

namespace Teste.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContatoController : Controller
    {
        private readonly ILogger<ContatoController> _logger;
        private readonly IContatoService _contatoService;

        public ContatoController(ILogger<ContatoController> logger, IContatoService contatoService)
        {
            _logger = logger;
            _contatoService = contatoService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ContatoVM>> GetAll()
        {
            return await _contatoService.GetAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> RetornaPorId(int id)
        {
            var result = await _contatoService.RetornaPorId(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound("Registro não encontrado");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] ContatoVM viewModel)
        {
            var model = _contatoService.Incluir(viewModel).Result;

            if(model != null && String.IsNullOrEmpty(model.MsgErro))
                return Ok(model);
            else
                return BadRequest(model.MsgErro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put([FromBody] ContatoVM viewModel)
        {
            var model = _contatoService.Alterar(viewModel).Result;

            if (model != null && model.Valido)
                return Ok(model);
            else
                return BadRequest(model.MsgErro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var result = await _contatoService.Excluir(id);
            if (result != null)
                return Ok(result);
            else
                return NotFound(result.MsgErro);
        }

    }
}
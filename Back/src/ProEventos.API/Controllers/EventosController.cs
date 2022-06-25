using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Persistence;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Application;
using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;

namespace ProEventos.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {

        private readonly IEventosService _eventosService;
        public EventosController(IEventosService service)
        {
            _eventosService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _eventosService.GetAllEventosAsync(true);
                if (eventos == null) return NotFound("Nenhum evento encontrado");

                return Ok(eventos);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar eventos. Error: {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdAsync(id, true);
                if (evento == null) return NotFound("Nenhum evento encontrado");

                return Ok(evento);
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar evento. Error: {e.Message}");
            }
        }

        [HttpGet("tema/{tema}")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var evento = await _eventosService.GetAllEventosByTemaAsync(tema, true);
                if (evento == null) return NotFound("Eventos por tema não encontrados");

                return Ok(evento);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar evento. Error: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {

            try
            {
                var obj = await _eventosService.AddEvento(model);
                if (obj == null) return BadRequest("Erro ao adicionar evento.");

                return Ok(model);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao adicionar evento. Error: {e.Message}");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento model)
        {
            try
            {
                var obj = await _eventosService.UpdateEvento(id, model);
                if (obj == null) return BadRequest("Erro ao atualizar evento.");

                return Ok(model);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao atualizar evento. Error: {e.Message}");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await _eventosService.DeleteEvento(id))
                    return Ok("Evento excluído");
                else return BadRequest("Evento não deletado");
            }
            
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao deletar evento. Error: {e.Message}");
            }
        }

    }
}

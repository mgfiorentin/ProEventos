using System;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ProEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;

using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {

        private readonly ILoteService _lotesService;
        public LotesController(ILoteService service)
        {
            _lotesService = service;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                var lotes = await _lotesService.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return NoContent();

                return Ok(lotes);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar lotes. Error: {e.Message}");
            }
        } 


        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotesService.SaveLotes(eventoId, models);
                if (lotes == null) return NoContent();
                return Ok(lotes);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao salvar lotes. Error: {e.Message}");
            }

        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotesService.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) return NoContent();

                if (await _lotesService.DeleteLote(lote.EventoId, lote.Id))
                    return Ok(new { message = "Lote excluído" });
                else return BadRequest("Ocorreu um problema durante exclusão do lote");
            }

            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao excluir lote. Error: {e.Message}");
            }
        }

    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {

        private readonly ILotePersist _lotePersist;
        private readonly IGeralPersist _geralPersist;
        private readonly IMapper _mapper;
        public LoteService(
            IGeralPersist gPersist,
            IMapper mapper,
            ILotePersist lPersist)
        {
            _geralPersist = gPersist;
            _lotePersist = lPersist;
            _mapper = mapper;
        }

        public async Task AddLote(int eventoId, LoteDto model)
        {

            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;
                _geralPersist.Add<Lote>(lote);

                await _geralPersist.SaveChangesAsync();
            }
            catch (System.Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdAsync(eventoId, loteId);
                if (lote == null) throw new Exception("Lote não encontrado.");

                _geralPersist.Delete<Lote>(lote);
                return await _geralPersist.SaveChangesAsync();
            }
            catch (System.Exception e)
            {

                throw new Exception("Erro ao excluir lote." + e.Message);
            }
        }

        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdAsync(eventoId, loteId);
                if (lote == null) return null;

                var resultado = _mapper.Map<LoteDto>(lote);

                return resultado;
            }
            catch (System.Exception e)
            {
                throw new Exception("Erro ao buscar lote." + e.Message);
            }
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;


                var resultados = _mapper.Map<LoteDto[]>(lotes);
                return resultados;
            }

            catch (System.Exception e)
            {
                throw new Exception("Erro ao buscar lotes." + e.Message);
            }
        }

        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        await AddLote(eventoId, model);

                    }
                    else
                    {

                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                        model.EventoId = eventoId;
                        
                        _mapper.Map(model, lote);

                        _geralPersist.Update<Lote>(lote);
                        
                        await _geralPersist.SaveChangesAsync();
                    }
                }

                var loteRetorno = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                return _mapper.Map<LoteDto[]>(loteRetorno);
            }

            catch (System.Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}
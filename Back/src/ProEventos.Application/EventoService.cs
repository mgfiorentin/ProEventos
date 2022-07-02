using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist _eventoPersist;
        private readonly IGeralPersist _geralPersist;
        private readonly IMapper _mapper;
        public EventoService(IEventoPersist ePersist, IGeralPersist gPersist, IMapper mapper)
        {
            _geralPersist = gPersist;
            _eventoPersist = ePersist;
            _mapper = mapper;
        }

        public async Task<EventoDto> AddEvento(EventoDto model)
        {
           try
            {
                var evento = _mapper.Map<Evento>(model);
                

                _geralPersist.Add<Evento>(evento);

                if (await _geralPersist.SaveChangesAsync())
                {
                     var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(evento.Id, false);
                     return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao adicionar evento!" + e.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            
            try{
                var obj = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
                if (obj == null) return null;

                var eventoResultado = _mapper.Map<Evento>(model);

                eventoResultado.Id = obj.Id;

                _geralPersist.Update<Evento>(eventoResultado);

                if (await _geralPersist.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(eventoResultado.Id, false);
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;

            }
            catch (Exception e)
            {
                throw new Exception("Erro ao atualizar evento!" + e.Message);
            }
        }

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                var obj = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
                if (obj == null) throw new Exception("Evento não encontrado!");


                _geralPersist.Delete<Evento>(obj);

                return await _geralPersist.SaveChangesAsync();



            }
            catch (Exception e)
            {
                throw new Exception("Erro ao atualizar evento!" + e.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
                
                var resultados = _mapper.Map<EventoDto[]>(eventos);
                return resultados == null ? null : resultados;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
                
                var resultados = _mapper.Map<EventoDto[]>(eventos);
                return resultados == null ? null : resultados;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);

                var resultado = _mapper.Map<EventoDto>(evento);
                return resultado == null ? null : resultado;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
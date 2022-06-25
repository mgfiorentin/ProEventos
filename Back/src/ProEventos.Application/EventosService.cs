using System;
using System.Threading.Tasks;
using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventosService : IEventosService
    {
        private readonly IEventoPersist _eventoPersist;
        private readonly IGeralPersist _geralPersist;
        public EventosService(IEventoPersist ePersist, IGeralPersist gPersist)
        {
            _geralPersist = gPersist;
            _eventoPersist = ePersist;
        }

        public async Task<Evento> AddEvento(Evento model)
        {
            try
            {
                _geralPersist.Add(model);
                if (await _geralPersist.SaveChangesAsync())
                {
                    return await _eventoPersist.GetEventoByIdAsync(model.Id, false);
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao adicionar evento!" + e.Message);
            }
        }

        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                var obj = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
                if (obj == null) return null;

                model.Id = obj.Id;

                _geralPersist.Update(model);

                if (await _geralPersist.SaveChangesAsync())
                {
                    return await _eventoPersist.GetEventoByIdAsync(model.Id, false);
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

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
                return eventos == null ? null : eventos;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
                return eventos == null ? null : eventos;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
                return eventos == null ? null : eventos;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
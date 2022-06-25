using System;
using ProEventos.Domain;
using System.Threading.Tasks;

namespace ProEventos.Application.Contratos
{
    public interface IEventosService
    {
         Task<Evento> AddEvento(Evento model);
         Task<Evento> UpdateEvento(int eventoId, Evento model);
         Task<bool> DeleteEvento(int eventoId);

        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes =false);
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes=false);
        Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes=false);



    }
}
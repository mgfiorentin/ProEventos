using System;
using System.Threading.Tasks;
using ProEventos.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;

namespace ProEventos.Persistence
{
    public class EventoPersist : IEventoPersist
    {
        private readonly ProEventosContext _context;
        public EventoPersist(ProEventosContext context) {
            _context = context;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
           

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e=>e.Lotes)
            .Include(e=>e.RedesSociais);

            if (includePalestrantes) 
            query.Include(e=>e.PalestrantesEventos)
            .ThenInclude(pe=>pe.Palestrante);

            query = query.OrderBy(e => e.Id).Where(e=> e.Id == eventoId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e=>e.Lotes)
            .Include(e=>e.RedesSociais);

            if (includePalestrantes) 
            query.Include(e=>e.PalestrantesEventos)
            .ThenInclude(pe=>pe.Palestrante);

            query = query.OrderBy(e => e.Id);
            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e=>e.Lotes)
            .Include(e=>e.RedesSociais);

            if (includePalestrantes) 
            query.Include(e=>e.PalestrantesEventos)
            .ThenInclude(pe=>pe.Palestrante);

            query = query.OrderBy(e => e.Id).
            Where(e => e.Tema.ToLower().Contains(tema.ToLower()));
            return await query.ToArrayAsync();
        }              
    }
}
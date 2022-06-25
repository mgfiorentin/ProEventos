using System;
using System.Threading.Tasks;
using ProEventos.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Contextos;

namespace ProEventos.Persistence
{
    public class GeralPersist : IGeralPersist
    {
        private readonly ProEventosContext _context;

        public GeralPersist(ProEventosContext context) {
            _context = context;
        }

        public void Add<T>(T entidade) where T : class
        {
            _context.Add(entidade);
        }

        public void Update<T>(T entidade) where T : class
        {
            _context.Update(entidade);
        }

        public void Delete<T>(T entidade) where T : class
        {
            _context.Remove(entidade);
        }

        public void DeleteRange<T>(T[] entidades) where T : class
        {
           _context.RemoveRange(entidades);
        }

        public async Task<bool> SaveChangesAsync()
        {
           return (await _context.SaveChangesAsync() > 0);
        }
        

      

        
    }
}
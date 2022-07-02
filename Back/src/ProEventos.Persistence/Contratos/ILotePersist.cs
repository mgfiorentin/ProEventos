using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface ILotePersist
    {
        /// <summary>
        /// Método get para retornar lista de lotes por eventoId
        /// </summary>
        /// <param name="eventoId">Código chave da tabela Evento</param>
        /// <returns>Array de lotes</returns>
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);

        /// <summary>
        /// Método get para retornar um único lote
        /// </summary>
        /// <param name="eventoId"><Código chave da tabela Eventos/param>
        /// <param name="id">Código chave da tabela Lotes</param>
        /// <returns>Um único lote</returns>
        Task<Lote> GetLoteByIdAsync(int eventoId, int id);


    }
}
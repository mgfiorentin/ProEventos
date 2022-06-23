using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public IEnumerable<Evento> _evento = new Evento[] {new Evento() {
                EventoId = 1, Tema = "Angular",
                Local = "BH", Lote = "1 lote",
                QtdPessoas = 250,
                DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"),
                ImageURL = "foto.png"

                }, new Evento() {
                EventoId = 2, Tema = "Angular 2",
                Local = "SP", Lote = "3 lote",
                QtdPessoas = 350,
                DataEvento = DateTime.Now.AddDays(4).ToString("dd/MM/yyyy"),
                ImageURL = "foto3.png"

                }
            };

        public EventoController()
        {

        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _evento;
        }

        [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)
        {
            return _evento.Where(e => e.EventoId == id);
        }
    }
}

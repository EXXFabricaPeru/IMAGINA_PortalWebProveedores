using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebProov_API.Data.Interfaces;
using WebProov_API.Dtos;
using WebProov_API.Models;

namespace WebProov_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private IFacturaRepository _repo;
        private IMapper _mapper;

        public FacturaController(IFacturaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet("Lista/{ruc}/{fi}/{ff}")]
        public ActionResult<List<FacturaByFPRucRead>> GetFacturaByFPRucList(string ruc, string fi, string ff, string estado)
        {
            try
            {
                var facturas = _repo.GetListaFechaPagoByRuc(ruc, fi, ff, estado);

                if (facturas == null)
                    return NotFound();

                return Ok(_mapper.Map<List<FacturaByFPRucRead>>(facturas));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj("0", ex.Message));
            }
        }

        [HttpGet("ListaDescargar/{ruc}/{fecIni}/{fecFin}")]
        public ActionResult<string> PedidoListDescargar(string ruc, string fecIni, string fecFin, string estado)
        {
            try
            {
                string xRpta = _repo.GetListaDescargar(ruc, fecIni, fecFin, estado);
                return Ok(xRpta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Crear")]
        public IActionResult CrearFactura([FromBody] Documento document) 
        {
            string xRpta = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                xRpta = _repo.CrearFactura(document);

                return Ok(xRpta);
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpPut("Update")]
        public IActionResult ActualizarFactura([FromBody] Documento document) 
        {
            string xRpta = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                xRpta = _repo.ActualizarFactura(document);

                return Ok(xRpta);
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpPost("CrearAnticipo")]
        public IActionResult CrearFacturaAnticipo([FromBody] Documento document) 
        {
            string xRpta = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                xRpta = _repo.CrearFacturaAnticipo(document);

                return Ok(xRpta);
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<FacturaRead> GetFacturaById(int id)
        {
            try
            {
                var pedido = _repo.GetFacturaId(id);

                if (pedido == null)
                    return NotFound();

                return Ok(_mapper.Map<FacturaRead>(pedido));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj(id.ToString(), ex.Message));
            }
        }
        [HttpGet("Anticipo/{id}")]
        public ActionResult<FacturaRead> GetAnticipoById(int id)
        {
            try
            {
                var pedido = _repo.GetAnticipoId(id);

                if (pedido == null)
                    return NotFound();

                return Ok(_mapper.Map<FacturaRead>(pedido));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj(id.ToString(), ex.Message));
            }
        }
        [HttpGet("Borrador/{id}")]
        public ActionResult<FacturaRead> GetBorradorById(int id)
        {
            try
            {
                var pedido = _repo.GetBorradorId(id);

                if (pedido == null)
                    return NotFound();

                return Ok(_mapper.Map<FacturaRead>(pedido));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj(id.ToString(), ex.Message));
            }
        }

        //[HttpPost]
        //public ActionResult<FacturaRead> CrearFactura(FacturaRead oferta)
        //{
        //    int entry = 0;
        //    try
        //    {
        //        var ofertaModel = _mapper.Map<Documento>(oferta);
        //        entry = _repo.CrearDocument(ofertaModel);

        //        var ofertaRead = _mapper.Map<FacturaRead>(ofertaModel);
        //        return CreatedAtRoute(nameof(GetFacturaById), new { id = entry }, ofertaRead);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return BadRequest(new ErrMsj(entry.ToString(), ex.Message));
        //    }
        //}

        //[HttpPatch("{id}", Name = "UpdateFactura")]
        //public ActionResult<FacturaRead> ActualizarFactura(string id, FacturaRead oferta)
        //{
        //    try
        //    {
        //        var ofertaModel = _mapper.Map<Documento>(oferta);
        //        ofertaModel.DocEntry = int.Parse(id);
        //        _repo.ActualizarDocumento(ofertaModel);

        //        var socioRead = _mapper.Map<FacturaRead>(ofertaModel);
        //        return Ok("{\"msj\":\"Actualizado\"}");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return BadRequest(new ErrMsj(id, ex.Message));
        //    }
        //}

    }
}
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class PedidoController : ControllerBase
    {
        private IPedidoRepository _repo;
        private IMapper _mapper;
        public PedidoController(IPedidoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        #region Pedido
        [HttpGet("{id}", Name = "GetPedidoById")]
        public ActionResult<PedidoRead> GetPedidoById(int id)
        {
            try
            {
                var pedido = _repo.GetDocumentoById(id);

                if (pedido == null)
                    return NotFound();

                return Ok(_mapper.Map<PedidoRead>(pedido));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj(id.ToString(), ex.Message));
            }
        }

        [HttpGet("Lista/{ruc}/{fecIni}/{fecFin}")]
        public ActionResult<List<PedidoByRucRead>> GetPedidoByRucList(string ruc, string fecIni, string fecFin, string estado)
        {
            try
            {
                var pedido = _repo.GetListaByRuc(ruc, fecIni, fecFin, estado);

                if (pedido == null)
                    return NotFound();

                return Ok(_mapper.Map<List<PedidoByRucRead>>(pedido));
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
                string xRpta = _repo.descargarLista(ruc, fecIni, fecFin, estado);
                if (!xRpta.Contains("ERROR!"))
                    return Ok(xRpta);
                else
                    return BadRequest(xRpta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }            
        }
        #endregion

        #region Conformidad
        [HttpGet("ConformidadApr/Id/{id}")]
        public ActionResult<PedidoRead> GetConformidadAprById(int id)
        {
            try
            {
                var pedido = _repo.GetConformidadAprById(id);

                if (pedido == null)
                    return NotFound();

                return Ok(_mapper.Map<PedidoRead>(pedido));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj(id.ToString(), ex.Message));
            }
        }

        [HttpGet("Conformidad/Id/{id}")]
        public ActionResult<PedidoRead> GetConformidadById(int id)
        {
            try
            {
                var pedido = _repo.GetConformidadById(id);

                if (pedido == null)
                    return NotFound();

                return Ok(_mapper.Map<PedidoRead>(pedido));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj(id.ToString(), ex.Message));
            }
        }

        [HttpGet("Conformidad/Lista/{ruc}/{fecIni}/{fecFin}")]
        public ActionResult<List<PedidoByRucRead>> GetConformidadByRucList(string ruc, string fecIni, string fecFin, string estado)
        {
            try
            {
                var pedido = _repo.GetConformidadByRucList(ruc, fecIni, fecFin, estado);

                if (pedido == null)
                    return NotFound();

                return Ok(_mapper.Map<List<PedidoByRucRead>>(pedido));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj("0", ex.Message));
            }
        }

        [HttpGet("Conformidad/ListaDescargar/{ruc}/{fecIni}/{fecFin}")]
        public ActionResult<string> ConformidadListDescargar(string ruc, string fecIni, string fecFin, string estado)
        {
            try
            {
                string xRpta = _repo.descargarListaConformidad(ruc, fecIni, fecFin, estado);
                return Ok(xRpta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Conformidad/Disponible/{ruc}/{fecIni}/{fecFin}/{sucursal}")]
        public ActionResult<List<PedidoByRucRead>> GetConformidadDispobible(string ruc, string fecIni, string fecFin, string sucursal)
        {
            try
            {
                var pedido = _repo.GetConformidadDisponible(ruc, fecIni, fecFin, sucursal);

                if (pedido == null)
                    return NotFound();

                return Ok(_mapper.Map<List<PedidoByRucRead>>(pedido));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj("0", ex.Message));
            }
        }

        [HttpPost("Confirmar")]
        public IActionResult ConfirmarOC([FromBody] Documento pedido)
        {
            string xRpta = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                xRpta = _repo.ConfirmarOC(pedido);

                return Ok(xRpta);
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("AprobarCS")]
        public IActionResult AprobarConformidadServicio([FromBody] Documento pedido)
        {
            string xRpta = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                xRpta = _repo.AprobarConformidadServicio(pedido.DocEntry.ToString(), pedido.UserReg, pedido.DocStatus, pedido.Comments, pedido.Password);

                return Ok(xRpta);
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("Archivo/{archivo}")]
        public ActionResult<string> getFile(string archivo)
        {
            try
            {
                string xrpta = _repo.file_Descargar(archivo);
                return Ok(xrpta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        //[HttpPost]
        //public ActionResult<PedidoRead> CrearPedido(PedidoRead oferta)
        //{
        //    int entry = 0;
        //    try
        //    {
        //        var ofertaModel = _mapper.Map<Documento>(oferta);
        //        entry = _repo.CrearDocument(ofertaModel);

        //        var ofertaRead = _mapper.Map<PedidoRead>(ofertaModel);
        //        return CreatedAtRoute(nameof(GetPedidoById), new { id = entry }, ofertaRead);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return BadRequest(new ErrMsj(entry.ToString(), ex.Message));
        //    }
        //}

        //[HttpPatch("{id}", Name = "UpdatePedido")]
        //public ActionResult<PedidoRead> ActualizarPedido(string id, PedidoRead oferta)
        //{
        //    try
        //    {
        //        var ofertaModel = _mapper.Map<Documento>(oferta);
        //        ofertaModel.DocEntry = int.Parse(id);
        //        _repo.ActualizarDocumento(ofertaModel);

        //        var socioRead = _mapper.Map<PedidoRead>(ofertaModel);
        //        return Ok("{\"msj\":\"Actualizado\"}");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return BadRequest(new ErrMsj(id, ex.Message));
        //    }
        //}

    }
}
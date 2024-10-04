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
    public class SociosController : ControllerBase
    {
        private IBusinessPartnerRepository _repo;
        private IMapper _mapper;

        [HttpGet("{id}", Name = "GetSocioByRUCRaz")]
        public ActionResult<SocioRead> GetSocioByRUCRaz(string id)
        {
            try
            {
                var socio = _repo.GetSocioByCardCode(id);

                if (socio == null)
                    return NotFound();

                return Ok(_mapper.Map<SocioRead>(socio));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj(id, ex.Message));
            }
        }

        public SociosController(IBusinessPartnerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("Draft/{id}")]
        public ActionResult<SocioDeNegocio> GetSocioDraft(string id)
        {
            try
            {
                var socio = _repo.GetDraft(id);

                if (socio == null)
                    return NotFound();

                return Ok(_mapper.Map<SocioDeNegocio>(socio));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj(id, ex.Message));
            }
        }
        
        [HttpGet("Lista/Factoring/")]
        public ActionResult<List<SocioDeNegocio>> GetListSocioFactor()
        {
            try
            {
                var socio = _repo.GetListaFactor();

                if (socio == null)
                    return NotFound();

                return Ok(_mapper.Map<List<SocioDeNegocio>>(socio));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj("", ex.Message));
            }
        }
            
        [HttpGet("Lista/")]
        public ActionResult<List<SocioDeNegocio>> GetListSocioDraft(string valor, string estado)
        {
            try
            {
                var socio = _repo.GetLista(valor, estado);

                if (socio == null)
                    return NotFound();

                return Ok(_mapper.Map<List<SocioDeNegocio>>(socio));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ErrMsj(valor, ex.Message));
            }
        }
                
        [HttpGet("Archivo/{code}, {ruc}")]
        public ActionResult<string> file_Descargar(string code, string ruc)
        {
            try
            {
                string xrpta = _repo.file_Descargar(code, ruc);
                return Ok(xrpta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult<string> CrearProveedor(SocioDeNegocio proveedor)
        {
            try
            {
                string xrpta = _repo.fn_Proveedor_Borrador_Registrar(proveedor);
                return Ok(xrpta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpPost("Aprobar")]
        public ActionResult<string> AprobarProveedor(SocioDeNegocio proveedor)
        {
            try
            {
                string xrpta = _repo.fn_Proveedor_Aprobar(proveedor);
                return Ok(xrpta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("Formato/{name}")]
        public ActionResult<string> formatoDescargar(string name)
        {
            try
            {
                string xrpta = _repo.formatoDescargar(name);
                return Ok(xrpta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region Comentado
        //[HttpPost]
        //public ActionResult<SocioDeNegocioCreate> CrearSocio(SocioDeNegocioCreate socio)
        //{
        //    string CardCode = "";
        //    try
        //    {
        //        var socioModel = _mapper.Map<SocioDeNegocio>(socio);
        //        CardCode = socioModel.CardType + socioModel.LicTradNum.PadLeft(12, '0');
        //        socioModel.CardCode = CardCode;

        //        _repo.CrearSocio(socioModel);
        //        var socioRead = _mapper.Map<SocioRead>(socioModel);
        //        return CreatedAtRoute(nameof(GetSocioByRUCRaz), new { id = socioRead.CardCode }, socioRead);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return BadRequest(new ErrMsj(CardCode, ex.Message));
        //    }
        //}

        //[HttpPatch("{id}", Name = "UpdateSocio")]
        //public ActionResult<SocioDeNegocioCreate> ActualizarSocio(string id, SocioDeNegocioCreate socio)
        //{
        //    try
        //    {
        //        var socioModel = _mapper.Map<SocioDeNegocio>(socio);
        //        socioModel.CardCode = id;
        //        _repo.ActualizarSocio(socioModel);

        //        var socioRead = _mapper.Map<SocioRead>(socioModel);
        //        socioRead.CardCode = socioRead.CardType + socioRead.LicTradNum;
        //        return Ok("{\"msj\":\"Actualizado\"}");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return BadRequest(new ErrMsj(id, ex.Message));
        //    }
        //}
        #endregion

    }
}
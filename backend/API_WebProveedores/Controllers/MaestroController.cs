using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebProov_API.Data.Interfaces;
using WebProov_API.Dtos;
using WebProov_API.Models;

namespace WebProov_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaestroController : ControllerBase
    {
        private IMaestroRepository _repo;
        private IMapper _mapper;

        public MaestroController(IMaestroRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("CondicionPago")]
        public ActionResult<List<Maestro>> GetCondicionPago()
        {
            try
            {
                var maestro = _repo.getCondicionesPago();

                if (maestro == null)
                    return NotFound();

                return Ok(_mapper.Map<List<Maestro>>(maestro));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Moneda")]
        public ActionResult<List<Maestro>> GetMoneda()
        {
            try
            {
                var maestro = _repo.getMoneda();

                if (maestro == null)
                    return NotFound();

                return Ok(_mapper.Map<List<Maestro>>(maestro));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("Banco")]
        public ActionResult<List<Maestro>> GetBanco()
        {
            try
            {
                var maestro = _repo.getBanco();

                if (maestro == null)
                    return NotFound();

                return Ok(_mapper.Map<List<Maestro>>(maestro));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("TipoCuenta")]
        public ActionResult<List<Maestro>> GetTipoCuenta()
        {
            try
            {
                var maestro = _repo.getTipoCuenta();

                if (maestro == null)
                    return NotFound();

                return Ok(_mapper.Map<List<Maestro>>(maestro));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Departamento/{id}")]
        public ActionResult<List<Maestro>> GetDepartamento(string id)
        {
            try
            {
                var maestro = _repo.getDepartamento(id);

                if (maestro == null)
                    return NotFound();

                return Ok(_mapper.Map<List<Maestro>>(maestro));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Provincia/{id}")]
        public ActionResult<List<Maestro>> GetProvincia(string id)
        {
            try
            {
                var maestro = _repo.getProvincia(id);

                if (maestro == null)
                    return NotFound();

                return Ok(_mapper.Map<List<Maestro>>(maestro));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Distrito/{id}")]
        public ActionResult<List<Maestro>> GetDistrito(string id)
        {
            try
            {
                var maestro = _repo.getDistrito(id);

                if (maestro == null)
                    return NotFound();

                return Ok(_mapper.Map<List<Maestro>>(maestro));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Configuracion")]
        public ActionResult<List<Maestro>> GetConfiguracion()
        {
            try
            {
                var maestro = _repo.getConfiguracion();

                if (maestro == null)
                    return NotFound();

                return Ok(_mapper.Map<List<Maestro>>(maestro));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("ProvFact")]
        public ActionResult<List<Maestro>> getProveedorFactoring()
        {
            try
            {
                List<Maestro> maestro = _repo.getProveedorFactoring();

                if (maestro == null)
                    maestro = new List<Maestro>();

                return Ok(_mapper.Map<List<Maestro>>(maestro));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Formatos")]
        public ActionResult<List<Maestro>> getFormatos()
        {
            try
            {
                List<Maestro> maestro = _repo.getFormatos();

                if (maestro == null)
                    maestro = new List<Maestro>();

                return Ok(_mapper.Map<List<Maestro>>(maestro));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

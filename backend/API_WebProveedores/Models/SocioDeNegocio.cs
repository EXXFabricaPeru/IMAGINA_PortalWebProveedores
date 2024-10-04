using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace WebProov_API.Models
{
    public class SocioDeNegocio
    {
        public string CardCode { get; set; }
        public int Type { get; set; }
        public int GroupCode { get; set; }
        public string GroupName { get; set; }
        public string CardName { get; set; }
        [Required]
        public string LicTradNum { get; set; }
        public string Currency { get; set; }
        public int PayTermsGrpCode { get; set; }
        public string Phone1 { get; set; }
        public string Cellular { get; set; }
        public string EmailAddress { get; set; }
        public string FormaPago { get; set; }
        public string U_EXX_TIPODOCU { get; set; }
        public string U_EXX_ESTCONTR { get; set; }
        public string U_EXX_CNDCONTR { get; set; }
        public string U_EXX_TIPOPERS { get; set; }//Tipo Persona
        public string Nombre { get; set; }
        public string SegundoNombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Archivo1 { get; set; }
        public string Archivo2 { get; set; }
        public string Archivo3 { get; set; }
        public string Archivo4 { get; set; }
        public string Archivo5 { get; set; }
        public string Archivo6 { get; set; }
        public string Archivo7 { get; set; }
        public string Archivo8 { get; set; }
        public string Archivo9 { get; set; }
        public string Archivo10 { get; set; }
        public string Aux1 { get; set; }
        public string Aux2 { get; set; }
        public string Aux3 { get; set; }
        public string Aux4 { get; set; }
        public string Aux5 { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public string Decision { get; set; }
        public string Comentario { get; set; }
        public string Estado { get; set; }
        public string Calificacion { get; set; }
        public string Vigencia { get; set; }
        public int Anexo { get; set; }
        public DateTime FechaSol { get; set; }
        public DateTime FechaApr { get; set; }
        public List<Direccion> Direcciones { get; set; }
        public List<BranchAssignment> BranchAssignments { get; set; }
        public List<Contacto> Contactos { get; set; }
        public List<CuentaBanco> CuentasBancarias { get; set; }

        public SocioDeNegocio()
        {
            Direcciones = new List<Direccion>();
            BranchAssignments = new List<BranchAssignment>();
        }
    }

    public class Direccion
    {
        [Required]
        [StringRange(ValoresValidos = new[] { "B", "S" }, MensajeError = "Tipo de dirección no válida. B: Fiscal; S: Envio")]
        public string AdressType { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Departamento { get; set; }
        [Required]
        public string Provincia { get; set; }
        [Required]
        public string Distrito { get; set; }
        public string ProvinciaDesc { get; set; }
        public string DistritoDesc { get; set; }
        [Required]
        public string DireccionDesc { get; set; }
    }

    public class Contacto
    {
        
        public string Nombre { get; set; }
        
        public string PrimerNombre { get; set; }
        
        public string SegundoNombre { get; set; }
        
        public string Apellido { get; set; }
        
        public string Celular { get; set; }
        
        public string Telefono { get; set; }
        
        public string Cargo { get; set; }
        
        public string Email { get; set; }
        
        public bool FlagEditar { get; set; }
        
        public bool FlagEliminar { get; set; }
    }

    public class CuentaBanco
    {
        public string BankCode { get; set; }
        public string Account { get; set; }
        public string AcctName { get; set; }
        public string Moneda { get; set; }
        public string Tipo { get; set; }
        public string U_CurrSAP { get; set; }
        public string U_EXM_INTERBANCARIA { get; set; }
        public string U_EXC_ACTIVO { get; set; }
        public bool EsDetraccion { get; set; }
    }

    public class BranchAssignment
    {
        [Required]
        public int BPLID { get; set; }
    }


    public class StringRangeAttribute : ValidationAttribute
    {
        public string[] ValoresValidos { get; set; }
        public string MensajeError { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (ValoresValidos?.Contains(value?.ToString()) == true)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(MensajeError);
        }
    }

}

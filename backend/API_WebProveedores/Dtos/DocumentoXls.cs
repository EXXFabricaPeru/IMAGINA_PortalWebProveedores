using System;
using System.Collections.Generic;
using WebProov_API.Models;

namespace WebProov_API.Dtos
{
    public class DocumentoXls
    {
        public string TipoDocumento { get; set; }
        public string Nro { get; set; }
        public string Sucursal { get; set; }
        public string NumAtCard { get; set; }
        public DateTime? FecDoc { get; set; }
        public DateTime? DocDueDate { get; set; }
        public string Estado { get; set; }
        public string DireccionFiscal { get; set; }
        public string CondicionPago { get; set; }
        public string Moneda { get; set; }
        public double DocTotal { get; set; }
        public string Comments { get; set; }
        public string Contacto { get; set; }
        public string Comprador { get; set; }
        public string FondoGrantia { get; set; }
        public double ImpAnticio { get; set; }
    }
}

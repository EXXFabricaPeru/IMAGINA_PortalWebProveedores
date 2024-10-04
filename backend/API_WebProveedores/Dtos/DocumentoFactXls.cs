using System;
using System.Collections.Generic;
using WebProov_API.Models;

namespace WebProov_API.Dtos
{
    public class DocumentoFactXls
    {
        public string TipoDocumento { get; set; }
        public string Nro { get; set; }
        public string Sucursal { get; set; }
        public string NumAtCard { get; set; }
        public DateTime? FecDoc { get; set; }
        public DateTime? DocDueDate { get; set; }
        public int DiasAtraso { get; set; }
        public string Estado { get; set; }
        public string CondicionPago { get; set; }
        public string Moneda { get; set; }
        public double FondoGrantia { get; set; }
        public double Detraccion { get; set; }
        public double Neto { get; set; }
        public double DocTotal { get; set; }
        public double ImportePagado { get; set; }
        public double Saldo { get; set; }
        public double ImpAnticio { get; set; }
        public string Comments { get; set; }
        
    }
}

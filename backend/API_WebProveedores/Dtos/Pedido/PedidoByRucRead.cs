using System;
using WebProov_API.Models;


namespace WebProov_API.Dtos
{
    public class PedidoByRucRead
    {
        public int DocEntry { get; set; }
        public string DocNum { get; set; }
        public string NumeroCotizacion { get; set; }
        //public string CardCode { get; set; }
        //public string CardName { get; set; }
        //public string LicTradNum { get; set; }
        public string NumAtCard { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public double DocTotal { get; set; }
        public double DocTotalFC { get; set; }
        public string DocCur { get; set; }
        public string Comments { get; set; }
        public string DocStatus { get; set; }
        public string SalesPersonCode { get; set; }
        public string CondicionPago { get; set; }
        public int IdSucursal { get; set; }
        public string Sucursal { get; set; }
        public DetalleDoc Item { get; set; }
    }
}

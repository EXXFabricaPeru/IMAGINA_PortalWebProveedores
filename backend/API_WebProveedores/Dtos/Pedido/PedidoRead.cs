using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProov_API.Models;

namespace WebProov_API.Dtos
{
    public class PedidoRead
    {
        public int DocEntry { get; set; }
        public string DocNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string NumAtCard { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? TaxDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public DateTime? U_EXC_FVCAFI { get; set; }
        public DateTime? U_EXC_INICON { get; set; }
        public DateTime? U_EXC_FINCON { get; set; }
        public int GroupNum { get; set; }
        //public string Project { get; set; }
        public string DocStatus { get; set; }
        public string ListaPrecio { get; set; }
        public string DireccionDespacho { get; set; }
        public string DireccionFiscal { get; set; }
        public string CondicionPago { get; set; }
        public string IdCotizacion { get; set; }
        public string NumeroCotizacion { get; set; }
        public string SalesPersonCode { get; set; }

        public double DiscSum { get; set; }
        public double DiscountPercent { get; set; }
        public double VatSum { get; set; }
        public double VatSumFC { get; set; }
        public double DocTotal { get; set; }
        public double DocTotalFC { get; set; }
        public string DocCur { get; set; }
        public double DocRate { get; set; }
        public string Comments { get; set; }
        public string TipoDocumento { get; set; }
        public string Sucursal { get; set; }
        public int IdSucursal { get; set; }
        public string Archivo { get; set; }
        public string NomArchivo { get; set; }
        public string Archivo2 { get; set; }
        public string NomArchivo2 { get; set; }
        public string Archivo3 { get; set; }
        public string NomArchivo3 { get; set; }
        public string Contacto { get; set; }
        public string Comprador { get; set; }
        public bool FlagAnticipo { get; set; }
        public bool FlagConformidad { get; set; }
        public string FondoGrantia { get; set; }
        public string UserReg { get; set; }
        public string Comentario1 { get; set; }
        public string Comentario2 { get; set; }
        public double PorFondoGar { get; set; }
        public double ImpAnticio { get; set; }

        public List<DetallePedido> DetallePedido { get; set; }
    }
}

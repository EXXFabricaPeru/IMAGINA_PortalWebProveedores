using System;
using System.Collections.Generic;

namespace WebProov_API.Models
{
    public class Documento
    {
        public int DocEntry { get; set; }
        public string DocNum { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string LicTradNum { get; set; }
        public string NumAtCard { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime? TaxDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? U_EXC_FVCAFI { get; set; }
        public DateTime? U_EXC_INICON { get; set; }
        public DateTime? U_EXC_FINCON { get; set; }
        public string FolioPref { get; set; }
        public int FolioNum { get; set; }

        public int GroupNum { get; set; }
        public string SalesPersonCode { get; set; }
        public string Contacto { get; set; }
        public string Comprador { get; set; }
        public string DocStatus { get; set; }
        public string ListaPrecio { get; set; }
        public string DireccionDespacho { get; set; }
        public string DireccionFiscal { get; set; }
        public string CondicionPago { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroCotizacion { get; set; }
        public double DiscSum { get; set; }
        public double DiscountPercent { get; set; }
        public double VatSum { get; set; }
        public double VatSumFC { get; set; }
        public double DocTotal { get; set; }
        public double Neto { get; set; }
        public double Detraccion { get; set; }
        public double DocTotalFC { get; set; }
        public double PaidToDate { get; set; }
        public double PaidToDateFC { get; set; }
        public string DocCur { get; set; }
        public double DocRate { get; set; }
        public int Atraso { get; set; }
        public double Saldo { get; set; }
        public double SaldoFC { get; set; }
        public string Comments { get; set; }
        public List<DetallePedido> DetallePedido { get; set; }

        //cub
        public string UserReg { get; set; }
        public int IdSucursal { get; set; }
        public string Sucursal { get; set; }
        public string Archivo { get; set; }
        public string NomArchivo { get; set; }
        public string Archivo2 { get; set; }
        public string NomArchivo2 { get; set; }
        public string Archivo3 { get; set; }
        public string NomArchivo3 { get; set; }
        public string Archivo4 { get; set; }
        public string NomArchivo4 { get; set; }
        public string Archivo5 { get; set; }
        public string NomArchivo5 { get; set; }
        public string AplicaFactoring { get; set; }
        public string ProveedorFactoring { get; set; }
        public string RSProveedorFactoring { get; set; }
        public string FondoGrantia { get; set; }
        public double PorFondoGar { get; set; }
        public bool FlagAnticipo { get; set; }
        public bool FlagConformidad { get; set; }
        public string Password { get; set; }
        public string Comentario1 { get; set; }
        public string Comentario2 { get; set; }
        public double ImpAnticio { get; set; }

        public DetalleDoc Item { get; set; }
    }

    public class DetallePedido : DetalleDoc
    {
        public double PendQuantity { get; set; }
        public DateTime ShipDate { get; set; }
        public double Stock { get; set; }
        public double Cantidad { get; set; }
        public double Importe { get; set; }
    }

    public class DetalleDoc
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string UnitMsr { get; set; }
        public double Quantity { get; set; }
        public string Dscription { get; set; }
        public double DiscountPercent { get; set; }
        public double Price { get; set; }
        public string WhsCode { get; set; }
        public string TaxCode { get; set; }
        public double PriceAfVAT { get; set; }
        public double LineTotal { get; set; }
        public string Project { get; set; }
        public int DocEntry { get; set; }
        public double CantInicial { get; set; } 
        public double CantAcumulada { get; set; } 
        public double CantActual { get; set; } 
        public double CantSaldo { get; set; }  
        public double ImpInicial { get; set; } 
        public double ImpAcumulada { get; set; } 
        public double ImpActual { get; set; } 
        public double ImpSaldo { get; set; } 
        public double PorInicial { get; set; } 
        public double PorAnterior { get; set; } 
        public double PorActual { get; set; } 
        public double PorAcumActual { get; set; } 
        public double PorSaldo { get; set; } 
    }
}


using System.Collections.Generic;
using WebProov_API.Models;

namespace WebProov_API.Data.Interfaces
{
    public interface IFacturaRepository
    {        
        List<Documento> GetListaFechaPagoByRuc(string ruc, string fi, string ff, string estado);

        string GetListaDescargar(string ruc, string fi, string ff, string estado);

        string CrearFactura(Documento document);
        string ActualizarFactura(Documento document);
        string CrearFacturaAnticipo(Documento document);

        Documento GetFacturaId(int id);
        Documento GetAnticipoId(int id);
        Documento GetBorradorId(int id);

    }
}
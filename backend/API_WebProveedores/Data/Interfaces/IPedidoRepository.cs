
using System.Collections.Generic;
using WebProov_API.Models;

namespace WebProov_API.Data.Interfaces
{
    public interface IPedidoRepository : IDocumentRepository
    {       
        List<Documento> GetListaByRuc(string ruc, string fecIni, string fecFin, string estado);

        string ConfirmarOC(Documento pedido);

        string AprobarConformidadServicio(string nroConformidad, string user, string estado, string commentario, string contrasena);

        Documento GetConformidadById(int id);

        Documento GetConformidadAprById(int id);

        List<Documento> GetConformidadByRucList(string ruc, string fecIni, string fecFin, string estado);

        List<Documento> GetConformidadDisponible(string ruc, string fecIni, string fecFin, string sucursal);

        string file_Descargar(string archivo);

        string descargarLista(string ruc, string fecIni, string fecFin, string estado);

        string descargarListaConformidad(string ruc, string fecIni, string fecFin, string estado);

    }
}
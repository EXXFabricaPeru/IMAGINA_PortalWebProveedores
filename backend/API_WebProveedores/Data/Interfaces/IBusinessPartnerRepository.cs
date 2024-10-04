using System.Collections.Generic;
using WebProov_API.Models;

namespace WebProov_API.Data.Interfaces
{
    public interface IBusinessPartnerRepository
    {
        //public bool CrearSocio(SocioDeNegocio socio);
        //public bool ActualizarSocio(SocioDeNegocio socio);
        //public SocioDeNegocio GetSocioByRUCRaz(string id);
        public SocioDeNegocio GetSocioByCardCode(string id);

        //C.U.B
        string fn_Proveedor_Borrador_Registrar(SocioDeNegocio cliente);
        string fn_Proveedor_Aprobar(SocioDeNegocio proveedor);
        string file_Descargar(string code, string ruc);
        SocioDeNegocio GetDraft(string ruc);
        List<SocioDeNegocio> GetLista(string valor, string estado);
        List<SocioDeNegocio> GetListaFactor();
        string formatoDescargar(string name);
    }
}
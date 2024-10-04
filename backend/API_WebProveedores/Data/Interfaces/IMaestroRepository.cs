using System.Collections.Generic;
using WebProov_API.Models;

namespace WebProov_API.Data.Interfaces
{
    public interface IMaestroRepository
    {
        List<Maestro> getCondicionesPago();
        List<Maestro> getDepartamento(string codPais);
        List<Maestro> getProvincia(string codDepartamento);
        List<Maestro> getDistrito(string codProvincia);
        List<Maestro> getMoneda();
        List<Maestro> getBanco();
        List<Maestro> getTipoCuenta();
        List<Maestro> getConfiguracion();
        List<Maestro> getProveedorFactoring();
        List<Maestro> getFormatos();
    }
}

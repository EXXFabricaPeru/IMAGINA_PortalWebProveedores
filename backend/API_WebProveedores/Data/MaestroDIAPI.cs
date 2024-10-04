using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.IO;
using WebProov_API.Data.Interfaces;
using WebProov_API.Models;
using WebProov_API.Util;

namespace WebProov_API.Data
{
    public class MaestroDIAPI : IMaestroRepository
    {
        private Company _company;
        public MaestroDIAPI()
        {
            _company = DIAPIConexion.GetDIAPIConexion();
        }

        public List<Maestro> getCondicionesPago()
        {
            List<Maestro> oLista = new List<Maestro>();
            try
            {
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.getCondicionesPago());
                if (oRS.RecordCount == 0)
                    return null;

                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    Maestro item = new Maestro();
                    item.Codigo = oRS.Fields.Item(0).Value.ToString();
                    item.Descripcion = oRS.Fields.Item(1).Value;
                    item.Valor_01 = oRS.Fields.Item(2).Value.ToString();
                    oLista.Add(item);
                    oRS.MoveNext();
                }
            }
            catch(Exception ex) 
            {
                oLista = null;
                string msg = ex.Message;
            }
            return oLista;
        }

        public List<Maestro> getDepartamento(string codPais)
        {
            List<Maestro> oLista = new List<Maestro>();
            try
            {
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.getDepartamento(codPais));
                if (oRS.RecordCount == 0)
                    return null;

                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    Maestro item = new Maestro();
                    item.Codigo = oRS.Fields.Item(0).Value;
                    item.Descripcion = oRS.Fields.Item(1).Value;
                    oLista.Add(item);
                    oRS.MoveNext();
                }
            }
            catch
            {
                oLista = null;
            }
            return oLista;
        }

        public List<Maestro> getProvincia(string codDepartamento)
        {
            List<Maestro> oLista = new List<Maestro>();
            try
            {
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.getProvincia(codDepartamento));
                if (oRS.RecordCount == 0)
                    return null;

                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    Maestro item = new Maestro();
                    item.Codigo = oRS.Fields.Item(0).Value;
                    item.Descripcion = oRS.Fields.Item(1).Value;
                    oLista.Add(item);
                    oRS.MoveNext();
                }
            }
            catch
            {
                oLista = null;
            }
            return oLista;
        }

        public List<Maestro> getDistrito(string codProvincia)
        {
            List<Maestro> oLista = new List<Maestro>();
            try
            {
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.getDistrito(codProvincia));
                if (oRS.RecordCount == 0)
                    return null;

                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    Maestro item = new Maestro();
                    item.Codigo = oRS.Fields.Item(0).Value;
                    item.Descripcion = oRS.Fields.Item(1).Value;
                    oLista.Add(item);
                    oRS.MoveNext();
                }
            }
            catch(Exception ex) 
            {
                string msg = ex.Message;
                oLista = null;
            }
            return oLista;
        }

        public List<Maestro> getMoneda()
        {
            List<Maestro> oLista = new List<Maestro>();
            try
            {
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.getMoneda());
                if (oRS.RecordCount == 0)
                    return null;

                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    Maestro item = new Maestro();
                    item.Codigo = oRS.Fields.Item(0).Value;
                    item.Descripcion = oRS.Fields.Item(1).Value;
                    oLista.Add(item);
                    oRS.MoveNext();
                }
            }
            catch
            {
                oLista = null;
            }
            return oLista;
        }

        public List<Maestro> getBanco()
        {
            List<Maestro> oLista = new List<Maestro>();
            try
            {
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.getBanco());
                if (oRS.RecordCount == 0)
                    return null;

                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    Maestro item = new Maestro();
                    item.Codigo = oRS.Fields.Item(0).Value;
                    item.Descripcion = oRS.Fields.Item(1).Value;
                    oLista.Add(item);
                    oRS.MoveNext();
                }
            }
            catch
            {
                oLista = null;
            }
            return oLista;
        }

        public List<Maestro> getTipoCuenta()
        {
            List<Maestro> oLista = new List<Maestro>();
            try
            {
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.getTipoCuenta());
                if (oRS.RecordCount == 0)
                    return null;

                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    Maestro item = new Maestro();
                    item.Codigo = oRS.Fields.Item(0).Value;
                    item.Descripcion = oRS.Fields.Item(1).Value;
                    oLista.Add(item);
                    oRS.MoveNext();
                }
            }
            catch
            {
                oLista = null;
            }
            return oLista;
        }

        public List<Maestro> getConfiguracion()
        {
            List<Maestro> oLista = new List<Maestro>();
            try
            {
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.getConfiguracion());
                if (oRS.RecordCount == 0)
                    return null;

                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    Maestro item = new Maestro();
                    item.Codigo = oRS.Fields.Item(0).Value;
                    item.Descripcion = oRS.Fields.Item(2).Value;
                    item.Valor_01 = oRS.Fields.Item(1).Value;
                    item.Flag01 = oRS.Fields.Item(3).Value == "N" ? false : true;
                    oLista.Add(item);
                    oRS.MoveNext();
                }
            }
            catch
            {
                oLista = null;
            }
            return oLista;
        }
        
        public List<Maestro> getProveedorFactoring()
        {
            List<Maestro> oLista = new List<Maestro>();
            try
            {
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.getProveedorFactoring());
                if (oRS.RecordCount == 0)
                    return null;

                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    Maestro item = new Maestro();
                    item.Codigo = oRS.Fields.Item(0).Value;
                    item.Descripcion = oRS.Fields.Item(2).Value;
                    item.Valor_01 = oRS.Fields.Item(1).Value;
                    oLista.Add(item);
                    oRS.MoveNext();
                }
            }
            catch
            {
                oLista = null;
            }
            return oLista;
        }

        public string getNameSucursal(string code)
        {
            string xSucursal;
            try
            {
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.getSucursal());
                if (oRS.RecordCount == 0)
                    return "No hay sucursales";

                xSucursal = oRS.Fields.Item(1).Value;
            }
            catch(Exception ex) 
            {
                xSucursal = ex.Message;
            }
            return xSucursal;
        }

        public List<Maestro> getFormatos()
        {
            List<Maestro> formatos = new List<Maestro>();
            string xRuta = Startup.StaConfig["DIAPI:RutaFormatoProv"];
            DirectoryInfo di = new DirectoryInfo(xRuta);

            foreach (FileInfo file in di.GetFiles())
            {
                Maestro formato = new Maestro();
                formato.Descripcion = file.Name;
                //string xRutaFile = xRuta + file.Name;

                //Byte[] fileBytes = File.ReadAllBytes(xRutaFile);
                //formato.Archivo = Convert.ToBase64String(fileBytes);

                formatos.Add(formato);
            }

            return formatos;
        }
    }
}

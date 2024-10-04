using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using WebProov_API.Data.Interfaces;
using WebProov_API.Models;
using WebProov_API.Util;

namespace WebProov_API.Data
{
    public class BusinessPartnerDIAPI : IBusinessPartnerRepository
    {
        private Company _company;
        BusinessPartners oBp;
        public BusinessPartnerDIAPI()
        {
            _company = DIAPIConexion.GetDIAPIConexion();
        }
        public SocioDeNegocio GetSocioByCardCode(string id)
        {
            try
            {
                SocioDeNegocio socio = new SocioDeNegocio();
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.GetBPbyCardCode(id));
                if (oRS.RecordCount == 0)
                    return socio;

                socio.CardCode = oRS.Fields.Item("CardCode").Value.ToString().Trim();
                socio.CardName = oRS.Fields.Item("CardName").Value.ToString().Trim();
                socio.LicTradNum = oRS.Fields.Item("LicTradNum").Value.ToString().Trim();
                socio.EmailAddress = oRS.Fields.Item("EmailAddress").Value.ToString().Trim();
                socio.Currency = oRS.Fields.Item("Currency").Value.ToString().Trim();
                socio.Calificacion = oRS.Fields.Item("Calificacion").Value.ToString().Trim();
                socio.Vigencia = oRS.Fields.Item("Vigencia").Value.ToString("dd/MM/yyyy");
                //socio.CardType = oRS.Fields.Item("CardType").Value.ToString().Trim();
                //socio.GroupName = oRS.Fields.Item("GroupCode").Value.ToString().Trim();
                //socio.Phone1 = oRS.Fields.Item("Phone1").Value.ToString().Trim();
                //socio.Cellular = oRS.Fields.Item("Cellular").Value.ToString().Trim();
                //socio.SalesPerson = oRS.Fields.Item("SalesPerson").Value.ToString().Trim();
                //socio.MainDirection = oRS.Fields.Item("MainDirection").Value.ToString().Trim();
                //socio.Contacto = oRS.Fields.Item("Contacto").Value.ToString().Trim();
                //socio.ContactoPhone = oRS.Fields.Item("ContactoPhone").Value.ToString().Trim();
                //socio.CreditLine = double.Parse(oRS.Fields.Item("CreditLine").Value.ToString());
                //socio.SaldoDisponible = double.Parse(oRS.Fields.Item("SaldoDisponible").Value.ToString());
                //socio.DeudaALaFecha = double.Parse(oRS.Fields.Item("DeudaALaFecha").Value.ToString());
                //socio.FormaPago = oRS.Fields.Item("FormaPago").Value.ToString();

                return socio;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        //public SocioDeNegocio GetSocioByRUCRaz(string id)
        //{
        //    try
        //    {
        //        SocioDeNegocio socio = new SocioDeNegocio();
        //        Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
        //        oRS.DoQuery(Queries.GetBPbyRUCRazSoc(id));
        //        if (oRS.RecordCount == 0)
        //            return null;

        //        socio.CardCode = oRS.Fields.Item("CardCode").Value.ToString().Trim();
        //        socio.CardName = oRS.Fields.Item("CardName").Value.ToString().Trim();
        //        //socio.CardType = oRS.Fields.Item("CardType").Value.ToString().Trim();
        //        socio.LicTradNum = oRS.Fields.Item("LicTradNum").Value.ToString().Trim();
        //         socio.GroupName = oRS.Fields.Item("GroupCode").Value.ToString().Trim();
        //        socio.Phone1 = oRS.Fields.Item("Phone1").Value.ToString().Trim();
        //        socio.Cellular = oRS.Fields.Item("Cellular").Value.ToString().Trim();
        //        socio.EmailAddress = oRS.Fields.Item("EmailAddress").Value.ToString().Trim();
        //        socio.Currency = oRS.Fields.Item("Currency").Value.ToString().Trim();
        //        socio.SalesPerson = oRS.Fields.Item("SalesPerson").Value.ToString().Trim();
        //        socio.MainDirection = oRS.Fields.Item("MainDirection").Value.ToString().Trim();
        //        socio.Contacto = oRS.Fields.Item("Contacto").Value.ToString().Trim();
        //        socio.ContactoPhone = oRS.Fields.Item("ContactoPhone").Value.ToString().Trim();
        //        socio.CreditLine = double.Parse(oRS.Fields.Item("CreditLine").Value.ToString());
        //        socio.SaldoDisponible = double.Parse(oRS.Fields.Item("SaldoDisponible").Value.ToString());
        //        socio.DeudaALaFecha = double.Parse(oRS.Fields.Item("DeudaALaFecha").Value.ToString());
        //        socio.FormaPago = oRS.Fields.Item("FormaPago").Value.ToString();

        //        return socio;
        //    }
        //    catch (System.Exception)
        //    {
        //        throw;
        //    }
        //}

        //public bool ActualizarSocio(SocioDeNegocio socio)
        //{
        //    try
        //    {
        //        oBp = _company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
        //        if (!oBp.GetByKey(socio.CardCode))
        //            throw new System.Exception("El socio con código: " + socio.CardCode + " no existe en la sociedad.");
        //        OCRD(socio, oBp);
        //        int i = 0;
        //        foreach (Direccion direccion in socio.Direcciones)
        //        {
        //            oBp.Addresses.SetCurrentLine(i);
        //            if (oBp.Addresses.AddressName == direccion.Address)
        //            {
        //                CRD1(direccion, oBp);
        //            }
        //            else
        //            {
        //                oBp.Addresses.AddressName = direccion.Address;
        //                oBp.Addresses.AddressType = direccion.AdressType == "B" ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo;
        //                CRD1(direccion, oBp);
        //                oBp.Addresses.Add();
        //            }
        //            i++;
        //        }
        //        if (oBp.Update() != 0)
        //            throw new System.Exception(_company.GetLastErrorDescription());

        //        return true;
        //    }
        //    catch (System.Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(oBp);
        //    }
        //}

        //public bool CrearSocio(SocioDeNegocio socio)
        //{
        //    try
        //    {
        //        oBp = _company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
        //        oBp.CardCode = socio.CardCode;
        //        OCRD(socio, oBp);
        //        foreach (Direccion direccion in socio.Direcciones)
        //        {
        //            oBp.Addresses.AddressName = direccion.Address;
        //            oBp.Addresses.AddressType = direccion.AdressType == "B" ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo;
        //            CRD1(direccion, oBp);
        //            oBp.Addresses.Add();
        //        }
        //        foreach (BranchAssignment sucursal in socio.BranchAssignments)
        //        {
        //            oBp.BPBranchAssignment.BPLID = sucursal.BPLID;
        //            oBp.BPBranchAssignment.Add();
        //        }
        //        if (oBp.Add() != 0)
        //            throw new System.Exception(_company.GetLastErrorDescription());

        //        return true;
        //    }
        //    catch (System.Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(oBp);
        //    }
        //}

        //private void OCRD(SocioDeNegocio socio, BusinessPartners oBp)
        //{
        //    if (socio.U_EXX_TIPOPERS == "TPJ")
        //        oBp.CardName = socio.CardName;
        //    else
        //    {
        //        oBp.CardName = string.Join(" ", socio.ApellidoPaterno, socio.ApellidoMaterno, socio.Nombre, socio.SegundoNombre);
        //        oBp.UserFields.Fields.Item("U_EXX_PRIMERNO").Value = socio.Nombre;
        //        oBp.UserFields.Fields.Item("U_EXX_SEGUNDNO").Value = socio.SegundoNombre;
        //        oBp.UserFields.Fields.Item("U_EXX_APELLPAT").Value = socio.ApellidoPaterno;
        //        oBp.UserFields.Fields.Item("U_EXX_APELLMAT").Value = socio.ApellidoMaterno;
        //    }
        //    //oBp.GroupCode = socio.GroupCode;
        //    oBp.FederalTaxID = socio.LicTradNum;
        //    oBp.Currency = socio.Currency;
        //    oBp.PayTermsGrpCode = socio.PayTermsGrpCode;
        //    oBp.Phone1 = socio.Phone1;
        //    oBp.Phone2 = socio.Phone2;
        //    oBp.Cellular = socio.Cellular;
        //    oBp.EmailAddress = socio.EmailAddress;

        //    oBp.UserFields.Fields.Item("U_EXX_TIPOPERS").Value = socio.U_EXX_TIPOPERS;
        //    oBp.UserFields.Fields.Item("U_EXX_TIPODOCU").Value = socio.U_EXX_TIPODOCU;
        //    if (!string.IsNullOrEmpty(socio.U_EXX_ESTCONTR)) oBp.UserFields.Fields.Item("U_EXX_ESTCONTR").Value = socio.U_EXX_ESTCONTR;
        //    if (!string.IsNullOrEmpty(socio.U_EXX_CNDCONTR)) oBp.UserFields.Fields.Item("U_EXX_CNDCONTR").Value = socio.U_EXX_CNDCONTR;
        //    //if ((socio.U_SCO_IDCLIENTE) != null) oBp.UserFields.Fields.Item("U_SCO_IDCLIENTE").Value = socio.U_SCO_IDCLIENTE;
        //}
        //private void CRD1(Direccion direccion, BusinessPartners oBp)
        //{
        //    oBp.Addresses.Country = "PE";
        //    oBp.Addresses.State = direccion.Departamento;
        //    oBp.Addresses.County = direccion.Provincia;
        //    oBp.Addresses.ZipCode = direccion.Distrito;
        //    oBp.Addresses.Street = direccion.DireccionDesc;
        //}

        public SocioDeNegocio GetDraft(string ruc)
        {
            try
            {
                SocioDeNegocio socio = new SocioDeNegocio();
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.GetDraft(ruc));
                if (oRS.RecordCount == 0)
                    return null;
                socio.Code = oRS.Fields.Item("Code").Value.ToString().Trim();
                socio.CardCode = oRS.Fields.Item("U_EXX_CARDCODE").Value.ToString().Trim();
                socio.CardName = oRS.Fields.Item("U_EXX_CARDNAME").Value.ToString().Trim();
                socio.LicTradNum = oRS.Fields.Item("U_EXX_RUC").Value.ToString().Trim();
                socio.Phone1 = oRS.Fields.Item("U_EXX_TELEFONO").Value.ToString().Trim();
                socio.Cellular = oRS.Fields.Item("U_EXX_CELULAR").Value.ToString().Trim();
                socio.EmailAddress = oRS.Fields.Item("U_EXX_EMAIL").Value.ToString().Trim();
                socio.Password = oRS.Fields.Item("U_EXX_PASSWORD").Value.ToString().Trim();
                socio.FormaPago = oRS.Fields.Item("U_EXX_CONDICIONPAGO").Value.ToString();
                socio.U_EXX_TIPODOCU = oRS.Fields.Item("U_EXX_TIPODOC").Value.ToString();
                socio.Anexo = int.Parse(oRS.Fields.Item("U_EXX_IDANEXO").Value.ToString());
                socio.Archivo1 = oRS.Fields.Item("U_EXX_ARCHIVO1").Value.ToString().Trim();
                socio.Archivo2 = oRS.Fields.Item("U_EXX_ARCHIVO2").Value.ToString().Trim();
                socio.Archivo3 = oRS.Fields.Item("U_EXX_ARCHIVO3").Value.ToString().Trim();
                socio.Archivo4 = oRS.Fields.Item("U_EXX_ARCHIVO4").Value.ToString().Trim();
                socio.Archivo5 = oRS.Fields.Item("U_EXX_ARCHIVO5").Value.ToString().Trim();
                socio.Archivo6 = oRS.Fields.Item("U_EXX_ARCHIVO6").Value.ToString().Trim();
                socio.Archivo7 = oRS.Fields.Item("U_EXX_ARCHIVO7").Value.ToString().Trim();
                socio.Archivo8 = oRS.Fields.Item("U_EXX_ARCHIVO8").Value.ToString().Trim();
                socio.Archivo9 = oRS.Fields.Item("U_EXX_ARCHIVO9").Value.ToString().Trim();
                socio.Archivo10 = oRS.Fields.Item("U_EXX_ARCHIVO10").Value.ToString().Trim();
                socio.Aux1 = oRS.Fields.Item("U_EXX_AUX1").Value.ToString().Trim();
                socio.Aux2 = oRS.Fields.Item("U_EXX_AUX2").Value.ToString().Trim();
                socio.Aux3 = oRS.Fields.Item("U_EXX_AUX3").Value.ToString().Trim();
                socio.Aux4 = oRS.Fields.Item("U_EXX_AUX4").Value.ToString().Trim();
                socio.Aux5 = oRS.Fields.Item("U_EXX_AUX5").Value.ToString().Trim();
                socio.Estado = oRS.Fields.Item("U_EXX_AUTORIZADO").Value.ToString().Trim();

                List<Contacto> contactos = new List<Contacto>();
                oRS.DoQuery(Queries.GetDraftContact(socio.Code));
                for(int i = 0; i< oRS.RecordCount; i++)
                {
                    Contacto contacto = new Contacto();
                    contacto.Nombre = oRS.Fields.Item("U_EXX_ID").Value.ToString().Trim();    
                    contacto.PrimerNombre = oRS.Fields.Item("U_EXX_PRIMERNOMBRE").Value.ToString().Trim();    
                    contacto.SegundoNombre = oRS.Fields.Item("U_EXX_SEGUNDONOMBRE").Value.ToString().Trim();    
                    contacto.Apellido = oRS.Fields.Item("U_EXX_APELLIDOS").Value.ToString().Trim();    
                    contacto.Celular = oRS.Fields.Item("U_EXX_CELULAR").Value.ToString().Trim();    
                    contacto.Telefono = oRS.Fields.Item("U_EXX_TELEFONO").Value.ToString().Trim();    
                    contacto.Email = oRS.Fields.Item("U_EXX_EMAIL").Value.ToString().Trim();    
                    contacto.Cargo = oRS.Fields.Item("U_EXX_CARGO").Value.ToString().Trim();  
                    contactos.Add(contacto);
                    oRS.MoveNext();
                }
                socio.Contactos = contactos;

                List<Direccion> direcciones = new List<Direccion>();
                oRS.DoQuery(Queries.GetDraftDireccion(socio.Code));
                for(int i = 0; i < oRS.RecordCount; i++)
                {
                    Direccion direccion = new Direccion();
                    direccion.Address = oRS.Fields.Item("U_EXX_ID").Value.ToString().Trim();
                    direccion.AdressType = oRS.Fields.Item("U_EXX_TIPO").Value.ToString().Trim();
                    direccion.Departamento = oRS.Fields.Item("U_EXX_DEPARTAMENTO").Value.ToString().Trim();
                    direccion.Provincia = oRS.Fields.Item("U_EXX_PROVINCIA").Value.ToString().Trim();
                    direccion.ProvinciaDesc = oRS.Fields.Item("Provincia").Value.ToString().Trim();
                    direccion.Distrito = oRS.Fields.Item("U_EXX_DISTRITO").Value.ToString().Trim();
                    direccion.DistritoDesc = oRS.Fields.Item("Distrito").Value.ToString().Trim();
                    direccion.DireccionDesc = oRS.Fields.Item("U_EXX_DIRECCION").Value.ToString().Trim();
                    direcciones.Add(direccion);
                    oRS.MoveNext();
                }
                socio.Direcciones = direcciones;

                List<CuentaBanco> cuentas = new List<CuentaBanco>();
                oRS.DoQuery(Queries.GetDraftCuentas(socio.Code));
                for(int i = 0; i < oRS.RecordCount; i++)
                {
                    CuentaBanco cuenta = new CuentaBanco();
                    cuenta.BankCode = oRS.Fields.Item("U_EXX_BANKCODE").Value.ToString().Trim();
                    cuenta.AcctName = oRS.Fields.Item("U_EXX_NOMBRECUENTA").Value.ToString().Trim();
                    cuenta.Account = oRS.Fields.Item("U_EXX_NROCUENTA").Value.ToString().Trim();
                    cuenta.Tipo = oRS.Fields.Item("U_EXX_TIPO").Value.ToString().Trim();
                    cuenta.Moneda = oRS.Fields.Item("U_EXX_MONEDA").Value.ToString().Trim();
                    cuenta.U_CurrSAP = oRS.Fields.Item("U_EXX_MONEDA").Value.ToString().Trim();
                    cuenta.U_EXM_INTERBANCARIA = oRS.Fields.Item("U_EXX_NROINTERBANCARIO").Value.ToString().Trim();
                    cuenta.EsDetraccion = oRS.Fields.Item("U_EXX_ESDETRACCION").Value.ToString().Trim() == "Y" ? true : false;
                    cuentas.Add(cuenta); 
                    oRS.MoveNext();
                }
                socio.CuentasBancarias = cuentas;

                return socio;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SocioDeNegocio> GetLista(string valor, string estado)
        {
            List<SocioDeNegocio> oLista = new List<SocioDeNegocio>();
            Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRs.DoQuery(Queries.GetListaProveedor(valor, estado));
            if(oRs.RecordCount == 0)
                return oLista;

            for(int i = 0; i< oRs.RecordCount; i++)
            {
                SocioDeNegocio item = new SocioDeNegocio();
                item.Code = oRs.Fields.Item("Code").Value;
                item.CardCode = oRs.Fields.Item("CardCode").Value;
                item.CardName = oRs.Fields.Item("CardName").Value;
                item.LicTradNum = oRs.Fields.Item("LicTradNum").Value;
                item.Estado = oRs.Fields.Item("Estado").Value;
                item.EmailAddress = oRs.Fields.Item("E_Mail").Value;
                item.Type = oRs.Fields.Item("Tipo").Value;
                item.FechaSol = oRs.Fields.Item("FecSol").Value;
                item.FechaApr = oRs.Fields.Item("FecApr").Value;
                oLista.Add(item);
                oRs.MoveNext();
            }

            return oLista;
        }
        
        public List<SocioDeNegocio> GetListaFactor()
        {
            List<SocioDeNegocio> oLista = new List<SocioDeNegocio>();
            Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRs.DoQuery(Queries.GetListaProveedorFactor());
            if(oRs.RecordCount == 0)
                return oLista;

            for(int i = 0; i< oRs.RecordCount; i++)
            {
                SocioDeNegocio item = new SocioDeNegocio();
                item.CardCode = oRs.Fields.Item("CardCode").Value;
                item.CardName = oRs.Fields.Item("CardName").Value;
                oLista.Add(item);
                oRs.MoveNext();
            }

            return oLista;
        }

        public string fn_Proveedor_Borrador_Registrar(SocioDeNegocio proveedor)
        {
            string xRpta;
            string xQuery = "";
            Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);

            try
            {
                xQuery = $"SELECT COUNT(*) FROM \"@EXX_WP_PROV_CAB\" WHERE U_EXX_RUC = '{proveedor.LicTradNum}' AND \"U_EXX_AUTORIZADO\" != 'N'";
                oRs.DoQuery(xQuery);

                if (oRs.Fields.Item(0).Value > 0)
                    return xRpta = "El documento de identidad ya cuenta con un registro en proceso de aprobación, se le confirmará el alta.";

                xQuery = $"SELECT COUNT(*) FROM OCRD WHERE \"LicTradNum\" = '{proveedor.LicTradNum}'";
                oRs.DoQuery(xQuery);

                if (oRs.Fields.Item(0).Value > 0)
                    return xRpta = "El documento de identidad ya se encuentra registrado. Sino recuerda su contraseña, haga clic en \"Olvide mi Contraseña\".";

                xQuery = "SELECT MAX(TO_NUMBER(\"Code\")) + 1 FROM \"@EXX_WP_PROV_CAB\"";
                oRs.DoQuery(xQuery);

                string code = oRs.Fields.Item(0).Value.ToString();
                code = code.PadLeft(10, '0');

                #region Adjuntos
                string path = "";

                Recordset rs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                xQuery = "SELECT \"AttachPath\" FROM OADP";
                rs.DoQuery(xQuery);
                if (rs.RecordCount > 0)
                {
                    path = rs.Fields.Item(0).Value;
                }

                string xRuta1 = "";
                string xRuta2 = "";
                string xRuta3 = "";
                string xRuta4 = "";
                string xRuta5 = "";
                string xRuta6 = "";
                string xRuta7 = "";
                string xRuta8 = "";
                string xRuta9 = "";
                string xRuta10 = "";
                string xRuta11 = "";
                string xRuta12 = "";
                string xRuta13 = "";
                string xRuta14 = "";
                string xRuta15 = "";

                int entryAtt = 0;
                Attachments2 oATT = _company.GetBusinessObject(BoObjectTypes.oAttachments2);
                //Archivo1
                xRuta1 = path + "DeclaracionJuradaAntiCorrupcion_" + proveedor.LicTradNum + ".pdf";
                File.WriteAllBytes(xRuta1, Convert.FromBase64String(proveedor.Archivo1));

                oATT.Lines.FileName = Path.GetFileNameWithoutExtension("DeclaracionJuradaAntiCorrupcion_" + proveedor.LicTradNum);
                oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                oATT.Lines.Override = BoYesNoEnum.tYES;

                oATT.Lines.Add();
                //Archivo2
                xRuta2 = path + "CodigoEticaConducta_" + proveedor.LicTradNum + ".pdf";
                File.WriteAllBytes(xRuta2, Convert.FromBase64String(proveedor.Archivo2));

                oATT.Lines.FileName = Path.GetFileNameWithoutExtension("CodigoEticaConducta_" + proveedor.LicTradNum);
                oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                oATT.Lines.Override = BoYesNoEnum.tYES;

                oATT.Lines.Add();
                //Archivo3
                xRuta3 = path + "DeclaracionJuradaConfidecialidad_" + proveedor.LicTradNum + ".pdf";
                File.WriteAllBytes(xRuta3, Convert.FromBase64String(proveedor.Archivo3));

                oATT.Lines.FileName = Path.GetFileNameWithoutExtension("DeclaracionJuradaConfidecialidad_" + proveedor.LicTradNum);
                oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                oATT.Lines.Override = BoYesNoEnum.tYES;

                oATT.Lines.Add();
                //Archivo4
                xRuta4 = path + "DeclaracionJuradaCononocimiento_" + proveedor.LicTradNum + ".pdf";
                File.WriteAllBytes(xRuta4, Convert.FromBase64String(proveedor.Archivo4));

                oATT.Lines.FileName = Path.GetFileNameWithoutExtension("DeclaracionJuradaCononocimiento_" + proveedor.LicTradNum);
                oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                oATT.Lines.Override = BoYesNoEnum.tYES;

                oATT.Lines.Add();
                //Archivo5
                xRuta5 = path + "FichaRUC_" + proveedor.LicTradNum + ".pdf";
                File.WriteAllBytes(xRuta5, Convert.FromBase64String(proveedor.Archivo5));

                oATT.Lines.FileName = Path.GetFileNameWithoutExtension("FichaRUC_" + proveedor.LicTradNum);
                oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                oATT.Lines.Override = BoYesNoEnum.tYES;

                oATT.Lines.Add();
                //Archivo6
                xRuta6 = path + "ReporteTributarioSunat_" + proveedor.LicTradNum + ".pdf";
                File.WriteAllBytes(xRuta6, Convert.FromBase64String(proveedor.Archivo6));

                oATT.Lines.FileName = Path.GetFileNameWithoutExtension("ReporteTributarioSunat_" + proveedor.LicTradNum);
                oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                oATT.Lines.Override = BoYesNoEnum.tYES;

                oATT.Lines.Add();
                //Archivo7
                xRuta7 = path + "ReportePlataformaALFT_" + proveedor.LicTradNum + ".pdf";
                File.WriteAllBytes(xRuta7, Convert.FromBase64String(proveedor.Archivo7));

                oATT.Lines.FileName = Path.GetFileNameWithoutExtension("ReportePlataformaALFT_" + proveedor.LicTradNum);
                oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                oATT.Lines.Override = BoYesNoEnum.tYES;

                oATT.Lines.Add();
                //Archivo8
                xRuta8 = path + "ReporteCentralRiesgo_" + proveedor.LicTradNum + ".pdf";
                File.WriteAllBytes(xRuta8, Convert.FromBase64String(proveedor.Archivo8));

                oATT.Lines.FileName = Path.GetFileNameWithoutExtension("ReporteCentralRiesgo_" + proveedor.LicTradNum);
                oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                oATT.Lines.Override = BoYesNoEnum.tYES;

                oATT.Lines.Add();
                //Archivo9
                xRuta9 = path + "ReporteR03_" + proveedor.LicTradNum + ".pdf";
                File.WriteAllBytes(xRuta9, Convert.FromBase64String(proveedor.Archivo9));

                oATT.Lines.FileName = Path.GetFileNameWithoutExtension("ReporteR03_" + proveedor.LicTradNum);
                oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                oATT.Lines.Override = BoYesNoEnum.tYES;

                oATT.Lines.Add();
                //Archivo10
                xRuta10 = path + "RepresentanteLegalDni_" + proveedor.LicTradNum + ".pdf" ;
                File.WriteAllBytes(xRuta10, Convert.FromBase64String(proveedor.Archivo10));

                oATT.Lines.FileName = Path.GetFileNameWithoutExtension("RepresentanteLegalDni_" + proveedor.LicTradNum);
                oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                oATT.Lines.Override = BoYesNoEnum.tYES;

                List<Maestro> oListaConfig = new MaestroDIAPI().getConfiguracion();

                if (proveedor.Aux1 != null && proveedor.Aux1 != "")
                {
                    oATT.Lines.Add();
                    //ArchivoAux1
                    xRuta11 = path + oListaConfig[0].Descripcion.Replace(" ", "_") + "_" + proveedor.LicTradNum + ".pdf";
                    File.WriteAllBytes(xRuta11, Convert.FromBase64String(proveedor.Aux1));

                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension(oListaConfig[0].Descripcion.Replace(" ", "_") + "_" + proveedor.LicTradNum);
                    oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;
                }

                if (proveedor.Aux2 != null && proveedor.Aux2 != "")
                {
                    oATT.Lines.Add();
                    //ArchivoAux1
                    xRuta12 = path + oListaConfig[1].Descripcion.Replace(" ", "_") + "_" + proveedor.LicTradNum + ".pdf";
                    File.WriteAllBytes(xRuta12, Convert.FromBase64String(proveedor.Aux2));

                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension(oListaConfig[1].Descripcion.Replace(" ", "_") + "_" + proveedor.LicTradNum);
                    oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;
                }

                if (proveedor.Aux3 != null && proveedor.Aux3 != "")
                {
                    oATT.Lines.Add();
                    //ArchivoAux1
                    xRuta13 = path + oListaConfig[2].Descripcion.Replace(" ", "_") + "_" + proveedor.LicTradNum + ".pdf";
                    File.WriteAllBytes(xRuta13, Convert.FromBase64String(proveedor.Aux3));

                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension(oListaConfig[2].Descripcion.Replace(" ", "_") + "_" + proveedor.LicTradNum);
                    oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;
                }

                if (proveedor.Aux4 != null && proveedor.Aux4 != "")
                {
                    oATT.Lines.Add();
                    //ArchivoAux1
                    xRuta14 = path + oListaConfig[3].Descripcion.Replace(" ", "_") + "_" + proveedor.LicTradNum + ".pdf";
                    File.WriteAllBytes(xRuta14, Convert.FromBase64String(proveedor.Aux4));

                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension(oListaConfig[3].Descripcion.Replace(" ", "_") + "_" + proveedor.LicTradNum);
                    oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;
                }

                if (proveedor.Aux5 != null && proveedor.Aux5 != "")
                {
                    oATT.Lines.Add();
                    //ArchivoAux1
                    xRuta15 = path + oListaConfig[4].Descripcion.Replace(" ", "_") + "_" + proveedor.LicTradNum + ".pdf";
                    File.WriteAllBytes(xRuta15, Convert.FromBase64String(proveedor.Aux5));

                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension(oListaConfig[4].Descripcion.Replace(" ", "_") + "_" + proveedor.LicTradNum);
                    oATT.Lines.FileExtension = Path.GetExtension(".pdf").Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;
                }

                if (oATT.Add() == 0)
                {
                    entryAtt = int.Parse(_company.GetNewObjectKey());
                }
                else
                {
                    throw new Exception("Attachments: " + _company.GetLastErrorDescription());
                }
                #endregion
                
                CompanyService oCompanyService;
                GeneralService oGeneralService;
                GeneralData oGeneralData;
                //Dim oGeneralDataParams As SAPbobsCOM.GeneralDataParams
                GeneralData oChild;
                GeneralDataCollection oChildren;
                oCompanyService = _company.GetCompanyService();
                //Get GeneralService (oCompany is the CompanyService)
                oGeneralService = oCompanyService.GetGeneralService("EXX_WP_PROVEEDOR");
                //Create data for new row in main UDO
                oGeneralData = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", code);
                oGeneralData.SetProperty("U_EXX_CARDCODE", proveedor.CardCode);
                oGeneralData.SetProperty("U_EXX_CARDNAME", proveedor.CardName);
                oGeneralData.SetProperty("U_EXX_RUC", proveedor.LicTradNum);
                oGeneralData.SetProperty("U_EXX_TELEFONO", proveedor.Phone1);
                oGeneralData.SetProperty("U_EXX_CONDICIONPAGO", proveedor.FormaPago);
                oGeneralData.SetProperty("U_EXX_TIPODOC", proveedor.U_EXX_TIPODOCU);
                oGeneralData.SetProperty("U_EXX_PASSWORD", proveedor.Password);
                oGeneralData.SetProperty("U_EXX_EMAIL", proveedor.EmailAddress);
                oGeneralData.SetProperty("U_EXX_ARCHIVO1", xRuta1);
                oGeneralData.SetProperty("U_EXX_ARCHIVO2", xRuta2);
                oGeneralData.SetProperty("U_EXX_ARCHIVO3", xRuta3);
                oGeneralData.SetProperty("U_EXX_ARCHIVO4", xRuta4);
                oGeneralData.SetProperty("U_EXX_ARCHIVO5", xRuta5);
                oGeneralData.SetProperty("U_EXX_ARCHIVO6", xRuta6);
                oGeneralData.SetProperty("U_EXX_ARCHIVO7", xRuta7);
                oGeneralData.SetProperty("U_EXX_ARCHIVO8", xRuta8);
                oGeneralData.SetProperty("U_EXX_ARCHIVO9", xRuta9);
                oGeneralData.SetProperty("U_EXX_ARCHIVO10", xRuta10);
                oGeneralData.SetProperty("U_EXX_AUX1", xRuta11);
                oGeneralData.SetProperty("U_EXX_AUX2", xRuta12);
                oGeneralData.SetProperty("U_EXX_AUX3", xRuta13);
                oGeneralData.SetProperty("U_EXX_AUX4", xRuta14);
                oGeneralData.SetProperty("U_EXX_AUX5", xRuta15);
                oGeneralData.SetProperty("U_EXX_IDANEXO", entryAtt);

                oChildren = oGeneralData.Child("EXX_WP_PROV_CONT");                
                //Persona de contacto
                for (int j = 0; j < proveedor.Contactos.Count; j++)
                {
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_EXX_ID", proveedor.Contactos[j].Nombre);
                    oChild.SetProperty("U_EXX_PRIMERNOMBRE", proveedor.Contactos[j].PrimerNombre);
                    oChild.SetProperty("U_EXX_SEGUNDONOMBRE", proveedor.Contactos[j].SegundoNombre);
                    oChild.SetProperty("U_EXX_APELLIDOS", proveedor.Contactos[j].Apellido);
                    oChild.SetProperty("U_EXX_CELULAR", proveedor.Contactos[j].Celular);
                    oChild.SetProperty("U_EXX_TELEFONO", proveedor.Contactos[j].Telefono);
                    oChild.SetProperty("U_EXX_EMAIL", proveedor.Contactos[j].Email);
                    oChild.SetProperty("U_EXX_CARGO", proveedor.Contactos[j].Cargo);
                }

                oChildren = oGeneralData.Child("EXX_WP_PROV_DIR");
                //Direcciones
                for (int i = 0; i < proveedor.Direcciones.Count; i++)
                {
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_EXX_TIPO", proveedor.Direcciones[i].AdressType);
                    oChild.SetProperty("U_EXX_ID", proveedor.Direcciones[i].Address);
                    oChild.SetProperty("U_EXX_DEPARTAMENTO", proveedor.Direcciones[i].Departamento);
                    oChild.SetProperty("U_EXX_PROVINCIA", proveedor.Direcciones[i].Provincia);
                    oChild.SetProperty("U_EXX_DISTRITO", proveedor.Direcciones[i].Distrito);
                    oChild.SetProperty("U_EXX_DIRECCION", proveedor.Direcciones[i].DireccionDesc);
                }
                int contador = proveedor.Direcciones.Select(t => t.AdressType == "B").Count();
                if (contador == 0)
                {
                    throw new Exception("Debe tener como minimo una dirección fiscal");
                }

                oChildren = oGeneralData.Child("EXX_WP_PROV_CTAS");
                //Cuentas Bancarias
                foreach (CuentaBanco ctaBco in proveedor.CuentasBancarias)
                {
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_EXX_BANKCODE", ctaBco.BankCode);
                    oChild.SetProperty("U_EXX_NOMBRECUENTA", ctaBco.AcctName);
                    oChild.SetProperty("U_EXX_NROCUENTA", ctaBco.Account);
                    oChild.SetProperty("U_EXX_MONEDA", ctaBco.Moneda);
                    oChild.SetProperty("U_EXX_TIPO", ctaBco.Tipo);
                    oChild.SetProperty("U_EXX_NROINTERBANCARIO", ctaBco.U_EXM_INTERBANCARIA);
                    oChild.SetProperty("U_EXX_ESDETRACCION", ctaBco.EsDetraccion ? "Y" : "N");
                }

                oGeneralService.Add(oGeneralData);

                xQuery = $"SELECT U_EXX_EMAIL FROM \"@EXX_WP_USUARIO\" ";
                oRs.DoQuery(xQuery);
                List<string> xEmail = new List<string>();

                for(int i = 0; i<oRs.RecordCount; i++)
                {
                    xEmail.Add(oRs.Fields.Item(0).Value); 
                    oRs.MoveNext();
                }

                string xRptaEmail = new EnvioCorreo().fn_EnvioCorreoAprobacionProveedor(xEmail, proveedor.CardName, proveedor.LicTradNum);
                string CorreoP = new EnvioCorreo().fn_EnvioCorreoRegistroProveedor(proveedor.EmailAddress, proveedor.CardName);

                xRpta = "Su solicitud de creación ha sido registrada con éxito, se le confirmará el alta.";
            }
            catch (Exception ex)
            {
                xRpta = ex.Message;
            }
            finally
            {
                _company.Disconnect();
            }
            return xRpta;
        }

        public string fn_Proveedor_Aprobar(SocioDeNegocio proveedor)
        {
            string xRpta = "";
            try
            {
                _company.StartTransaction();
                CompanyService oCompanyService = _company.GetCompanyService();;
                GeneralService oGeneralService = oCompanyService.GetGeneralService("EXX_WP_PROVEEDOR");
                GeneralDataParams oGeneralParams = (GeneralDataParams)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                oGeneralParams.SetProperty("Code", proveedor.Code);
                GeneralData oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oGeneralData.SetProperty("U_EXX_AUTORIZADO", proveedor.Decision);
                oGeneralService.Update(oGeneralData);

                if(proveedor.Decision == "Y")
                {
                    xRpta = fn_Proveedor_Registrar(proveedor);
                }
                else
                {
                    xRpta = new EnvioCorreo().fn_EnvioCorreoRechazoProveedor(proveedor.EmailAddress, proveedor.CardName, proveedor.Comentario);
                    if (xRpta == "")
                        xRpta = "Se rechazó con éxito al proveedor";
                }

                _company.EndTransaction(BoWfTransOpt.wf_Commit);
            }
            catch (Exception ex)
            {
                if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_RollBack);
                xRpta = ex.Message;
            }
            return xRpta;
        }

        private string fn_Proveedor_Registrar(SocioDeNegocio proveedor)
        {
            string xRpta;

            BusinessPartners oProveedor = _company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
            oProveedor.CardCode = proveedor.CardCode;
            oProveedor.FederalTaxID = proveedor.LicTradNum;
            oProveedor.CardName = proveedor.CardName;
            oProveedor.CardType = BoCardTypes.cSupplier;
            oProveedor.Phone1 = proveedor.Phone1;
            oProveedor.Cellular = proveedor.Cellular;
            oProveedor.EmailAddress = proveedor.EmailAddress;
            oProveedor.GroupCode = 102;
            oProveedor.PayTermsGrpCode = Convert.ToInt32(proveedor.FormaPago);
            //oCliente.Currency = cliente.Currency;

            //oCliente.UserFields.Fields.Item("U_EXX_APELLPAT").Value = cliente.ApellidoPaterno;
            //oCliente.UserFields.Fields.Item("U_EXX_APELLMAT").Value = cliente.ApellidoMaterno;
            //oCliente.UserFields.Fields.Item("U_EXX_PRIMERNO").Value = cliente.Nombre;
            //oCliente.UserFields.Fields.Item("U_EXX_SEGUNDNO").Value = cliente.SegundoNombre;
            //oCliente.UserFields.Fields.Item("U_EXX_TIPOPERS").Value = cliente.U_EXX_TIPOPERS;
            oProveedor.UserFields.Fields.Item("U_EXX_TIPODOCU").Value = proveedor.U_EXX_TIPODOCU;
            oProveedor.UserFields.Fields.Item("U_EXX_CONVENIO").Value = "00";
            oProveedor.UserFields.Fields.Item("U_EXX_TIPVINECO").Value = "00";
            oProveedor.UserFields.Fields.Item("U_EXX_WP_FCVG").Value = DateTime.Now.AddYears(1);
            oProveedor.AttachmentEntry = proveedor.Anexo;
            oProveedor.Password = proveedor.Password;

            //Persona de contacto
            for (int j = 0; j < proveedor.Contactos.Count; j++)
            {
                oProveedor.ContactEmployees.Name = proveedor.Contactos[j].Nombre;
                oProveedor.ContactEmployees.FirstName = proveedor.Contactos[j].PrimerNombre;
                oProveedor.ContactEmployees.MiddleName = proveedor.Contactos[j].SegundoNombre;
                oProveedor.ContactEmployees.LastName = proveedor.Contactos[j].Apellido;
                oProveedor.ContactEmployees.MobilePhone = proveedor.Contactos[j].Celular;
                oProveedor.ContactEmployees.Phone1 = proveedor.Contactos[j].Telefono;
                oProveedor.ContactEmployees.E_Mail = proveedor.Contactos[j].Email;
                oProveedor.ContactEmployees.Position = proveedor.Contactos[j].Cargo;

                if (j + 1 < proveedor.Contactos.Count)
                    oProveedor.ContactEmployees.Add();
            }

            int contador = proveedor.Direcciones.Select(t => t.AdressType == "B").Count();
            if (contador == 0)
            {
                throw new Exception("Debe tener como minimo una dirección fiscal");
            }

            //Direcciones
            for (int i = 0; i < proveedor.Direcciones.Count; i++)
            {
                oProveedor.Addresses.AddressType = proveedor.Direcciones[i].AdressType == "B" ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo;
                oProveedor.Addresses.AddressName = proveedor.Direcciones[i].Address;
                oProveedor.Addresses.Country = "PE";
                oProveedor.Addresses.State = proveedor.Direcciones[i].Departamento;
                oProveedor.Addresses.County = proveedor.Direcciones[i].ProvinciaDesc;
                oProveedor.Addresses.ZipCode = proveedor.Direcciones[i].DistritoDesc;
                oProveedor.Addresses.Street = proveedor.Direcciones[i].DireccionDesc;
                //oCliente.Addresses.GlobalLocationNumber = cliente.Direcciones[i].Ubigeo;
                //oCliente.Addresses.UserFields.Fields.Item("U_EXX_TPED_ZONA").Value = cliente.Direcciones[i].U_EXX_TPED_ZONA;

                if (i + 1 < proveedor.Direcciones.Count)
                    oProveedor.Addresses.Add();
            }

            //Cuentas Bancarias
            int ib = 1;
            foreach (CuentaBanco ctaBco in proveedor.CuentasBancarias)
            {
                oProveedor.BPBankAccounts.BankCode = ctaBco.BankCode;
                oProveedor.BPBankAccounts.AccountName = ctaBco.AcctName;
                oProveedor.BPBankAccounts.AccountNo = ctaBco.Account;
                oProveedor.BPBankAccounts.UserNo1 = ctaBco.Moneda;
                oProveedor.BPBankAccounts.UserNo2 = ctaBco.Tipo;
                oProveedor.BPBankAccounts.UserFields.Fields.Item("U_CurrSAP").Value = ctaBco.U_CurrSAP;
                oProveedor.BPBankAccounts.UserFields.Fields.Item("U_EXM_INTERBANCARIA").Value = ctaBco.U_EXM_INTERBANCARIA;
                oProveedor.BPBankAccounts.UserFields.Fields.Item("U_EXD_CUENTADETRAC").Value = ctaBco.EsDetraccion ? "Y" : "N";

                if (ib < proveedor.CuentasBancarias.Count)
                    oProveedor.BPBankAccounts.Add();
            }

            int intrpt = oProveedor.Add();

            if (intrpt < 0)
            {
                throw new Exception("0-" + _company.GetLastErrorDescription().Replace("-", " "));
            }
            else
            {
                xRpta = "1-Se registró con éxito el proveedor";
                string Correo = new EnvioCorreo().fn_EnvioCorreoCreacionProveedor(proveedor.EmailAddress, proveedor.CardName);
            }

            return xRpta;
        }

        public string file_Descargar(string code, string ruc)
        {
            string path = "";
            Recordset rs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string xQuery = "SELECT \"AttachPath\" FROM OADP";
            rs.DoQuery(xQuery);
            path = rs.Fields.Item(0).Value;

            List<Maestro> oListaConfig = new MaestroDIAPI().getConfiguracion();

            string xRuta = "";
            switch (code)
            {
                case "1":
                    xRuta = path + "DeclaracionJuradaAntiCorrupcion_" + ruc + ".pdf";
                    break;
                case "2":
                    xRuta = path + "CodigoEticaConducta_" + ruc + ".pdf";
                    break;
                case "3":
                    xRuta = path + "DeclaracionJuradaConfidecialidad_" + ruc + ".pdf";
                    break;
                case "4":
                    xRuta = path + "DeclaracionJuradaCononocimiento_" + ruc + ".pdf";
                    break;
                case "5":
                    xRuta = path + "FichaRUC_" + ruc + ".pdf";
                    break;
                case "6":
                    xRuta = path + "ReporteTributarioSunat_" + ruc + ".pdf";
                    break;
                case "7":
                    xRuta = path + "ReportePlataformaALFT_" + ruc + ".pdf";
                    break;
                case "8":
                    xRuta = path + "ReporteCentralRiesgo_" + ruc + ".pdf";
                    break;
                case "9":
                    xRuta = path + "ReporteR03_" + ruc + ".pdf";
                    break;
                case "10":
                    xRuta = path + "RepresentanteLegalDni_" + ruc + ".pdf";
                    break;
                case "11":
                    xRuta = path + oListaConfig[0].Descripcion.Replace(" ", "") + "_" + ruc + ".pdf";
                    break;
                case "12":
                    xRuta = path + oListaConfig[1].Descripcion.Replace(" ", "") + "_" + ruc + ".pdf";
                    break;
                case "13":
                    xRuta = path + oListaConfig[2].Descripcion.Replace(" ", "") + "_" + ruc + ".pdf";
                    break;
                case "14":
                    xRuta = path + oListaConfig[3].Descripcion.Replace(" ", "") + "_" + ruc + ".pdf";
                    break;
                case "15":
                    xRuta = path + oListaConfig[4].Descripcion.Replace(" ", "") + "_" + ruc + ".pdf";
                    break;
            }

            Byte[] fileBytes = File.ReadAllBytes(xRuta);
            var content = Convert.ToBase64String(fileBytes);
            return content;
        }

        public string formatoDescargar(string name)
        {
            string xRuta = Startup.StaConfig["DIAPI:RutaFormatoProv"];
            string xRutaFile = xRuta + name;

            Byte[] fileBytesAux = File.ReadAllBytes(xRutaFile);

            var content = Convert.ToBase64String(fileBytesAux);

            return content;
        }
    }
}
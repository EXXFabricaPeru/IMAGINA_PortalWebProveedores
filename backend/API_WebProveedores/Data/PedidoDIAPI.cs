using SAPbobsCOM;
using WebProov_API.Data.Interfaces;
using WebProov_API.Models;
using System;
using System.Collections.Generic;
using WebProov_API.Util;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Data;
using WebProov_API.Dtos;

namespace WebProov_API.Data
{
    public class PedidoDIAPI : IPedidoRepository
    {
        private Company _company;
        Documents oPor;
        public PedidoDIAPI()
        {
            _company = DIAPIConexion.GetDIAPIConexion();
        }

        #region Orden Compra
        public Documento GetDocumentoById(int id)
        {
            try
            {
                Documento pedido = new Documento();

                oPor = _company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
                if (!oPor.GetByKey(id))
                    return null;

                pedido.DocEntry = oPor.DocEntry;
                pedido.DocNum = oPor.DocNum.ToString();
                pedido.CardCode = oPor.CardCode;
                pedido.CardName = oPor.CardName;
                pedido.LicTradNum = oPor.FederalTaxID;
                pedido.NumAtCard = oPor.NumAtCard;
                pedido.DocDate = oPor.DocDate;
                pedido.TaxDate = oPor.TaxDate;
                pedido.DocDueDate = oPor.DocDueDate;
                pedido.GroupNum = oPor.GroupNumber;
                string estado = "";
                switch (oPor.DocumentStatus)
                {
                    case BoStatus.bost_Open:
                        estado = "Abierto";
                        break;
                    case BoStatus.bost_Close:
                        if (oPor.Cancelled == BoYesNoEnum.tYES)
                            estado = "Anulado";
                        else
                            estado = "Cerrado";
                        break;
                    case BoStatus.bost_Paid:
                        estado = "Pagado";
                        break;
                    case BoStatus.bost_Delivered:
                        estado = "Entregado";
                        break;
                    default:
                        break;
                }
                pedido.DocStatus = estado;
                pedido.ListaPrecio = "";//??
                pedido.DireccionDespacho = oPor.Address2;
                pedido.DireccionFiscal = oPor.Address;
                pedido.CondicionPago = GetDescCondicionPago(oPor.PaymentGroupCode);//TODO
                pedido.SalesPersonCode = GetSLPName(oPor.SalesPersonCode.ToString());
                pedido.DiscountPercent = oPor.DiscountPercent;
                pedido.DiscSum = oPor.TotalDiscount;
                pedido.VatSum = oPor.VatSum;
                pedido.VatSumFC = oPor.VatSumFc;
                pedido.DocTotal = oPor.DocTotal;
                pedido.DocTotalFC = oPor.DocTotalFc;
                pedido.DocRate = oPor.DocRate;
                pedido.Comments = oPor.Comments;
                pedido.DocCur = oPor.DocCurrency;
                pedido.TipoDocumento = oPor.DocType.ToString() == "dDocument_Items" ? "I" : "S";
                pedido.Sucursal = oPor.BPLName;
                //pedido.NumeroCotizacion = coti

                pedido.Archivo = oPor.UserFields.Fields.Item("U_EXX_OC_PDF").Value.ToString();
                pedido.FondoGrantia = oPor.UserFields.Fields.Item("U_EXC_FONGAR").Value.ToString();
                pedido.PorFondoGar = oPor.UserFields.Fields.Item("U_EXC_PORFGR").Value;
                pedido.ImpAnticio = oPor.UserFields.Fields.Item("U_EXC_IMPANT").Value;

                DateTime fecha = DateTime.Parse("1899-12-30");

                pedido.U_EXC_FVCAFI = oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value;
                pedido.U_EXC_INICON = oPor.UserFields.Fields.Item("U_EXC_INICON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_INICON").Value;
                pedido.U_EXC_FINCON = oPor.UserFields.Fields.Item("U_EXC_FINCON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FINCON").Value;

                //obtenemos el contacto
                string xQuery = $"SELECT \"lastName\" || ' ' || \"firstName\" FROM OHEM WHERE \"empID\" = {oPor.UserFields.Fields.Item("U_EXC_USRCON").Value} AND COALESCE(\"email\", '') != ''";
                Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRs.DoQuery(xQuery);
                pedido.Contacto = oRs.Fields.Item(0).Value;

                //Obtnemos el comprador
                xQuery = $"SELECT \"SlpName\"  FROM OSLP WHERE \"SlpCode\" = {oPor.SalesPersonCode} AND COALESCE(\"Email\", '') != ''";
                oRs.DoQuery(xQuery);
                pedido.Comprador = oRs.Fields.Item(0).Value;

                double xCantidad = 0;
                double xCantidadPend = 0;
                List<DetallePedido> detalle = new List<DetallePedido>();

                string oQuery = $"SELECT T1.\"ItemCode\", \"Dscription\", \"Price\", \"Quantity\", \"OpenQty\", \"VisOrder\", \"TaxCode\", \"WhsCode\", " +
                                $"\"LineTotal\", \"UomCode\", \"PQTReqDate\", \"InvntItem\" " +
                                $"FROM POR1 T1 " +
                                $"INNER JOIN OITM T2 ON T2.\"ItemCode\" = T1.\"ItemCode\" " +
                                $"WHERE T1.\"DocEntry\"={id}";
                oRs.DoQuery(oQuery);

                int contador = 0;
                for (int i = 0; i < oRs.RecordCount; i++)
                {
                    DetallePedido det = new DetallePedido();

                    det.Dscription = oRs.Fields.Item("Dscription").Value;
                    det.DiscountPercent = oPor.Lines.DiscountPercent;
                    det.Price = oRs.Fields.Item("Price").Value;
                    det.TaxCode = oRs.Fields.Item("TaxCode").Value;
                    //det.PriceAfVAT = oPor.Lines.PriceAfterVAT;
                    det.LineNum = oRs.Fields.Item("VisOrder").Value;// oPor.Lines.LineNum;
                    det.ItemCode = oRs.Fields.Item("ItemCode").Value;
                    det.Cantidad = oRs.Fields.Item("OpenQty").Value;// oPor.Lines.Quantity;
                    det.PendQuantity = oRs.Fields.Item("OpenQty").Value;
                    det.UnitMsr = oRs.Fields.Item("UomCode").Value;
                    det.WhsCode = oRs.Fields.Item("WhsCode").Value;
                    det.LineTotal = oRs.Fields.Item("LineTotal").Value;
                    det.ShipDate = oRs.Fields.Item("PQTReqDate").Value;
                    //det.Stock = 0;
                    detalle.Add(det);

                    xCantidad += oRs.Fields.Item("Quantity").Value;
                    xCantidadPend += det.PendQuantity;

                    if (oRs.Fields.Item("InvntItem").Value == "N")
                        contador++;

                    oRs.MoveNext();
                }

                //pedido.FlagAnticipo = ((xCantidad == xCantidadPend) && (pedido.ImpAnticio > 0)) ? true : false;
                pedido.FlagAnticipo = ((xCantidad - xCantidadPend < xCantidad) && (pedido.ImpAnticio > 0)) ? true : false;
                pedido.FlagConformidad = contador == oRs.RecordCount ? true : false;
                pedido.DetallePedido = detalle;

                return pedido;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Documento> GetListaByRuc(string ruc, string fecIni, string fecFin, string estado)
        {
            try
            {
                Documento ped = new Documento();
                List<Documento> listPed = new List<Documento>();
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                string oQuery = Queries.GetPedidosByRUC(ruc, fecIni, fecFin, estado);
                oRS.DoQuery(oQuery);
                if (oRS.RecordCount == 0)
                    return listPed;
                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    ped = new Documento();
                    ped.DocEntry = oRS.Fields.Item("DocEntry").Value;
                    ped.DocNum = oRS.Fields.Item("DocNum").Value.ToString().Trim();
                    ped.NumAtCard = oRS.Fields.Item("NumAtCard").Value.ToString().Trim();
                    ped.DocDate = DateTime.Parse(oRS.Fields.Item("DocDate").Value.ToString().Trim());
                    ped.DocDueDate = DateTime.Parse(oRS.Fields.Item("DocDueDate").Value.ToString().Trim());
                    ped.SalesPersonCode = oRS.Fields.Item("SlpName").Value.ToString().Trim();
                    ped.DocCur = oRS.Fields.Item("DocCur").Value.ToString().Trim();
                    ped.DocTotal = oRS.Fields.Item("DocTotal").Value;
                    ped.DocTotalFC = oRS.Fields.Item("DocTotalFC").Value;
                    ped.Comments = oRS.Fields.Item("Comments").Value.ToString().Trim();
                    ped.DocStatus = oRS.Fields.Item("Estado").Value.ToString().Trim();
                    ped.CondicionPago = oRS.Fields.Item("PymntGroup").Value.ToString().Trim();
                    ped.Sucursal = oRS.Fields.Item("BPLName").Value;
                    oRS.MoveNext();
                    listPed.Add(ped);
                }
                return listPed;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public string descargarLista(string ruc, string fecIni, string fecFin, string estado)
        {
            string xRpta = "";
            try
            {
                DocumentoXls ped = new DocumentoXls();
                List<DocumentoXls> listPed = new List<DocumentoXls>();
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.GetPedidosByRUC(ruc, fecIni, fecFin, estado));
                if (oRS.RecordCount == 0)
                    return "";

                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    ped = new DocumentoXls();
                    ped.Nro = oRS.Fields.Item("DocNum").Value.ToString().Trim();
                    ped.NumAtCard = oRS.Fields.Item("NumAtCard").Value.ToString().Trim();
                    ped.FecDoc = DateTime.Parse(oRS.Fields.Item("DocDate").Value.ToString().Trim());
                    ped.DocDueDate = DateTime.Parse(oRS.Fields.Item("DocDueDate").Value.ToString().Trim());
                    ped.Moneda = oRS.Fields.Item("DocCur").Value.ToString().Trim();
                    ped.DocTotal = oRS.Fields.Item("DocTotal").Value;
                    ped.Comments = oRS.Fields.Item("Comments").Value.ToString().Trim();
                    ped.Estado = oRS.Fields.Item("Estado").Value.ToString().Trim();
                    ped.CondicionPago = oRS.Fields.Item("PymntGroup").Value.ToString().Trim();
                    ped.Sucursal = oRS.Fields.Item("BPLName").Value;
                    oRS.MoveNext();
                    listPed.Add(ped);
                }

                string xQuery = "SELECT \"AttachPath\" FROM OADP";
                oRS.DoQuery(xQuery);
                string xRuta = oRS.Fields.Item(0).Value + "pedido_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                xRpta = Common.GenerateExcel(ConvertToDataTable(listPed), xRuta);
            }
            catch (Exception ex)
            {
                xRpta = "ERROR! " + ex.ToString();
            }

            return xRpta;
        }
        #endregion

        #region Conformidad Servicio
        public string ConfirmarOC(Documento pedido)
        {
            string xRespuesta = "";
            string xQuery;
            Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
            if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_RollBack);
            _company.StartTransaction();
            try
            {
                Documents oEntrada = _company.GetBusinessObject(BoObjectTypes.oDrafts);
                Documents oOC;

                int xDocEntry = pedido.DocEntry;
                int result = -1;
                int entryAtt = -1;

                oOC = _company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);        //-->Orden de Compra

                oOC.GetByKey(xDocEntry);

                string path = "";
                if (pedido.Archivo != null && pedido.Archivo != string.Empty)
                {
                    Recordset rs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                    xQuery = "SELECT \"AttachPath\" FROM OADP";
                    rs.DoQuery(xQuery);
                    string xRuta = "";
                    if (rs.RecordCount > 0)
                    {
                        xRuta = rs.Fields.Item(0).Value;
                    }
                    path = xRuta + pedido.NomArchivo;
                    File.WriteAllBytes(path, Convert.FromBase64String(pedido.Archivo));

                    Attachments2 oATT = _company.GetBusinessObject(BoObjectTypes.oAttachments2);
                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension(pedido.NomArchivo);
                    oATT.Lines.FileExtension = Path.GetExtension(pedido.NomArchivo).Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;

                    if (pedido.Archivo2 != null && pedido.Archivo2 != string.Empty)
                    {
                        oATT.Lines.Add();

                        path = xRuta + pedido.NomArchivo2;
                        File.WriteAllBytes(path, Convert.FromBase64String(pedido.Archivo2));

                        oATT.Lines.FileName = Path.GetFileNameWithoutExtension(pedido.NomArchivo2);
                        oATT.Lines.FileExtension = Path.GetExtension(pedido.NomArchivo2).Replace(".", "");
                        oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                        oATT.Lines.Override = BoYesNoEnum.tYES;
                    }

                    if (pedido.Archivo3 != null && pedido.Archivo3 != string.Empty)
                    {
                        oATT.Lines.Add();

                        path = xRuta + pedido.NomArchivo3;
                        File.WriteAllBytes(path, Convert.FromBase64String(pedido.Archivo3));

                        oATT.Lines.FileName = Path.GetFileNameWithoutExtension(pedido.NomArchivo3);
                        oATT.Lines.FileExtension = Path.GetExtension(pedido.NomArchivo3).Replace(".", "");
                        oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                        oATT.Lines.Override = BoYesNoEnum.tYES;
                    }

                    int res = oATT.Add();

                    if (res == 0)
                    {
                        entryAtt = int.Parse(_company.GetNewObjectKey());
                    }
                    else
                    {
                        int lErrCode = 0;
                        string sErrMsg = string.Empty;
                        _company.GetLastError(out lErrCode, out sErrMsg);
                        throw new Exception("Attachments: " + lErrCode + " " + sErrMsg);
                    }
                }

                string xCorreoTitular = "";
                string xCorreoLogistica = "";

                //obtenemos correo del titular
                xQuery = $"SELECT \"email\"  FROM OHEM WHERE \"empID\" = {oOC.UserFields.Fields.Item("U_EXC_USRCON").Value} AND COALESCE(\"email\", '') != ''";
                oRs.DoQuery(xQuery);
                if (oRs.RecordCount != 0)
                {
                    xCorreoTitular = oRs.Fields.Item(0).Value;
                }

                //obtenemos el correo de logistica
                if (oOC.SalesPersonCode != -1)
                {
                    xQuery = $"SELECT \"Email\"  FROM OSLP WHERE \"SlpCode\" = {oOC.SalesPersonCode} AND COALESCE(\"Email\", '') != ''";
                    oRs.DoQuery(xQuery);
                    if (oRs.RecordCount != 0)
                    {
                        xCorreoLogistica = oRs.Fields.Item(0).Value;
                    }
                }

                oEntrada.DocObjectCode = BoObjectTypes.oPurchaseDeliveryNotes;
                oEntrada.CardCode = oOC.CardCode;
                oEntrada.CardName = oOC.CardName;
                oEntrada.NumAtCard = oOC.DocNum.ToString();
                oEntrada.FolioNumber = oOC.DocNum;
                oEntrada.DocDate = DateTime.Now;
                oEntrada.DocDueDate = DateTime.Now;
                oEntrada.TaxDate = DateTime.Now;
                oEntrada.DocType = oOC.DocType;
                oEntrada.Comments = pedido.Comments;
                oEntrada.SalesPersonCode = oOC.SalesPersonCode;
                oEntrada.BPL_IDAssignedToInvoice = oOC.BPL_IDAssignedToInvoice;

                if (pedido.Archivo != "")
                {
                    oEntrada.AttachmentEntry = entryAtt;
                }

                oEntrada.UserFields.Fields.Item("U_EXX_TIPOOPER").Value = "02";
                oEntrada.UserFields.Fields.Item("U_EXC_USRCON").Value = oOC.UserFields.Fields.Item("U_EXC_USRCON").Value;
                oEntrada.UserFields.Fields.Item("U_EXC_FONGAR").Value = oOC.UserFields.Fields.Item("U_EXC_FONGAR").Value;
                oEntrada.UserFields.Fields.Item("U_EXC_PORFGR").Value = oOC.UserFields.Fields.Item("U_EXC_PORFGR").Value;
                oEntrada.UserFields.Fields.Item("U_EXC_FINCON").Value = oOC.UserFields.Fields.Item("U_EXC_FINCON").Value;
                oEntrada.UserFields.Fields.Item("U_EXC_IMPANT").Value = oOC.UserFields.Fields.Item("U_EXC_IMPANT").Value;
                oEntrada.UserFields.Fields.Item("U_EXC_FVCAFI").Value = oOC.UserFields.Fields.Item("U_EXC_FVCAFI").Value;
                oEntrada.UserFields.Fields.Item("U_EXC_INICON").Value = oOC.UserFields.Fields.Item("U_EXC_INICON").Value;
                oEntrada.UserFields.Fields.Item("U_EXC_FINCON").Value = oOC.UserFields.Fields.Item("U_EXC_FINCON").Value;
                oEntrada.UserFields.Fields.Item("U_EXX_WP_REGUSER").Value = pedido.UserReg;

                int contador = 0;
                int i = 0;
                foreach (DetallePedido item in pedido.DetallePedido)
                {
                    int xNroLineas = oOC.Lines.Count;
                    oOC.Lines.SetCurrentLine(item.LineNum);

                    oEntrada.Lines.BaseLine = oOC.Lines.LineNum;
                    oEntrada.Lines.BaseEntry = oOC.Lines.DocEntry;
                    oEntrada.Lines.BaseType = (int)oOC.DocObjectCode;
                    oEntrada.Lines.ItemCode = oOC.Lines.ItemCode;
                    oEntrada.Lines.FreeText = oOC.Lines.FreeText;
                    oEntrada.Lines.Quantity = Convert.ToDouble(item.Cantidad);
                    oEntrada.Lines.WarehouseCode = oOC.Lines.WarehouseCode;
                    oEntrada.Lines.UnitPrice = oOC.Lines.UnitPrice;
                    oEntrada.Lines.ItemDescription = oOC.Lines.ItemDescription;
                    oEntrada.Lines.TaxCode = oOC.Lines.TaxCode;
                    oEntrada.Lines.ProjectCode = oOC.Lines.ProjectCode;
                    oEntrada.Lines.TaxLiable = oOC.Lines.TaxLiable;
                    oEntrada.Lines.MeasureUnit = oOC.Lines.MeasureUnit;
                    oEntrada.Lines.CostingCode2 = oOC.Lines.CostingCode2;
                    oEntrada.Lines.CostingCode = oOC.Lines.CostingCode;

                    oRs.DoQuery(Queries.GetTypeItem(oOC.Lines.ItemCode));

                    if(oRs.Fields.Item(0).Value == "Y")
                        contador++;

                    if (i + 1 < pedido.DetallePedido.Count)
                        oEntrada.Lines.Add();
                    i++;
                }

                if (contador > 0)
                {
                    oEntrada.Indicator = "09";
                    oEntrada.FolioPrefixString = "GR";
                }
                else
                {
                    oEntrada.Indicator = "CS";
                    oEntrada.FolioPrefixString = "CS";
                }

                result = oEntrada.Add();

                _company.GetLastError(out result, out xRespuesta);

                if (result > -1)
                {
                    string xNro = "";
                    _company.GetNewObjectCode(out xNro);
                    Documents oEntMerc = _company.GetBusinessObject(BoObjectTypes.oDrafts);
                    oEntMerc.GetByKey(Convert.ToInt32(xNro));

                    //Flujo de aprobación de la conformidad de servicio
                    CompanyService oCompanyService = _company.GetCompanyService();
                    // Get GeneralService (oCmpSrv is the CompanyService)
                    GeneralService oGeneralService = oCompanyService.GetGeneralService("EXX_WP_ACS_APCOSRV");
                    // Create data for new row in main UDO
                    GeneralData oGeneralData = ((GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData));
                    oGeneralData.SetProperty("U_EXX_NROCONFORMIDAD", xNro);
                    oGeneralData.SetProperty("U_EXX_FECSOLICITUD", DateTime.Now);

                    //Se llena el detalle
                    GeneralDataCollection oChildren;
                    GeneralData oChild;

                    oChildren = oGeneralData.Child("EXX_WP_ACSD_APCOSRV");
                    oChild = oChildren.Add();

                    oChild.SetProperty("U_EXX_USUARIO", xCorreoTitular);
                    oChild.SetProperty("U_EXX_APROBADO", "P");

                    if (xCorreoLogistica != string.Empty && contador == 0)
                    {
                        oChild = oChildren.Add();
                        oChild.SetProperty("U_EXX_USUARIO", xCorreoLogistica);
                        oChild.SetProperty("U_EXX_APROBADO", "P");
                    }

                    // Add the new row, including children, to database
                    GeneralDataParams oGeneralParams = oGeneralService.Add(oGeneralData);

                    xRespuesta = "La conformidad de servicio se registro con éxito con número " + oEntMerc.DocNum + "-" + oEntMerc.DocEntry.ToString();

                    //Envio de correo al titular de aprobacion
                    string envio = new EnvioCorreo().fn_EnvioCorreo(xCorreoTitular, oEntMerc.DocEntry.ToString(), "CS", oEntMerc.BPLName, oEntMerc.CardName);
                    if (envio != string.Empty)
                    {
                        if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_RollBack);
                        throw new Exception("Error Enviar correo - " + envio);
                    }

                    if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_Commit);
                }
                else
                {
                    if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_RollBack);
                    throw new Exception("Error- " + xRespuesta);
                }
            }
            catch (System.Exception ex)
            {
                xRespuesta = ex.Message;
            }

            return xRespuesta;
        }

        public string AprobarConformidadServicio(string nroConformidad, string user, string estado, string commentario, string contrasena)
        {
            string xRpta = "";
            string query;
            Recordset ors = _company.GetBusinessObject(BoObjectTypes.BoRecordset);

            try
            {
                ors.DoQuery(Queries.ValidarUserTra(user, contrasena));
                if(ors.RecordCount == 0)
                    return xRpta = "La contraseña es incorrecta";

                query = $"SELECT \"DocEntry\" FROM \"@EXX_WP_ACSC_APCOSRV\" WHERE U_EXX_NROCONFORMIDAD='{nroConformidad}' ";
                ors.DoQuery(query);
                int code = ors.Fields.Item(0).Value;

                query = $"SELECT COUNT(*) FROM \"@EXX_WP_ACSD_APCOSRV\" WHERE \"DocEntry\" = {code} AND U_EXX_USUARIO = '{user}' ";
                ors.DoQuery(query);

                if (ors.Fields.Item(0).Value == 0)
                    return xRpta = "El usuario no está en este proceso de aprobación.";

                query = $"SELECT COUNT(*) FROM \"@EXX_WP_ACSD_APCOSRV\" WHERE \"DocEntry\" = {code} AND U_EXX_APROBADO='Y' AND U_EXX_USUARIO = '{user}' ";
                ors.DoQuery(query);

                if (ors.Fields.Item(0).Value > 0)
                    return xRpta = "No puedes aprobar más de una vez una solicitud de conformidad de servicio";
                
                ors.DoQuery(Queries.AprobacionConformidadServicio(code, user, estado, commentario));

                xRpta = "¡Se Aprobó con éxito!";

                if (estado == "Y")
                {
                    query = $"SELECT U_EXX_USUARIO FROM \"@EXX_WP_ACSD_APCOSRV\" WHERE \"DocEntry\" = {code} AND U_EXX_APROBADO='P' ";
                    ors.DoQuery(query);
                    if (ors.RecordCount > 0)
                    {
                        Documents oEntrega = _company.GetBusinessObject(BoObjectTypes.oDrafts);
                        oEntrega.GetByKey(Convert.ToInt32(nroConformidad));

                        string xRptCorreo = new EnvioCorreo().fn_EnvioCorreo(ors.Fields.Item(0).Value, nroConformidad, "CS", oEntrega.BPLName, oEntrega.CardName);
                        if (xRptCorreo != "")
                            throw new Exception(xRptCorreo);
                    }
                    else//Se crea el documento
                    {
                        Documents oEntrega = _company.GetBusinessObject(BoObjectTypes.oDrafts);
                        oEntrega.GetByKey(Convert.ToInt32(nroConformidad));

                        oEntrega.TaxDate = DateTime.Now;

                        if (oEntrega.Update() != 0)
                        {
                            return xRpta = _company.GetLastErrorDescription();
                        }

                        if (oEntrega.SaveDraftToDocument() != 0)
                        {
                            return xRpta = _company.GetLastErrorDescription();
                        }

                        string entry = _company.GetNewObjectKey();

                        query = $"UPDATE \"@EXX_WP_ACSC_APCOSRV\" SET U_EXX_ENTRYCONFORMIDAD = {entry} WHERE \"DocEntry\" = {code} ";
                        ors.DoQuery(query);

                        Documents oEntregaNew = _company.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);
                        oEntregaNew.GetByKey(Convert.ToInt32(entry));

                        oEntregaNew.Lines.SetCurrentLine(0);
                        int ocEntry = oEntregaNew.Lines.BaseEntry;

                        query = $"SELECT DISTINCT T2.\"DocEntry\" " +
                                $"FROM POR1 T0 " +
                                $"INNER JOIN PDN1 T1 ON T0.\"DocEntry\" = T1.\"BaseEntry\" " +
                                                  $"AND T0.\"LineNum\" = T1.\"BaseLine\" " +
                                                  $"AND T0.\"ObjType\" = T1.\"BaseType\" " +
                                $"INNER JOIN OPDN T2 ON T2.\"DocEntry\" = T1.\"DocEntry\" " +
                                $"WHERE T0.\"DocEntry\" = {ocEntry} " +
                                  $"AND T2.\"CANCELED\" != 'Y'";

                        query = $"SELECT COUNT(*) FROM ({query}) t";
                        ors.DoQuery(query);
                        oEntregaNew.NumAtCard = "VAL" + ors.Fields.Item(0).Value.ToString() + "-" + oEntregaNew.NumAtCard;
                        oEntregaNew.JournalMemo = oEntregaNew.Comments + "-" + oEntrega.NumAtCard;
                        oEntregaNew.Update();

                        //Envio de correo de aprovacion al proveedor
                        query = $"SELECT \"E_Mail\" FROM OCRD WHERE \"CardCode\" = '{oEntregaNew.CardCode}'";
                        ors.DoQuery(query);

                        string correoProveedor = ors.Fields.Item(0).Value;
                        xRpta += Environment.NewLine + new EnvioCorreo().fn_EnvioCorreoAcptacionCS(correoProveedor, oEntrega.DocNum.ToString() + "-" + oEntrega.DocEntry.ToString(), oEntregaNew.DocNum.ToString());
                    }
                }
                else //Enviar correo de rechazo
                {
                    //Se validaria si tiene el correo registrado
                    Documents oGuia = _company.GetBusinessObject(BoObjectTypes.oDrafts);
                    oGuia.GetByKey(Convert.ToInt32(nroConformidad));

                    if (oGuia.Close() != 0) 
                        return xRpta = _company.GetLastErrorDescription();

                    query = $"SELECT \"E_Mail\" FROM OCRD WHERE \"CardCode\" = '{oGuia.CardCode}'";
                    ors.DoQuery(query);

                    string correoProveedor = ors.Fields.Item(0).Value;

                    xRpta += Environment.NewLine + new EnvioCorreo().fn_EnvioCorreoRechazo(correoProveedor, commentario, oGuia.DocNum.ToString() + "-" + oGuia.DocEntry.ToString());

                    //Correo persona contacto
                    query = $"SELECT U_EXX_USUARIO FROM \"@EXX_WP_ACSD_APCOSRV\" WHERE \"DocEntry\" = {code} AND \"LineId\" = 1 ";
                    ors.DoQuery(query);
                    string correoContacto = ors.Fields.Item(0).Value;
                    xRpta += Environment.NewLine + new EnvioCorreo().fn_EnvioCorreoRechazo(correoContacto, commentario, oGuia.DocNum.ToString() + "-" + oGuia.DocEntry.ToString());
                }
            }
            catch (Exception ex)
            {
                xRpta += Environment.NewLine + ex.Message;
            }

            return xRpta;
        }

        public Documento GetConformidadAprById(int id)
        {
            try
            {
                Documento pedido = new Documento();

                oPor = _company.GetBusinessObject(BoObjectTypes.oDrafts);
                if (!oPor.GetByKey(id))
                    return null;

                pedido.DocEntry = oPor.DocEntry;
                pedido.DocNum = oPor.DocNum.ToString();
                pedido.CardCode = oPor.CardCode;
                pedido.CardName = oPor.CardName;
                pedido.LicTradNum = oPor.FederalTaxID;
                pedido.NumAtCard = oPor.NumAtCard;
                pedido.DocDate = oPor.DocDate;
                pedido.TaxDate = oPor.TaxDate;
                pedido.DocDueDate = oPor.DocDueDate;
                pedido.GroupNum = oPor.GroupNumber;
                string estado = "";
                switch (oPor.DocumentStatus)
                {
                    case BoStatus.bost_Open:
                        estado = "Abierto";
                        break;
                    case BoStatus.bost_Close:
                        estado = "Anulado";
                        break;
                    case BoStatus.bost_Delivered:
                        estado = "Entregado";
                        break;
                    default:
                        break;
                }
                pedido.DocStatus = estado;
                pedido.ListaPrecio = "";//??
                pedido.DireccionDespacho = oPor.Address2;
                pedido.DireccionFiscal = oPor.Address;
                pedido.CondicionPago = GetDescCondicionPago(oPor.PaymentGroupCode);//TODO
                pedido.SalesPersonCode = GetSLPName(oPor.SalesPersonCode.ToString());
                pedido.DiscountPercent = oPor.DiscountPercent;
                pedido.DiscSum = oPor.TotalDiscount;
                pedido.VatSum = oPor.VatSum;
                pedido.VatSumFC = oPor.VatSumFc;
                pedido.DocTotal = oPor.DocTotal;
                pedido.DocTotalFC = oPor.DocTotalFc;
                pedido.DocRate = oPor.DocRate;
                pedido.Comments = oPor.Comments;
                pedido.DocCur = oPor.DocCurrency;
                pedido.Sucursal = oPor.BPLName;
                pedido.IdSucursal = oPor.BPL_IDAssignedToInvoice;

                DateTime fecha = DateTime.Parse("1899-12-30");

                pedido.U_EXC_FVCAFI = oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value;
                pedido.U_EXC_INICON = oPor.UserFields.Fields.Item("U_EXC_INICON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_INICON").Value;
                pedido.U_EXC_FINCON = oPor.UserFields.Fields.Item("U_EXC_FINCON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FINCON").Value;

                //obtenemos el contacto
                string xQuery = $"SELECT \"lastName\" || ' ' || \"firstName\" FROM OHEM WHERE \"empID\" = {oPor.UserFields.Fields.Item("U_EXC_USRCON").Value} AND COALESCE(\"email\", '') != ''";
                Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRs.DoQuery(xQuery);
                pedido.Contacto = oRs.Fields.Item(0).Value;

                //Obtnemos el comprador
                xQuery = $"SELECT \"SlpName\"  FROM OSLP WHERE \"SlpCode\" = {oPor.SalesPersonCode} AND COALESCE(\"Email\", '') != ''";
                oRs.DoQuery(xQuery);
                pedido.Comprador = oRs.Fields.Item(0).Value;

                string query = $"SELECT \"Line\", \"srcPath\", \"FileName\", \"FileExt\" FROM ATC1 WHERE \"AbsEntry\" = {oPor.AttachmentEntry}";
                oRs.DoQuery(query);

                if(oRs.RecordCount > 0)
                {
                    for (int i = 1; i <= oRs.RecordCount; i++)
                    {
                        if (i == 1)
                        {
                            pedido.Archivo = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }
                        if (i == 2)
                        {
                            pedido.Archivo2 = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo2 = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }
                        if (i == 3)
                        {
                            pedido.Archivo3 = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo3 = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }

                        oRs.MoveNext();
                    }
                }

                query = $"SELECT \"DocEntry\" FROM \"@EXX_WP_ACSC_APCOSRV\" WHERE U_EXX_NROCONFORMIDAD='{id}' ";
                oRs.DoQuery(query);
                int code = oRs.Fields.Item(0).Value;

                query = $"SELECT U_EXX_COMENTARIO FROM \"@EXX_WP_ACSD_APCOSRV\" WHERE \"DocEntry\" = {code} ";
                oRs.DoQuery(query);

                string comentario1 = "";
                string comentario2 = "";

                for(int x = 0; x < oRs.RecordCount; x++)
                {
                    if (x == 0)
                        comentario1 = oRs.Fields.Item(0).Value;
                    if (x == 1)
                        comentario2 = oRs.Fields.Item(0).Value;

                    oRs.MoveNext();
                }

                pedido.Comentario1 = comentario1;
                pedido.Comentario2 = comentario2;

                int entryOC = 0;
                List<DetallePedido> detalle = new List<DetallePedido>();
                oRs.DoQuery(Queries.GetDetalleConfoServAprb(id.ToString()));
                for (int i = 0; i < oRs.RecordCount; i++)
                {
                    DetallePedido det = new DetallePedido();
                    det.LineNum = oRs.Fields.Item("LineNum").Value;
                    det.ItemCode = oRs.Fields.Item("ItemCode").Value;
                    det.UnitMsr = oRs.Fields.Item("U.M").Value;
                    det.Quantity = oRs.Fields.Item("Quantity").Value;
                    det.Dscription = oRs.Fields.Item("Dscription").Value;
                    det.Price = oRs.Fields.Item("Price").Value;
                    det.TaxCode = oRs.Fields.Item("TaxCode").Value;
                    det.LineTotal = oRs.Fields.Item("LineTotal").Value;
                    det.Stock = oRs.Fields.Item("Quantity").Value;
                    det.CantInicial = oRs.Fields.Item("Cant.Ini").Value;
                    det.CantAcumulada = oRs.Fields.Item("Cant.Acum").Value;
                    det.CantActual = oRs.Fields.Item("Cant.Acum.Actual").Value;
                    det.CantSaldo = oRs.Fields.Item("Cant.Saldo").Value;
                    det.ImpInicial = oRs.Fields.Item("Imp.Ini").Value;
                    det.ImpAcumulada = oRs.Fields.Item("Imp.Acum").Value;
                    det.ImpActual = oRs.Fields.Item("Imp.Acum.Actual").Value;
                    det.ImpSaldo = oRs.Fields.Item("Imp.Saldo").Value;
                    det.PorInicial = oRs.Fields.Item("Por.Ini").Value;
                    det.PorAnterior = oRs.Fields.Item("Por.Acum.Ant").Value;
                    det.PorActual = oRs.Fields.Item("Por.Actual").Value;
                    det.PorAcumActual = oRs.Fields.Item("Por.Acum.Actual").Value;
                    det.PorSaldo = oRs.Fields.Item("Por.Saldo").Value;
                    detalle.Add(det);

                    entryOC = oPor.Lines.BaseEntry;

                    oRs.MoveNext();
                }               

                pedido.NumeroCotizacion = entryOC.ToString();
                pedido.DetallePedido = detalle;

                return pedido;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Documento GetConformidadById(int id)
        {
            try
            {
                Documento pedido = new Documento();

                oPor = _company.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);
                if (!oPor.GetByKey(id))
                    return null;

                pedido.DocEntry = oPor.DocEntry;
                pedido.DocNum = oPor.DocNum.ToString();
                pedido.CardCode = oPor.CardCode;
                pedido.CardName = oPor.CardName;
                pedido.LicTradNum = oPor.FederalTaxID;
                pedido.NumAtCard = oPor.NumAtCard;
                pedido.DocDate = oPor.DocDate;
                pedido.TaxDate = oPor.TaxDate;
                pedido.DocDueDate = pedido.DocDate.AddDays(30);
                pedido.GroupNum = oPor.GroupNumber;
                string estado = "";
                switch (oPor.DocumentStatus)
                {
                    case BoStatus.bost_Open:
                        estado = "Abierto";
                        break;
                    case BoStatus.bost_Close:
                        if (oPor.Cancelled == BoYesNoEnum.tYES)
                            estado = "Anulado";
                        else
                            estado = "Cerrado";
                        break;
                    case BoStatus.bost_Delivered:
                        estado = "Entregado";
                        break;
                    default:
                        break;
                }
                pedido.DocStatus = estado;
                pedido.DireccionDespacho = oPor.Address2;
                pedido.DireccionFiscal = oPor.Address;
                pedido.CondicionPago = GetDescCondicionPago(oPor.PaymentGroupCode);
                pedido.SalesPersonCode = GetSLPName(oPor.SalesPersonCode.ToString());
                pedido.DiscountPercent = oPor.DiscountPercent;
                pedido.DiscSum = oPor.TotalDiscount;
                pedido.VatSum = oPor.VatSum;
                pedido.VatSumFC = oPor.VatSumFc;
                pedido.DocTotal = oPor.DocTotal;
                pedido.DocTotalFC = oPor.DocTotalFc;
                pedido.DocRate = oPor.DocRate;
                pedido.Comments = oPor.Comments;
                pedido.DocCur = oPor.DocCurrency;
                pedido.Sucursal = oPor.BPLName;
                pedido.IdSucursal = oPor.BPL_IDAssignedToInvoice;
                pedido.ImpAnticio = oPor.UserFields.Fields.Item("U_EXC_IMPANT").Value;

                DateTime fecha = DateTime.Parse("1899-12-30");

                pedido.U_EXC_FVCAFI = oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value;
                pedido.U_EXC_INICON = oPor.UserFields.Fields.Item("U_EXC_INICON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_INICON").Value;
                pedido.U_EXC_FINCON = oPor.UserFields.Fields.Item("U_EXC_FINCON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FINCON").Value;

                //obtenemos el contacto
                string xQuery = $"SELECT \"lastName\" || ' ' || \"firstName\" FROM OHEM WHERE \"empID\" = {oPor.UserFields.Fields.Item("U_EXC_USRCON").Value} AND COALESCE(\"email\", '') != ''";
                Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRs.DoQuery(xQuery);
                pedido.Contacto = oRs.Fields.Item(0).Value;

                //Obtnemos el comprador
                xQuery = $"SELECT \"SlpName\"  FROM OSLP WHERE \"SlpCode\" = {oPor.SalesPersonCode} AND COALESCE(\"Email\", '') != ''";
                oRs.DoQuery(xQuery);
                pedido.Comprador = oRs.Fields.Item(0).Value;

                //fondo de garantia y numOC
                xQuery = $"SELECT T0.\"U_EXC_FONGAR\", T0.\"U_EXC_PORFGR\", T0.\"DocNum\"" +
                         $"FROM OPOR T0 " +
                         $"INNER JOIN POR1 T1 ON T0.\"DocEntry\" = T1.\"DocEntry\" " +
                         $"INNER JOIN PDN1 T2 ON T2.\"BaseEntry\" = T1.\"DocEntry\" " +
                                           $"AND T2.\"BaseLine\" = T1.\"LineNum\" " +
                                           $"AND T2.\"BaseType\" = T1.\"ObjType\" " +
                         $"WHERE T2.\"DocEntry\"={id} ";
                oRs.DoQuery(xQuery);
                pedido.FondoGrantia = oRs.Fields.Item("U_EXC_FONGAR").Value == "Y" ? "SI" : "NO";
                pedido.PorFondoGar = oRs.Fields.Item("U_EXC_PORFGR").Value;
                pedido.NumeroCotizacion = oRs.Fields.Item("DocNum").Value.ToString();

                string query = $"SELECT \"Line\", \"trgtPath\", \"FileName\", \"FileExt\" FROM ATC1 WHERE \"AbsEntry\" = {oPor.AttachmentEntry}";
                oRs.DoQuery(query);

                if (oRs.RecordCount > 0)
                {
                    for (int i = 1; i <= oRs.RecordCount; i++)
                    {
                        if (i == 1)
                        {
                            pedido.Archivo = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }
                        if (i == 2)
                        {
                            pedido.Archivo2 = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo2 = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }
                        if (i == 3)
                        {
                            pedido.Archivo3 = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo3 = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }

                        oRs.MoveNext();
                    }
                }

                List<DetallePedido> detalle = new List<DetallePedido>();
                oRs.DoQuery(Queries.GetDetalleConfoServ(id.ToString()));
                for (int i = 0; i < oRs.RecordCount; i++)
                {
                    //oPor.Lines.SetCurrentLine(i);
                    DetallePedido det = new DetallePedido();
                    det.LineNum = oRs.Fields.Item("LineNum").Value;
                    det.ItemCode = oRs.Fields.Item("ItemCode").Value;
                    det.UnitMsr = oRs.Fields.Item("U.M").Value;
                    det.Quantity = oRs.Fields.Item("Quantity").Value;
                    det.Dscription = oRs.Fields.Item("Dscription").Value;
                    det.Price = oRs.Fields.Item("Price").Value;
                    det.TaxCode = oRs.Fields.Item("TaxCode").Value;
                    det.LineTotal = oRs.Fields.Item("LineTotal").Value;
                    det.DocEntry = id;
                    det.PendQuantity = oRs.Fields.Item("OpenQty").Value;
                    det.ShipDate = oRs.Fields.Item("ShipDate").Value;
                    detalle.Add(det);
                    oRs.MoveNext();
                }

                pedido.DetallePedido = detalle;

                return pedido;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Documento> GetConformidadByRucList(string ruc, string fecIni, string fecFin, string estado)
        {
            try
            {
                Documento ped = new Documento();
                List<Documento> listPed = new List<Documento>();
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.GetConformidadByRUC(ruc, fecIni, fecFin, estado));
                if (oRS.RecordCount == 0)
                    return listPed;
                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    ped = new Documento();
                    ped.DocEntry = oRS.Fields.Item("DocEntry").Value;
                    ped.DocNum = oRS.Fields.Item("DocNum").Value.ToString().Trim();
                    ped.NumAtCard = oRS.Fields.Item("NumAtCard").Value.ToString().Trim();
                    ped.DocDate = DateTime.Parse(oRS.Fields.Item("DocDate").Value.ToString().Trim());
                    ped.DocDueDate = DateTime.Parse(oRS.Fields.Item("DocDueDate").Value.ToString().Trim());
                    //ped.SalesPersonCode = oRS.Fields.Item("SlpName").Value.ToString().Trim();
                    ped.DocCur = oRS.Fields.Item("DocCur").Value.ToString().Trim();
                    ped.DocTotal = oRS.Fields.Item("DocTotal").Value;
                    ped.DocTotalFC = oRS.Fields.Item("DocTotalFC").Value;
                    ped.Comments = oRS.Fields.Item("Comments").Value.ToString().Trim();
                    ped.DocStatus = oRS.Fields.Item("Estado").Value.ToString().Trim();
                    ped.NumeroCotizacion = oRS.Fields.Item("NroOC").Value.ToString();
                    ped.Sucursal = oRS.Fields.Item("BPLName").Value;
                    oRS.MoveNext();
                    listPed.Add(ped);
                }
                return listPed;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        
        public List<Documento> GetConformidadDisponible(string ruc, string fecIni, string fecFin, string sucursal)
        {
            try
            {
                Documento ped = new Documento();
                List<Documento> listPed = new List<Documento>();
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.GetConformidadDisponible(ruc, fecIni, fecFin, sucursal));
                
                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    ped = new Documento();
                    ped.DocEntry = oRS.Fields.Item("DocEntry").Value;
                    ped.DocNum = oRS.Fields.Item("DocNum").Value.ToString().Trim();
                    ped.NumAtCard = oRS.Fields.Item("NumAtCard").Value.ToString().Trim();
                    ped.TipoDocumento = oRS.Fields.Item("ObjType").Value.ToString().Trim();
                    ped.DocDate = DateTime.Parse(oRS.Fields.Item("DocDate").Value.ToString().Trim());
                    ped.DocDueDate = DateTime.Parse(oRS.Fields.Item("DocDueDate").Value.ToString().Trim());
                    ped.DocCur = oRS.Fields.Item("DocCur").Value.ToString().Trim();
                    ped.Sucursal = oRS.Fields.Item("BPLName").Value;
                    ped.NumeroCotizacion = oRS.Fields.Item("NumOC").Value.ToString();
                    ped.Item = new DetalleDoc();
                    ped.Item.LineNum = oRS.Fields.Item("LineNum").Value;
                    ped.Item.ItemCode = oRS.Fields.Item("ItemCode").Value;
                    ped.Item.Dscription = oRS.Fields.Item("Dscription").Value;
                    ped.Item.Quantity = oRS.Fields.Item("Quantity").Value;
                    ped.Item.LineTotal = oRS.Fields.Item("LineTotal").Value;
                    ped.Item.TaxCode = oRS.Fields.Item("TaxCode").Value;
                    oRS.MoveNext();
                    listPed.Add(ped);
                }
                return listPed;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public string file_Descargar(string archivo)
        {
            archivo = archivo.Replace("|", "\\");
            Byte[] fileBytes = File.ReadAllBytes(archivo);
            var content = Convert.ToBase64String(fileBytes);
            return content;
        }

        public string descargarListaConformidad(string ruc, string fecIni, string fecFin, string estado)
        {
            try
            {
                DocumentoXls ped = new DocumentoXls();
                List<DocumentoXls> listPed = new List<DocumentoXls>();
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.GetConformidadByRUC(ruc, fecIni, fecFin, estado));
                if (oRS.RecordCount == 0)
                    return "";
                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    ped = new DocumentoXls();
                    ped.Nro = oRS.Fields.Item("DocNum").Value.ToString().Trim();
                    ped.NumAtCard = oRS.Fields.Item("NumAtCard").Value.ToString().Trim();
                    ped.FecDoc = DateTime.Parse(oRS.Fields.Item("DocDate").Value.ToString().Trim());
                    ped.DocDueDate = DateTime.Parse(oRS.Fields.Item("DocDueDate").Value.ToString().Trim());
                    ped.Moneda = oRS.Fields.Item("DocCur").Value.ToString().Trim();
                    ped.DocTotal = oRS.Fields.Item("DocTotal").Value;
                    ped.Comments = oRS.Fields.Item("Comments").Value.ToString().Trim();
                    ped.Estado = oRS.Fields.Item("Estado").Value.ToString().Trim();
                    ped.Sucursal = oRS.Fields.Item("BPLName").Value;
                    oRS.MoveNext();
                    listPed.Add(ped);
                }

                string xQuery = "SELECT \"AttachPath\" FROM OADP";
                oRS.DoQuery(xQuery);
                string xRuta = oRS.Fields.Item(0).Value + "conformidad_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                return Common.GenerateExcel(ConvertToDataTable(listPed), xRuta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region funciones
        public static string GetDescCondicionPago(int code)
        {
            string val = DIAPIConexion.GenericQuery(Queries.GetDescCondicionPago(code));
            return val;
        }

        private string GetSLPName(string code)
        {
            string val = "-";
            if (code != "-1")
                val = DIAPIConexion.GenericQuery(Queries.GetNombreEmpleado(code));

            return val;
        }

        private string GetWHSName(string code)
        {
            string val = code;
            if (code != "-1")
                val = DIAPIConexion.GenericQuery(Queries.GetNombreAlmacen(code));

            return val;
        }

        public static DataTable ConvertToDataTable<T>(List<T> models)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Loop through all the properties            
            // Adding Column to our datatable
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            // Adding Row
            foreach (T item in models)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows  
                    values[i] = Props[i].GetValue(item, null);
                }
                // Finally add value to datatable  
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        #endregion

        #region Comentado
        //public static string GetOPQTDocNumByEntry(string entry)
        //{
        //    string val = DIAPIConexion.GenericQuery(Queries.GetOPQTDocNum(entry));
        //    return val;
        //}

        //private double GetStock(string whsCode, string itemCode)
        //{
        //    double val = 0;

        //        val = double.Parse(DIAPIConexion.GenericQuery(Queries.GetStockXAlmacen(whsCode, itemCode)));

        //    return val;
        //}

        //public bool ActualizarDocumento(Documento ordCompra)
        //{
        //    try
        //    {
        //        oPor = _company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
        //        if (!oPor.GetByKey(ordCompra.DocEntry))
        //            throw new System.Exception("La oferta con id: " + ordCompra.DocEntry + " no existe en la sociedad.");

        //        if (oPor.DocumentStatus != BoStatus.bost_Open)
        //            throw new System.Exception("La oferta con id: " + ordCompra.DocEntry + " se encuentra cerrada.");
        //           OPOR(ordCompra);
        //        int i = 0;
        //        foreach (DetalleOferta det in ordCompra.DetalleOferta)
        //        {
        //            if (oPor.Lines.Count <= det.LineNum)
        //                oPor.Lines.Add();

        //            oPor.Lines.SetCurrentLine(det.LineNum);
        //            if (oPor.Lines.LineStatus == BoStatus.bost_Open)
        //                POR1(det);
        //            i++;
        //        }
        //        if (oPor.Update() != 0)
        //            throw new System.Exception(_company.GetLastErrorDescription());

        //        //if (!string.IsNullOrEmpty(oferta.Project) && oferta.U_SCO_ESTADO == "A")
        //        //    OfertaToOrder(oferta);

        //        return true;
        //    }
        //    catch (System.Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(oPor);
        //    }
        //}

        //public int CrearDocument(Documento oferta)
        //{
        //    try
        //    {
        //        oPor = _company.GetBusinessObject(BoObjectTypes.oQuotations);
        //        OPOR(oferta);
        //        foreach (DetalleOferta det in oferta.DetalleOferta)
        //        {
        //            POR1(det);
        //            oPor.Lines.Add();
        //        }
        //        if (oPor.Add() != 0)
        //            throw new System.Exception(_company.GetLastErrorDescription());

        //        string offerKey = "";
        //        _company.GetNewObjectCode(out offerKey);
        //        oferta.DocEntry = int.Parse(offerKey);

        //        //if (!string.IsNullOrEmpty(oferta.Project) && oferta.U_SCO_ESTADO == "A")
        //        //{
        //        //    oQuot.GetByKey(oferta.DocEntry);
        //        //    OfertaToOrder(oferta);
        //        //}

        //        return oferta.DocEntry;
        //    }
        //    catch (System.Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(oPor);

        //    }
        //}

        //private void OPOR(Documento oferta)
        //{
        //    oPor.CardCode = oferta.CardCode;
        //    oPor.DocDate = (DateTime)oferta.DocDate;
        //    oPor.DocDueDate = (DateTime)oferta.DocDueDate;
        //    oPor.GroupNumber = oferta.GroupNum;
        //    oPor.Project = oferta.Project;
        //    oPor.DiscountPercent = oferta.DiscountPercent;
        //    oPor.DocTotal = oferta.DocTotal;
        //    oPor.DocCurrency = oferta.DocCur;
        //    oPor.Comments = oferta.Comments;

        //}
        //private void POR1(DetalleOferta detOfer)
        //{
        //    oPor.Lines.ItemCode = detOfer.ItemCode;

        //    oPor.Lines.WarehouseCode = detOfer.WhsCode;
        //    oPor.Lines.Quantity = detOfer.Quantity;
        //    oPor.Lines.TaxCode = detOfer.TaxCode;
        //    oPor.Lines.DiscountPercent = detOfer.DiscountPercent;
        //    //oQuot.Lines.Price = detOfer.Price;
        //    //oQuot.Lines.PriceAfterVAT = detOfer.PriceAfVAT;
        //    oPor.Lines.ProjectCode = detOfer.Project;
        //    oPor.Lines.LineTotal = detOfer.LineTotal;
        //    oPor.Lines.CostingCode = detOfer.OcrCode;
        //    oPor.Lines.CostingCode2 = detOfer.OcrCode2;
        //    oPor.Lines.CostingCode3 = detOfer.OcrCode3;
        //    oPor.Lines.CostingCode4 = detOfer.OcrCode4;
        //    oPor.Lines.CostingCode5 = detOfer.OcrCode5;
        //    if (!string.IsNullOrEmpty(detOfer.U_EXX_GRUPODET)) oPor.Lines.UserFields.Fields.Item("U_EXX_GRUPODET").Value = detOfer.U_EXX_GRUPODET;

        //}
        #endregion

    }
}
using SAPbobsCOM;
using WebProov_API.Data.Interfaces;
using WebProov_API.Models;
using System;
using System.Collections.Generic;
using WebProov_API.Util;
using System.IO;
using System.Xml;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Data;
using WebProov_API.Dtos;

namespace WebProov_API.Data
{
    public class FacturaDIAPI : IFacturaRepository
    {
        private Company _company;
        Documents oPor, oOrd;
        public FacturaDIAPI()
        {
            _company = DIAPIConexion.GetDIAPIConexion();
        }


        public List<Documento> GetListaFechaPagoByRuc(string ruc, string fi, string ff, string estado)
        {
            try
            {
                Documento oFact = new Documento();
                List<Documento> listFact = new List<Documento>();
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.GetFacturasOrdeByDateByRUC(ruc, fi, ff, estado));
                if (oRS.RecordCount == 0)
                    return listFact;
                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    oFact = new Documento();
                    oFact.DocEntry = oRS.Fields.Item("DocEntry").Value;
                    oFact.DocNum = oRS.Fields.Item("DocNum").Value.ToString().Trim();
                    oFact.DocDate = DateTime.Parse(oRS.Fields.Item("DocDate").Value.ToString().Trim());
                    oFact.TaxDate = DateTime.Parse(oRS.Fields.Item("TaxDate").Value.ToString().Trim());
                    oFact.DocDueDate = DateTime.Parse(oRS.Fields.Item("DocDueDate").Value.ToString().Trim());
                    oFact.DocCur = oRS.Fields.Item("DocCur").Value.ToString().Trim();
                    //oFact.TipoDocumento = oRS.Fields.Item("DocSubType").Value;
                    oFact.CondicionPago = oRS.Fields.Item("PymntGroup").Value;
                    oFact.FolioPref = oRS.Fields.Item("FolioPref").Value;
                    oFact.FolioNum = oRS.Fields.Item("FolioNum").Value;
                    oFact.DocTotal = oRS.Fields.Item("DocTotal").Value;
                    oFact.DocTotalFC = oRS.Fields.Item("DocTotalFC").Value;
                    oFact.PaidToDate = oRS.Fields.Item("PaidToDate").Value;
                    oFact.PaidToDateFC = oRS.Fields.Item("PaidFC").Value;
                    oFact.Saldo = oRS.Fields.Item("Saldo").Value;
                    oFact.SaldoFC = oRS.Fields.Item("SaldoFC").Value;
                    oFact.DocStatus = oRS.Fields.Item("Estado").Value;
                    oFact.Atraso = (oFact.Saldo > 0 && oFact.DocDueDate < DateTime.Now) ? (int)(DateTime.Now - (DateTime)oFact.DocDueDate).TotalDays : 0;
                    oFact.Sucursal = oRS.Fields.Item("BPLName").Value;
                    oFact.PorFondoGar = oRS.Fields.Item("FondoGarantia").Value;
                    oFact.TipoDocumento = oRS.Fields.Item("Tipo").Value;
                    oFact.NumAtCard = oRS.Fields.Item("NumAtCard").Value;
                    oFact.Detraccion = oRS.Fields.Item("Detraccion").Value;
                    oFact.Neto = oRS.Fields.Item("Neto").Value;
                    oFact.DueDate = oRS.Fields.Item("FecGarantia").Value;
                    oRS.MoveNext();
                    listFact.Add(oFact);

                }
                return listFact;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public string GetListaDescargar(string ruc, string fi, string ff, string estado)
        {
            try
            {
                DocumentoFactXls oFact = new DocumentoFactXls();
                List<DocumentoFactXls> listFact = new List<DocumentoFactXls>();
                Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRS.DoQuery(Queries.GetFacturasOrdeByDateByRUC(ruc, fi, ff, estado));
                if (oRS.RecordCount == 0)
                    return "";
                for (int i = 0; i < oRS.RecordCount; i++)
                {
                    oFact = new DocumentoFactXls();
                    oFact.Nro = oRS.Fields.Item("DocNum").Value.ToString().Trim();
                    oFact.FecDoc = DateTime.Parse(oRS.Fields.Item("DocDate").Value.ToString().Trim());
                    oFact.DocDueDate = DateTime.Parse(oRS.Fields.Item("DocDueDate").Value.ToString().Trim());
                    oFact.Moneda = oRS.Fields.Item("DocCur").Value.ToString().Trim();
                    oFact.CondicionPago = oRS.Fields.Item("PymntGroup").Value;
                    //oFact.FolioPref = oRS.Fields.Item("FolioPref").Value;
                    //oFact.FolioNum = oRS.Fields.Item("FolioNum").Value;
                    oFact.DocTotal = oFact.Moneda == "SOL" ? oRS.Fields.Item("DocTotal").Value : oRS.Fields.Item("DocTotalFC").Value;
                    oFact.ImportePagado = oFact.Moneda == "SOL" ? oRS.Fields.Item("PaidToDate").Value : oRS.Fields.Item("PaidFC").Value;
                    oFact.Saldo = oFact.Moneda == "SOL" ? oRS.Fields.Item("Saldo").Value : oRS.Fields.Item("SaldoFC").Value;
                    oFact.Estado = oRS.Fields.Item("Estado").Value;
                    oFact.DiasAtraso = (oFact.Saldo > 0 && oFact.DocDueDate < DateTime.Now) ? (int)(DateTime.Now - (DateTime)oFact.DocDueDate).TotalDays : 0;
                    oFact.Sucursal = oRS.Fields.Item("BPLName").Value;
                    oFact.FondoGrantia = oRS.Fields.Item("FondoGarantia").Value;
                    oFact.TipoDocumento = oRS.Fields.Item("Tipo").Value;
                    oFact.NumAtCard = oRS.Fields.Item("NumAtCard").Value;
                    oFact.Detraccion = oRS.Fields.Item("Detraccion").Value;
                    oFact.Neto = oRS.Fields.Item("Neto").Value;
                    oRS.MoveNext();
                    listFact.Add(oFact);

                }

                string xQuery = "SELECT \"AttachPath\" FROM OADP";
                oRS.DoQuery(xQuery);
                string xRuta = oRS.Fields.Item(0).Value + "factura_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                return Common.GenerateExcel(ConvertToDataTable(listFact), xRuta);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static string GetDescCondicionPago(int code)
        {
            string val = DIAPIConexion.GenericQuery(Queries.GetDescCondicionPago(code));
            return val;
        }

        public string CrearFactura(Documento document)
        {
            string xRpta = "";
            string xQuery;
            _company.StartTransaction();
            try
            {
                string path = "";
                int entryAtt = 0;
                string xRutaXML = "";
                Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);

                if (document.Archivo != null && document.Archivo != string.Empty &&
                    document.Archivo2 != null && document.Archivo2 != string.Empty &&
                    document.Archivo3 != null && document.Archivo3 != string.Empty)
                {
                    Recordset rs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                    xQuery = "SELECT \"AttachPath\" FROM OADP";
                    rs.DoQuery(xQuery);
                    if (rs.RecordCount > 0)
                    {
                        path = rs.Fields.Item(0).Value;
                    }

                    string xRuta = "";

                    Attachments2 oATT = _company.GetBusinessObject(BoObjectTypes.oAttachments2);

                    //XML
                    xRuta = path + "XML_" + document.NomArchivo;
                    xRutaXML = xRuta;
                    File.WriteAllBytes(xRuta, Convert.FromBase64String(document.Archivo));

                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension("XML_" + document.NomArchivo);
                    oATT.Lines.FileExtension = Path.GetExtension(document.NomArchivo).Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;

                    oATT.Lines.Add();

                    //CDR
                    xRuta = path + "CDR_" + document.NomArchivo2;
                    File.WriteAllBytes(xRuta, Convert.FromBase64String(document.Archivo2));

                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension("CDR_" + document.NomArchivo2);
                    oATT.Lines.FileExtension = Path.GetExtension(document.NomArchivo2).Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;

                    oATT.Lines.Add();

                    //PDF
                    xRuta = path + "PDF_" + document.NomArchivo3;
                    File.WriteAllBytes(xRuta, Convert.FromBase64String(document.Archivo3));

                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension("PDF_" + document.NomArchivo3);
                    oATT.Lines.FileExtension = Path.GetExtension(document.NomArchivo3).Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;


                    //Garantia por cumplimiento
                    if (document.Archivo4 != null && document.Archivo4 != string.Empty)
                    {
                        oATT.Lines.Add();
                        xRuta = path + document.NomArchivo4;
                        File.WriteAllBytes(xRuta, Convert.FromBase64String(document.Archivo4));

                        oATT.Lines.FileName = Path.GetFileNameWithoutExtension(document.NomArchivo4);
                        oATT.Lines.FileExtension = Path.GetExtension(document.NomArchivo4).Replace(".", "");
                        oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                        oATT.Lines.Override = BoYesNoEnum.tYES;
                    }

                    //Carta Fianza
                    if (document.Archivo5 != null && document.Archivo5 != string.Empty)
                    {
                        oATT.Lines.Add();
                        xRuta = path + document.NomArchivo5;
                        File.WriteAllBytes(xRuta, Convert.FromBase64String(document.Archivo5));

                        oATT.Lines.FileName = Path.GetFileNameWithoutExtension(document.NomArchivo5);
                        oATT.Lines.FileExtension = Path.GetExtension(document.NomArchivo5).Replace(".", "");
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

                Documents oFactura = _company.GetBusinessObject(BoObjectTypes.oDrafts);//_company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
                Documents oGuia = _company.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);

                oGuia.GetByKey(document.DocEntry);

                DateTime xFecFinCon = oGuia.UserFields.Fields.Item("U_EXC_FINCON").Value;

                xQuery = $"SELECT \"LicTradNum\" FROM OCRD WHERE \"CardCode\" = '{oGuia.CardCode}'";
                oRs.DoQuery(xQuery);
                string xRucPro = oRs.Fields.Item(0).Value;
                
                xQuery = $"SELECT \"GlblLocNum\" FROM OBPL WHERE \"BPLId\" = '{oGuia.BPL_IDAssignedToInvoice}'";
                oRs.DoQuery(xQuery);
                string xRucSuc = oRs.Fields.Item(0).Value;

                double impDif = 0;
                xRpta = validarXML(xRutaXML, oGuia.DocCurrency, oGuia.DocTotal, document.FolioPref + "-" + document.FolioNum.ToString().PadLeft(8, '0'), xRucPro, xRucSuc, ref impDif);
                if (xRpta != "")
                    throw new Exception(xRpta);

                oFactura.TaxDate = (DateTime)document.DocDate;
                oFactura.DocDate = DateTime.Now;
                oFactura.DocDueDate = DateTime.Now.AddDays(30);
                oFactura.UserFields.Fields.Item("U_EXC_FECREC").Value = DateTime.Now;
                oFactura.DocObjectCode = BoObjectTypes.oPurchaseInvoices;
                oFactura.CardCode = oGuia.CardCode;
                oFactura.DocCurrency = oGuia.DocCurrency;
                oFactura.Indicator = "01";
                oFactura.BPL_IDAssignedToInvoice = oGuia.BPL_IDAssignedToInvoice;
                oFactura.GroupNumber = oGuia.GroupNumber;
                oFactura.PayToCode = oGuia.PayToCode;
                oFactura.ShipToCode = oGuia.ShipToCode;
                oFactura.DocType = oGuia.DocType;
                oFactura.FolioPrefixString = "FT";// document.FolioPref;
                oFactura.FolioNumber = document.FolioNum;
                oFactura.NumAtCard = document.FolioPref + "-" + document.FolioNum.ToString();
                oFactura.SalesPersonCode = oGuia.SalesPersonCode;
                oFactura.UserFields.Fields.Item("U_EXC_USRCON").Value = oGuia.UserFields.Fields.Item("U_EXC_USRCON").Value;
                oFactura.UserFields.Fields.Item("U_EXC_FONGAR").Value = oGuia.UserFields.Fields.Item("U_EXC_FONGAR").Value;
                oFactura.UserFields.Fields.Item("U_EXC_PORFGR").Value = oGuia.UserFields.Fields.Item("U_EXC_PORFGR").Value;
                oFactura.UserFields.Fields.Item("U_EXC_IMPANT").Value = oGuia.UserFields.Fields.Item("U_EXC_IMPANT").Value;
                oFactura.UserFields.Fields.Item("U_EXC_FVCAFI").Value = oGuia.UserFields.Fields.Item("U_EXC_FVCAFI").Value;
                oFactura.UserFields.Fields.Item("U_EXC_INICON").Value = oGuia.UserFields.Fields.Item("U_EXC_INICON").Value;
                oFactura.UserFields.Fields.Item("U_EXC_FINCON").Value = oGuia.UserFields.Fields.Item("U_EXC_FINCON").Value;
                oFactura.UserFields.Fields.Item("U_EXX_WP_REGUSER").Value = document.UserReg;
                oFactura.UserFields.Fields.Item("U_EXC_COMAPFAC").Value = document.AplicaFactoring;
                if (document.AplicaFactoring == "Y")
                {
                    oFactura.UserFields.Fields.Item("U_EXC_CODPRFAC").Value = document.ProveedorFactoring;
                    oFactura.UserFields.Fields.Item("U_EXC_NOMPRFAC").Value = document.RSProveedorFactoring;
                }

                if (impDif != 0)
                {
                    oFactura.UserFields.Fields.Item("U_EXX_WP_DIFERENCIA").Value = Math.Abs(impDif);
                    oFactura.UserFields.Fields.Item("U_EXX_WP_OBSERVADA").Value = "Y";
                }

                oFactura.Comments = oGuia.NumAtCard + "-" + document.Comments;
                oFactura.JournalMemo = oGuia.NumAtCard + "-" + document.Comments;
                oFactura.AttachmentEntry = entryAtt;

                double xTotal = 0;//document.DocTotal;

                List<Anticipo> oListaAnticipo = new List<Anticipo>();
                double xImporteAnticipo = 0;
                //AJ
                double xTotalDocumento = 0, xTotalBase = 0, xTotalImpuesto = 0;
                for (int i = 0; i < document.DetallePedido.Count; i++)
                {
                    //oGuia.Lines.SetCurrentLine(document.DetallePedido[i].LineNum);
                    oFactura.Lines.BaseEntry = document.DetallePedido[i].DocEntry;//oGuia.DocEntry;
                    oFactura.Lines.BaseType = (int)oGuia.DocObjectCode;
                    oFactura.Lines.BaseLine = document.DetallePedido[i].LineNum;//oGuia.Lines.LineNum;
                    oFactura.Lines.Quantity = document.DetallePedido[i].Quantity;//oGuia.Lines.Quantity;
                                                                                 //oFactura.Lines.UnitPrice = oGuia.Lines.UnitPrice;
                                                                                 //oFactura.Lines.DiscountPercent = oGuia.Lines.DiscountPercent;

                    //AJ 202407002
                    xQuery = $"SELECT T1.\"Quantity\",T1.\"Price\",T1.\"VatPrcnt\",T1.\"DiscPrcnt\",T2.\"DocRate\" " +
                         $"FROM PDN1 T1 " +
                         $"INNER JOIN OPDN T2 ON T2.\"DocEntry\" = T1.\"DocEntry\"  " +
                         $"WHERE T1.\"DocEntry\" = {document.DetallePedido[i].DocEntry}  " +
                         $"AND T1.\"LineNum\" = {document.DetallePedido[i].LineNum}";

                    oRs.DoQuery(xQuery);

                    if (oRs.RecordCount > 0)
                    {
                        if (double.Parse(oRs.Fields.Item("DiscPrcnt").Value.ToString()) > 0)
                        {
                            xTotalBase += Math.Round(Math.Round(double.Parse(oRs.Fields.Item("Price").Value.ToString()) * double.Parse(oRs.Fields.Item("DocRate").Value.ToString()), 4) * double.Parse(oRs.Fields.Item("Quantity").Value.ToString()) * ((100 - double.Parse(oRs.Fields.Item("DiscPrcnt").Value.ToString())) / 100), 2);
                            xTotalImpuesto += Math.Round(Math.Round(Math.Round(double.Parse(oRs.Fields.Item("Price").Value.ToString()) * double.Parse(oRs.Fields.Item("DocRate").Value.ToString()), 4) * double.Parse(oRs.Fields.Item("Quantity").Value.ToString()) * ((100 - double.Parse(oRs.Fields.Item("DiscPrcnt").Value.ToString())) / 100), 2) * double.Parse(oRs.Fields.Item("VatPrcnt").Value.ToString()) / 100, 2);
                        }
                        else
                        {
                            xTotalBase += Math.Round(Math.Round(double.Parse(oRs.Fields.Item("Price").Value.ToString()) * double.Parse(oRs.Fields.Item("DocRate").Value.ToString()), 4) * double.Parse(oRs.Fields.Item("Quantity").Value.ToString()), 2);
                            xTotalImpuesto += Math.Round(Math.Round(Math.Round(double.Parse(oRs.Fields.Item("Price").Value.ToString()) * double.Parse(oRs.Fields.Item("DocRate").Value.ToString()), 4) * double.Parse(oRs.Fields.Item("Quantity").Value.ToString()), 2) * double.Parse(oRs.Fields.Item("VatPrcnt").Value.ToString()) / 100, 2);
                        }

                    }


                    //Se valida si la linea viene de OC con Anticipo
                    xQuery = $"SELECT DISTINCT T4.\"DocEntry\", " +
                                             $"T4.\"DpmAmnt\", " +
                                             $"T4.\"DpmPrcnt\" / 100 AS \"DpmPrcnt\", " +
                                             $"CASE WHEN T1.\"DocCur\" = 'USD' THEN T5.\"TotalFrgn\" ELSE T5.\"LineTotal\" END * T4.\"DpmPrcnt\" / 100 AS \"LineTotal\" " +
                             $"FROM OPOR T1 " +
                             $"INNER JOIN POR1 T2 ON T1.\"DocEntry\" = T2.\"DocEntry\" " +
                             $"INNER JOIN DPO1 T3 ON T3.\"BaseEntry\" = T2.\"DocEntry\" " +
                             $"AND T3.\"BaseType\" = T2.\"ObjType\" " +
                             $"AND T3.\"BaseLine\" = T2.\"LineNum\" " +
                             $"INNER JOIN ODPO T4 ON T4.\"DocEntry\" = T3.\"DocEntry\" " +
                             $"INNER JOIN PDN1 T5 ON T5.\"BaseEntry\" = T2.\"DocEntry\" " +
                             $"AND T5.\"BaseType\" = T2.\"ObjType\" " +
                             $"AND T5.\"BaseLine\" = T2.\"LineNum\" " +
                             $"LEFT  JOIN RPC1 T6 ON T6.\"BaseEntry\" = T3.\"DocEntry\" " +
                             $"AND T6.\"BaseType\" = T3.\"ObjType\" " +
                             $"AND T6.\"BaseLine\" = T3.\"LineNum\"" +
                             $"WHERE (T4.\"DpmAmntFC\" - T4.\"DpmApplFc\" > 0 OR " +
                                    $"T4.\"DpmAmnt\" - T4.\"DpmAppl\" > 0) " +
                             $"AND IFNULL(T6.\"DocEntry\", 0) = 0 " +
                             $"AND T5.\"DocEntry\" = {document.DetallePedido[i].DocEntry} " +
                             $"AND T5.\"LineNum\" = {document.DetallePedido[i].LineNum}";

                    oRs.DoQuery(xQuery);

                    if (oRs.RecordCount > 0)
                    {
                        Anticipo anticipo = new Anticipo();
                        anticipo.DocEntry = oRs.Fields.Item("DocEntry").Value;
                        anticipo.Importe = oRs.Fields.Item("LineTotal").Value;
                        oListaAnticipo.Add(anticipo);
                    }

                    if (i + 1 < document.DetallePedido.Count)
                        oFactura.Lines.Add();
                }

                if (oListaAnticipo.Count > 0)
                {
                    var auxAnt = (from item in oListaAnticipo
                                  group item by new { item.DocEntry } into g
                                  select new Anticipo
                                  {
                                      DocEntry = g.Key.DocEntry,
                                      Importe = g.Sum(x => x.Importe)
                                  }).ToList();

                    foreach (Anticipo anticipo in auxAnt)
                    {
                        oFactura.DownPaymentsToDraw.DocEntry = anticipo.DocEntry;
                        if (oGuia.DocCurrency == "SOL")
                            oFactura.DownPaymentsToDraw.AmountToDraw = anticipo.Importe;
                        else
                            oFactura.DownPaymentsToDraw.AmountToDrawFC = anticipo.Importe;

                        xImporteAnticipo += anticipo.Importe;

                        oFactura.DownPaymentsToDraw.Add();
                    }
                }

                //Detraccion y Solicitud de fondo de garantia
                List<int> oLista = document.DetallePedido.Select(x => x.DocEntry).Distinct().ToList();
                double dPorcDetra = 0;
                double dMontoMinimoDetra = 1000000;
                double dImporFondoGarantia = 0;
                double dMontoFondoGarantia = 0;
                double dPorceFondoGarantia = 0;
                string dNecesFondoGarantia = "";
                //AJ
                double xxPorcent = 0;
                int numCuota = 1;
                foreach (int i in oLista)
                {
                    xQuery = $"SELECT MAX(T4.\"U_EXX_PORDET\") \"Porcentaje\", " +
                                    $"MIN(T4.\"U_EXX_MONMIN\") \"MontoMin\" " +
                             $"FROM OPOR T1 " +
                             $"INNER JOIN POR1 T2 ON T1.\"DocEntry\" = T2.\"DocEntry\" " +
                             $"INNER JOIN PDN1 T3 ON T3.\"BaseEntry\" = T2.\"DocEntry\" " +
                             $"INNER JOIN OITM T0 ON T0.\"ItemCode\" = T2.\"ItemCode\" " +
                             $"INNER JOIN \"@EXX_GRUDET\" T4 ON T0.\"U_EXX_GRUPODET\" = T4.\"Code\"" +
                             $"AND T3.\"BaseType\" = T2.\"ObjType\" " +
                             $"AND T3.\"BaseLine\" = T2.\"LineNum\" " +
                             $"WHERE T3.\"DocEntry\" = {i} ";
                    oRs.DoQuery(xQuery);

                    double dPorcDetraAux = oRs.Fields.Item("Porcentaje").Value;
                    double dMontoDetraAux = oRs.Fields.Item("MontoMin").Value;
                    //if (dPorcDetraAux > dPorcDetra)
                    //    dPorcDetra = dPorcDetraAux;

                    //if(dMontoDetraAux < dMontoMinimoDetra)
                    //    dMontoMinimoDetra = dMontoDetraAux;

                    //AJ 20240702
                    if (dPorcDetraAux > dPorcDetra)
                    {
                        dPorcDetra = dPorcDetraAux;
                        dMontoMinimoDetra = dMontoDetraAux;
                    }


                    xQuery = $"SELECT T1.\"U_EXC_FONGAR\" \"Fondo\", " +
                                    $"T1.\"U_EXC_PORFGR\" \"Porcentaje\", " +
                                    //$"SUM(T2.\"LineTotal\" * CASE WHEN T2.\"TaxCode\" = 'IGV' THEN 1.18 ELSE 1 END) \"Total\" " +
                                    $"SUM(T3.\"LineTotal\") \"Total\" " +
                             $"FROM OPOR T1 " +
                             $"INNER JOIN POR1 T2 ON T1.\"DocEntry\" = T2.\"DocEntry\" " +
                             $"INNER JOIN PDN1 T3 ON T3.\"BaseEntry\" = T2.\"DocEntry\" " +
                             $"INNER JOIN OITM T0 ON T0.\"ItemCode\" = T2.\"ItemCode\" " +
                             $"AND T3.\"BaseType\" = T2.\"ObjType\" " +
                             $"AND T3.\"BaseLine\" = T2.\"LineNum\" " +
                             $"WHERE T3.\"DocEntry\" = {i} " +
                             $"GROUP BY T1.\"U_EXC_FONGAR\", T1.\"U_EXC_PORFGR\"";
                    oRs.DoQuery(xQuery);

                    if (oRs.RecordCount > 0)
                    {
                        dMontoFondoGarantia = oRs.Fields.Item("Total").Value;
                        dPorceFondoGarantia = oRs.Fields.Item("Porcentaje").Value;
                        dNecesFondoGarantia = oRs.Fields.Item("Fondo").Value;
                        //double xxPorcent = dPorceFondoGarantia < 1 ? dPorceFondoGarantia : Math.Round(dPorceFondoGarantia / 100, 2);
                        xxPorcent = dPorceFondoGarantia < 1 ? dPorceFondoGarantia : Math.Round(dPorceFondoGarantia / 100, 2);
                        double xxImporte = dMontoFondoGarantia * xxPorcent;
                        dImporFondoGarantia += xxImporte;
                    }
                }

                if (dImporFondoGarantia > 0)
                {
                    if (oLista.Count > 1)
                    {
                        throw new Exception("Solo se puede elegir guias o conformidades de servicio de una sola Orden de Compra");
                    }

                    numCuota++;
                }

                if (dPorcDetra > 0 && Math.Round(Math.Round(xTotalBase, 2) + Math.Round(xTotalImpuesto, 2), 2) > dMontoMinimoDetra)
                    numCuota++;
                else
                    dPorcDetra = 0;

                if (numCuota > 1)
                {
                    //AJ
                    //double xImporteGuia = oGuia.DocTotal;
                    double xImporteGuia = Math.Round(Math.Round(xTotalBase, 2) + Math.Round(xTotalImpuesto, 2), 2);

                    double sImporteAnti = Math.Round((xImporteAnticipo * 1.18), 2);
                    double sImporteGuia = xImporteGuia - sImporteAnti;
                    double dCuotaDetra = Math.Round((sImporteGuia * dPorcDetra) / 100, 0);
                    double dCuotaDetra2 = Math.Round((sImporteGuia * dPorcDetra) / 100, 2);
                    double dCuotaDetraFinal = 0;

                    if (dPorcDetra > 0)
                    {
                        //CuotaDetra
                        DateTime sFecha1;
                        if (DateTime.Now.Month + 1 <= 12)
                        {
                            sFecha1 = new DateTime(DateTime.Now.Year, (DateTime.Now.Month + 1), DateTime.Now.Day);
                        }
                        else
                        {
                            sFecha1 = new DateTime((DateTime.Now.Year + 1), 1, DateTime.Now.Day);
                        }

                        //Se setea el número de cuotas
                        oFactura.NumberOfInstallments = numCuota;

                        //Cuota de Detraccion
                        oFactura.Installments.DueDate = sFecha1;
                        if (oGuia.DocCurrency == "SOL")
                        {
                            oFactura.Installments.Total = dCuotaDetra;
                            dCuotaDetraFinal = dCuotaDetra;
                        }
                        else
                        {
                            oFactura.Installments.TotalFC = dCuotaDetra2;
                            //oFactura.Installments.Percentage = dPorcDetra;
                            dCuotaDetraFinal = dCuotaDetra2;
                        }

                        //oFactura.Installments.Percentage = dPorcDetra;

                        oFactura.Installments.UserFields.Fields.Item("U_EXX_CONFTIPODET").Value = "Si";
                        oFactura.Installments.UserFields.Fields.Item("U_EXC_FONGAR").Value = "N";
                        oFactura.Installments.Add();
                    }


                    //Cuota de fondo de garantia
                    if (dImporFondoGarantia > 0)
                    {
                        dImporFondoGarantia = Math.Round(Math.Round(Math.Round(xTotalBase, 2)) * xxPorcent, 2);
                        oFactura.Installments.DueDate = xFecFinCon.AddMonths(12);
                        if (oGuia.DocCurrency == "SOL")
                        {
                            oFactura.Installments.Total = Math.Round(dImporFondoGarantia, 2);
                        }
                        else
                        {
                            oFactura.Installments.TotalFC = Math.Round(dImporFondoGarantia, 2);
                        }

                        oFactura.Installments.UserFields.Fields.Item("U_EXC_FONGAR").Value = "Y";
                        oFactura.Installments.UserFields.Fields.Item("U_EXX_BLOPAG").Value = "Y";
                        oFactura.Installments.Add();
                    }

                    //Ultima Cuota
                    oFactura.Installments.DueDate = oGuia.DocDueDate;
                    if (oGuia.DocCurrency == "SOL")
                    {
                        //AJ
                        //double XxImporteGuia = oGuia.DocTotal;

                        double XxImporteGuia = xImporteGuia;

                        //double xxImporte = XxImporteGuia - dCuotaDetra - Math.Round(dImporFondoGarantia, 2) - sImporteAnti;

                        double xxImporte = XxImporteGuia - dCuotaDetraFinal - Math.Round(dImporFondoGarantia, 2) - sImporteAnti;

                        oFactura.Installments.Total = Math.Round(xxImporte, 2);//oGuia.DocTotal - dCuotaDetra - dImporFondoGarantia - (oListaAnticipo.Count > 0 ? oListaAnticipo.Sum(t=>t.Importe) * 1.18 : 0);
                    }
                    else
                    {
                        //double xxImporteUltima = oGuia.DocTotalFc - dCuotaDetra - Math.Round(dImporFondoGarantia, 2) - sImporteAnti;
                        double xxImporteUltima = xImporteGuia - dCuotaDetraFinal - Math.Round(dImporFondoGarantia, 2) - sImporteAnti;
                        oFactura.Installments.TotalFC = xxImporteUltima;
                    }

                    oFactura.Installments.UserFields.Fields.Item("U_EXC_FONGAR").Value = "N";
                    oFactura.Installments.Add();
                }

                if (oFactura.Add() != 0)
                {
                    xRpta = _company.GetLastErrorDescription();

                    if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                else
                {
                    int docEntry = Convert.ToInt32(_company.GetNewObjectKey());

                    if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_Commit);

                    xRpta = "¡Se registró la factura con éxito!";
                }
            }
            catch (Exception ex)
            {
                if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_RollBack);

                xRpta = ex.Message;
            }
            finally
            {
                _company.Disconnect();
            }
            return xRpta;
        }

        public string ActualizarFactura(Documento document)
        {
            string xRpta = "";

            _company.StartTransaction();
            try
            {
                Documents oFactura = _company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);

                oFactura.GetByKey(document.DocEntry);

                oFactura.UserFields.Fields.Item("U_EXC_COMAPFAC").Value = document.AplicaFactoring;
                if (document.AplicaFactoring == "Y")
                {
                    oFactura.UserFields.Fields.Item("U_EXC_CODPRFAC").Value = document.ProveedorFactoring;
                    oFactura.UserFields.Fields.Item("U_EXC_NOMPRFAC").Value = document.RSProveedorFactoring;
                }
                else
                {
                    oFactura.UserFields.Fields.Item("U_EXC_CODPRFAC").Value = string.Empty;
                    oFactura.UserFields.Fields.Item("U_EXC_NOMPRFAC").Value = string.Empty;
                }

                if (oFactura.Update() != 0)
                {
                    xRpta = _company.GetLastErrorDescription();

                    if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                else
                {
                    if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_Commit);

                    xRpta = "¡Se actualizó la factura con éxito!";
                }
            }
            catch (Exception ex)
            {
                if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_RollBack);

                xRpta = ex.Message;
            }
            finally
            {
                _company.Disconnect();
            }
            return xRpta;
        }

        public string CrearFacturaAnticipo(Documento document)
        {
            string xRpta = "";
            _company.StartTransaction();
            try
            {
                string path = "";
                int entryAtt = 0;
                string xRutaXML = "";
                Recordset rs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                string xQuery;

                if (document.Archivo != null && document.Archivo != string.Empty &&
                    document.Archivo2 != null && document.Archivo2 != string.Empty &&
                    document.Archivo3 != null && document.Archivo3 != string.Empty)
                {
                    xQuery = "SELECT \"AttachPath\" FROM OADP";
                    rs.DoQuery(xQuery);
                    if (rs.RecordCount > 0)
                    {
                        path = rs.Fields.Item(0).Value;
                    }

                    string xRuta = "";

                    Attachments2 oATT = _company.GetBusinessObject(BoObjectTypes.oAttachments2);

                    //XML
                    xRuta = path + "XML_" + document.NomArchivo;
                    xRutaXML = xRuta;
                    File.WriteAllBytes(xRuta, Convert.FromBase64String(document.Archivo));

                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension("XML_" + document.NomArchivo);
                    oATT.Lines.FileExtension = Path.GetExtension(document.NomArchivo).Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;

                    oATT.Lines.Add();

                    //CDR
                    xRuta = path + "CDR_" + document.NomArchivo2;
                    File.WriteAllBytes(xRuta, Convert.FromBase64String(document.Archivo2));

                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension("CDR_" + document.NomArchivo2);
                    oATT.Lines.FileExtension = Path.GetExtension(document.NomArchivo2).Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;

                    oATT.Lines.Add();

                    //PDF
                    xRuta = path + "PDF_" + document.NomArchivo3;
                    File.WriteAllBytes(xRuta, Convert.FromBase64String(document.Archivo3));

                    oATT.Lines.FileName = Path.GetFileNameWithoutExtension("PDF_" + document.NomArchivo3);
                    oATT.Lines.FileExtension = Path.GetExtension(document.NomArchivo3).Replace(".", "");
                    oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                    oATT.Lines.Override = BoYesNoEnum.tYES;

                    //Garantia por cumplimiento
                    if (document.Archivo5 != null && document.Archivo5 != string.Empty)
                    {
                        oATT.Lines.Add();

                        xRuta = path + document.NomArchivo4;
                        File.WriteAllBytes(xRuta, Convert.FromBase64String(document.Archivo4));

                        oATT.Lines.FileName = Path.GetFileNameWithoutExtension(document.NomArchivo4);
                        oATT.Lines.FileExtension = Path.GetExtension(document.NomArchivo4).Replace(".", "");
                        oATT.Lines.SourcePath = Path.GetDirectoryName(path);
                        oATT.Lines.Override = BoYesNoEnum.tYES;
                    }

                    //Carta Fianza
                    if (document.Archivo5 != null && document.Archivo5 != string.Empty)
                    {
                        oATT.Lines.Add();
                        xRuta = path + document.NomArchivo5;
                        File.WriteAllBytes(xRuta, Convert.FromBase64String(document.Archivo5));

                        oATT.Lines.FileName = Path.GetFileNameWithoutExtension(document.NomArchivo5);
                        oATT.Lines.FileExtension = Path.GetExtension(document.NomArchivo5).Replace(".", "");
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

                Documents oFactura = _company.GetBusinessObject(BoObjectTypes.oDrafts);//BoObjectTypes.oPurchaseDownPayments
                Documents oOrden = _company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);

                oOrden.GetByKey(document.DocEntry);

                xQuery = $"SELECT \"LicTradNum\" FROM OCRD WHERE \"CardCode\" = '{oOrden.CardCode}'";
                rs.DoQuery(xQuery);
                string xRucPro = rs.Fields.Item(0).Value;

                xQuery = $"SELECT \"GlblLocNum\" FROM OBPL WHERE \"BPLId\" = '{oOrden.BPL_IDAssignedToInvoice}'";
                rs.DoQuery(xQuery);
                string xRucSuc = rs.Fields.Item(0).Value;

                double impDif = 0;
                xRpta = validarXML(xRutaXML, oOrden.DocCurrency, oOrden.DocTotal, document.FolioPref + "-" + document.FolioNum.ToString().PadLeft(8, '0'), xRucPro, xRucSuc, ref impDif);
                if (xRpta != "")
                    throw new Exception(xRpta);

                Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRs.DoQuery(Queries.validarImporteAnticipo(document.DocEntry));

                double xImporteAnticipo = 0;
                if (oRs.RecordCount > 0)
                    xImporteAnticipo = oRs.Fields.Item(0).Value;

                if (oOrden.UserFields.Fields.Item("U_EXC_IMPANT").Value < (document.PaidToDate + xImporteAnticipo))
                {
                    throw new Exception("El importe del anticipo no puede ser mayor al establecido en la orden de compra");
                }

                oFactura.DocObjectCode = BoObjectTypes.oPurchaseDownPayments;
                oFactura.CardCode = oOrden.CardCode;
                oFactura.DocCurrency = oOrden.DocCurrency;
                oFactura.Indicator = "01";
                oFactura.BPL_IDAssignedToInvoice = oOrden.BPL_IDAssignedToInvoice;
                oFactura.GroupNumber = oOrden.GroupNumber;
                oFactura.PayToCode = oOrden.PayToCode;
                oFactura.ShipToCode = oOrden.ShipToCode;
                oFactura.DocType = oOrden.DocType;
                //oFactura.DownPayment = document.DiscountPercent;
                oFactura.FolioPrefixString = "FT";// document.FolioPref;
                oFactura.FolioNumber = document.FolioNum;
                oFactura.NumAtCard = document.FolioPref + "-" + document.FolioNum.ToString();
                oFactura.UserFields.Fields.Item("U_EXC_USRCON").Value = oOrden.UserFields.Fields.Item("U_EXC_USRCON").Value;
                oFactura.SalesPersonCode = oOrden.SalesPersonCode;
                oFactura.AttachmentEntry = entryAtt;
                oFactura.DownPaymentPercentage = document.DiscountPercent;
                oFactura.DownPaymentType = DownPaymentTypeEnum.dptInvoice;
                oFactura.UserFields.Fields.Item("U_EXC_FONGAR").Value = oOrden.UserFields.Fields.Item("U_EXC_FONGAR").Value;
                oFactura.UserFields.Fields.Item("U_EXC_PORFGR").Value = oOrden.UserFields.Fields.Item("U_EXC_PORFGR").Value;
                oFactura.UserFields.Fields.Item("U_EXX_WP_REGUSER").Value = document.UserReg;

                if (impDif != 0)
                {
                    oFactura.UserFields.Fields.Item("U_EXX_WP_DIFERENCIA").Value = Math.Abs(impDif);
                    oFactura.UserFields.Fields.Item("U_EXX_WP_OBSERVADA").Value = "Y";
                }

                double xImpuesto = 0;

                for (int i = 0; i < oOrden.Lines.Count; i++)
                {
                    oOrden.Lines.SetCurrentLine(i);
                    oFactura.Lines.BaseEntry = oOrden.DocEntry;
                    oFactura.Lines.BaseType = (int)oOrden.DocObjectCode;
                    oFactura.Lines.BaseLine = oOrden.Lines.LineNum;
                    oFactura.Lines.TaxCode = oOrden.Lines.TaxCode;

                    //if (oOrden.Lines.TaxCode == "IGV")
                    //    xImpuesto++;

                    if (i + 1 < oOrden.Lines.Count)
                        oFactura.Lines.Add();
                }

                ////Se valida si la OC tiene impuesto
                //if(xImpuesto > 0)
                //{
                //    xQuery = $"SELECT \"Rate\" FROM OSTA WHERE \"Code\" = 'IGV'";
                //    oRs.DoQuery(xQuery);
                //    oFactura.DocTotal = document.PaidToDate * (1 + (oRs.Fields.Item(0).Value / 100));
                //}

                if (oFactura.Add() != 0)
                {
                    xRpta = _company.GetLastErrorDescription();

                    if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                else
                {
                    int docEntry = Convert.ToInt32(_company.GetNewObjectKey());

                    oFactura.GetByKey(docEntry);

                    if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_Commit);

                    xRpta = "¡Se registró la factura de anticipo con exitó!";
                }
            }
            catch (Exception ex)
            {
                if (_company.InTransaction) _company.EndTransaction(BoWfTransOpt.wf_RollBack);

                xRpta = ex.Message;
            }
            finally
            {
                _company.Disconnect();
            }
            return xRpta;
        }

        public Documento GetFacturaId(int id)
        {
            try
            {
                Documento pedido = new Documento();
                Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);

                oPor = _company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
                if (!oPor.GetByKey(id))
                    return pedido;

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
                        estado = "Pendiente de Pago";
                        break;
                    case BoStatus.bost_Close:
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
                pedido.DireccionDespacho = oPor.Address2;
                pedido.DireccionFiscal = oPor.Address;
                pedido.CondicionPago = GetDescCondicionPago(oPor.PaymentGroupCode);//TODO
                pedido.SalesPersonCode = oPor.SalesPersonCode.ToString(); ;
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
                pedido.FolioNum = oPor.FolioNumber;
                pedido.FolioPref = oPor.FolioPrefixString;

                DateTime fecha = DateTime.Parse("1899-12-30");
                pedido.U_EXC_FVCAFI = oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value;
                pedido.U_EXC_INICON = oPor.UserFields.Fields.Item("U_EXC_INICON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_INICON").Value;
                pedido.U_EXC_FINCON = oPor.UserFields.Fields.Item("U_EXC_FINCON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FINCON").Value;

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
                        if (i == 4)
                        {
                            pedido.Archivo4 = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo4 = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }
                        if (i == 5)
                        {
                            pedido.Archivo5 = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo5 = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }

                        oRs.MoveNext();
                    }
                }

                List<DetallePedido> detalle = new List<DetallePedido>();
                for (int i = 0; i < oPor.Lines.Count; i++)
                {
                    oPor.Lines.SetCurrentLine(i);
                    DetallePedido det = new DetallePedido();
                    det.LineNum = oPor.Lines.LineNum;
                    det.ItemCode = oPor.Lines.ItemCode;
                    det.UnitMsr = oPor.Lines.MeasureUnit;
                    det.Quantity = oPor.Lines.Quantity;
                    det.Dscription = oPor.Lines.ItemDescription;
                    det.DiscountPercent = oPor.Lines.DiscountPercent;
                    det.Price = oPor.Lines.Price;
                    det.TaxCode = oPor.Lines.TaxCode;
                    det.PriceAfVAT = oPor.Lines.PriceAfterVAT;
                    det.LineTotal = oPor.Lines.LineTotal;
                    det.WhsCode = oPor.Lines.WarehouseCode;
                    detalle.Add(det);
                    pedido.DetallePedido = detalle;
                }
                return pedido;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Documento GetAnticipoId(int id)
        {
            try
            {
                Documento pedido = new Documento();

                oPor = _company.GetBusinessObject(BoObjectTypes.oPurchaseDownPayments);
                if (!oPor.GetByKey(id))
                    return null;

                pedido.DocEntry = oPor.DocEntry;
                pedido.DocNum = oPor.DocNum.ToString();
                pedido.CardCode = oPor.CardCode;
                pedido.CardName = oPor.CardName;
                pedido.LicTradNum = oPor.FederalTaxID;
                pedido.NumAtCard = oPor.NumAtCard;
                pedido.DocDate = oPor.DocDate;
                pedido.DocDueDate = oPor.DocDueDate;
                pedido.GroupNum = oPor.GroupNumber;
                string estado = "";
                switch (oPor.DocumentStatus)
                {
                    case BoStatus.bost_Open:
                        estado = "Pendiente de Pago";
                        break;
                    case BoStatus.bost_Close:
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
                pedido.SalesPersonCode = oPor.SalesPersonCode.ToString(); ;
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
                pedido.FolioNum = oPor.FolioNumber;
                pedido.FolioPref = oPor.FolioPrefixString;

                DateTime fecha = DateTime.Parse("1899-12-30");
                pedido.U_EXC_FVCAFI = oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value;
                pedido.U_EXC_INICON = oPor.UserFields.Fields.Item("U_EXC_INICON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_INICON").Value;
                pedido.U_EXC_FINCON = oPor.UserFields.Fields.Item("U_EXC_FINCON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FINCON").Value;

                string query = $"SELECT \"Line\", \"trgtPath\", \"FileName\", \"FileExt\" FROM ATC1 WHERE \"AbsEntry\" = {oPor.AttachmentEntry}";
                Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
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
                        if (i == 4)
                        {
                            pedido.Archivo4 = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo4 = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }
                        if (i == 5)
                        {
                            pedido.Archivo5 = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo5 = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }

                        oRs.MoveNext();
                    }
                }

                List<DetallePedido> detalle = new List<DetallePedido>();
                for (int i = 0; i < oPor.Lines.Count; i++)
                {
                    oPor.Lines.SetCurrentLine(i);
                    DetallePedido det = new DetallePedido();
                    det.LineNum = oPor.Lines.LineNum;
                    det.ItemCode = oPor.Lines.ItemCode;
                    det.UnitMsr = oPor.Lines.MeasureUnit;
                    det.Quantity = oPor.Lines.Quantity;
                    det.Dscription = oPor.Lines.ItemDescription;
                    det.DiscountPercent = oPor.Lines.DiscountPercent;
                    det.Price = oPor.Lines.Price;
                    det.TaxCode = oPor.Lines.TaxCode;
                    det.PriceAfVAT = oPor.Lines.PriceAfterVAT;
                    det.LineTotal = oPor.Lines.LineTotal;
                    det.WhsCode = oPor.Lines.WarehouseCode;
                    detalle.Add(det);
                    pedido.DetallePedido = detalle;
                }
                return pedido;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Documento GetBorradorId(int id)
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
                pedido.DocDueDate = oPor.DocDueDate;
                pedido.GroupNum = oPor.GroupNumber;
                string estado = "";
                switch (oPor.DocumentStatus)
                {
                    case BoStatus.bost_Open:
                        estado = "Pendiente de Pago";
                        break;
                    case BoStatus.bost_Close:
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
                pedido.SalesPersonCode = oPor.SalesPersonCode.ToString(); ;
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
                pedido.FolioNum = oPor.FolioNumber;
                pedido.FolioPref = oPor.FolioPrefixString;

                DateTime fecha = DateTime.Parse("1899-12-30");
                pedido.U_EXC_FVCAFI = oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FVCAFI").Value;
                pedido.U_EXC_INICON = oPor.UserFields.Fields.Item("U_EXC_INICON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_INICON").Value;
                pedido.U_EXC_FINCON = oPor.UserFields.Fields.Item("U_EXC_FINCON").Value == fecha ? null : oPor.UserFields.Fields.Item("U_EXC_FINCON").Value;

                string query = $"SELECT \"Line\", \"trgtPath\", \"FileName\", \"FileExt\" FROM ATC1 WHERE \"AbsEntry\" = {oPor.AttachmentEntry}";
                Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
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
                        if (i == 4)
                        {
                            pedido.Archivo4 = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo4 = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }
                        if (i == 5)
                        {
                            pedido.Archivo5 = oRs.Fields.Item(1).Value + "\\" + oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                            pedido.NomArchivo5 = oRs.Fields.Item(2).Value + "." + oRs.Fields.Item(3).Value;
                        }

                        oRs.MoveNext();
                    }
                }

                List<DetallePedido> detalle = new List<DetallePedido>();
                for (int i = 0; i < oPor.Lines.Count; i++)
                {
                    oPor.Lines.SetCurrentLine(i);
                    DetallePedido det = new DetallePedido();
                    det.LineNum = oPor.Lines.LineNum;
                    det.ItemCode = oPor.Lines.ItemCode;
                    det.UnitMsr = oPor.Lines.MeasureUnit;
                    det.Quantity = oPor.Lines.Quantity;
                    det.Dscription = oPor.Lines.ItemDescription;
                    det.DiscountPercent = oPor.Lines.DiscountPercent;
                    det.Price = oPor.Lines.Price;
                    det.TaxCode = oPor.Lines.TaxCode;
                    det.PriceAfVAT = oPor.Lines.PriceAfterVAT;
                    det.LineTotal = oPor.Lines.LineTotal;
                    det.WhsCode = oPor.Lines.WarehouseCode;
                    detalle.Add(det);
                    pedido.DetallePedido = detalle;
                }
                return pedido;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string validarXML(string xml, string moneda, double importe, string serieNumero, string rucProveedor, string rucSucursal, ref double impDif)
        {
            XmlDocument xmlRuta = new XmlDocument();
            xmlRuta.Load(xml);
            string xRpta = "";
            string xMonedaXML = "";
            double xImportXML = 0;
            string xSerieNumero = "";
            string xRucPro = "";
            string xRucCli = "";

            //List<Maestro> xConfiguraciones = new MaestroDIAPI().getConfiguracion();
            double xImporteRango = 1;//Convert.ToDouble(xConfiguraciones.Where(t=>t.Codigo == "0000007").ToList()[0].Valor_01);

            foreach (XmlNode n1 in xmlRuta.DocumentElement.ChildNodes)
            {
                if (n1.Name == "cbc:ID")
                {
                    xSerieNumero = n1.InnerText;
                }

                if (n1.Name == "cac:AccountingSupplierParty")
                {
                    foreach (XmlNode n2 in n1.ChildNodes)
                    {
                        if (n2.Name == "cac:Party")
                        {
                            foreach (XmlNode n3 in n2.ChildNodes)
                            {
                                if (n3.Name == "cac:PartyIdentification")
                                {
                                    foreach (XmlNode n4 in n3.ChildNodes)
                                    {
                                        if (n4.Name == "cbc:ID")
                                        {
                                            xRucPro = n4.InnerText;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (n1.Name == "cac:AccountingCustomerParty")
                {
                    foreach (XmlNode n2 in n1.ChildNodes)
                    {
                        if (n2.Name == "cac:Party")
                        {
                            foreach (XmlNode n3 in n2.ChildNodes)
                            {
                                if (n3.Name == "cac:PartyIdentification")
                                {
                                    foreach (XmlNode n4 in n3.ChildNodes)
                                    {
                                        if (n4.Name == "cbc:ID")
                                        {
                                            xRucCli = n4.InnerText;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (n1.Name == "cbc:DocumentCurrencyCode")
                {
                    xMonedaXML = n1.InnerText;
                }

                if (n1.Name == "cac:TaxTotal")
                {
                    foreach (XmlNode n2 in n1.ChildNodes)
                    {
                        if (n2.Name == "cac:TaxSubtotal")
                        {
                            foreach (XmlNode n3 in n2.ChildNodes)
                            {
                                if (n3.Name == "cbc:TaxableAmount")
                                {
                                    xImportXML = Convert.ToDouble(n3.InnerText);
                                }
                            }
                        }
                    }
                }
            }

            xMonedaXML = xMonedaXML == "PEN" ? "SOL" : xMonedaXML;

            if (xSerieNumero != serieNumero)
            {
                xRpta += $"\n - La serie y numero del XML es diferente a la del documento.\n Serie y Nro. XML: {xSerieNumero}, Serie y Nro. Documento: {serieNumero}\n";
            }

            if (xMonedaXML != moneda)
            {
                xRpta += $"\n - La moneda del XML es diferente a la moneda del documento.\n Moneda XML: {xMonedaXML}, Moneda Documento: {moneda}\n";
            }

            if (xImportXML != importe)
            {
                //if (Math.Abs(xImportXML - importe) > xImporteRango)
                //    xRpta += $"\n - El importe del XML es diferente al importe del documento.\n Importe XML: {xImportXML.ToString("N2")}, Importe Documento: {importe.ToString("N2")}\n";
                impDif = importe-xImportXML;
            }

            if (xRucPro != rucProveedor)
            {
                xRpta += $"\n - El RUC emisor del XML es diferente al RUC emisor del documento.\n RUC XML: {xRucPro}, RUC Documento: {rucProveedor}\n";
            }
            
            if (xRucCli != rucSucursal)
            {
                xRpta += $"\n - El RUC receptor del XML es diferente al RUC emisor del documento.\n RUC XML: {xRucCli}, RUC Documento: {rucSucursal}\n";
            }

            return xRpta;
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

        //public bool ActualizarDocumento(Documento oferta)
        //{
        //    try
        //    {
        //        oPor = _company.GetBusinessObject(BoObjectTypes.oQuotations);
        //        if (!oPor.GetByKey(oferta.DocEntry))
        //            throw new System.Exception("La oferta con id: " + oferta.DocEntry + " no existe en la sociedad.");

        //        if (oPor.DocumentStatus != BoStatus.bost_Open)
        //            throw new System.Exception("La oferta con id: " + oferta.DocEntry + " se encuentra cerrada.");
        //        //if (oQuot.UserFields.Fields.Item("U_EXS_APROB").Value == "A" && oferta.U_SCO_ESTADO == "-")
        //        //    throw new System.Exception("La oferta no puede retroceder luego de aprobada.");
        //        //switch (oferta.U_SCO_ESTADO)
        //        //{
        //        //    case "R":
        //        //        if (oQuot.Close() != 0)
        //        //            throw new System.Exception(_company.GetLastErrorDescription());

        //        //        return true;
        //        //    case "-":
        //        //        for (int a = 0; a < oQuot.Lines.Count; a++)
        //        //        {
        //        //            oQuot.Lines.SetCurrentLine(a);
        //        //            oQuot.Lines.Delete();
        //        //        }
        //        //        break;
        //        //}
        //        OQUT(oferta);
        //        int i = 0;
        //        foreach (DetalleOferta det in oferta.DetalleOferta)
        //        {
        //            if (oPor.Lines.Count <= det.LineNum)
        //                oPor.Lines.Add();

        //            oPor.Lines.SetCurrentLine(det.LineNum);
        //            if (oPor.Lines.LineStatus == BoStatus.bost_Open)
        //                QUT1(det);
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
        //        OQUT(oferta);
        //        foreach (DetalleOferta det in oferta.DetalleOferta)
        //        {
        //            QUT1(det);
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

        //private void OfertaToOrder(Documento oferta)
        //{
        //    try
        //    {
        //        int count = 0;
        //        oOrd = _company.GetBusinessObject(BoObjectTypes.oOrders);
        //        ORDR(oferta);
        //        int i = 0;
        //        foreach (DetalleOferta det in oferta.DetalleOferta)
        //        {
        //            oPor.Lines.SetCurrentLine(det.LineNum);
        //            if (!string.IsNullOrEmpty(det.Project) && oPor.Lines.LineStatus == BoStatus.bost_Open)
        //            {
        //                RDR1(det);
        //                oOrd.Lines.BaseEntry = oferta.DocEntry;
        //                oOrd.Lines.BaseLine = oPor.Lines.LineNum;
        //                oOrd.Lines.BaseType = (int)BoObjectTypes.oQuotations;

        //                oOrd.Lines.Add();
        //                count++;
        //            }
        //            i++;
        //        }
        //        if (count > 0)
        //            if (oOrd.Add() != 0)
        //                throw new Exception(_company.GetLastErrorDescription());
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(oOrd);
        //    }
        //}
        //private void OQUT(Documento oferta)
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
        //private void QUT1(DetalleOferta detOfer)
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

        //private void ORDR(Documento oferta)
        //{
        //    oOrd.CardCode = oferta.CardCode;
        //    oOrd.DocDate = (DateTime)oferta.DocDate;
        //    oOrd.DocDueDate = (DateTime)oferta.DocDueDate;

        //    //oQuot.GroupNumber = oferta.GroupNum;
        //    //oQuot.Project = oferta.Project;
        //    //oQuot.DiscountPercent = oferta.DiscountPercent;
        //    //oOrd.DocTotal = oferta.DocTotal;
        //    //oQuot.DocCurrency = oferta.DocCur;
        //    //oQuot.Comments = oferta.Comments;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_NOPERACION)) oQuot.UserFields.Fields.Item("U_SCO_NOPERACION").Value = oferta.U_SCO_NOPERACION;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_ESTADO)) oQuot.UserFields.Fields.Item("U_SCO_ESTADO").Value = oferta.U_SCO_ESTADO;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_MARGEN)) oQuot.UserFields.Fields.Item("U_SCO_MARGEN").Value = oferta.U_SCO_MARGEN;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_NUMEROCOTIZACION)) oQuot.UserFields.Fields.Item("U_SCO_NUMEROCOTIZACION").Value = oferta.U_SCO_NUMEROCOTIZACION;
        //    //if (oferta.U_SCO_FECHACREACION != null) oQuot.UserFields.Fields.Item("U_SCO_FECHACREACION").Value = oferta.U_SCO_FECHACREACION;
        //    //if (oferta.U_SCO_FECHAACTUALIZACION != null) oQuot.UserFields.Fields.Item("U_SCO_FECHAACTUALIZACION").Value = oferta.U_SCO_FECHAACTUALIZACION;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_USUARIOCREA)) oQuot.UserFields.Fields.Item("U_SCO_USUARIOCREA").Value = oferta.U_SCO_USUARIOCREA;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_USUARIOACT)) oQuot.UserFields.Fields.Item("U_SCO_USUARIOACT").Value = oferta.U_SCO_USUARIOACT;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_RUBRO1)) oQuot.UserFields.Fields.Item("U_SCO_RUBRO1").Value = oferta.U_SCO_RUBRO1;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_RUCCLIENTE2)) oQuot.UserFields.Fields.Item("U_SCO_RUCCLIENTE2").Value = oferta.U_SCO_RUCCLIENTE2;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_RSCLIENTE2)) oQuot.UserFields.Fields.Item("U_SCO_RSCLIENTE2").Value = oferta.U_SCO_RSCLIENTE2;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_RUBROCF)) oQuot.UserFields.Fields.Item("U_SCO_RUBROCF").Value = oferta.U_SCO_RUBROCF;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_DESCRIPCION)) oQuot.UserFields.Fields.Item("U_SCO_DESCRIPCION").Value = oferta.U_SCO_DESCRIPCION;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_CAMPO1)) oQuot.UserFields.Fields.Item("U_SCO_CAMPO1").Value = oferta.U_SCO_CAMPO1;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_CAMPO2)) oQuot.UserFields.Fields.Item("U_SCO_CAMPO2").Value = oferta.U_SCO_CAMPO2;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_CAMPO3)) oQuot.UserFields.Fields.Item("U_SCO_CAMPO3").Value = oferta.U_SCO_CAMPO3;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_CAMPO4)) oQuot.UserFields.Fields.Item("U_SCO_CAMPO4").Value = oferta.U_SCO_CAMPO4;
        //    //if (!string.IsNullOrEmpty(oferta.U_SCO_CAMPO5)) oQuot.UserFields.Fields.Item("U_SCO_CAMPO5").Value = oferta.U_SCO_CAMPO5;
        //}
        //private void RDR1(DetalleOferta detOfer)
        //{
        //    oOrd.Lines.ItemCode = detOfer.ItemCode;
        //    oOrd.Lines.UserFields.Fields.Item("U_EXX_GRUPOPER").Value = "0000";
        //    oOrd.Lines.LineTotal = detOfer.LineTotal;
        //    //oQuot.Lines.FreeText = detOfer.Text;
        //    //oQuot.Lines.WarehouseCode = detOfer.WhsCode;
        //    //oQuot.Lines.Quantity = detOfer.Quantity;
        //    //oQuot.Lines.TaxCode = detOfer.TaxCode;
        //    //oQuot.Lines.Price = detOfer.Price;
        //    //oQuot.Lines.PriceAfterVAT = detOfer.PriceAfVAT;
        //    //oQuot.Lines.DiscountPercent = detOfer.DISCPRCNT;
        //    //oQuot.Lines.CostingCode = detOfer.OcrCode;
        //    //oQuot.Lines.CostingCode3 = detOfer.OcrCode3;
        //    //if (!string.IsNullOrEmpty(detOfer.U_EXX_GRUPODET)) oQuot.Lines.UserFields.Fields.Item("U_EXX_GRUPODET").Value = detOfer.U_EXX_GRUPODET;
        //    //if (!string.IsNullOrEmpty(detOfer.U_SCO_NUMEROCOTIZACION)) oQuot.Lines.UserFields.Fields.Item("U_SCO_NUMEROCOTIZACION").Value = detOfer.U_SCO_NUMEROCOTIZACION;
        //    //if (!string.IsNullOrEmpty(detOfer.U_SCO_ORIGEN)) oQuot.Lines.UserFields.Fields.Item("U_SCO_ORIGEN").Value = detOfer.U_SCO_ORIGEN;
        //    //if (!string.IsNullOrEmpty(detOfer.U_SCO_DESTINO)) oQuot.Lines.UserFields.Fields.Item("U_SCO_DESTINO").Value = detOfer.U_SCO_DESTINO;
        //    //if (!string.IsNullOrEmpty(detOfer.U_SCO_CAMPO1)) oQuot.Lines.UserFields.Fields.Item("U_SCO_CAMPO1").Value = detOfer.U_SCO_CAMPO1;
        //    //if (!string.IsNullOrEmpty(detOfer.U_SCO_CAMPO2)) oQuot.Lines.UserFields.Fields.Item("U_SCO_CAMPO2").Value = detOfer.U_SCO_CAMPO2;
        //    //if (!string.IsNullOrEmpty(detOfer.U_SCO_CAMPO3)) oQuot.Lines.UserFields.Fields.Item("U_SCO_CAMPO3").Value = detOfer.U_SCO_CAMPO3;
        //    //if (!string.IsNullOrEmpty(detOfer.U_SCO_CAMPO4)) oQuot.Lines.UserFields.Fields.Item("U_SCO_CAMPO4").Value = detOfer.U_SCO_CAMPO4;
        //    //if (!string.IsNullOrEmpty(detOfer.U_SCO_CAMPO5)) oQuot.Lines.UserFields.Fields.Item("U_SCO_CAMPO5").Value = detOfer.U_SCO_CAMPO5;
        //}

    }
}
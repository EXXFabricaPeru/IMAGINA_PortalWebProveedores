using System;
using System.Collections.Generic;
using System.Text;
using WebProov_API.Models;

namespace WebProov_API.Util
{
    public class Queries
    {

        #region _Attributes_

        private static StringBuilder m_sSQL = new StringBuilder();

        #endregion

        #region _Functions_
        #region Login
        public static string ValidarUserTra(string mail, string password)
        {
            m_sSQL.Length = 0;
            m_sSQL.Append("SELECT TOP 1 '' \"CardCode\", '' \"CardName\", '' \"E_Mail\", '' \"LicTradNum\", 0 \"CreditLine\"");
            m_sSQL.Append("FROM \"@EXX_WP_USUARIO\" ");
            m_sSQL.AppendFormat("WHERE (U_EXX_USUARIO='{0}' OR \"U_EXX_EMAIL\" = '{0}') and \"U_EXX_PASSWORD\"='{1}' ", mail, password);
            return m_sSQL.ToString();
        }

        public static string ValidarUserTraActivo(string user)
        {
            m_sSQL.Length = 0;
            m_sSQL.Append("SELECT COUNT(*) ");
            m_sSQL.Append("FROM \"@EXX_WP_USUARIO\" ");
            m_sSQL.AppendFormat("WHERE (U_EXX_USUARIO='{0}' OR \"U_EXX_EMAIL\" = '{0}') AND U_EXX_ACTIVO = 'Y' ", user);
            return m_sSQL.ToString();
        }

        public static string ValidarUser(string mail, string password)
        {
            string query = $"SELECT TOP 1 \"CardCode\"," +
                                        $"\"CardName\"," +
                                        $"\"E_Mail\"," +
                                        $"\"LicTradNum\"," +
                                        $"\"CreditLine\" " +
                           $"FROM OCRD " +
                           $"WHERE \"LicTradNum\"='{mail}' " +
                           $"AND \"Password\"='{password}' " +
                           $"AND \"CardType\"='S' " +
                           //$"AND IFNULL(U_EXX_WP_ACTIVO, 'Y') = 'Y' " +
                           //$"AND U_EXX_WP_FCVG >= '{DateTime.Now.ToString("yyyyMMdd")}'" +
                           $"AND LEFT(\"CardCode\", 1) = 'P' ";

            return query;
        }

        public static string ValidarUserVigencia(string user)
        {
            string query = $"SELECT COUNT(*) " +
                           $"FROM OCRD " +
                           $"WHERE \"LicTradNum\"='{user}' " +
                           $"AND \"CardType\"='S' " +
                           $"AND U_EXX_WP_FCVG >= '{DateTime.Now.ToString("yyyyMMdd")}' " +
                           $"AND LEFT(\"CardCode\", 1) = 'P' ";

            return query;
        }

        public static string ValidarUserActivo(string user)
        {
            string query = $"SELECT COUNT(*) " +
                           $"FROM OCRD " +
                           $"WHERE \"LicTradNum\"='{user}' " +
                           $"AND \"CardType\"='S' " +
                           $"AND IFNULL(U_EXX_WP_ACTIVO, 'Y') = 'Y' " +
                           $"AND LEFT(\"CardCode\", 1) = 'P' ";

            return query;
        }
        #endregion

        #region Proveedores
        public static string GetDraft(string ruc)
        {
            string xQuery = $"CALL \"EXX_WP_Proveedor_Buscar\" ('{ruc}')";
            return xQuery;
        }
        public static string GetDraftContact(string code)
        {
            string xQuery = $"CALL \"EXX_WP_ProveedorContacto_Buscar\" ('{code}')";
            return xQuery;
        }
        public static string GetDraftDireccion(string code)
        {
            string xQuery = $"CALL \"EXX_WP_ProveedorDireccion_Buscar\" ('{code}')";
            return xQuery;
        }
        public static string GetDraftCuentas(string code)
        {
            string xQuery = $"CALL \"EXX_WP_ProveedorCuentaBanco_Buscar\" ('{code}')";
            return xQuery;
        }
        public static string GetBPbyCardCode(string input)
        {
            //string sQuery = "SELECT TO_NVARCHAR(ADD_YEARS(\"Fecha\", 1), 'DD/MM/YYYY' ) FROM (SELECT MAX(T1.\"UpdateDate\") \"Fecha\" FROM \"@EXX_WP_PROV_CAB\" T1 WHERE T1.U_EXX_RUC = C1.\"LicTradNum\")";

            string query = $"SELECT TOP 1  C1.\"CardCode\"," +
                                         $"C1.\"CardName\"," +
                                         $"C1.\"LicTradNum\"," +
                                         $"C1.\"E_Mail\" \"EmailAddress\", " +
                                         $"CASE WHEN C1.\"Currency\"='##' THEN 'MULTI' ELSE  C1.\"Currency\"END  \"Currency\", " +
                                         $"CASE U_EXC_CALIPROV " +
                                         $"WHEN 'A1' THEN 'Apto Destacado' " +
                                         $"WHEN 'A2' THEN 'Apto Normal' " +
                                         $"WHEN 'A3' THEN 'Apto Observado' " +
                                         $"WHEN 'A4' THEN 'Bloqueado' " +
                                         $"END \"Calificacion\"," +
                                         //$"IFNULL(({sQuery}), '') AS \"Vigencia\"" +
                                         $"U_EXX_WP_FCVG AS \"Vigencia\"" +
                           $"FROM OCRD C1 " +
                           $"WHERE C1.\"CardCode\" LIKE '%{input}' " +
                           $"OR C1.\"E_Mail\"='{input}' ";

            return query;
        }
        public static string GetListaProveedor(string valor, string estado)
        {
            string xQuery = $"CALL \"EXX_WP_Proveedor_Listar\" ('{valor}', '{estado}')";
            return xQuery;
        }
        public static string GetListaProveedorFactor()
        {
            string xQuery = $"CALL \"EXX_WP_ProveedorFN_Listar\"";
            return xQuery;
        }
        #endregion

        #region Ordenes de Compra
        public static string GetPedidosByRUC(string ruc, string fecIni, string fecFin, string estado)
        {
            m_sSQL.Length = 0;
            m_sSQL.Append($"CALL \"EXX_WP_OrdenCompra_Listar\" ('{ruc}','{fecIni}','{fecFin}','{estado}')");
            return m_sSQL.ToString();
        }
        #endregion

        #region Conformidad de Servicio
        public static string AprobacionConformidadServicio(int code, string user, string estado, string commentario)
        {
            m_sSQL.Length = 0;
            m_sSQL.Append($"UPDATE \"@EXX_WP_ACSD_APCOSRV\" SET U_EXX_APROBADO='{estado}', ");
            m_sSQL.Append($"U_EXX_COMENTARIO = '{commentario}', ");
            m_sSQL.Append($"U_EXX_FECAPROB = '{DateTime.Now.ToString("yyyyMMdd")}' ");
            m_sSQL.Append($"WHERE \"DocEntry\"={code} AND U_EXX_USUARIO='{user}' ");

            return m_sSQL.ToString();
        }
        public static string GetConformidadByRUC(string ruc, string fecIni, string fecFin, string estado)
        {
            m_sSQL.Length = 0;
            m_sSQL.Append($"CALL \"EXX_WP_ConformidadServicio_Listar\" ('{ruc}','{fecIni}','{fecFin}','{estado}')");
            return m_sSQL.ToString();
        }
        public static string GetConformidadDisponible(string ruc, string fecIni, string fecFin, string sucursal)
        {
            string xQuery = $"CALL \"EXX_WP_ConformidadServicioDisponible_Listar\" ('{ruc}','{fecIni}','{fecFin}',{sucursal})";
            return xQuery;
        }

        public static string GetDetalleConfoServAprb(string docEntry)
        {
            string xQuery = $"CALL \"EXX_WP_ConformidadServicioDetalleAprobacion_Buscar\" ('{docEntry}')";
            return xQuery;
        }
        public static string GetDetalleConfoServ(string docEntry)
        {
            string xQuery = $"CALL \"EXX_WP_ConformidadServicioDetalle_Buscar\" ('{docEntry}')";
            return xQuery;
        }

        #endregion

        #region Facturas
        public static string GetFacturasOrdeByDateByRUC(string ruc, string fi, string ff, string estado)
        {
            m_sSQL.Length = 0;
            m_sSQL.Append($"CALL \"EXX_WP_FacturaProv_Listar\" ('{ruc}','{fi}','{ff}','{estado}')");
            return m_sSQL.ToString();
        }

        public static string validarImporteAnticipo(int docEntry)
        {
            string xQuery = "";
            xQuery += "SELECT IFNULL(SUM(CASE WHEN \"DocCur\" = 'SOL' THEN \"DpmAmnt\" ELSE \"DpmAmntFC\" END), 0) \"ImpAnt\" " +
                      "FROM(SELECT DISTINCT T4.\"DocEntry\", " +
                                           "T4.\"DpmAmntFC\", " +
                                           "T4.\"DpmAmnt\"," +
                                           "T4.\"DocCur\" " +
                           "FROM OPOR T1 " +
                           "INNER JOIN POR1 T2 ON T1.\"DocEntry\" = T2.\"DocEntry\" " +
                           "INNER JOIN DPO1 T3 ON T3.\"BaseEntry\" = T2.\"DocEntry\" " +
                                             "AND T3.\"BaseLine\" = T2.\"LineNum\" " +
                                             "AND T3.\"BaseType\" = T2.\"ObjType\" " +
                           "INNER JOIN ODPO T4 ON T4.\"DocEntry\" = T3.\"DocEntry\" " +
                           "LEFT  JOIN RPC1 T5 ON T5.\"BaseEntry\" = T3.\"DocEntry\" " +
                                             "AND T5.\"BaseLine\" = T3.\"LineNum\" " +
                                             "AND T5.\"BaseType\" = T3.\"ObjType\" " +
                           "LEFT  JOIN ORPC T6 ON T6.\"DocEntry\" = T5.\"DocEntry\" " +
                           "WHERE IFNULL(T6.\"DocEntry\", 0) = 0 " +
                                        $"AND T1.\"DocEntry\" = {docEntry} " +
                           "UNION ALL " +
                           "SELECT DISTINCT T4.\"DocEntry\", " +
                                           "T4.\"DpmAmntFC\", " +
                                           "T4.\"DpmAmnt\", " +
                                           "T1.\"DocCur\" " +
                           "FROM OPOR T1 " +
                           "INNER JOIN POR1 T2 ON T1.\"DocEntry\" = T2.\"DocEntry\" " +
                           "INNER JOIN DRF1 T3 ON T3.\"BaseEntry\" = T2.\"DocEntry\" " +
                                             "AND T3.\"BaseLine\" = T2.\"LineNum\" " +
                                             "AND T3.\"BaseType\" = T2.\"ObjType\" " +
                           "INNER JOIN ODRF T4 ON T4.\"DocEntry\" = T3.\"DocEntry\" " +
                           "WHERE T4.\"DocStatus\" = 'O' " +
                             $"AND T1.\"DocEntry\" = {docEntry}" +
                       ") t";
            return xQuery;
        }
        #endregion

        #region General
        public static string ListarProyectos()
        {
            m_sSQL.Length = 0;
            m_sSQL.Append("SELECT \"PrjCode\"  FROM OPRJ");
            return m_sSQL.ToString();
        }
        public static string getCondicionesPago()
        {
            string xQuery = $"SELECT \"GroupNum\" \"Codigo\", \"PymntGroup\" \"Descripcion\", \"ExtraDays\" + 1 \"ExtraDays\" FROM OCTG WHERE \"GroupNum\" = 11 ";
            return xQuery;
        }
        public static string getDepartamento(string codPais)
        {
            string xQuery = $"SELECT \"Code\", \"Name\"\r\n\tFROM OCST\r\n\tWHERE \"Country\" = '{codPais}'";
            return xQuery;
        }
        public static string getProvincia(string codDepartamento)
        {
            string xQuery = $"SELECT \"Code\", \"Name\"\r\n\tFROM \"@EXX_PROVIN\"\r\n\tWHERE \"U_EXX_CODPAI\" = 'PE'\r\n\t  AND \"U_EXX_CODDEP\" = '{codDepartamento}'";
            return xQuery;
        }
        public static string getDistrito(string codProvincia)
        {
            string xQuery = $"SELECT \"Code\", \"U_EXX_DESDIS\"\r\n\tFROM \"@EXX_DISTRI\"\r\n\tWHERE U_EXX_CODPRO = '{codProvincia}' ORDER BY U_EXX_DESDIS;";
            return xQuery;
        }
        public static string getMoneda()
        {
            string xQuery = $"SELECT \"CurrCode\" As \"Codigo\", \"CurrCode\" ||' - '|| \"CurrName\" As \"Descripcion\" FROM OCRN WHERE \"Locked\" = 'N'";
            return xQuery;
        }
        public static string getBanco()
        {
            string xQuery = $"SELECT \"BankCode\", \"BankName\" FROM ODSC";
            return xQuery;
        }
        public static string getTipoCuenta()
        {
            string xQuery = $"CALL \"EXX_WP_TipoCuenta_Listar\"";
            return xQuery;
        }
        public static string getConfiguracion()
        {
            string xQuery = $"CALL \"EXX_WP_Configuracion_Listar\"";
            return xQuery;
        }
        public static string getProveedorFactoring()
        {
            string xQuery = $"SELECT T0.\"CardCode\", T0.\"CardName\" FROM OCRD T0 WHERE T0.\"CardType\" = 'S' AND T0.\"CardCode\" LIKE 'P%' AND T0.\"U_EXC_PROVFACT\" = 'Y'";
            return xQuery;
        }
        public static string getSucursal()
        {
            string xQuery = $"SELECT \"BPLId\", \"BPLName\" FROM OBPL WHERE \"Disabled\" <> 'Y' ORDER BY \"BPLId\"";
            return xQuery;
        }

        //Busquedas
        public static string GetDescCondicionPago(int entry)
        {
            m_sSQL.Length = 0;
            m_sSQL.Append("SELECT \"PymntGroup\"  \"Value\" FROM OCTG ");
            m_sSQL.AppendFormat("WHERE \"GroupNum\"='{0}'", entry);
            return m_sSQL.ToString();
        }
        public static string GetNombreEmpleado(string entry)
        {
            m_sSQL.Length = 0;
            m_sSQL.Append("SELECT \"SlpName\"  \"Value\" FROM OSLP ");
            m_sSQL.AppendFormat("WHERE \"SlpCode\"='{0}'", entry);
            return m_sSQL.ToString();
        }
        public static string GetNombreAlmacen(string entry)
        {
            m_sSQL.Length = 0;
            m_sSQL.Append("SELECT \"WhsName\"  \"Value\" FROM OWHS ");
            m_sSQL.AppendFormat("WHERE \"WhsCode\"='{0}'", entry);
            return m_sSQL.ToString();
        }
        public static string ConvertAccount(string account, bool fromFormatCode)
        {
            m_sSQL.Length = 0;
            m_sSQL.AppendFormat("SELECT \"{0}\" \"Value\" ", fromFormatCode ? "AcctCode" : "FormatCode");
            m_sSQL.Append("FROM OACT ");
            m_sSQL.AppendFormat("WHERE \"{0}\" = '{1}'", !fromFormatCode ? "AcctCode" : "FormatCode", account);

            return m_sSQL.ToString();
        }
        public static string GetTypeItem(string code)
        {
            string xQuery = $"SELECT \"InvntItem\" FROM OITM WHERE \"ItemCode\" = '{code}'";
            return xQuery;
        }
        #endregion
        #endregion
    }
}

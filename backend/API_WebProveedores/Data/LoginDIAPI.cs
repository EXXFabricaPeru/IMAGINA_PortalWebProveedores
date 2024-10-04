using SAPbobsCOM;
using System;
using WebProov_API.Data.Interfaces;
using WebProov_API.Models;
using WebProov_API.Util;

namespace WebProov_API.Data
{
    public class LoginDIAPI : ILoginRepository
    {
        private Company _company;
        Documents oPor, oOrd;
        public LoginDIAPI()
        {
            _company = DIAPIConexion.GetDIAPIConexion();
        }

        public User GetLogin(User user)
        {
            user.Name = "";
            Recordset oRS = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
            Recordset oRSActivo = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRS.DoQuery(Queries.ValidarUser(user.Mail,user.Password));
            if (oRS.RecordCount == 0)
            {
                oRS.DoQuery(Queries.ValidarUserTra(user.Mail, user.Password));
                if(oRS.RecordCount == 0)
                {
                    user.Flag = false;
                    user.Name = "Usuario y Clave incorrectos.";
                }
                else
                {
                    //Se Valida que este activo
                    oRSActivo.DoQuery(Queries.ValidarUserTraActivo(user.Mail));
                    if (oRSActivo.Fields.Item(0).Value == 0)
                    {
                        user.Flag = false;
                        user.Name = "Usuario inactivo.";
                    }
                }
            }
            else
            {
                //Se Valida que este activo
                oRSActivo.DoQuery(Queries.ValidarUserActivo(user.Mail));
                if(oRSActivo.Fields.Item(0).Value == 0)
                {
                    user.Flag = false;
                    user.Name = "El usuario esta inactivo.";
                }

                //Se Valida que este activo
                oRSActivo.DoQuery(Queries.ValidarUserVigencia(user.Mail));
                if (oRSActivo.Fields.Item(0).Value == 0)
                {
                    user.Flag = false;
                    user.Name = "La fecha de vigencia ha caducado, coordine con su contacto de Imagina.";
                }
            }

            if(user.Name == "")
            {
                user.Flag = true;
                user.Code= oRS.Fields.Item("CardCode").Value;
                user.Name= oRS.Fields.Item("CardName").Value;
                user.Ruc= oRS.Fields.Item("LicTradNum").Value;
                user.CreditLine= oRS.Fields.Item("CreditLine").Value.ToString();
            }

            return user;
        }

        public string ChangePassword(User user)
        {
            BusinessPartners oBp;
            oBp = _company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
            if (!oBp.GetByKey(user.Code))
                return "Error: El socio con código: " + user.Code + " no existe en la sociedad.";

            if (oBp.UserFields.Fields.Item("U_EXX_TOKEN").Value != user.Ruc)
                return "El código de validación es incorrecto";

            oBp.Password = user.Password;
            oBp.UserFields.Fields.Item("U_EXX_TOKEN").Value = "OK";
            if (oBp.Update() != 0)
                return _company.GetLastErrorDescription();

            return "Se actualizó con éxito la contraseña";
        }

        public string envioReestablecerPass(User user)
        {
            string xRpta = ""; // new EnvioCorreo();
            Recordset oRs = _company.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                oRs.DoQuery(Queries.GetBPbyCardCode(user.Mail));
                if (oRs.RecordCount == 0)
                    return xRpta = $"No se encontro un proveedor con el correo {user.Mail}";

                string xCorreo = oRs.Fields.Item("EmailAddress").Value;
                string xCodigo = oRs.Fields.Item("CardCode").Value;

                string xQuery = $"UPDATE OCRD SET U_EXX_TOKEN = '{user.Code}' WHERE \"CardCode\" = '{xCodigo}' ";
                oRs.DoQuery(xQuery);

                xRpta = new EnvioCorreo().fn_EnvioCorreoReestablecerPass(xCorreo, user.Code, xCodigo);
            }
            catch (Exception ex)
            { 
                xRpta = ex.Message;
            }
            return xRpta;
        }
    }
}
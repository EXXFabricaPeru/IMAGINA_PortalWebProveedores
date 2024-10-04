using System;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Security.Policy;

namespace WebProov_API.Data
{
    public class EnvioCorreo
    {
        /// <summary>
        /// Envio de correo para aprovacion de conformidad
        /// </summary>
        /// <param name="correo">Correo Proveedor</param>
        /// <param name="nro">Nro de borrador</param>
        /// <param name="tipo"></param>
        /// <param name="sucursal">sede de la OC</param>
        /// <param name="proveedor">Razon social proveedor</param>
        /// <returns></returns>
        public string fn_EnvioCorreo(string correo, string nro, string tipo, string sucursal = "", string proveedor = "")
        {
            string xRpta = string.Empty;
            try
            {
                string dominio = Startup.StaConfig["DIAPI:Dominio"];
                string puerto = Startup.StaConfig["DIAPI:Puerto"];
                string correoEnvio = Startup.StaConfig["DIAPI:Email"];
                string password = Startup.StaConfig["DIAPI:PasswordEmail"];
                string UrlWP = Startup.StaConfig["DIAPI:UrlWP"];

                string _tipo = string.Empty;
                switch (tipo)
                {
                    case "CS":
                        _tipo += "Conformidad de Servicio";
                        break;
                    case "FP":
                        _tipo += "Factura";
                        break;
                }

                string asunto = $"APROBACION DE CONFORMIDAD DE SERVICO NRO {nro} - {sucursal} - {proveedor}";
                string _url = $"{UrlWP}aprobacion/{nro}/{correo}";
                string mensaje = $"<p>Para aprobar la {_tipo} hacer clic <a href='{_url}' target='_blank'>AQUI</a> ";

                var networkCredential = new NetworkCredential
                {
                    Password = password,
                    UserName = correoEnvio
                };

                var mailMsg = new MailMessage
                {
                    Body = mensaje,
                    Subject = asunto,
                    IsBodyHtml = true // This indicates that message body contains the HTML part as well.
                };
                mailMsg.To.Add(correo);
                mailMsg.From = new MailAddress(correoEnvio, "Imagina SAP");

                var smtpClient = new SmtpClient(dominio)
                {
                    Port = Convert.ToInt32(puerto),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = networkCredential
                };
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                xRpta = ex.Message;
            }
            return xRpta;
        }

        /// <summary>
        /// Envio de correo rechazo de la conformidaad de servicio
        /// </summary>
        /// <param name="correo">Correo destino</param>
        /// <param name="motivo">Motivo de rechazo</param>
        /// <param name="nro">Nro de la conformidad de servicio</param>
        /// <returns></returns>
        public string fn_EnvioCorreoRechazo(string correo, string motivo, string nro)
        {
            string xRpta = string.Empty;
            try
            {
                string dominio = Startup.StaConfig["DIAPI:Dominio"];
                string puerto = Startup.StaConfig["DIAPI:Puerto"];
                string correoEnvio = Startup.StaConfig["DIAPI:Email"];
                string password = Startup.StaConfig["DIAPI:PasswordEmail"];
                string UrlWP = Startup.StaConfig["DIAPI:UrlWP"];

                string asunto = $"Rechazo de Confirmidad de Servicio Nro {nro}";
                string mensaje = $"<p>Se rechazó la conformidad de servicio por este motivo: ";
                mensaje += $"<br> ";
                mensaje += $"{motivo} </p> " +
                    $"<br><a href='{UrlWP}' target='_blank'>Ir al Portal de Proveedores Imagina</a>";

                var networkCredential = new NetworkCredential
                {
                    Password = password,
                    UserName = correoEnvio
                };

                var mailMsg = new MailMessage
                {
                    Body = mensaje,
                    Subject = asunto,
                    IsBodyHtml = true // This indicates that message body contains the HTML part as well.
                };
                mailMsg.To.Add(correo);
                mailMsg.From = new MailAddress(correoEnvio, "Imagina SAP");

                var smtpClient = new SmtpClient(dominio)
                {
                    Port = Convert.ToInt32(puerto),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = networkCredential
                };
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                xRpta = ex.Message;
            }
            return xRpta;
        }

        /// <summary>
        /// Envio de correo de Aporobacion de conformidad de servicio
        /// </summary>
        /// <param name="correo">Correo destino</param>
        /// <param name="nroBorrador">Nro Borrador que fue aprobado</param>
        /// <param name="nro">Nuevo numero que se genero</param>
        /// <returns></returns>
        public string fn_EnvioCorreoAcptacionCS(string correo, string nroBorrador, string nro)
        {
            string xRpta = string.Empty;
            try
            {
                string dominio = Startup.StaConfig["DIAPI:Dominio"];
                string puerto = Startup.StaConfig["DIAPI:Puerto"];
                string correoEnvio = Startup.StaConfig["DIAPI:Email"];
                string password = Startup.StaConfig["DIAPI:PasswordEmail"];
                string UrlWP = Startup.StaConfig["DIAPI:UrlWP"];

                string asunto = $"Aceptación de Conformidad de Servicio Nro {nroBorrador}";
                string mensaje = $"<p>Se aprobó la conformidad de servicio con Nro preliminar {nroBorrador} " +
                    $"y se genero la conformidad de servicio con N° {nro} " +
                    $"<br>Puede proceder a registrar su factura en la web de proveedores." +
                    $"<br><a href='{UrlWP}' target='_blank'>Ir al Portal de Proveedores Imagina</a>";

                var networkCredential = new NetworkCredential
                {
                    Password = password,
                    UserName = correoEnvio
                };

                var mailMsg = new MailMessage
                {
                    Body = mensaje,
                    Subject = asunto,
                    IsBodyHtml = true // This indicates that message body contains the HTML part as well.
                };
                mailMsg.To.Add(correo);
                mailMsg.From = new MailAddress(correoEnvio, "Imagina SAP");

                var smtpClient = new SmtpClient(dominio)
                {
                    Port = Convert.ToInt32(puerto),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = networkCredential
                };
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                xRpta = ex.Message;
            }
            return xRpta;
        }

        public string fn_EnvioCorreoAprobacionProveedor(List<string> correos, string nombre, string ruc)
        {
            string xRpta = string.Empty;
            try
            {
                string dominio = Startup.StaConfig["DIAPI:Dominio"];
                string puerto = Startup.StaConfig["DIAPI:Puerto"];
                string correoEnvio = Startup.StaConfig["DIAPI:Email"];
                string password = Startup.StaConfig["DIAPI:PasswordEmail"];
                string UrlWP = Startup.StaConfig["DIAPI:UrlWP"];

                string asunto = $"Aprobacion del proveedor { ruc } - { nombre }";
                string mensaje = $"<p>Se requiere la aprobación del proveedor { ruc } - { nombre } en el portal de proveedores</p> " +
                    $"<br><a href='{UrlWP}' target='_blank'>Ir al Portal de Proveedores Imagina</a>";

                var networkCredential = new NetworkCredential
                {
                    Password = password,
                    UserName = correoEnvio
                };

                var mailMsg = new MailMessage
                {
                    Body = mensaje,
                    Subject = asunto,
                    IsBodyHtml = true
                };

                foreach (string correo in correos)
                    mailMsg.To.Add(correo);

                mailMsg.From = new MailAddress(correoEnvio, "Imagina SAP");

                var smtpClient = new SmtpClient(dominio)
                {
                    Port = Convert.ToInt32(puerto),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = networkCredential
                };
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                xRpta = ex.Message;
            }
            return xRpta;
        }
        
        public string fn_EnvioCorreoRegistroProveedor(string correo, string nombre)
        {
            string xRpta = string.Empty;
            try
            {
                string dominio = Startup.StaConfig["DIAPI:Dominio"];
                string puerto = Startup.StaConfig["DIAPI:Puerto"];
                string correoEnvio = Startup.StaConfig["DIAPI:Email"];
                string password = Startup.StaConfig["DIAPI:PasswordEmail"];
                string UrlWP = Startup.StaConfig["DIAPI:UrlWP"];

                string asunto = $"Estimado { nombre }";
                string mensaje = $"<p>Su solicitud de creación ha sido registrada con éxito, se le confirmará el alta.</p> " +
                    $"<br><a href='{UrlWP}' target='_blank'>Ir al Portal de Proveedores Imagina</a>";

                var networkCredential = new NetworkCredential
                {
                    Password = password,
                    UserName = correoEnvio
                };

                var mailMsg = new MailMessage
                {
                    Body = mensaje,
                    Subject = asunto,
                    IsBodyHtml = true
                };

                mailMsg.To.Add(correo);
                mailMsg.From = new MailAddress(correoEnvio, "Imagina SAP");

                var smtpClient = new SmtpClient(dominio)
                {
                    Port = Convert.ToInt32(puerto),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = networkCredential
                };
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                xRpta = ex.Message;
            }
            return xRpta;
        }
        
        public string fn_EnvioCorreoCreacionProveedor(string correo, string nombre)
        {
            string xRpta = string.Empty;
            try
            {
                string dominio = Startup.StaConfig["DIAPI:Dominio"];
                string puerto = Startup.StaConfig["DIAPI:Puerto"];
                string correoEnvio = Startup.StaConfig["DIAPI:Email"];
                string password = Startup.StaConfig["DIAPI:PasswordEmail"];
                string UrlWP = Startup.StaConfig["DIAPI:UrlWP"];

                string asunto = $"Estimado { nombre }";
                string mensaje = $"<p>Se aprobó la creación de su usuario en el portal de proveedores</p> " +
                    $"<br><a href='{UrlWP}' target='_blank'>Ir al Portal de Proveedores Imagina</a>";

                var networkCredential = new NetworkCredential
                {
                    Password = password,
                    UserName = correoEnvio
                };

                var mailMsg = new MailMessage
                {
                    Body = mensaje,
                    Subject = asunto,
                    IsBodyHtml = true
                };

                mailMsg.To.Add(correo);
                mailMsg.From = new MailAddress(correoEnvio, "Imagina SAP");

                var smtpClient = new SmtpClient(dominio)
                {
                    Port = Convert.ToInt32(puerto),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = networkCredential
                };
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                xRpta = ex.Message;
            }
            return xRpta;
        }
        
        public string fn_EnvioCorreoRechazoProveedor(string correo, string nombre, string comentario)
        {
            string xRpta = string.Empty;
            try
            {
                string dominio = Startup.StaConfig["DIAPI:Dominio"];
                string puerto = Startup.StaConfig["DIAPI:Puerto"];
                string correoEnvio = Startup.StaConfig["DIAPI:Email"];
                string password = Startup.StaConfig["DIAPI:PasswordEmail"];
                string UrlWP = Startup.StaConfig["DIAPI:UrlWP"];

                string asunto = $"Estimado de { nombre }";
                string mensaje = $"<p>Se rechazó la creación de su usuario en el portal de proveedores por el siguiente motivo:</p> <br>";
                mensaje += $"<p><strong>{comentario}</strong></p>" +
                    $"<br><a href='{UrlWP}' target='_blank'>Ir al Portal de Proveedores Imagina</a>";

                var networkCredential = new NetworkCredential
                {
                    Password = password,
                    UserName = correoEnvio
                };

                var mailMsg = new MailMessage
                {
                    Body = mensaje,
                    Subject = asunto,
                    IsBodyHtml = true
                };

                mailMsg.To.Add(correo);
                mailMsg.From = new MailAddress(correoEnvio, "Imagina SAP");

                var smtpClient = new SmtpClient(dominio)
                {
                    Port = Convert.ToInt32(puerto),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = networkCredential
                };
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                xRpta = ex.Message;
            }
            return xRpta;
        }

        public string fn_EnvioCorreoReestablecerPass(string correo, string token, string codigo)
        {
            string xRpta = string.Empty;
            try
            {
                string dominio = Startup.StaConfig["DIAPI:Dominio"];
                string puerto = Startup.StaConfig["DIAPI:Puerto"];
                string correoEnvio = Startup.StaConfig["DIAPI:Email"];
                string password = Startup.StaConfig["DIAPI:PasswordEmail"];
                string UrlWP = Startup.StaConfig["DIAPI:UrlWP"];
                string _url = $"{UrlWP}/password-lost/{codigo}";

                string asunto = $"Reestablecer contraseña";
                string mensaje = $"<p>Para reestablecer su contraseña haga clic <a href='{_url}' target='_blank'>AQUI</a> </p> ";
                mensaje += $"<p>Su codigo de validacion es {token}</p> ";

                var networkCredential = new NetworkCredential
                {
                    Password = password,
                    UserName = correoEnvio
                };

                var mailMsg = new MailMessage
                {
                    Body = mensaje,
                    Subject = asunto,
                    IsBodyHtml = true
                };

                mailMsg.To.Add(correo);
                mailMsg.From = new MailAddress(correoEnvio, "Imagina SAP");

                var smtpClient = new SmtpClient(dominio)
                {
                    Port = Convert.ToInt32(puerto),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = networkCredential
                };
                smtpClient.Send(mailMsg);

                xRpta = $"Se envió con éxito el correo de restablecimiento de contraseña al correo: {correo}";
            }
            catch (Exception ex)
            {
                xRpta = ex.Message;
            }
            return xRpta;
        }
    }
}

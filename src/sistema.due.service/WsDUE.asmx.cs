using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Xml.Serialization;

namespace Sistema.DUE.Service
{
    /// <summary>
    /// Summary description for WsDUE
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WsDUE : System.Web.Services.WebService
    {
        [WebMethod]
        public List<string> ListarCertificados()
        {
            var certificados = SisComex.ListarCertificadosInstalados(StoreLocation.LocalMachine);

            if (certificados.Count() == 0)
            {
                certificados = SisComex.ListarCertificadosInstalados(StoreLocation.CurrentUser);
            }

            return certificados
                .Select(c => c.SubjectName.Name)
                .ToList();
        }

        [WebMethod]
        public DueDadosResumidos ObterDadosResumidos(string due, string cpfCertificado)
        {
            try
            {
                return SisComex.ConsultarDadosResumidos(due, cpfCertificado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        [WebMethod]
        public DueDadosCompletos ObterDadosCompletos(string due, string cpfCertificado)
        {
            try
            {
                return SisComex.ConsultarDadosCompletos(due, cpfCertificado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        [WebMethod]
        public DadosNotaPreACD ObterDadosNotaPreACD(string chave, string cpfCertificado)
        {
            try
            {
                return SisComex.ConsultarDadosNotaPreACD(chave, cpfCertificado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        [WebMethod]
        public RetornoSiscomex EnviarDUESemNF(string xml, string cpfCertificado)
        {
            try
            {
                var response = SisComex.EnviarDUESemNF(xml, cpfCertificado);

                var retornoSiscomex = new RetornoSiscomex();

                var retornoResponse = response.Content.ReadAsStringAsync().Result;

                using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(Regex.Unescape(retornoResponse))))
                {
                    var reader = new StreamReader(memoryStream, Encoding.UTF8);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var xmlSerializer = new XmlSerializer(typeof(RetornoCriticas));
                        var criticas = (RetornoCriticas)xmlSerializer.Deserialize(reader);

                        retornoSiscomex.Sucesso = false;
                        retornoSiscomex.Message = criticas.Message;
                        retornoSiscomex.Criticas = criticas.Detail?.Errors;
                        retornoSiscomex.XmlRetorno = retornoResponse;
                    }
                    else
                    {
                        var xmlSerializer = new XmlSerializer(typeof(RetornoSucesso));
                        var sucesso = (RetornoSucesso)xmlSerializer.Deserialize(reader);

                        retornoSiscomex.Sucesso = true;
                        retornoSiscomex.Message = sucesso.Message;
                        retornoSiscomex.DUE = sucesso.Due;
                        retornoSiscomex.RUC = sucesso.Ruc;
                        retornoSiscomex.ChaveDeAcesso = sucesso.ChaveDeAcesso;
                        retornoSiscomex.XmlRetorno = retornoResponse;
                    }
                }

                return retornoSiscomex;
            }
            catch (Exception ex)
            {
                return new RetornoSiscomex
                {
                    Sucesso = false,
                    Message = ex.Message
                };
            }
        }

        [WebMethod]
        public RetornoSiscomex EnviarRetificacao(string xml, string due, string cpfCertificado)
        {
            try
            {
                var response = SisComex.EnviarRetificacao(xml, due, cpfCertificado);

                var retornoSiscomex = new RetornoSiscomex();

                var retornoResponse = response.Content.ReadAsStringAsync().Result;

                using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(Regex.Unescape(retornoResponse))))
                {
                    var reader = new StreamReader(memoryStream, Encoding.UTF8);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        try
                        {
                            var xmlSerializer = new XmlSerializer(typeof(RetornoCriticas));
                            var criticas = (RetornoCriticas)xmlSerializer.Deserialize(reader);

                            retornoSiscomex.Sucesso = false;
                            retornoSiscomex.Message = criticas.Message;
                            retornoSiscomex.Criticas = criticas.Detail?.Errors;
                        }
                        catch (Exception ex)
                        {
                            retornoSiscomex.Sucesso = false;
                            retornoSiscomex.Message = "Erro 500. XML recusado pelo Serpro";
                        }                        
                    }
                    else
                    {
                        var xmlSerializer = new XmlSerializer(typeof(RetornoSucesso));
                        var sucesso = (RetornoSucesso)xmlSerializer.Deserialize(reader);

                        retornoSiscomex.Sucesso = true;
                        retornoSiscomex.Message = sucesso.Message;
                        retornoSiscomex.DUE = sucesso.Due;
                        retornoSiscomex.RUC = sucesso.Ruc;
                    }
                }

                return retornoSiscomex;
            }
            catch (Exception ex)
            {
                return new RetornoSiscomex
                {
                    Sucesso = false,
                    Message = ex.Message
                };
            }
        }
    }
}

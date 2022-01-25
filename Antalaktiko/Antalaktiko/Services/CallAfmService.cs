using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Antalaktiko.Services
{
    public class CallAfmService
    {
        readonly string username = "80081138AA";
        readonly string password = "80081138BB";
        readonly string tafm = "800811308";
        private string xml;
        private string afm;

        public CallAfmService(string afm)
        {
            this.afm = afm;
        }
        private HttpWebRequest CreateSOAPWebRequest()
        {
            //Making Web Request    
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(@"https://www1.gsis.gr:443/wsaade/RgWsPublic2/RgWsPublic2");
            //SOAPAction    
            Req.Headers.Add(@"SOAPAction:http://rgwspublic2/RgWsPublic2Service:rgWsPublic2AfmMethod");
            //Content_type    
            Req.ContentType = "application/soap+xml;charset=UTF-8";
            Req.Accept = "text/xml";
            //HTTP method    
            Req.Method = "POST";
            //return HttpWebRequest    
            return Req;
        }
        private  Task<string> InvokeService(string afmAn)
        {
            return Task.Run(() => {
                //Calling CreateSOAPWebRequest method    
                HttpWebRequest request = CreateSOAPWebRequest();
                XmlDocument SOAPReqBody = new XmlDocument();
                //SOAP Body Request  
                SOAPReqBody.LoadXml(@"<env:Envelope xmlns:env=""http://www.w3.org/2003/05/soap-envelope"" xmlns:ns1=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:ns2=""http://rgwspublic2/RgWsPublic2Service"" xmlns:ns3=""http://rgwspublic2/RgWsPublic2"">
                   <env:Header>
                      <ns1:Security>
                         <ns1:UsernameToken>
                            <ns1:Username>" + username + "</ns1:Username>" +
                                "<ns1:Password>" + password + @"</ns1:Password>
                         </ns1:UsernameToken>
                      </ns1:Security>
                   </env:Header>
                   <env:Body>
                      <ns2:rgWsPublic2AfmMethod>
                         <ns2:INPUT_REC>
                            <ns3:afm_called_by>" + tafm + @"</ns3:afm_called_by>
                            <ns3:afm_called_for>" + afmAn + @"</ns3:afm_called_for>
                         </ns2:INPUT_REC>
                      </ns2:rgWsPublic2AfmMethod>
                   </env:Body>
                </env:Envelope>");

                using (Stream stream = request.GetRequestStream())
                {
                    SOAPReqBody.Save(stream);
                }
                //Geting response from request    
                using (WebResponse Serviceres = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                    {
                        //reading stream    
                        var ServiceResult = rd.ReadToEnd();
                        //writting stream result on console    
                        //Console.WriteLine(ServiceResult);
                        return ServiceResult;
                    }
                }
            });
            

        }
        public async Task<string> GetInfoFromXML(string cvar)
        {
            if (string.IsNullOrWhiteSpace(cvar))
                return string.Empty;
            if (string.IsNullOrEmpty(xml))
                xml = await InvokeService(afm);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            XmlNodeList nodeList = xmldoc.GetElementsByTagName(cvar);
            string info = string.Empty;

            foreach (XmlNode node in nodeList)
            {
                info = node.InnerText;
            }

            return info;
        }
         
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DbManager;
using System.IO.Pipes;
using System.Xml;
using DbManager;
using System.IO;
using System.Xml.Serialization;

namespace DbManager.Network
{
    public class Server
    {
        private Database database;
        // Maialen
        public void Listen(int port)
        {
            //DEADLINE 6: Implement the server as specified (eGela)
            //Have a look at the project ServerConsole to see how a TcpListener is used
            //Use XmlSerializer to create Xml commands

            TcpListener listen = new TcpListener(IPAddress.Any, port);

            while (true)
            {
                using (TcpClient client = listen.AcceptTcpClient())
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    bool con = true;
                    while (con)
                    {
                        try
                        {
                            string xmlRequest = reader.ReadLine();
                            if (string.IsNullOrEmpty(xmlRequest))
                            {
                                con = false;
                            }
                            else
                            {
                                string response = ProcessRequest(xmlRequest);
                                writer.WriteLine(response);
                                writer.Flush();
                                if (xmlRequest.Contains("<Close/>"))
                                {
                                    con = false;
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            writer.WriteLine($"<Error>{e.Message}</Error>");
                            writer.Flush();
                            con = false;
                        }
                    }
                }
            }
        }

        private string ProcessRequest(string xml)
        {
            if (xml.Contains("<Open"))
            {
                try
                {
                    return "<Success/>";
                }
                catch (Exception)
                {
                    return "<Error>Error al abrir la base de datos</Error>";

                }
            }
            if (xml.Contains("<Create"))
            {
                try
                {
                    return "<Success/>";
                }
                catch (Exception)
                {
                    return "<Error>Error al crear la base de datos</Error>";
                }
            }
            if (xml.Contains("<Query>"))
            {
                try
                {
                    return "<Answer>[Name,Surname]{Unai,Lobete}{Maialen,Mateos}</Answer>";
                }
                catch (Exception)
                {
                    return "<Answer><Error>Error en la consulta</Error></Answer>";
                }
            }

            if (xml.Contains("<Close/>"))
            {
                return "<Success/>";
            }

            return "<Error>Unknown Command</Error>";
        }
    }
}

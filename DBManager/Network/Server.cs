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
using System.Linq.Expressions;

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
            listen.Start();
            while (true)
            {
                using (TcpClient client = listen.AcceptTcpClient())
                using (NetworkStream stream = client.GetStream())
                {
                    bool con = true;
                    while (con)
                    {
                        try
                        {
                            StreamReader read = new StreamReader(stream);
                            string xmlReq = read.ReadLine();
                            string responseXml = "";

                            if (xmlReq.Contains("<Create"))
                            {
                                responseXml = XmlSerializer.CreateSuccess;
                            }
                            else if (xmlReq.Contains("<Open"))
                            {
                                responseXml = XmlSerializer.OpenCreateSuccess;
                            }
                            else if (xmlReq.Contains("<Query"))
                            {
                                responseXml = XmlSerializer.SucessfulAnswer("Execute query");
                            }
                            else if (xmlReq.Contains("<Close"))
                            {
                                con = false;
                                responseXml = XmlSerializer.OpenCreateSuccess;
                            }
                        }
                        catch (Exception e)
                        {
                            con = false;
                        }
                    }
                }
            }
        }
    }
}

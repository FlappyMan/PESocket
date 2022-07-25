using PENet;
using System;
using Protocol;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Xml;

namespace ConsoleServer {
    class ServerStart {
        static void Main(string[] args) {
            PESocket<ServerSession, NetMsg> server = new PESocket<ServerSession, NetMsg>();
            server.StartAsServer(IPCfg.srvIP, IPCfg.srvPort);

            Console.WriteLine("\nInput 'quit' to stop server!");

            ConnectMySql();

            while (true) {
                string ipt = Console.ReadLine();
                if (ipt == "quit") {
                    server.Close();
                    break;
                }
                if (ipt == "all") {
                    List<ServerSession> sessionLst = server.GetSesstionLst();
                    for (int i = 0; i < sessionLst.Count; i++) {
                        sessionLst[i].SendMsg(new NetMsg {
                            text = "broadcast from server."
                        });
                    }
                }
            }
        }

        private static void ConnectMySql()
        {
            string dataBase = SelectValueByKey("DataBase");
            string ip = SelectValueByKey("ServerIP");
            string port = SelectValueByKey("port"); 
            string username = SelectValueByKey("username");
            string password = SelectValueByKey("password");

            System.Console.WriteLine(dataBase);
        }
        private static string SelectValueByKey(string strKey)
        {
            string result = "";
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径
            string strFileName = "Server.config";//位于根目录下
            doc.Load(strFileName);
            //找出名称为“add”的所有元素
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性
                XmlAttribute att = nodes[i].Attributes["key"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素
                if (att.Value == strKey)
                {
                    //对目标元素中的第二个属性赋值
                    XmlAttribute zf = nodes[i].Attributes["value"];
                    result = zf.Value;
                    break;
                }
            }
            //返回Value的值
            return result.ToString();
        }
    }
}
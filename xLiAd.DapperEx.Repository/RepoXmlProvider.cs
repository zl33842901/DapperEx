using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xLiAd.DapperEx.Repository
{
    public class RepoXmlProvider
    {
        public RepoXmlProvider(string xmlPath)
        {
            if (string.IsNullOrEmpty(xmlPath))
            {
                throw new Exception("XML文档参数为空！");
            }
            try
            {
                Statu = RepoXmlStatu.Loading;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                XmlNode rootNode = xmlDoc.SelectSingleNode("DapperEx");
                foreach (XmlNode xxNode in rootNode.ChildNodes)
                {
                    string sql = xxNode.InnerText?.Trim();
                    string oprate = xxNode.Name;
                    string id = xxNode.Attributes["id"]?.Value ?? string.Empty;
                    RepoSqlOprate o;
                    var b = Enum.TryParse<RepoSqlOprate>(oprate, out o);
                    if(b && !string.IsNullOrWhiteSpace(sql) && !string.IsNullOrWhiteSpace(id))
                        dataList.Add(new XmlSqlModel(id, o, sql));
                }
                Statu = RepoXmlStatu.Loaded;
            }
            catch(Exception e)
            {
                Statu = RepoXmlStatu.LoadError;
                throw new Exception("载入XML文档时出错！", e);
            }
        }
        private List<XmlSqlModel> dataList = new List<XmlSqlModel>();
        public XmlSqlModel[] DataList => dataList.ToArray();
        public RepoXmlStatu Statu { get; private set; } = RepoXmlStatu.Waiting;
    }

    public enum RepoXmlStatu
    {
        Waiting = 0,
        Loading = 1,
        Loaded = 2,
        Unload = 3,
        LoadError = 4
    }
    public enum RepoSqlOprate
    {
        insert = 1,
        update = 2,
        delete = 3,
        select = 4
    }

    public class XmlSqlModel
    {
        public XmlSqlModel(string id, RepoSqlOprate oprate, string sql)
        {
            Id = id;
            Sql = sql;
            Oprate = oprate;
        }
        public string Id { get; private set; }
        public string Sql { get; private set; }
        public RepoSqlOprate Oprate { get; private set; }
    }
}

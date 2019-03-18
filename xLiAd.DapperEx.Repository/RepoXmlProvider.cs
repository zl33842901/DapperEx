using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xLiAd.DapperEx.Repository
{
    /// <summary>
    /// XML查询语句提供器
    /// </summary>
    public class RepoXmlProvider : IRepoXmlProvider
    {
        /// <summary>
        /// 初始化 XML查询语句提供器
        /// </summary>
        /// <param name="xmlPath"></param>
        public RepoXmlProvider(string xmlPath)
        {
            if (string.IsNullOrEmpty(xmlPath))
            {
                Statu = RepoXmlStatu.LoadError;
                //throw new Exception("XML文档参数为空！");
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
            catch(Exception)
            {
                Statu = RepoXmlStatu.LoadError;
                //throw new Exception("载入XML文档时出错！", e);
            }
        }
        private List<XmlSqlModel> dataList = new List<XmlSqlModel>();
        /// <summary>
        /// 一个提供器里的所有语句
        /// </summary>
        public XmlSqlModel[] DataList => dataList.ToArray();
        /// <summary>
        /// 状态
        /// </summary>
        public RepoXmlStatu Statu { get; private set; } = RepoXmlStatu.Waiting;
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IRepoXmlProvider
    {
        /// <summary>
        /// 一个提供器里的所有语句
        /// </summary>
        XmlSqlModel[] DataList { get; }
        /// <summary>
        /// 状态
        /// </summary>
        RepoXmlStatu Statu { get; }
    }
    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum RepoXmlStatu
    {
        /// <summary>
        /// 等待中
        /// </summary>
        Waiting = 0,
        /// <summary>
        /// 加载中
        /// </summary>
        Loading = 1,
        /// <summary>
        /// 加载完成
        /// </summary>
        Loaded = 2,
        /// <summary>
        /// 已卸载
        /// </summary>
        Unload = 3,
        /// <summary>
        /// 加载出错
        /// </summary>
        LoadError = 4
    }
    /// <summary>
    /// 操作枚举
    /// </summary>
    public enum RepoSqlOprate
    {
        /// <summary>
        /// 插入
        /// </summary>
        insert = 1,
        /// <summary>
        /// 更新
        /// </summary>
        update = 2,
        /// <summary>
        /// 删除
        /// </summary>
        delete = 3,
        /// <summary>
        /// 查询
        /// </summary>
        select = 4
    }
    /// <summary>
    /// 一条语句
    /// </summary>
    public class XmlSqlModel
    {
        /// <summary>
        /// 初始化一条语句
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oprate"></param>
        /// <param name="sql"></param>
        public XmlSqlModel(string id, RepoSqlOprate oprate, string sql)
        {
            Id = id;
            Sql = sql;
            Oprate = oprate;
        }
        /// <summary>
        /// 不可重复的Id
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// 语句
        /// </summary>
        public string Sql { get; private set; }
        /// <summary>
        /// 操作
        /// </summary>
        public RepoSqlOprate Oprate { get; private set; }
    }
}

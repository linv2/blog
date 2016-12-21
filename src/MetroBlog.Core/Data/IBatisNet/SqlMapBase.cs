using IBatisNet.DataMapper;
using System.Collections.Generic;
using IBatisNet.DataMapper.Configuration;
using System.Reflection;
using System.Xml;

namespace MetroBlog.Core.Data.IBatisNet
{
    public class SqlMapBase
    {
        protected ISqlMapper SqlMapper { set; get; }

        public SqlMapBase()
        {
            var builder = new DomSqlMapBuilder();

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MetroBlog.Core.Data.IBatisNet.sqlMap.config");
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);
            SqlMapper = builder.Build(xmlDoc, false);

        }

        protected TSource Insert<TSource>(string statementName, object value, ISqlMapper sqlMapper = null)
        {
            if (sqlMapper == null)
            {
                sqlMapper = SqlMapper;
            }
            return (TSource)sqlMapper.Insert(statementName, value);
        }
        protected int Update(string statementName, object value, ISqlMapper sqlMapper = null)
        {
            if (sqlMapper == null)
            {
                sqlMapper = SqlMapper;
            }
            return sqlMapper.Update(statementName, value);
        }
        protected int Delete(string statementName, object value, ISqlMapper sqlMapper = null)
        {
            if (sqlMapper == null)
            {
                sqlMapper = SqlMapper;
            }
            return sqlMapper.Delete(statementName, value);
        }
        protected TSource QueryForObject<TSource>(string statementName, object value, ISqlMapper sqlMapper = null)
        {
            if (sqlMapper == null)
            {
                sqlMapper = SqlMapper;
            }
            return sqlMapper.QueryForObject<TSource>(statementName, value);
        }
        protected IList<TSource> QueryForList<TSource>(string statementName, object value, ISqlMapper sqlMapper = null)
        {
            if (sqlMapper == null)
            {
                sqlMapper = SqlMapper;
            }
            return sqlMapper.QueryForList<TSource>(statementName, value);
        }
    }
}

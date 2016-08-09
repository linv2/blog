using IBatisNet.DataMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBatisNet.Common;
using IBatisNet.DataMapper.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;

namespace MetroBlog.Core.Data.IBatisNet
{
    public class SqlMapBase
    {
        protected ISqlMapper sqlMapper { set; get; }

        public SqlMapBase()
        {
            DomSqlMapBuilder builder = new DomSqlMapBuilder();

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MetroBlog.Core.Data.IBatisNet.sqlMap.config");
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);

            sqlMapper = builder.Build(xmlDoc, false);

        }

        protected TSource Insert<TSource>(string statementName, object value)
        {
            return (TSource)sqlMapper.Insert(statementName, value);
        }
        protected int Update(string statementName, object value)
        {
            return sqlMapper.Update(statementName, value);
        }
        protected int Delete(string statementName, object value)
        {
            return sqlMapper.Delete(statementName, value);
        }
        protected TSource QueryForObject<TSource>(string statementName, object value)
        {
            return sqlMapper.QueryForObject<TSource>(statementName, value);
        }
        protected IList<TSource> QueryForList<TSource>(string statementName, object value)
        {
            return sqlMapper.QueryForList<TSource>(statementName, value);
        }
    }
}

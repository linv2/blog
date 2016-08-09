using System;

namespace MetroBlog.Template
{

    /// <summary>
    /// 页面实体
    /// </summary>
    public class ViewParamEntity
    {
        public string Name { get; set; }
        public int GroupIndex { get; set; }
        public string Default { get; set; }
        public Type ParamType { get; set; }
    }
}

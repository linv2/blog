

using System;

namespace MetroBlog.Core.Model.ViewModel
{
    public class Setting : IDbModelface
    {
        public string Site { get; set; }
        public string Title { get; set; }
        public string KeyWord { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public bool Comment { get; set; }

        public string DesKey { get; set; }
        public string DesVi { get; set; }

        public string SmtplSrvStatus { get; set; }
        public string SmtpSrvAddr { get; set; }
        public string SmtpFormAddr { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassWord { get; set; }

        public string AdminPath { get; set; }

        public bool LocalFileUpload { get; set; } = true;
        public string UploadPath { get; set; } = "upload";
        public bool ThrowError { get; set; } = true;

        private string _host;
        public string Host
        {
            get
            {
                if (string.IsNullOrEmpty(_host))
                {
                    try
                    {

                        var url = new Uri(Site);
                        _host = url.Host;
                        if ((url.Scheme.ToLower() == "http" && url.Port != 80) || (url.Scheme.ToLower() == "https" && url.Port != 443))
                        {
                            _host += (":" + url.Port);
                        }
                    }
                    catch (Exception)
                    {
                        _host = "www.newguid.cn";
                    }
                }
                return _host;
            }
        }
    }
}

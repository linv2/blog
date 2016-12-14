using MetroBlog.Core.Common;
using MetroBlog.Core.Data.IService;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MetroBlog.Admin.Module
{
    public class ThemeModule : NancyModule
    {
        private readonly ISettingService _settingService;
        public ThemeModule(ISettingService settingService)
        {

            _settingService = settingService;
            ModulePath = "theme";
            Get["list"] = _ => List();
            Post["filelist"] = _ => FileList();
            Get["getfile"] = _ => GetFile();
            Post["savefile"] = _ => SaveFile();
            Get["compile"] = _ => CompileFile();
        }
        public dynamic List()
        {
            return Response.AsJson(Rsp.Create(Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes")).Select(x => new DirectoryInfo(x).Name)));
        }
        public dynamic FileList()
        {
            string theme = Request.Form.themeName;
            string dir = Request.Form.id;
            if (dir == "/")
            {
                dir = "";
            }
            if (!string.IsNullOrEmpty(dir))
            {
                if (!dir.StartsWith("/"))
                {
                    theme += ("\\" + dir);
                }
                else
                {
                    theme += dir;
                }
            }
            var list = new List<dynamic>();
            var directories = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", theme)).Select(x => new DirectoryInfo(x));
            foreach (var directory in directories)
            {
                list.Add(new
                {
                    id = dir + "/" + directory.Name,
                    name = directory.Name,
                    isParent = true
                });
            }
            var fileInfos = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", theme)).Select(x => new FileInfo(x));

            var setting = _settingService.GetSetting();
            var key = Convert.FromBase64String(setting.DesKey);
            var vi = Convert.FromBase64String(setting.DesVi);
            foreach (var fileInfo in fileInfos)
            {
                list.Add(new
                {
                    id = dir + "/" + fileInfo.Name,
                    name = fileInfo.Name,
                    isParent = false,
                    icon = "/public/assets/css/ztree/icons/18/" + fileInfo.Extension.Remove(0, 1) + ".png",
                    skey = DES.Encrypt(fileInfo.FullName, key, vi)
                });
            }
            return Response.AsJson(list);
        }
        public dynamic GetFile()
        {
            string skey = Request.Query.skey;
            var setting = _settingService.GetSetting();
            var key = Convert.FromBase64String(setting.DesKey);
            var vi = Convert.FromBase64String(setting.DesVi);
            try
            {
                string fileName = DES.Decrypt(skey, key, vi);
                if (fileName != null && File.Exists(fileName))
                {

                    return Response.AsJson(Rsp.Create<dynamic>(new
                    {
                        file = new FileInfo(fileName).Name,
                        content = File.ReadAllText(fileName, Encoding.UTF8)
                    }));
                }

                return Response.AsJson(Rsp.Error("文件不存在"));
            }
            catch (Exception ex)
            {


                return Response.AsJson(Rsp.Error(ex.Message));
            }
        }

        public dynamic SaveFile()
        {
            try
            {
                string skey = Request.Form.skey;
                var setting = _settingService.GetSetting();
                var key = Convert.FromBase64String(setting.DesKey);
                var vi = Convert.FromBase64String(setting.DesVi);
                var fileName = DES.Decrypt(skey, key, vi);
                var content = Request.Form.content;
                File.WriteAllText(fileName, content, Encoding.UTF8);
                return Response.AsJson(Rsp.Success);
            }
            catch (Exception ex)
            {
                return Response.AsJson(Rsp.Error(ex.Message));
            }
        }
        public dynamic CompileFile()
        {
            return Response.AsJson(Rsp.Success);
        }
    }
}

using MetroBlog.Core.Common;
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

        public ThemeModule()
        {
            base.ModulePath = "theme";
            Get["list"] = _ => List();
            Get["filelist"] = _ => FileList();
            Get["getfile"] = _ => GetFile();
            Post["savefile"] = _ => SaveFile();
            Get["compile"] = _ => CompileFile();
        }
        public dynamic List()
        {
            return Response.AsJson(Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes")).Select(x => new DirectoryInfo(x).Name));
        }
        public dynamic FileList()
        {
            string theme = Request.Query.themeName;
            string dir = Request.Query.dir;
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
            var rsp = new
            {
                directories = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", theme)).Select(x => new DirectoryInfo(x).Name),
                files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", theme)).Select(x => new FileInfo(x).Name)
            };
            return Response.AsJson(rsp);
        }
        public dynamic GetFile()
        {
            string theme = Request.Query.themeName;
            string fileName = Request.Query.fileName;
            theme = theme.Replace("/", "\\\\");
            fileName = fileName.Replace("/", "\\");
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", theme, fileName);
            if (!File.Exists(fileName))
            {
                return Response.AsJson(Rsp.Error("文件不存在"));
            }
            else
            {
                return Response.AsJson(Rsp.Error<string>(File.ReadAllText(fileName, Encoding.UTF8)));
            }
        }

        public dynamic SaveFile()
        {
            try
            {
                string theme = Request.Query.themeName;
                string fileName = Request.Query.fileName;
                string content = Request.Form.content;
                theme = theme.Replace("/", "\\\\");
                fileName = fileName.Replace("/", "\\");
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", theme, fileName);
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

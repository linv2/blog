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
            foreach (var fileInfo in fileInfos)
            {
                list.Add(new
                {
                    id = dir + "/" + fileInfo.Name,
                    name = fileInfo.Name,
                    isParent = false,
                    icon = "/public/assets/css/ztree/icons/18/" + fileInfo.Extension.Remove(0, 1) + ".png"
                });
            }
            ////[{ id:'011',	name:'n1.n1',	isParent:true},{ id:'012',	name:'n1.n2',	isParent:false},{ id:'013',	name:'n1.n3',	isParent:true},{ id:'014',	name:'n1.n4',	isParent:false}]

            //var rsp = new
            //{
            //    directories = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", theme)).Select(x => new DirectoryInfo(x).Name),
            //    files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", theme)).Select(x => new FileInfo(x).Name)
            //};
            return Response.AsJson(list);
        }
        public dynamic GetFile()
        {
            string theme = Request.Query.themeName;
            string fileName = Request.Query.fileName;
            if (fileName.StartsWith("/"))
            {
                fileName = fileName.Remove(0, 1);
            }
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

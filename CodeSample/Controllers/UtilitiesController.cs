using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CodeSample.Controllers
{
    public class UtilitiesController : Controller
    {
        public class VDOFileUploadStatus
        {
            public string Name { get; set; }
            public string Ext { get; set; }
            public string Status { get; set; }
            public string Msg { get; set; }
        }
        public class VDOFile
        {
            public string Name { get; set; }
            public string Ext { get; set; }
            public long Length { get; set; }
            public string Duration { get; set; }
        }

        private static readonly int ATTEMPTS_TO_WRITE = 3;
        private static readonly int ATTEMPT_WAIT = 100; //msec
        private static readonly int BUFFER_SIZE = 4 * 1024 * 1024;

        private static class HttpMethods
        {
            public static readonly string GET = "GET";
            public static readonly string POST = "POST";
            public static readonly string DELETE = "DELETE";
        }

        private void FromStreamToStream(Stream input, Stream output)
        {
            //  int BufferSize = input.Length >= BUFFER_SIZE ? BUFFER_SIZE : (int)input.Length;
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        } 
        
        public ActionResult GetFile()
        {
            HttpFileCollection uploadFiles = System.Web.HttpContext.Current.Request.Files;
            string result = string.Empty;
            bool allowMultiple = false;
            long fileLength = 609715200; //default 500+ MB
            //if (uploadFiles.Count > 0)
            //{
            VDOFileUploadStatus fileStatus = new VDOFileUploadStatus();
            string sPath = string.Empty;
            string sFileName = string.Empty;
            string sFileTypes = string.Empty;

            int iFileWidth = 0;
            int iFileHeight = 0;

            //long fileLength = 4194304; //default 4 MB
            if (Request["path"] != null && !string.IsNullOrEmpty(Request["path"]))
            {
                sPath += Request["path"];
            }
            else
            {
                sPath += "temp/";
            }

            if (Request["filename"] != null && !string.IsNullOrEmpty(Request["filename"]))
            {
                //try
                //{
                //    sFileName = Request["filename"].Split(',')[0];
                //}
                //catch
                //{
                sFileName = Request["filename"];
                //}
            }

            if (Request["filetypes"] != null && !string.IsNullOrEmpty(Request["filetypes"]))
            {
                sFileTypes += Request["filetypes"];
            }

            if (Request["fileLength"] != null && !string.IsNullOrEmpty(Request["fileLength"]))
            {
                try
                {
                    fileLength = Convert.ToInt64(Request["fileLength"]);
                }
                catch
                {
                    //fileLength = 4194304;//default 4 MB
                }
            }

            if (Request["filewidth"] != null && !string.IsNullOrEmpty(Request["filewidth"]))
            {
                try
                {
                    iFileWidth = Convert.ToInt32(Request["filewidth"].ToLower().Replace("px", "").Replace("%", "").Trim());
                }
                catch { }
            }

            if (Request["fileheight"] != null && !string.IsNullOrEmpty(Request["fileheight"]))
            {
                try
                {
                    iFileHeight = Convert.ToInt32(Request["fileheight"].ToLower().Replace("px", "").Replace("%", "").Trim());
                }
                catch { }
            }

            if (Request["allowMultiple"] != null)
            {
                allowMultiple = Convert.ToBoolean(Request["allowMultiple"]);

            }

            if (Request["resume"] != null && !string.IsNullOrEmpty(Request["resume"]))
            {
                long size = 0;
                string file = Server.MapPath("~/" + sPath) + sFileName;
                if (System.IO.File.Exists(file))
                {
                    string[] files = Directory.GetFiles(Server.MapPath("~/" + sPath), Path.GetFileNameWithoutExtension(file) + "*" + Path.GetExtension(file));
                    foreach (string f in files)
                    {
                        size += new FileInfo(f).Length;
                    }

                    fileStatus.Status = "-101";
                    fileStatus.Msg = size.ToString();
                }
                else
                {
                    fileStatus.Status = "200";
                    fileStatus.Msg = size.ToString();
                }
            }
            else
            {
                HttpPostedFile postedFile = uploadFiles[0];
                try
                {
                    if (allowMultiple == false)
                    {
                        if (Directory.Exists(Server.MapPath("~/" + sPath)))
                        {
                            try
                            {
                                System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/" + sPath));
                                var fileName = postedFile.FileName;
                                foreach (FileInfo file2 in di.GetFiles().Where(x => (x.Name + "").ToUpper() != (fileName + "").ToUpper()))
                                {
                                    file2.Delete();
                                }
                            }
                            catch { }
                        }
                    }
                    if (!Directory.Exists(Server.MapPath("~/" + sPath)))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/" + sPath));
                    }

                    string file = Server.MapPath("~/" + sPath) + postedFile.FileName;
                    for (int Attempts = 0; Attempts < ATTEMPTS_TO_WRITE; Attempts++)
                    {
                        if (!string.IsNullOrEmpty(sFileName) && Path.GetFileNameWithoutExtension(postedFile.FileName).ToLower() != sFileName.ToLower())
                        {
                            fileStatus.Status = "-500";
                        }
                        else if (!string.IsNullOrEmpty(sFileTypes) && !sFileTypes.ToLower().Contains(Path.GetExtension(postedFile.FileName).ToLower()))
                        {
                            fileStatus.Status = "-200";
                        }
                        else if (fileLength > 0 && postedFile.ContentLength > fileLength)
                        {
                            fileStatus.Status = "-300";
                        }
                        else
                        {
                            try
                            {
                                using (Stream FileStreamWriter = new FileStream(file, FileMode.Append, FileAccess.Write))
                                {
                                    FromStreamToStream(postedFile.InputStream, FileStreamWriter);
                                    fileStatus.Status = "200";
                                    fileStatus.Name = new FileInfo(file).Name;
                                    fileStatus.Ext = Path.GetExtension(postedFile.FileName).ToLower();
                                }
                            }
                            catch// (Exception exception)
                            {
                                System.Threading.Thread.Sleep(ATTEMPT_WAIT);
                                continue;
                            }
                        }
                        //  }


                        break;
                    }
                }
                catch (Exception ex)
                {
                    fileStatus.Status = "-100";
                    fileStatus.Msg = "Error: " + ex.ToString();
                }
            }

            result = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(fileStatus);

            return Content(result);
        }
        public ActionResult GetFileList()
        {
            string result = string.Empty;
            string sAction = string.Empty;
            string sPath = "";// Server.MapPath("~/");
            bool allowMultiple = false;
            string sFileTypes = string.Empty;

            if (Request["path"] != null && !string.IsNullOrEmpty(Request["path"]))
            {
                sPath += Server.UrlDecode(Request["path"]);
                //sPath = Request.QueryString["path"];
            }
            else
            {
                sPath += "temp/";
            }

            if (Request["action"] != null)
            {
                sAction = Request["action"];
            }

            if (Request["allowMultiple"] != null)
            {
                allowMultiple = Convert.ToBoolean(Request["allowMultiple"]);
            }

            sPath = Server.MapPath("~/" + sPath);
            if (Request["filetypes"] != null && !string.IsNullOrEmpty(Request["filetypes"]))
            {
                sFileTypes += Request["filetypes"];
            }

            //result = sAction;
            if (sAction.Trim() == "delete")
            {
                FileInfo fi = new FileInfo(sPath + (string.IsNullOrEmpty(Request["file"]) ? "" : Request["file"]));
                if (fi.Exists)
                    fi.Delete();

                result = "100";
            }
            else
            {
                List<VDOFile> files = new List<VDOFile>();
                try
                {
                    DirectoryInfo di = new DirectoryInfo(sPath);
                    if (!di.Exists)
                        di.Create();

                    if (allowMultiple || !System.IO.File.Exists(sPath))
                    {
                        DirectoryInfo diFiles = new DirectoryInfo(sPath);

                        var filesList = (from f in diFiles.GetFiles()
                                         orderby f.LastWriteTime descending
                                         select new VDOFile
                                         {
                                             Name = f.Name,
                                             Ext = f.Extension,
                                             Length = f.Length,
                                         })
                                         .AsEnumerable()
                                         .Select(x => new VDOFile
                                         {
                                             Name = x.Name,
                                             Ext = x.Ext,
                                             Length = x.Length,
                                         });

                        int length = !allowMultiple && filesList.Count() > 0 ? 1 : filesList.Count();
                        for (int index = 0; index < length; index++)
                        {
                            if (string.IsNullOrEmpty(sFileTypes) || sFileTypes.ToLower().Contains(filesList.ElementAt(index).Ext.ToLower()))
                            {
                                files.Add(filesList.ElementAt(index));
                            }
                        }
                    }
                    else
                    {
                        if (System.IO.File.Exists(sPath))
                        {
                            files.Add(new VDOFile { Name = new FileInfo(sPath).Name, Ext = new FileInfo(sPath).Extension });
                        }
                    }
                    result = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(files);
                }
                catch (Exception ex)
                {
                    result = "Error: " + ex.ToString();
                }
            }

            return Content(result);
        }
    }
}
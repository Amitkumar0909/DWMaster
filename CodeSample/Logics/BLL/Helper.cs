using System;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Collections;
using System.Collections.Generic;

namespace CodeSample.Logics.BLL
{ 
    public static class Helper
    {
        public static void LogException(Exception exc, string source=null)
        {
            // Include logic for logging exceptions
            // Get the absolute path to the log file
            string logFile = "~/Content/ErrorLog.txt";
            logFile = HttpContext.Current.Server.MapPath(logFile);

            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
                if (exc.InnerException != null && exc.InnerException.InnerException != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.InnerException.Message);
                    sw.WriteLine(exc.InnerException.InnerException.Source);
                    sw.WriteLine(exc.InnerException.InnerException.InnerException + "");
                }
            }
            sw.Write("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Source: " + source);
            sw.WriteLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
            sw.Close();
        }
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }





        public static string FilePaths(string path)
        {
            string fileNames = string.Empty;
            try
            {
                DirectoryInfo diFiles = new DirectoryInfo(path);
                if (diFiles.Exists)
                {
                    foreach (FileInfo fi in diFiles.GetFiles())
                    {
                        fileNames += fi.Name + ";";
                    }

                    return fileNames.Length > 0 ? fileNames.Substring(0, fileNames.Length - 1) : fileNames;
                }
            }
            catch
            {
            }
            return fileNames;
        }

        public static string MoveFiles(string from, string to)
        {
            string fileNames = string.Empty;
            try
            {
                DirectoryInfo diFiles = new DirectoryInfo(from);
                if (diFiles.Exists)
                {
                    if (!Directory.Exists(to))
                    {
                        Directory.CreateDirectory(to);
                    }

                    foreach (FileInfo fi in diFiles.GetFiles())
                    {
                        fi.MoveTo((to.EndsWith("/") ? to : to + "/") + fi.Name);
                        fileNames += fi.Name + ";";
                    }
                     
                    return fileNames.Length > 0 ? fileNames.Substring(0, fileNames.Length - 1) : fileNames;
                }
            }
            catch
            {
            }
            return fileNames;
        }
    }
}
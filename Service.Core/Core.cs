using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public class Core
    {
        public FileSystemWatcher watcher = null;
        string strEldenRingSavePath = string.Empty;
        string strRootBackUpPath = string.Empty;

        public Core()
        {
            string backupPath = string.Empty;

            try
            {
                string appSettingBackUpPath = ConfigurationManager.AppSettings.Get("BackUpPath");
                string userFolder = string.Empty;

                userFolder = GetWindowsUserAccountName();

                if(string.IsNullOrEmpty(userFolder))
                {
                    throw new Exception();
                }
                else
                {
                    strEldenRingSavePath = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), string.Format(@"Users\{0}\AppData\Roaming\EldenRing", userFolder));
                }

                if(string.IsNullOrEmpty(appSettingBackUpPath))
                {
                    strRootBackUpPath = "C:\\EldenRingAutoBackUpFiles";
                }
                else
                {
                    strRootBackUpPath = Path.GetFullPath(ConfigurationManager.AppSettings.Get("BackUpPath"));
                }    

                backupPath = Path.Combine(strRootBackUpPath, DateTime.Now.ToString("yyyyMMddHHmm"), "EldenRing");

                if (!Directory.Exists(strRootBackUpPath))
                {
                    //최초 백업
                    Directory.CreateDirectory(backupPath);
                    CopyFolder(strEldenRingSavePath, backupPath);
                }

                OldFileDelete();
                SaveFileWatcher();
            }
            catch (Exception ex)
            {
                throw new Exception("Error");
            }
        }


        public void OldFileDelete()
        {
            int savedFileMaxCount = 100;
            int overFileCount = 0;
            var backUpDir = Directory.GetDirectories(strRootBackUpPath);

            try
            {
                savedFileMaxCount = Convert.ToInt32(ConfigurationManager.AppSettings.Get("MaxBackUpFileCount"));
            }
            catch(Exception ex)
            {
                savedFileMaxCount = 100;
            }

            overFileCount = backUpDir.Length - savedFileMaxCount;

            if (overFileCount > 0)
            {
                Array.Sort(backUpDir);

                for(int i = 0; i < overFileCount; i++)
                {
                    try
                    {
                        Directory.Delete(backUpDir[i], true);
                    }
                    catch(Exception ex)
                    {
                    }
                }
            }
        }

        public static string GetWindowsUserAccountName()
        {
            string userName = string.Empty;
            ManagementScope ms = new ManagementScope("\\\\.\\root\\cimv2");
            ObjectQuery query = new ObjectQuery("select * from win32_computersystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(ms, query);

            foreach (ManagementObject mo in searcher?.Get())
            {
                userName = mo["username"]?.ToString();
            }
            userName = userName?.Substring(userName.IndexOf(@"\") + 1);

            return userName;
        }

        public void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }

            string[] files = Directory.GetFiles(sourceFolder);
            string[] folders = Directory.GetDirectories(sourceFolder);

            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest, true);
            }

            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }

        /// <summary>
        /// 저장파일 변경 감지기
        /// </summary>
        public void SaveFileWatcher()
        {
            watcher = new FileSystemWatcher();
            watcher.Path = strEldenRingSavePath;

            watcher.NotifyFilter = NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;

            watcher.Filter = "*.*";

            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Renamed += OnChanged;

            watcher.EnableRaisingEvents = true;
        }


        #region 이벤트
        private void OnChanged(object source, FileSystemEventArgs e)
        {

            try
            {
                watcher.EnableRaisingEvents = false;
                System.Threading.Thread.Sleep(7500);
                CopyFolder(strEldenRingSavePath, Path.Combine(strRootBackUpPath, DateTime.Now.ToString("yyyyMMddHHmm"), "EldenRing"));

                OldFileDelete();

            }
            catch(Exception ex)
            {

            }
            finally
            {
                watcher.EnableRaisingEvents = true;
            }
            

            
        }
        #endregion
    }
}

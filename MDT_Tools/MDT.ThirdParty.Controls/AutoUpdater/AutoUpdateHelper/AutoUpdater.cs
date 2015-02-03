/*****************************************************************
 * Copyright (C) Knights Warrior Corporation. All rights reserved.
 * 
 * Author:   圣殿骑士（Knights Warrior） 
 * Email:    KnightsWarrior@msn.com
 * Website:  http://www.cnblogs.com/KnightsWarrior/       http://knightswarrior.blog.51cto.com/
 * Create Date:  5/8/2010 
 * Usage:
 *
 * RevisionHistory
 * Date         Author               Description
 * 
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace MDT.ThirdParty.Controls
{
    #region The delegate
    public delegate void ShowHandler();
    #endregion
    
    public class AutoUpdater : IAutoUpdater
    {
        #region The private fields
        private Config config = null;
        private bool bNeedRestart = false;
        private bool bDownload = false;
        List<DownloadFileInfo> downloadFileListTemp = null;
        #endregion

        #region The public event
        public event ShowHandler OnShow;
        #endregion

        #region The constructor of AutoUpdater
        public AutoUpdater()
        {
            config = Config.LoadConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstFile.FILENAME));
        }
        #endregion

        #region The public method
        public int IsUpdate()
        {
            int isUpdate = 0;

            if (config.Enabled)
            {
                string urls = "";
                LogHelper.Debug(string.Format("ServerUrl:{0}", config.ServerUrl));
                using (var webClient = new HttpWebClient())
                {
                    try
                    {
                        urls = webClient.DownloadString(config.ServerUrl);
                    }
                    catch(Exception ex)
                    {
                       LogHelper.Error(ex);
                    }
                    
                }
                LogHelper.Debug(string.Format("AutoUpdaterUrl:{0}", urls));
                string[] strs = urls.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                if (strs.Length > 0)
                {
                    RemoteConfig remoteConfig = null;

                    foreach (var str in strs)
                    {
                        try
                        {
                            remoteConfig = ParseRemoteXml(str);
                            break;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex);
                            //EventLog.WriteEntry("AutoUpdater isUpdate", ex.StackTrace, EventLogEntryType.Error);

                        }
                    }



                    if (remoteConfig != null)
                    {

                        string version = remoteConfig.Version;
                        string mversion = remoteConfig.MinimumRequiredVersion;
                      
                        try
                        {
                            Version v = new Version(version);
                            Version mv = new Version(mversion);
                            Version vc = new Version(config.Version);

                            if (vc < mv)
                            {
                                isUpdate = 3;
                                return isUpdate;
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex);
                            //EventLog.WriteEntry("AutoUpdater ", ex.StackTrace, EventLogEntryType.Error);

                        }


                        Dictionary<string, RemoteFile> listRemotFile = remoteConfig.RemoteFiles;

                        List<DownloadFileInfo> downloadList = new List<DownloadFileInfo>();

                        foreach (LocalFile file in config.UpdateFileList)
                        {
                            if (listRemotFile.ContainsKey(file.Path))
                            {

                                try
                                {
                                    RemoteFile rf = listRemotFile[file.Path];
                                    Version v1 = new Version(rf.LastVer);
                                    Version v2 = new Version(file.LastVer);
                                    if (v1 > v2 || rf.Size != file.Size || rf.MD5 != file.MD5)
                                    {
                                        downloadList.Add(new DownloadFileInfo(rf.Url, file.Path, rf.LastVer, rf.Size));
                                        file.LastVer = rf.LastVer;
                                        file.Size = rf.Size;

                                        if (rf.NeedRestart)
                                            bNeedRestart = true;
                                        isUpdate = 2;
                                        break;
                                    }
                                    listRemotFile.Remove(file.Path);
                                }
                                catch (Exception ex)
                                {
                                 LogHelper.Error(ex);
                                    //EventLog.WriteEntry("AutoUpdater IsUpdate", ex.StackTrace, EventLogEntryType.Error);

                                }


                            }
                        }

                        if (listRemotFile.Count > 0)
                            isUpdate = 2;
                    }
                    else
                    {
                        LogHelper.Error("AutoUpdater IsUpdate no autoUpdaterUrl:" + config.ServerUrl);
                        //EventLog.WriteEntry("AutoUpdater IsUpdate", "no autoUpdaterUrl:" + config.ServerUrl, EventLogEntryType.Error);
                    }
                }
                else
                {
                    LogHelper.Error("AutoUpdater IsUpdate no autoUpdaterUrl:" + config.ServerUrl);
                    //EventLog.WriteEntry("AutoUpdater IsUpdate", "no autoUpdaterUrl:" + config.ServerUrl, EventLogEntryType.Error);

                }
            }

            return isUpdate;
        }

        public void Update()
        {


            if (!config.Enabled)
                return;
          
            string urls = "";
            using (var webClient = new HttpWebClient())
            {
                 
                try
                {
                    urls = webClient.DownloadString(config.ServerUrl);
                }
                catch
                {
                    try
                    {
                        urls = webClient.DownloadString(config.ServerUrl);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                    }
                }
            }
            string[] strs = urls.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (strs.Length > 0)
            {
                RemoteConfig remoteConfig = null;

                foreach (var str in strs)
                {
                    try
                    {

                        remoteConfig = ParseRemoteXml(str);
                        break;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                        
                    }
                }


                if (remoteConfig != null)
                {
                    string version = remoteConfig.Version;
                    string mversion = remoteConfig.MinimumRequiredVersion;
                   
                     LogHelper.Info(string.Format("version:{0},mversion:{1}", version,mversion));
                     LogHelper.Info(string.Format("comment:{0}",remoteConfig.Comment));
                    try
                    {
                        Version v = new Version(version);
                        Version mv = new Version(mversion);
                        Version vc = new Version(config.Version);

                       
                        config.Version = v.ToString();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                    }


                    Dictionary<string, RemoteFile> listRemotFile = remoteConfig.RemoteFiles;
                    List<DownloadFileInfo> downloadList = new List<DownloadFileInfo>();

                    foreach (LocalFile file in config.UpdateFileList)
                    {
                        if (listRemotFile.ContainsKey(file.Path))
                        {
                            try
                            {
                                RemoteFile rf = listRemotFile[file.Path];
                                Version v1 = new Version(rf.LastVer);
                                Version v2 = new Version(file.LastVer);
                                if (v1 > v2 || rf.Size != file.Size || rf.MD5 != file.MD5)
                                {
                                    downloadList.Add(new DownloadFileInfo(rf.Url, file.Path, rf.LastVer, rf.Size));
                                    file.LastVer = rf.LastVer;
                                    file.Size = rf.Size;
                                    file.MD5 = rf.MD5;
                                    if (rf.NeedRestart)
                                        bNeedRestart = true;
                                    bDownload = true;
                                }

                                listRemotFile.Remove(file.Path);
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error(ex);
                                //EventLog.WriteEntry("AutoUpdater Update", ex.StackTrace, EventLogEntryType.Error);
                            }
                        }
                    }

                    foreach (RemoteFile file in listRemotFile.Values)
                    {
                        downloadList.Add(new DownloadFileInfo(file.Url, file.Path, file.LastVer, file.Size));
                        LocalFile lf = new LocalFile(file.Path, file.LastVer, file.Size, file.MD5);
                        config.UpdateFileList.Add(lf);
                        if (file.NeedRestart)
                            bNeedRestart = true;
                        bDownload = true;
                    }
                    downloadFileListTemp = new List<DownloadFileInfo>();
                    foreach (var downloadFileInfo in downloadList)
                    {
                        downloadFileListTemp.Add(downloadFileInfo);
                    }
                    if (bDownload)
                    {
                        
                        DownloadConfirm dc = new DownloadConfirm(downloadList);
                        //dc.IsCanCancel = isCanCancel;
                        dc.IsCanCancel = true;
                        if (this.OnShow != null)
                            this.OnShow();

                        if (DialogResult.OK == dc.ShowDialog())
                        {
                            StartDownload(downloadList);
                        }
                        else
                        {
                            CommonUnitity.RestartApplication();
                        }
                    }
                }
                else
                {
                    LogHelper.Error("remoteConfig is null");
                    CommonUnitity.ShowErrorAndRestartApplication();

                }
            }
            else
            {
                LogHelper.Error("url is null");
                CommonUnitity.ShowErrorAndRestartApplication();

            }

        }

        public void RollBack()
        {
            foreach (DownloadFileInfo file in downloadFileListTemp)
            {
                string tempUrlPath = CommonUnitity.GetFolderUrl(file);
                string oldPath = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(tempUrlPath))
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1), file.FileName);
                    }
                    else
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl, file.FileName);
                    }

                    if (oldPath.EndsWith("_"))
                        oldPath = oldPath.Substring(0, oldPath.Length - 1);

                    MoveFolderToOld(oldPath + ".old", oldPath);

                }
                catch (Exception ex)
                {
                    LogHelper.Error("RollBack:"+ex.StackTrace);
                    //EventLog.WriteEntry("AutoUpdater RollBack", ex.StackTrace, EventLogEntryType.Error);
                }
            }
        }

        public void SetAppName(string name)
        {
            ConstFile.AppName = name;
        }
        public void SetAppExe(string name)
        {
            ConstFile.AppExe = name;
        }

        public void SetServerUrl(string url)
        {
            config.ServerUrl = url;
            config.Enabled = true;
        }

        #endregion

        #region The private method

        public void deleteOld()
        {
            foreach (DownloadFileInfo file in downloadFileListTemp)
            {
                string tempUrlPath = CommonUnitity.GetFolderUrl(file);
                string oldPath = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(tempUrlPath))
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl + tempUrlPath.Substring(1), file.FileName);
                    }
                    else
                    {
                        oldPath = Path.Combine(CommonUnitity.SystemBinUrl, file.FileName);
                    }

                    if (oldPath.EndsWith("_"))
                        oldPath = oldPath.Substring(0, oldPath.Length - 1);

                    DeleteFolderToOld(oldPath + ".old");

                }
                catch
                {
                    //log the error message,you can use the application's log code
                }
            }
        }

        string newfilepath = string.Empty;
        private void MoveFolderToOld(string oldPath, string newPath)
        {
            if (File.Exists(oldPath) && File.Exists(newPath))
            {
                LogHelper.Debug(string.Format("copy:{0} to {1}",oldPath,newPath));
                System.IO.File.Copy(oldPath, newPath, true);
            }
        }
        private void DeleteFolderToOld(string oldPath)
        {
            if (File.Exists(oldPath))
            {
                LogHelper.Debug(string.Format("DeleteFolderToOld:{0}", oldPath));
                File.Delete(oldPath);
            }
        }
        private void StartDownload(List<DownloadFileInfo> downloadList)
        {
            DownloadProgress dp = new DownloadProgress(downloadList);
            if (dp.ShowDialog() == DialogResult.OK)
            {
                //
                if (DialogResult.Cancel == dp.ShowDialog())
                {
                    return;
                }
                //Update successfully
                config.SaveConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstFile.FILENAME));

                if (bNeedRestart)
                {
                    //Delete the temp folder

                    deleteOld();
                    Directory.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConstFile.TEMPFOLDERNAME), true);
                    MessageBox.Show(ConstFile.APPLYTHEUPDATE, ConstFile.MESSAGETITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CommonUnitity.RestartApplication();
                }
            }
        }

        private RemoteConfig ParseRemoteXml(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xml);

            RemoteConfig remoteConfig = new RemoteConfig(document);
            return remoteConfig;
        }
        #endregion

    }

}

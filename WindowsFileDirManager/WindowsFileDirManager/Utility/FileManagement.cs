using System;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using WindowsFileDirManager.Models;
namespace WindowsFileDirManager.Utility
{
    public static class FileManagement
    {
        public static ApplicationData ReadFolder(string directoryPath)
        {
            ApplicationData applicationData = new ApplicationData();

            string[] files = Directory.GetFiles(directoryPath);
            applicationData.Files = new ObservableCollection<string>(files);
            return applicationData;
        }

        public static bool DoChanges(ApplicationData applicationData, string currentDirectory, bool preview)
        {
            //If preview is True use a cloned ApplicationData object, do not change original object
            if (preview)
            {
                applicationData = (ApplicationData)applicationData.Clone();
            }
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory);
                foreach (Operation op in applicationData.OperationsConfigured)
                {
                    //Collect files based on filter type
                    FileInfo[] fileInfos;
                    string searchPattern = string.Empty;
                    if (op.FilterType == FilterType.Contains)
                    {
                        searchPattern = string.Format("*{0}*", op.Filter);
                        fileInfos = directoryInfo.GetFiles(searchPattern);
                    }
                    else if (op.FilterType == FilterType.StartsWith)
                    {
                        searchPattern = string.Format("{0}*", op.Filter);
                        fileInfos = directoryInfo.GetFiles(searchPattern);
                    }
                    else if (op.FilterType == FilterType.EndsWith)
                    {
                        searchPattern = string.Format("*{0}", op.Filter);
                        fileInfos = directoryInfo.GetFiles(searchPattern);
                    }
                    else if (op.FilterType == FilterType.ExtensionIs)
                    {
                        searchPattern = string.Format("*.{0}", op.Filter);
                        fileInfos = directoryInfo.GetFiles(searchPattern);
                    }
                    else
                    {
                        return false; //FilterType not valid
                    }

                    if (fileInfos.Length == 0)
                    {
                        return false; //Filter did not collect any result
                    }

                    if (preview) //logical removal from application data
                    {
                        if (op.ActionType == ActionType.Delete)
                        {
                            foreach (FileInfo file in fileInfos)
                            {
                                applicationData.Files.Remove(file.FullName);
                            }
                        }
                    }
                    else //actually do changes
                    {
                        if (op.ActionType == ActionType.Delete)
                        {
                            foreach (FileInfo file in fileInfos)
                            {
                                File.Delete(file.FullName);
                            }
                        }
                        else //Rename
                        {
                            foreach (FileInfo file in fileInfos)
                            {
                                File.Move(file.FullName, Path.Combine(file.DirectoryName, "1" + file.Name));
                            }
                        }
                    }

                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }

}

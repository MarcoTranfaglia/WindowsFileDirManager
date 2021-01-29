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

        public static bool DoChanges(ApplicationData applicationData, ApplicationData previewData, string currentDirectory, bool preview)
        {
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

                    if (preview) //logical removal from preview data
                    {
                        if (op.ActionType == ActionType.Rename)
                        {
                            foreach (FileInfo file in fileInfos)
                            {
                                //TODO
                                

                                string newName = "1"+ Path.GetFileNameWithoutExtension(file.FullName);
                                string newNameFullPath = Path.Combine(file.DirectoryName, newName);
                                previewData.Files.Remove(file.FullName);
                                previewData.Files.Add(newNameFullPath);
                            }
                        }
                        else if (op.ActionType == ActionType.Delete)
                        {
                            foreach (FileInfo file in fileInfos)
                            {
                                previewData.Files.Remove(file.FullName);
                            }
                        }
                        else { }
                    }
                    else //actually do changes
                    {
                        if (op.ActionType == ActionType.Rename)
                        {
                            foreach (FileInfo file in fileInfos)
                            {
                                //TODO
                                string newName = "1" + Path.GetFileNameWithoutExtension(file.FullName);
                                string newNameFullPath = Path.Combine(file.DirectoryName, newName); 
                                applicationData.Files.Remove(file.FullName);
                                applicationData.Files.Add(newNameFullPath);
                                File.Move(file.FullName, Path.Combine(file.DirectoryName, "1" + file.Name));
                            }
                        }
                        if (op.ActionType == ActionType.Delete)
                        {
                            foreach (FileInfo file in fileInfos)
                            {
                                applicationData.Files.Remove(file.FullName);
                                File.Delete(file.FullName);
                            }
                        }
                        else { }
                       
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

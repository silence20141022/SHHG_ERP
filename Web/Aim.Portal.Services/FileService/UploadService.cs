using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Web.Hosting;
using System.ServiceModel.Activation;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using Aim.Common;
using Aim.Portal.Model;
using Aim.Portal.FileSystem;

namespace Aim.Portal.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UploadService : IUploadService
    {
        #region 成员属性

        private string _TemporaryUploadFolder = null;

        // 上传文件临时目录
        public string TemporaryUploadFolder
        {
            get
            {
                if (String.IsNullOrEmpty(_TemporaryUploadFolder))
                {
                    _TemporaryUploadFolder = FileService.GetTemporaryFolder();
                }

                return _TemporaryUploadFolder;
            }
        }

        #endregion

        #region IUploadService成员

        /// <summary>
        /// 取消上传并删除临时文件
        /// </summary>
        /// <param name="fileName"></param>
        public void CancelUpload(string fileName)
        {
            string tmpFilePath = GetTemporaryFilePathByName(fileName);

            if (File.Exists(tmpFilePath))
            {
                File.Delete(tmpFilePath);
            }
        }

        /// <summary>
        /// 接收并存储数据块
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <param name="dataLength"></param>
        /// <param name="parameters"></param>
        /// <param name="firstChunk"></param>
        /// <param name="lastChunk"></param>
        public string StoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk)
        {
            string newName = String.Empty;

            string filePath = GetUploadedFilePathByName(fileName);
            string tmpFilePath = GetTemporaryFilePathByName(fileName);

            // 是否第一个数据块
            if (firstChunk)
            {
                // 删除临时文件 
                if (File.Exists(tmpFilePath))
                {
                    File.Delete(tmpFilePath);
                }

                // 删除目标文件
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            using (FileStream fs = File.Open(tmpFilePath, FileMode.Append))
            {
                fs.Write(data, 0, dataLength);

                fs.Close();
            }

            // 如果数据块为最后上传块则结束上传
            if (lastChunk)
            {
                // 重命名源文件
                File.Move(tmpFilePath, filePath);

                // 执行结束方法
                newName = FinishedFileUpload(filePath, parameters);
            }

            return newName;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 删除已上传文件
        /// </summary>
        /// <param name="fileName"></param>
        protected void DeleteUploadedFile(string fileName)
        {
            string filePath = GetUploadedFilePathByName(fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 文件上传完毕时执行保存方法
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="parameters"></param>
        protected string FinishedFileUpload(string filePath, string parameters)
        {
            // Thread.Sleep(5000);
            string fileFullID = String.Empty;

            if (parameters.Trim().Length > 0)
            {
                StringParam sp = new StringParam(parameters, ";", ":");
                string folderKey = sp.Get("FolderKey");

                if (!String.IsNullOrEmpty(folderKey))
                {
                    fileFullID = FileService.MoveFile(filePath, folderKey);
                }
            }

            return fileFullID;
        }

        /// <summary>
        /// 由文件名获取临时文件路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetTemporaryFilePathByName(string fileName)
        {
            string tempFileName = FileService.GetTemporaryFileName(fileName);

            return Path.Combine(TemporaryUploadFolder, tempFileName);
        }

        /// <summary>
        /// 获取上传后的临时文件路径（文件名为源文件名）
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetUploadedFilePathByName(string fileName)
        {
            return Path.Combine(TemporaryUploadFolder, fileName);
        }

        #endregion
    }
}

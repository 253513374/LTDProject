using ScanCode.Model.Enum;
using System;

namespace ScanCode.Model.Entity
{
    /// <summary>
    /// 文件上传记录
    /// </summary>
    public class FileUploadRecord : IEntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 原文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 短文件名称
        /// </summary>
        public string FileName2 { get; set; }

        /// <summary>
        ///  上传文件的大小，单位为字节
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文本数据量
        /// </summary>
        public long FileCount { get; set; }

        /// <summary>
        /// 文件存储路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件唯一哈希值，使用SHA256计算
        /// </summary>
        public string FileHash { get; set; }

        /// <summary>
        /// 文件上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 文本状态
        /// </summary>
        public ImportStatus Status { get; set; }
    }
}
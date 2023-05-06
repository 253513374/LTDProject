using MediatR;
using ScanCode.Model.ResponseModel;
using System;

namespace ScanCode.Model.Entity
{
    public abstract class IEntityBase : TResult, IRequest<string>, INotification
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string AdminUser { get; set; }
    }
}
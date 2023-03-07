using MediatR;
using System;

namespace Weitedianlan.Model.Entity
{
    public abstract class IEntityBase : IRequest<string>, INotification
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
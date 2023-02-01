using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

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
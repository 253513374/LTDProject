using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Wtdl.Admin.Authenticated.IdentityModel
{
    public static class Permissions
    {
        [DisplayName("RedPacket")]
        [Description("红包管理权限")]
        public static class RedPackets
        {
            public const string RedPacketRecrodView = "Permissions.RedPacket.RedPacketRecrodView";
            public const string RedPacketConfigView = "Permissions.RedPacket.RedPacketConfigView";
            public const string RedPacketGiveOutView = "Permissions.RedPacket.RedPacketGiveOutView";
            //public const string Delete = "Permissions.RedPacket.Delete";
            //public const string Export = "Permissions.RedPacket.Export";
            //public const string Search = "Permissions.RedPacket.Search";
        }

        [DisplayName("ScanOut")]
        [Description("扫码出库权限")]
        public static class ScanOuts
        {
            public const string ScanOutView = "Permissions.ScanOutView.ScanOutView";
            //public const string Create = "Permissions.ScanOutView.Create";
            //public const string Edit = "Permissions.ScanOutView.Edit";
            //public const string Delete = "Permissions.RedPacket.Delete";
            //public const string Export = "Permissions.RedPacket.Export";
            //public const string Search = "Permissions.RedPacket.Search";
        }

        [DisplayName("Lottery")]
        [Description("抽奖权限")]
        public static class Lotterys
        {
            public const string LotteryRecrodView = "Permissions.Lotterys.LotteryRecrodView";
            public const string LotteryActivityView = "Permissions.Lotterys.LotteryActivityView";
            public const string LotteryPrizeView = "Permissions.Lotterys.LotteryPrizeView";

            //public const string Export = "Permissions.Lotterys.Export";
            //public const string Search = "Permissions.Lotterys.Search";
            //public const string Import = "Permissions.Lotterys.Import";
        }

        [DisplayName("TxtImport")]
        [Description("txt文本数据导入")]
        public static class TxtImports
        {
            public const string TxtImportView = "Permissions.TxtImports.TxtImport";
            //public const string Create = "Permissions.Documents.Create";
            //public const string Edit = "Permissions.Documents.Edit";
            //public const string Delete = "Permissions.Documents.Delete";
            //public const string Search = "Permissions.Documents.Search";
        }

        //[DisplayName("Document Types")]
        //[Description("Document Types Permissions")]
        //public static class DocumentTypes
        //{
        //    public const string View = "Permissions.DocumentTypes.View";
        //    public const string Create = "Permissions.DocumentTypes.Create";
        //    public const string Edit = "Permissions.DocumentTypes.Edit";
        //    public const string Delete = "Permissions.DocumentTypes.Delete";
        //    public const string Export = "Permissions.DocumentTypes.Export";
        //    public const string Search = "Permissions.DocumentTypes.Search";
        //}

        //[DisplayName("Document Extended Attributes")]
        //[Description("Document Extended Attributes Permissions")]
        //public static class DocumentExtendedAttributes
        //{
        //    public const string View = "Permissions.DocumentExtendedAttributes.View";
        //    public const string Create = "Permissions.DocumentExtendedAttributes.Create";
        //    public const string Edit = "Permissions.DocumentExtendedAttributes.Edit";
        //    public const string Delete = "Permissions.DocumentExtendedAttributes.Delete";
        //    public const string Export = "Permissions.DocumentExtendedAttributes.Export";
        //    public const string Search = "Permissions.DocumentExtendedAttributes.Search";
        //}

        [DisplayName("Users")]
        [Description("账号权限")]
        public static class Users
        {
            /// <summary>
            /// 访问权
            /// </summary>
            public const string View = "Permissions.Users.View";

            /// <summary>
            /// 创建权限
            /// </summary>
            public const string Create = "Permissions.Users.Create";

            /// <summary>
            /// 编辑权限
            /// </summary>
            public const string Edit = "Permissions.Users.Edit";

            //public const string Delete = "Permissions.Users.Delete";
            //public const string Export = "Permissions.Users.Export";
            //public const string Search = "Permissions.Users.Search";
        }

        [DisplayName("Roles")]
        [Description("角色权限")]
        public static class Roles
        {
            /// <summary>
            /// 访问权
            /// </summary>
            public const string View = "Permissions.Roles.View";

            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            //public const string Delete = "Permissions.Roles.Delete";
            //public const string Search = "Permissions.Roles.Search";
        }

        [DisplayName("Role Claims")]
        [Description("角色声明权限")]
        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            //public const string Create = "Permissions.RoleClaims.Create";
            //public const string Edit = "Permissions.RoleClaims.Edit";
            //public const string Delete = "Permissions.RoleClaims.Delete";
            //public const string Search = "Permissions.RoleClaims.Search";
        }

        //[DisplayName("Communication")]
        //[Description("Communication Permissions")]
        //public static class Communication
        //{
        //    public const string Chat = "Permissions.Communication.Chat";
        //}

        //[DisplayName("Preferences")]
        //[Description("Preferences Permissions")]
        //public static class Preferences
        //{
        //    public const string ChangeLanguage = "Permissions.Preferences.ChangeLanguage";

        //    //TODO - add permissions
        //}

        [DisplayName("Dashboards")]
        [Description("数据显示面板")]
        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
        }

        //[DisplayName("Hangfire")]
        //[Description("Hangfire Permissions")]
        //public static class Hangfire
        //{
        //    public const string View = "Permissions.Hangfire.View";
        //}

        //[DisplayName("Audit Trails")]
        //[Description("Audit Trails Permissions")]
        //public static class AuditTrails
        //{
        //    public const string View = "Permissions.AuditTrails.View";
        //    public const string Export = "Permissions.AuditTrails.Export";
        //    public const string Search = "Permissions.AuditTrails.Search";
        //}

        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permissions = new List<string>();
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permissions.Add(propertyValue.ToString());
            }
            return permissions;
        }
    }
}
namespace PPK_Logistics.ERP.Interface
{
    internal class SQLCommandtext
    {
        #region GJPCommands 管家婆ERP 数据SQL

        public static string GJPCommands = @"SELECT
	                                            --TT.BillNumberId,
	                                            TT.BillCode,
	                                            --s.Stypeid,分支机构id
	                                            --Qty ,
	                                            --TT.PtypeId,--商品ID
	                                            TT.FullName,
	                                            --仓库名称
	                                            --f.FullName,分支机构名称 
	                                            --p.FullName,产品名称
	                                            --p.UserCode,产品编号
	                                            --p.Standard,规格
	                                            TT.BillDate,
	                                            TT.danwei,
	                                            TT.UserCode,
	                                            TT.KHMC,
	                                            TT.Comment,
	                                            TT.BillType,
	                                            SUM (TT.Qty) AS Qty
                                            FROM
	                                            (
		                                            SELECT
			                                            s.BillNumberId,
			                                            Temptab.BillCode,
			                                            --s.Stypeid,分支机构id
			                                            s.Qty,
			                                            s.PtypeId,
			                                            --商品ID
			                                            t.FullName,
			                                            --仓库名称
			                                            --f.FullName,分支机构名称 
			                                            --p.FullName,产品名称
			                                            --p.UserCode,产品编号
			                                            --p.Standard,规格
			                                            Temptab.BillDate,
			                                            pun.FullName AS danwei,
			                                            Temptab.UserCode,
			                                            Temptab.FullName AS KHMC,
			                                            Temptab.Comment,
			                                            --Temptab.DealBTypeID,
			                                            Temptab.BillType --SUM (s.Qty) AS s.Qty
		                                            FROM
			                                            saleBill s --JOIN ptype p ON p.typeId = s.PtypeId
		                                            JOIN PType_Units pun ON pun.UnitsId = s.UnitID
		                                            JOIN Stock t ON t.typeId = s.KTypeID
		                                            JOIN stype f ON f.TypeId = s.Stypeid
		                                            JOIN (
			                                            SELECT
				                                            b.BillNumberId,
				                                            b.BillDate,
				                                            b.BillCode,
				                                            b.BillType,
				                                            bt.UserCode,
				                                            bt.FullName,
				                                            b.DealBTypeID,
				                                            b.Comment
			                                            FROM
				                                            BillIndex b
			                                            JOIN btype bt ON bt.typeId = b.DealBTypeID
			                                            WHERE
				                                            DateDiff(dd, b.BillDate, getdate()) <= 47 --BillCode = 'XK-wjzb-180312-0046'
		                                            ) Temptab ON Temptab.BillNumberId = s.BillNumberId
	                                            ) TT 
                                            WHERE
	                                            danwei = '扎'
                                            GROUP BY
	                                            TT.BillCode,
	                                            TT.FullName,
	                                            TT.BillDate,
	                                            TT.danwei,
	                                            TT.UserCode,
	                                            TT.KHMC,
	                                            TT.Comment,
	                                            TT.BillType";

        #endregion GJPCommands 管家婆ERP 数据SQL

        #region GTDCommands 定制ERP 数据获取SQL

        public static string GTDCommands = @"SELECT
	                                        T5.KCSWZMX_JZRQ,
	                                        T5.KCSWZ_KHID,
	                                        T5.KH_MC,
	                                        T5.KCSWZ_SWLX,
	                                        T5.KCSWZMX_SWKCBH,
	                                        T5.WL_FJLDW,
	                                        SUM (T5.KCSWZMX_FZCKSL) AS KCSWZMX_FZCKSL
                                        FROM
	                                        (
		                                        SELECT
			                                        T.KCSWZMX_JZRQ,
			                                        T2.KCSWZ_KHID,
			                                        T2.KCSWZ_SWLX,
			                                        T3.KH_MC,
			                                        T.KCSWZMX_SWKCBH,
			                                        T.KCSWZMX_CKSL,
			                                        T.KCSWZMX_FZCKSL,
			                                        T4.WL_FJLDW
		                                        FROM
			                                        JSERP8.KCSWZMX T
		                                        JOIN JSERP8.KCSWZ T2 ON T2.KCSWZ_SWKCBH = T.KCSWZMX_SWKCBH
		                                        JOIN JSERP8.KH T3 ON T2.KCSWZ_KHID = T3.KH_KHID
		                                        JOIN JSERP8.WL T4 ON T.KCSWZMX_WLID = T4.WL_WLID
                                           WHERE
				                                        DateDiff(dd,T.KCSWZMX_WHSJ,getdate())<=5 AND T2.KCSWZ_SFTK = 'N' AND T2.KCSWZ_SWLX = 'XSCK'
	                                        ) T5
                                        GROUP BY
	                                        T5.KCSWZMX_JZRQ,
	                                        T5.KCSWZ_KHID,
	                                        T5.KH_MC,
	                                        T5.KCSWZ_SWLX,
	                                        T5.WL_FJLDW,
	                                        T5.KCSWZMX_SWKCBH";

        #endregion GTDCommands 定制ERP 数据获取SQL

        #region WTDLCommands 溯源数据库 数据获取SQL

        public static string WTDLCommands = (@"SELECT
	                                                fhPaper1,
	                                                COUNT (fhPaper1) AS yfsl,
	                                                fhOriginalDate1
                                                FROM
	                                                tLabels_X T
                                                WHERE
	                                                DateDiff(dd, T.fhDate1, getdate()) <= 10
                                                GROUP BY
                                                fhPaper1,
                                                fhOriginalDate1");

        #endregion WTDLCommands 溯源数据库 数据获取SQL

        //string.Format(@"  select TOP 100 fhPaper1, COUNT(fhPaper1) AS yfsl,fhOriginalDate1  from tLabels_X T
        //                              GROUP BY fhPaper1, fhOriginalDate1
        //                              ORDER BY fhOriginalDate1 DESC");
    }
}
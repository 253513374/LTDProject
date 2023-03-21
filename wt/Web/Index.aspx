<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<html>
<head>
<meta charset="utf-8">
<meta http-equiv="pragma" content="no-cache">
<meta http-equiv="cache-control" content="no-cache">
<meta http-equiv="expires" content="0">
<meta http-equiv="apple-mobile-web-app-capable" content="yes">
<meta name="format-detection" content="telephone=no" />
<meta name="viewport" content="product-scalable=no, width=device-width, initial-scale=1.0, maximum-scale=1.0">
<title>威特电缆</title>

<link rel="stylesheet" type="text/css" href="css/style.css">
<script src="js/jquery-1.7.2.min.js"></script>
<script src="js/loading.js"></script>

<script type="text/javascript" src="layer_mobile/layer.js"></script>

</head>

<body>
    <form id="form1" runat="server">
<div class="demo-wrapper" id="app">
     <div>     
    
    <img src="img/1_01.gif" width="100%">
    <img src="img/1_02.gif" width="100%">
    <img src="img/1_03.gif" width="100%">
    <img src="img/1_04.gif" width="100%">
    <img src="img/1_05.gif" width="100%">
    <img src="img/1_06.gif" width="100%">
    <img src="img/1_07.gif" width="100%">
    <img src="img/1_08.gif" width="100%">
    <img src="img/1_09.gif" width="100%">
    <img src="img/1_10.gif" width="100%">
    <img src="img/1_11.gif" width="100%">
    <img src="img/1_12.gif" width="100%">
    <img src="img/1_13.gif" width="100%">
    <img src="img/1_14.gif" width="100%">
    <img src="img/1_15.gif" width="100%">
    <img src="img/1_16.gif" width="100%">
    <img src="img/1_17.gif" width="100%">
    
   
    
     <div id="CodeContent" runat="server" class="product" style=" text-align:left;
     color:#00d5ef; background-color:#0f548e  ">

      <div id="fw"  class="content" > 
          <asp:Literal ID="Literal1" runat="server"></asp:Literal>


<%--               <h3>尊敬的用户：</h3>
                <p style=" text-indent: 2.0em;">
                您所查询的防伪标签照片如下图所示，请逐一观察彩色亮片的分布位置、数量是否相符，若相符，则说明是真品，否则，谨防假冒!
                </p>
                <img class="bq" src="img/bq.jpg" width="60%" style="margin:10px auto;"/>
                <br>
                <div class="fw_info">
                  <p><span>标签序号：</span><span>000000000001</span></p>
                  <p><span>纤维特征：</span><span>彩色亮片</span></p>
                  <p><span>商品名称：</span><span>测试标签</span></p>
                  <p><span>用户名称：</span><span>测试标签</span></p>
                  <p><span>首次查询时间：</span><span>2023-01-31 21:00:00</span></p>
                  <p><span>查询次数：</span><span>第5 次查询</span> <span style="color:#F00;">超次</span></p><br>
                 
                  <p><span style="color: #f40;">重要提示：</span><span>彩色亮片可用针或刀尖挑出。非印刷墨迹，请注意区别。</span></p>
                  
                </div>
                <div class="fw_info" >
                    <p>感谢您使用纹理防伪系统查询真伪，纹理防伪一个也假不了。</p>
                    <p>如有疑问，请拨打电话<a href="400-6800-315"><span style="color: #f40;">400-6800-315</span></a>详细咨询</p>
                </div>--%>
               
               
            </div>

           <h3>溯源信息：</h3>
                <div id="sy" class="fw_info">
                  <%--<p><span>订单号：</span><span>000000000001</span></p>
                  <p><span>发货日期：</span><span>2023-01-30</span></p>
                  <p><span>收货经销商：</span><span>*******</span></p>--%>
                   <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                 
                </div>
  
 </div>
    
    
   
   <div class="footer" style="background-color:#0f548e; ">

         <a id="hongbao"  href="#">
            <div class="navbox">
               
                <img src="img/lqhb.png" alt="领取红包"  style="width:80%">
            </div>
        </a>

        <a href="#" onclick="choujiang()">
            <div class="navbox">
                
                <img src="img/choujiang.png" alt="抽奖"  style="width:80%">
            </div>
        </a>

     </div>
    
    
    <img src="img/1_19.gif" width="100%">
    <img src="img/1_20.gif" width="100%">
    	
    <img src="img/1_21.gif" width="100%">
    <img src="img/1_22.gif" width="100%">
    <img src="img/1_23.gif" width="100%">
    <img src="img/1_24.gif" width="100%">
    <img src="img/1_25.gif" width="100%">
    <img src="img/1_26.gif" width="100%">
    <img src="img/1_27.gif" width="100%">
    <img src="img/1_28.gif" width="100%">
    <img src="img/1_29.gif" width="100%">
    <img src="img/1_30.gif" width="100%">  
        
    <img src="img/1_31.gif" width="100%">
    <img src="img/1_32.gif" width="100%">
    <img src="img/1_33.gif" width="100%">
    <img src="img/1_34.gif" width="100%">
    <img src="img/1_35.gif" width="100%">
    <img src="img/1_36.gif" width="100%">
    <img src="img/1_37.gif" width="100%">
    <img src="img/1_38.gif" width="100%">
    <img src="img/1_39.gif" width="100%">
    <img src="img/1_40.gif" width="100%">  
    
    <img src="img/1_41.gif" width="100%">
    <img src="img/1_42.gif" width="100%">
    <img src="img/1_43.gif" width="100%">
    <img src="img/1_44.gif" width="100%">
    <img src="img/1_45.gif" width="100%">
    <img src="img/1_46.gif" width="100%">
    <img src="img/1_47.gif" width="100%">
    <img src="img/1_48.gif" width="100%">
    <img src="img/1_49.gif" width="100%">
    <img src="img/1_50.gif" width="100%">
    
    <img src="../img/1_51.gif" width="100%">
    <img src="../img/1_52.gif" width="100%">
    <img src="../img/1_53.gif" width="100%">
    <img src="../img/1_54.gif" width="100%">
    <img src="../img/1_55.gif" width="100%">
    <img src="../img/1_56.gif" width="100%">
    <img src="../img/1_57.gif" width="100%">
    
        
        
     </div>
</div>
</form>

    <script type="text/javascript">

    //根据防伪码查询溯源信息和防伪信息
        $(document).ready(function () {
            var QRCode = getUrlParam("id");//标签
      
            if (QRCode == "") {
                window.location.href = "Error.html";
                return;
            }


          //查询第一次领取红包状态
            $.ajax(
                {
                    type: 'get',
                    url: 'Index.ashx?code=' + QRCode + '&ctype=2&callback?',
                    dataType: 'json',
                    jsonp: "callback",
                    success: function (dataRestul) {

                        if (dataRestul.meg=="1") {
                            //第一次可以领取红包，
                            $("#hongbao").attr('onclick', 'hongbao1(' + QRCode + ')');
                        }
                        if (dataRestul.meg == "2") {
                            //第二次可以领取红包，
                            $("#hongbao").attr('onclick', 'hongbao2(' + QRCode + ')');
                        }
                        if (dataRestul.meg == "0") {
                            //没有红包活动
                            $("#hongbao").attr('onclick', 'meiyouhongdong()');
                        }
                    }
                });


        });


        //微信红包没有活动的提示
        function meiyouhongdong() {
            //信息框
            layer.open({
                content: '目前没有红包活动'
              , btn: '确定'
            });
            return;
        }

        //微信红包第一次抽取红包
        function hongbao1(code)
        {
            //第一次红包
            window.location.href = "Hongbao1.aspx?id=" + code;
        }

        //微信红包第二次抽取红包
        function hongbao2(code) {
            //第二次红包
            window.location.href = "Hongbao2Code.aspx?id=" + code;
        }


        ///抽奖点击
        function choujiang() {

            var QRCode = GetQueryValue("id");//标签
            $.ajax(
               {
                   type: 'get',
                   url: 'Index.ashx?code=' + QRCode + '&ctype=3&callback?',
                   dataType: 'json',
                   jsonp: "callback",
                   success: function (dataRestul) {

                       if (dataRestul.meg == "1") {
                           window.location.href = "ChouJiang.aspx?id=" + QRCode;
                       }
                       else {
                           //信息框
                           layer.open({
                               content: '没有活动'
                             , btn: '确定'
                           });
                           return;
                       }
                   }
               });
        }



            //获取URL值
            function GetQueryValue() {
            
                var query = decodeURI(window.location.search.substring(1));
                var vars = query.lastIndexOf('/');
                var pathType = query.slice(vars + 1);
                if (Number.isNaN(pathType)) {
                    return pathType;
                }
                else {
                    // return "";
                    return pathType;
                }
            }



            //获取URL参数
            function getUrlParam(paraName) {
                var url = document.location.toString();
                var arrObj = url.split("?");
                if (arrObj.length > 1) {
                    var arrPara = arrObj[1].split("&");
                    var arr;
                    for (var i = 0; i < arrPara.length; i++) {
                        arr = arrPara[i].split("=");
                        if (arr != null && arr[0] == paraName) {
                            return arr[1];
                        }
                    }
                    return "";
                }
                else {
                    return "";
                }
            }
    </script>

</body>
</html>

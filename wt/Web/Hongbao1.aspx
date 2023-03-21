<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Hongbao1.aspx.cs" Inherits="Hongbao1" %>


<!doctype html>
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

    <style type="text/css">

         body {
            background: url(/images/hongbaobeijing.jpg) no-repeat center center;
            background-size: cover;
            background-attachment: fixed;
            background-color: #CCCCCC;
        }

        #main {

text-align:center;
}
#fullbg {
background-color:gray;
left:0;
opacity:0.8;
position:absolute;
top:0;
z-index:3;
filter:alpha(opacity=50);
-moz-opacity:0.5;
-khtml-opacity:0.5;
}
#dialog {
/*background-color:#fff;
border:5px solid rgba(0,0,0, 0.4);*/
height:400px;
left:50%;
margin:-200px 0 0 -200px;
/*padding:1px;*/
position:fixed !important; /* 浮动对话框 */
position:absolute;
top:50%;
width:400px;
z-index:5;
border-radius:5px;
display:none;
}
#dialog p {
margin:0 0 12px;
height:24px;
line-height:24px;
background:#CCCCCC;
}
#dialog p.close {
text-align:right;
padding-right:10px;
}
#dialog p.close a {
color:#fff;
text-decoration:none;
}



        .imgbeijing {
            /*背景图片*/
            background: url(Img/lingqu.png);
				background-size: 100% 400px;
				-moz-background-size: 100% 400px;
				/* 老版本的 Firefox */
				background-repeat: no-repeat;
				padding-top: 80px;
                height:440px;
                
              
        }

        .spanlingjiang {
            /*获取奖项的文字内容*/
            font-size: 20px;
            font-weight: bold;
            color: red;
        }

    </style>

</head>

<body style="background-color:red;">
<div class="cotainer" id="app" >
     <div id="main">  
    
    <%--<img src="images/hongbaobeijing.jpg" width="100%">
    --%>
        
<div style="width:100%; min-height:1500px; height:auto;">

</div>

        
     </div>

       <div id="fullbg"></div>
    <div id="dialog">
<%--<p class="close"><a href="#" onclick="closeBg();">关闭</a></p>--%>
<div class="imgbeijing" onclick="javascript:btn()">

    <div style="text-align:center; width:100%">
        <span  class="spanlingjiang" >
        <br />
      您的现金红包已经被领取！
         
    </span>
    </div>

    
</div>
</div>

</div>

 

<script type="text/javascript">

    //加载显示现金红包金额
    $(function () {

        //loading带文字
        layer.open({
            type: 2
          , content: '加载中...'
        });


        //查验真伪
        $.ajax(
       {
           type: 'get',
           url: 'Hongbao1.ashx?ctype=0&callback?',
           dataType: 'json',
           jsonp: "callback",
           success: function (dataRestul) {

               if (dataRestul.meg == 1) {
                   //有中奖红包记录
                   var html = " <br />";
                   html += " 恭喜你获取现金红包<br /><br />";
                   html += "  <b style=\"font-size:24px;\">" + parseFormatNum(dataRestul.zt, 0) + "元</b>";
                   $(".spanlingjiang").empty();
                   $(".spanlingjiang").append(html);

               }
               else if (dataRestul.meg == 0) {
                   var html = " <br />";
                   html += " 很遗憾，您没有中奖！";

                   $(".spanlingjiang").empty();
                   $(".spanlingjiang").append(html);

               }
               else {
                   //信息框
                   layer.open({
                       content: '非法访问！',
                       btn: '确定',
                       shadeClose: false,
                       yes: function () {
                           window.location.href = "index.aspx";
                       }
                   });
               }

               //关闭弹出框加载信息
               layer.closeAll();

               //加载显示弹出框
               //加载弹出层
               var bh = $("body").height();
               var bw = $("body").width();
               $("#fullbg").css({
                   height: bh,
                   width: bw,
                   display: "block"
               });
               $("#dialog").show();
               //加载弹出层结束

              

           }
       });




    }
    );



    //点击领取按钮
    function btn() {

        //loading带文字
        layer.open({
            type: 2
          , content: '加载中...'
        });


        //判断是否有红包奖励
        //发放红包
        $.ajax(
       {
           type: 'get',
           url: 'Hongbao1.ashx?ctype=1&callback?',
           dataType: 'json',
           jsonp: "callback",
           success: function (dataRestul) {


               //关闭弹出框加载信息
               layer.closeAll();


               //信息框
               layer.open({
                   content: '领取成功！',
                   btn: '确定',
                   shadeClose: false,
                   yes: function () {
                       window.location.href = "home_page1.aspx";
                   }
               });



           }
       });

        //window.location.href = "home_page1.aspx";
    }


    //获取URL值
    function GetQueryValue(queryName) {
        var query = decodeURI(window.location.search.substring(1));
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == queryName) { return pair[1]; }
        }
        return "";
    }


</script>

</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="home_page1.aspx.cs" Inherits="home_page1" %>

<html><head>
<meta charset="utf-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="renderer" content="webkit">
<meta name="format-detection" content="telephone=no">
<meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no">
<title>海南威特电缆有限公司</title>
<link rel="stylesheet" href="css/red_record.css">
<link rel="stylesheet" href="css/windows1.css">
<script src="js/jquery.min.js"></script> 
<!--弹出框-->
<link rel="stylesheet" href="css/layer.css">
<script type="text/javascript" src="layer_mobile/layer.js"></script>

</head>
<body>

    <form id="form1" runat="server">

<div class="header header4">
 <div class="header-main">
   
   <div class="header-left">
       <p>个人中心</p>
       <h1>
          <img src="images/tuxiang.png" />

       </h1>

   </div>
   <div class="clear1"></div>
   <div class="header-right">
  
   </div>
  
 </div>
 <div class="header-icon1"></div>

</div>

<div class="main1">
<a href="home_page2.aspx?type=0" class="main1-but1" style="text-align:center">红包记录</a>
    <a href="home_page2.aspx?type=1" class="main1-but2" style="text-align:center">抽奖记录</a>


</div>
<br />
<div class="main1" style="padding-top:20px;">
   
<p id="hbjl" class="main1-but1" style="font-size:16px; line-height:35px;" runat="server" >
        领取次数：0次
        <br />
        总金额：0元
</p>

     <p id="cjjl" class="main1-but2"  style="font-size:16px; line-height:35px;"  runat="server" >
        抽奖次数：0次
        <br />
        中奖次数：0次
    </p>

</div>
    <div class="clear1"></div>

<div class="main1">
<div class="footer4">
长按扫描关注微信公众号 <br>
<img src="images/erweima.png" width="250px" height="250px">

</div>
</div>


<div class="clear1"></div>

<div class="footer4"><br>
海南威特电缆有限公司，有机会获取百元大红包
</div>

    



    </form>


</body>

</html>
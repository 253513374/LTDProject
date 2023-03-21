<%@ Page Language="C#" AutoEventWireup="true" CodeFile="home_page2.aspx.cs" Inherits="home_page2" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="renderer" content="webkit">
<meta name="format-detection" content="telephone=no" />
<meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no">
<title></title>
<link rel="stylesheet" href="css/red_record.css">
<link rel="stylesheet" href="css/windows1.css">
</head>
<body>

<div class="header">


      <div class="header-left">
       <p>个人中心</p>
       <h1>
          <img src="images/tuxiang.png" />

       </h1>

   </div>



 <div class="header-icon1"></div>
   
</div>
<div class="options-main">
  <ul>
  <li class="options-on"><p>全部记录</p></li>
  <li><p>
      红包记录</p></li>
  <li><p>抽奖记录</p></li>
  </ul>
</div>

<div id="options0">
<div class="teb" id="all" runat="server">
   <%--<ul>
   <li>
    <div class="teb-left1"><h3>领取175XXXX2130的福袋</h3><p>2017/01/18 14:27</p></div>
    <div class="teb-left2"><h3>50M</h3></div>
    <div class="teb-right3"><h3>已生效</h3></div>
   </li>
    <li>
    <div class="teb-left1"><h3>领取175XXXX2130的福袋</h3><p>2017/01/18 14:27</p></div>
    <div class="teb-left2"><h3>1000M</h3></div>
    <div class="teb-right3"><h3 class="reb1">未生效</h3></div>
   </li>
   </ul>--%>
</div>
</div>


<div id="options1">
<div class="teb teb4" id="hongbao" runat="server">
   <%--<ul>
   <li>
    <div class="teb-left1"><h3>购买流量塞进福袋</h3><p>2017/01/18 14:27</p></div>
    <div class="teb-left2"><h3>2个共200M </h3></div>
    <div class="teb-right3"><h3>10元</h3></div>
    <div class="teb-right4"><h3>成功</h3></div>
   </li>
    <li>
    <div class="teb-left1"><h3>购买流量塞进福袋</h3><p>2017/01/18 14:27</p></div>
    <div class="teb-left2"><h3>1个共100M</h3></div>
    <div class="teb-right3"><h3>5元</h3></div>
    <div class="teb-right4"><h3 class="reb1">失败</h3></div>
   </li>
   <li>
    <div class="teb-left1"><h3>购买流量塞进福袋</h3><p>2017/01/18 14:27</p></div>
    <div class="teb-left2"><h3>1个共100M</h3></div>
    <div class="teb-right3"><h3>5元</h3></div>
    <div class="teb-right4"><h3 class="reb1">受理中</h3></div>
   </li>
     <li>
    <div class="teb-left1"><h3>首次"送好友"奖励 </h3><p>2017/01/18 14:27</p></div>
    <div class="teb-left2"><h3>50M</h3></div>
    <div class="teb-right3"><h3>0元</h3></div>
    <div class="teb-right4"><h3 class="reb1">成功</h3></div>
   </li>
     <li>
    <div class="teb-left1"><h3>免费领取新年流量福袋</h3><p>2017/01/18 14:27</p></div>
    <div class="teb-left2"><h3>200M</h3></div>
    <div class="teb-right3"><h3>0元</h3></div>
    <div class="teb-right4"><h3 class="reb1">成功</h3></div>
   </li>
   </ul>--%>
</div>
</div>


<div id="options2">

<div class="teb2" id="choujiang" runat="server">
   <%--<ul>
   <li>
     <a href="#">
     <div class="teb-left1"><h3 class="text-indent1">拆福袋</h3><p>2017/01/18 14:27</p></div>
     <div class="teb-right2"><h3>50M</h3><p>已拆</p></div>
     </a>
   </li>
   <li>
    <a href="#">
    <div class="teb-left1"><h3 class="text-indent1">派发福袋</h3><p>2017/01/18 14:27</p></div>
    <div class="teb-right2"><h3>80M</h3><p>已领完</p></div>
    </a>
   </li>
    <li>
    <a href="#">
    <div class="teb-left1"><h3 class="text-indent1">派发福袋</h3><p>2017/01/18 14:27</p></div>
    <div class="teb-right2"><h3>80M</h3><p>已撤回1000M</p></div>
    </a>
   </li>
   <li>
    <a href="#">
    <div class="teb-left1"><h3 class="text-indent1">派发福袋</h3><p>2017/01/18 14:27</p></div>
    <div class="teb-right2"><h3>80M</h3><p class="reb1">未领完</p></div>
    </a>
   </li>
   </ul>--%>
</div>

</div>



     
<script src="js/jquery.min.js"></script> 
<script type="text/javascript">
    $(".new-year-but1").click(function () {
        $('#receive1').show();
    });
    $(".header-a1").click(function () {
        $('#rules').show();
    });
    $(".rules-but1").click(function () {
        $('#rules').hide();
    });
    $(".main1-but1").click(function () {
        $('#reb-envelope1').show();
    });
    $(".main1-but2").click(function () {
        $('#reb-envelope2').show();
    });

    $(".options-main ul li").click(function () {
        var index1 = $(this).index();
        $(this).addClass("options-on").siblings().removeClass("options-on");
        $('#options0,#options1,#options2').hide();
        $('#options' + index1).show();
    });


    //接受url传值
    $(document).ready(function () {
        var id = getUrlParam("type");
        if (id == "0") {
            //显示红包记录
            $('#options0,#options1,#options2').hide();
            $('#options1').show();
        }
        if (id == "1") {
            //显示抽奖记录
            $('#options0,#options1,#options2').hide();
            $('#options2').show();
        }

    });


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
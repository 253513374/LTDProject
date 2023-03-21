<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Hongbao2Code.aspx.cs" Inherits="Hongbao2Code" %>



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
<link rel="stylesheet" type="text/css" href="css/css.css">
<script type="text/javascript"  src="js/jquery-3.6.0.min.js"></script>
<script src="js/loading.js"></script>

    <script type="text/javascript" src="layer_mobile/layer.js"></script>


<style type="text/css">
         input {
            height: 2.8125rem;
            border: 1px solid #ddd;
            padding: 0.5rem .625rem;
            font-size: 1.2rem;
            border-radius: .1875rem;
            -webkit-appearance: none;
            outline: none;
            -ms-line-height: 2.8125rem;
        }

        #F_Form {
            display: flex;
            position: relative;
            margin-bottom: 1.25rem;
        }




    </style>

</head>

<body>
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
    
   
    
     <!--输入验证码开始-->
    
         <div class="content">
       
    	<div class="title">验证码</div>


         <div class="info">
        
          
           <div id="F_Form">
                <br />
                <input type="number" id="Code"  placeholder="刮开涂层输入验证码" maxlength="20">
          
            </div>

             <br />

          <div class="title" onClick="BtnList()">确定</div>
           
        </div>

  </div>
    
    <!--输入验证码结束-->
   
    
    
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
    
    <img src="img/1_51.gif" width="100%">
    <img src="img/1_52.gif" width="100%">
    <img src="img/1_53.gif" width="100%">
    <img src="img/1_54.gif" width="100%">
    <img src="img/1_55.gif" width="100%">
    <img src="img/1_56.gif" width="100%">
    <img src="img/1_57.gif" width="100%">
    
        
        
     </div>
</div>



    <script type="text/javascript">

        //判断验证码是否输入正确
        function BtnList() {
            var Code = $("#Code").val();
            if (Code == "" || Code.length == 0) {
                //信息框
                layer.open({
                    content: '请输入验证码'
                  , btn: '确定'
                });
                return;
            }


            //loading带文字
            layer.open({
                type: 2
              , content: '加载中...'
            });

            //查验真伪
            $.ajax(
                {
                    type: 'get',
                    url: 'Hongbao2Code.ashx?yzm=' + Code + '&callback?',
                    dataType: 'json',
                    jsonp: "callback",
                    success: function (dataRestul) {
                        if (dataRestul.meg == 1) {
                            //验证码正确
                            window.location = 'Hongbao2.aspx?yzm=' + Code;
                        }
                        else {
                            //信息框
                            layer.open({
                                content: dataRestul.zt
                              , btn: '确定'
                            });
                            return;
                        }

                        //关闭弹出框加载信息
                        layer.closeAll();

                    }
                });


        }


    </script>

</body>
</html>
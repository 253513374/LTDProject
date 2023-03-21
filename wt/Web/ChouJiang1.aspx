<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChouJiang1.aspx.cs" Inherits="ChouJiang" %>

<html><head>

	<meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
	<title>服装颜色选择</title>
	<meta name="description" content="Neat">
	<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" user-scalable="no">
	<link rel="stylesheet" href="./css/style1.css">
	<script src="./js/jquery.min.js"></script>
	<script src="./js/choice.js"></script>
</head>
<body>

	<div class="main-group">
		<h2 class="title">
			抽奖
		</h2>
		<div class="data-group">
			<div class="item-group">
				<a href="#">
					<div class="img-group">
						<img class="goods-img" src="./images/hongbao.jpg" alt="">
                        <p>
                            点击抽取，有机会获取本奖品
                        </p>
					</div>
				</a>
				<div class="desc-group">
					
					<div class="price-group">点击抽取，有机会获取本奖品</div>
				</div>
			</div>
			<div class="item-group">
				<a href="#">
					<div class="img-group">
						<img class="goods-img" src="./images/hongbao.jpg" alt="" style="padding: 2em;">
					</div>
				</a>
				<div class="desc-group">
					
					<div class="price-group">点击抽取，有机会获取本奖品</div>
				</div>
			</div>
			<div class="item-group">
				<a href="#">
					<div class="img-group">
                        <img class="goods-img" src="./images/hongbao.jpg" alt="" style="padding: 2em;">
						
					</div>
				</a>
				<div class="desc-group">
					
					<div class="price-group">点击抽取，有机会获取本奖品</div>
				</div>
			</div>
			
		</div>
	</div>

<script type="text/javascript">
    new SwitchGoods({
        box_class: 'item-group',

    });
</script>



</body></html>
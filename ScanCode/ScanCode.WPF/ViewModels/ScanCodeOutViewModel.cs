using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using ScanCode.Share.SignalR;
using ScanCode.WPF.HubServer.Model;
using ScanCode.WPF.HubServer.ReQuest;
using ScanCode.WPF.HubServer.Services;
using ScanCode.WPF.Model;

namespace ScanCode.WPF.ViewModels
{
    public partial class ScanCodeOutViewModel : ObservableObject
    {
        /// <summary>
        /// 出库单的详细单品信息  一个出库单有多个单品出库
        /// </summary>
        public ObservableCollection<OrderDetail> OrderListDetail { set; get; } = new();

        /// <summary>
        /// 出库单的详细单品信息  一个出库单有多个单品出库
        /// </summary>
        public ObservableCollection<W_LabelStorage> ScanOrderOutDetails { set; get; } = new();

        private readonly HubClientService? hubService;
        private readonly OutOrderService? outOrderService;

        public ScanCodeOutViewModel(HubClientService service, OutOrderService orderService)
        {
            hubService = service;
            outOrderService = orderService;

            ScanOrderOutDetails.CollectionChanged += ScanOrderOutDetails_CollectionChanged;
            if (!string.IsNullOrWhiteSpace(outOrderService?.OrdersDto?.Ddno))
            {
                _ = GetOrderDetail(outOrderService?.OrdersDto?.Ddno);
            }
        }

        public string? SelectDDNO => outOrderService?.OrdersDto?.Ddno;

        public string? SelectKH => outOrderService?.OrdersDto?.Kh;

        public string? SelectDDRQ => outOrderService?.OrdersDto?.Ddrq;

        public int? SelectTotalsl => outOrderService?.OrdersDto?.Totalsl;

        [ObservableProperty]
        private string? qrcodeKey;

        [ObservableProperty]
        private int? actualOutCount;

        [ObservableProperty]
        private string? errorInfo;

        /// <summary>
        /// 点击按钮扫码发货
        /// </summary>
        /// <param name="text"></param>
        [RelayCommand]
        private async Task ExecuteTextBox(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length < 12)
            {
                ErrorInfo = $"请输入正确的二维码序号";
                return;
            }

            var code = App.ReplaceScanCode(text);
            bool isNumber = Regex.IsMatch(code, @"^[1-9]\d*$");
            if (isNumber)
            {
                await ExecuteScanCode(code);
            }
            else
            {
                ErrorInfo = $"请输入正确的二维码序号";
                return;
            }

            QrcodeKey = "";
            return;
        }

        /// <summary>
        ///Enter按键自动扫码发货
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteScanCode(string text)
        {
            if (ActualOutCount >= SelectTotalsl)
            {
                ErrorInfo = $"当前客户订单已完成扫码工作!";
                return;
            }

            var outstorage = new W_LabelStorage();
            outstorage.OutTime = DateTime.Now;
            outstorage.OrderNumbels = SelectDDNO;
            outstorage.QRCode = text;
            outstorage.OrderTime = DateTime.Parse(SelectDDRQ);
            outstorage.Dealers = SelectDDNO;
            outstorage.OutType = "THFX";
            outstorage.ExtensionName = "";

            var result = await hubService.AddScanCodeAsync(outstorage);

            if (result.Successed)
            {
                outstorage.ExtensionName = SelectKH;
                ScanOrderOutDetails.Insert(0, outstorage);
            }
            else
            {
                ErrorInfo = $"{text}：{result.Message}";
            }

            return;
        }

        /// <summary>
        /// 返回订单号的详细信息
        /// </summary>
        /// <param name="ddno"></param>
        /// <returns></returns>
        public async Task GetOrderDetail(string? ddno)
        {
            var result = await hubService.GetOrderDetailAsync(ddno);
            OrderListDetail.Clear();
            foreach (var item in result)
            {
                // OrderListDetail.Add(item);
                OrderListDetail.Add(new OrderDetail
                {
                    DDNO = item.DDNO,
                    DDRQ = item.DDRQ,
                    KH = item.KH,
                    XH = item.XH,
                    GGXH = item.GGXH,
                    SL = item.SL,
                    DW = item.DW,
                    DJ = item.DJ,
                    YS = item.YS
                });
            }
            ActualOutCount = await GetActualOutCount();
        }

        //返回实际出库扫码数量
        public async Task<int> GetActualOutCount()
        {
            return await hubService.GetBdxOrderTotalCountAsync(SelectDDNO);
        }

        /// <summary>
        /// 当发货成功 通知发货数量自增1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanOrderOutDetails_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ActualOutCount += 1;
            }
        }
    }
}
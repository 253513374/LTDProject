using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScanCode.Share;
using ScanCode.WPF.HubServer.Services;
using ScanCode.WPF.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using ScanCode.WPF.HubServer.ReQuest;
using ScanCode.WPF.View;

namespace ScanCode.WPF.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        public ObservableCollection<GroupOrdersDTO> GroupOrdersDTOs { get; set; }

        [ObservableProperty] private string? querykeywords = "";

        [ObservableProperty] private int? collectionCount;
        [ObservableProperty] private string? loadDataTime;

        private HubClientService hubService;
        private IMapper Mapper;

        public HomeViewModel()
        {
            GroupOrdersDTOs = new ObservableCollection<GroupOrdersDTO>();
            GroupOrdersDTOs.CollectionChanged += GroupOrdersDTOs_CollectionChanged;

            hubService = App.GetService<HubClientService>();
            Mapper = App.GetService<IMapper>();
            _ = Search();
        }

        private void GroupOrdersDTOs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                CollectionCount += 1;
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                CollectionCount = 0;
            }
        }

        [RelayCommand]
        public async Task Search()
        {
            try
            {
                List<GroupedBdxOrder> resultBdxOrder;
                if (string.IsNullOrWhiteSpace(Querykeywords))
                {
                    // 使用ConfigureAwait(false)可以提高性能
                    resultBdxOrder = await hubService.GetGroupedBdxOrdersAsync().ConfigureAwait(false);
                }
                else
                {
                    // 使用ConfigureAwait(false)可以提高性能，注意：由于ConfigureAwait(false)的使用，代码可能在非UI线程上运行，
                    resultBdxOrder = await hubService.GetGroupedBdxOrdersAsync(Querykeywords).ConfigureAwait(false);
                }
                // 检查result是否为空，如果是，则创建一个新的List
                resultBdxOrder = resultBdxOrder ?? new List<GroupedBdxOrder>();
                // 如果Mapper.Map出错，使用try/catch捕获异常
                ObservableCollection<GroupOrdersDTO> mappedResult;
                try
                {
                    mappedResult = Mapper.Map<ObservableCollection<GroupOrdersDTO>>(resultBdxOrder);
                }
                catch (Exception ex)
                {
                    // 处理映射异常的代码
                    mappedResult = new ObservableCollection<GroupOrdersDTO>();

                    Debug.WriteLine(ex.Message);
                    //throw;
                }

                // 由于ConfigureAwait(false)的使用代码可能在非UI线程上运行，
                //使用Application.Current.Dispatcher.Invoke()可以确保ObservableCollection的修改在UI线程上执行，
                //即使是从异步方法中调用的。这样做是为了确保ObservableCollection的线程关联性得到尊重，从而避免错误。
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // 在添加新元素之前清空集合
                    GroupOrdersDTOs.Clear();
                    foreach (var groupOrder in mappedResult.OrderByDescending(O => O.Ddrq))
                    {
                        GroupOrdersDTOs.Add(groupOrder);
                    }
                });
            }
            catch (Exception ex)
            {
                // 处理查询异常的代码
                throw;
            }
            LoadDataTime = $"刷新时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
        }

        [RelayCommand]
        public void ItemButtonClick(object? selectedItem)
        {
            if (selectedItem is null)
            {
                return;
            }
            var outOrder = App.GetService<OutOrderService>();
            outOrder.OrdersDto = selectedItem as GroupOrdersDTO;

            /*此代码段创建了一个新的任务来执行异步操作，并没有等待该任务完成。
             Task.Run方法会在另一个线程上运行操作，并立即返回一个任务，而_ =表示忽略这个返回的任务。*/
            _ = Task.Run(async () =>
            {
                await hubService.AddAgent(new AddAgent()
                {
                    AID = outOrder?.OrdersDto?.Ddno,
                    AName = outOrder?.OrdersDto?.Kh,
                    ABelong = "公司",
                    AType = 0
                });
            });
            // 在这里处理按钮点击事件逻辑，并使用传递的选中项数据

            var scanCodeOutWindow = App.GetService<ScanCodeOutWindow>();
            scanCodeOutWindow.Owner = App.GetService<HomeWindow>();
            scanCodeOutWindow.ShowDialog();
        }

        [RelayCommand]
        public Task ReturnButtonClick()
        {
            //var outOrder = App.GetService<OutOrderService>();
            //outOrder.OrdersDto = selectedItem as GroupOrdersDTO;
            // 在这里处理按钮点击事件逻辑，并使用传递的选中项数据

            var scanCodeOutWindow = App.GetService<ScanCodeReturnWindow>();

            scanCodeOutWindow.Owner = App.GetService<HomeWindow>();
            scanCodeOutWindow.ShowDialog();

            return Task.CompletedTask;
        }

        //[RelayCommand]
        //private void OpenScanCodeOutDialog()
        //{
        //    ScanCodeOutWindow scanCodeOutWindow = new ScanCodeOutWindow();

        //    scanCodeOutWindow.ShowDialog();
        //}
    }
}
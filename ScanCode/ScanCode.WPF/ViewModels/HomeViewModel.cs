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
using ScanCode.WPF.HubServer.ReQuest;
using ScanCode.WPF.View;

namespace ScanCode.WPF.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        public ObservableCollection<GroupOrdersDTO> GroupOrdersDTOs { get; set; }

        [ObservableProperty] private string? querykeywords = "";

        [ObservableProperty] private int? collectionCount = 0;

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
        }

        [RelayCommand]
        public async Task Search()
        {
            //

            if (GroupOrdersDTOs.Any())
            {
                GroupOrdersDTOs.Clear();
            }

            if (string.IsNullOrWhiteSpace(Querykeywords))
            {
                //GroupOrdersDTOs = new ObservableCollection<GroupOrdersDTO>();
                var result = await hubService.GetGroupedBdxOrdersAsync();
                result = result.OrderByDescending(O => O.DDRQ).ToList();
                var results = new ObservableCollection<GroupOrdersDTO>(
                    Mapper.Map<ObservableCollection<GroupOrdersDTO>>(result));

                foreach (var VARIABLE in results)
                {
                    GroupOrdersDTOs?.Add(VARIABLE);
                }
            }
            else
            {
                var hubResult = await hubService.GetGroupedBdxOrdersAsync(Querykeywords);

                hubResult = hubResult.OrderByDescending(O => O.DDRQ).ToList();
                var results = new ObservableCollection<GroupOrdersDTO>(
                    Mapper.Map<ObservableCollection<GroupOrdersDTO>>(hubResult));

                //  GroupOrdersDTOs = new ObservableCollection<GroupOrdersDTO>();
                foreach (var VARIABLE in results)
                {
                    GroupOrdersDTOs?.Add(VARIABLE);
                }
            }

            //  CollectionCount = $"DDNO  {GroupOrdersDTOs.Count}";
            //  Mapper.Map(result, GroupOrdersDTOs);
            // throw new NotImplementedException();
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
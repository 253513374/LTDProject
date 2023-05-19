using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScanCode.WPF.HubServer.Services;
using ScanCode.WPF.Model;

namespace ScanCode.WPF.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private HubClientService _hubService;
        private IMapper _mapper;
        private readonly ObjectFileStorage _fileStorage;

        public MainWindowViewModel(ObjectFileStorage fileStorage)
        {
            _fileStorage = fileStorage;

            //  groupOrdersDTOs = new ObservableCollection<GroupOrdersDTO>();
            _hubService = App.GetService<HubClientService>();
            _mapper = App.GetService<IMapper>();
        }

        public BindingList<GroupOrdersDto> GroupOrdersDtOs { get; }

        [ObservableProperty]
        private string? _searchString;

        [RelayCommand]
        public async Task Search()
        {
            var result = await _hubService.GetGroupedBdxOrdersAsync();
            // Mapper.Map(result, GroupOrdersDTOs);
            // throw new NotImplementedException();
        }

        [RelayCommand]
        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
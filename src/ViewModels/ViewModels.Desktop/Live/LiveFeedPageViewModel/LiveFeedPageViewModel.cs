﻿// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop.Base;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Live;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;

namespace Bili.ViewModels.Desktop.Live
{
    /// <summary>
    /// 直播首页视图模型.
    /// </summary>
    public sealed partial class LiveFeedPageViewModel : InformationFlowViewModelBase<ILiveItemViewModel>, ILiveFeedPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveFeedPageViewModel"/> class.
        /// </summary>
        public LiveFeedPageViewModel(
            ILiveProvider liveProvider,
            IAuthorizeProvider authorizeProvider,
            IResourceToolkit resourceToolkit,
            INavigationViewModel navigationViewModel)
        {
            _liveProvider = liveProvider;
            _authorizeProvider = authorizeProvider;
            _resourceToolkit = resourceToolkit;
            _navigationViewModel = navigationViewModel;

            Banners = new ObservableCollection<IBannerViewModel>();
            Follows = new ObservableCollection<ILiveItemViewModel>();
            HotPartitions = new ObservableCollection<Models.Data.Community.Partition>();

            SeeAllPartitionsCommand = new RelayCommand(SeeAllPartitions);

            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;
            IsLoggedIn = _authorizeProvider.State == AuthorizeState.SignedIn;
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            _liveProvider.ResetFeedState();
            TryClear(Banners);
            TryClear(Follows);
            TryClear(HotPartitions);
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestLiveFailed)}\n{errorMsg}";

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            var data = await _liveProvider.GetLiveFeedsAsync();

            if (data.Banners.Any())
            {
                data.Banners.ToList().ForEach(p =>
                {
                    var vm = Locator.Instance.GetService<IBannerViewModel>();
                    vm.InjectData(p);
                    Banners.Add(vm);
                });
            }

            if (data.HotPartitions.Any())
            {
                data.HotPartitions.ToList().ForEach(p => HotPartitions.Add(p));
            }

            if (data.RecommendLives.Any())
            {
                foreach (var item in data.RecommendLives)
                {
                    var liveVM = Locator.Instance.GetService<ILiveItemViewModel>();
                    liveVM.InjectData(item);
                    Items.Add(liveVM);
                }
            }

            if (data.FollowLives.Any())
            {
                foreach (var item in data.FollowLives)
                {
                    var liveVM = Locator.Instance.GetService<ILiveItemViewModel>();
                    liveVM.InjectData(item);
                    Follows.Add(liveVM);
                }
            }

            IsFollowsEmpty = Follows.Count == 0;
        }

        private void SeeAllPartitions()
            => _navigationViewModel.NavigateToSecondaryView(PageIds.LivePartition);

        private void OnAuthorizeStateChanged(object sender, AuthorizeStateChangedEventArgs e)
            => IsLoggedIn = e.NewState == AuthorizeState.SignedIn;
    }
}

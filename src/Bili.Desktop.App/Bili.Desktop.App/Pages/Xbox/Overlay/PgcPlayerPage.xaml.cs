﻿// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Desktop.App.Pages.Base;
using Bili.Models.Data.Local;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;

namespace Bili.Desktop.App.Pages.Xbox.Overlay
{
    /// <summary>
    /// PGC 播放页面.
    /// </summary>
    public sealed partial class PgcPlayerPage : PgcPlayerPageBase
    {
        private bool _isLikeHoldCompleted;
        private bool _isLikeHoldSuspend;

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcPlayerPage"/> class.
        /// </summary>
        public PgcPlayerPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PlaySnapshot shot)
            {
                ViewModel.SetSnapshot(shot);
            }
        }

        /// <inheritdoc/>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
            => ViewModel.ClearCommand.Execute(null);

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.GamepadLeftShoulder)
            {
                // 后退
                ViewModel.MediaPlayerViewModel.BackwardSkipCommand.ExecuteAsync(null);
            }
            else if (e.Key == Windows.System.VirtualKey.GamepadRightShoulder)
            {
                // 跳进
                ViewModel.MediaPlayerViewModel.ForwardSkipCommand.ExecuteAsync(null);
            }
            else if (e.Key == Windows.System.VirtualKey.GamepadB)
            {
                // 关闭控制器.
                if (ViewModel.MediaPlayerViewModel.IsShowMediaTransport)
                {
                    ViewModel.MediaPlayerViewModel.IsShowMediaTransport = false;
                    e.Handled = true;
                }
            }
        }

        private void OnSectionHeaderItemInvoked(object sender, Models.App.Other.PlayerSectionHeader e)
        {
            if (ViewModel.CurrentSection != e)
            {
                ViewModel.CurrentSection = e;
            }
        }

        private void OnRefreshFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
            => ViewModel.RequestFavoriteFoldersCommand.ExecuteAsync(null);

        private void OnGiveCoinButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var num = int.Parse((sender as FrameworkElement).Tag.ToString());
            ViewModel.CoinCommand.Execute(num);
        }

        private void OnLikeButtonHoldingCompleted(object sender, System.EventArgs e)
        {
            _isLikeHoldCompleted = true;
            ViewModel.TripleCommand.ExecuteAsync(null);
            CoinButton.ShowBubbles();
            FavoriteButton.ShowBubbles();
        }

        private void OnLikeButtonHoldingSuspend(object sender, EventArgs e)
            => _isLikeHoldSuspend = true;

        private void OnLikeButtonClick(object sender, RoutedEventArgs e)
        {
            if (_isLikeHoldCompleted || _isLikeHoldSuspend)
            {
                _isLikeHoldCompleted = false;
                _isLikeHoldSuspend = false;
                return;
            }

            ViewModel.LikeCommand.ExecuteAsync(null);
        }

        private void OnCoinButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.IsCoined = !ViewModel.IsCoined;
            ViewModel.IsCoined = !ViewModel.IsCoined;

            if (!ViewModel.IsCoined)
            {
                CoinFlyout.ShowAt(CoinButton);
            }
        }

        private void OnFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
        {
            ViewModel.IsFavorited = !ViewModel.IsFavorited;
            ViewModel.IsFavorited = !ViewModel.IsFavorited;

            if (ViewModel.FavoriteFolders.Count == 0)
            {
                ViewModel.RequestFavoriteFoldersCommand.ExecuteAsync(null);
            }

            FavoriteFlyout.ShowAt(FavoriteButton);
        }
    }
}

﻿// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.Models.Data.Local;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls.Dialogs
{
    /// <summary>
    /// 继续播放对话框.
    /// </summary>
    public sealed partial class ContinuePlayDialog : ContentDialog
    {
        private readonly INavigationViewModel _navigationViewModel;
        private readonly IRecordViewModel _recordViewModel;
        private PlaySnapshot _snapshot = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuePlayDialog"/> class.
        /// </summary>
        public ContinuePlayDialog()
        {
            InitializeComponent();
            _navigationViewModel = Locator.Instance.GetService<INavigationViewModel>();
            _recordViewModel = Locator.Instance.GetService<IRecordViewModel>();
            Loaded += OnLoadedAsync;
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            var settingsToolkit = Locator.Instance.GetService<ISettingsToolkit>();
            _snapshot = await _recordViewModel.GetLastPlayItemAsync();
            if (_snapshot == null)
            {
                settingsToolkit.WriteLocalSetting(Models.Enums.SettingNames.CanContinuePlay, false);
                Hide();
            }

            VideoTitle.Text = _snapshot.Title ?? string.Empty;
        }

        private void OnContentDialogPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
            => _navigationViewModel.NavigateToPlayView(_snapshot);

        private void OnContentDialogCloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
            => _recordViewModel.DeleteLastPlayItemCommand.ExecuteAsync(null);
    }
}

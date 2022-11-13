﻿// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Bili.Models.App.Constants;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 主题设置.
    /// </summary>
    public sealed partial class ThemeSettingSection : SettingSectionControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeSettingSection"/> class.
        /// </summary>
        public ThemeSettingSection()
            : base()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            switch (ViewModel.AppTheme)
            {
                case AppConstants.ThemeDefault:
                    SystemThemeRadioButton.IsChecked = true;
                    break;
                case AppConstants.ThemeLight:
                    LightThemeRadioButton.IsChecked = true;
                    break;
                case AppConstants.ThemeDark:
                    DarkThemeRadioButton.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        private void OnThemeRadioButtonSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Any() && e.AddedItems.First() is RadioButton btn)
            {
                var selectTheme = string.Empty;
                if (btn == LightThemeRadioButton)
                {
                    selectTheme = AppConstants.ThemeLight;
                }
                else if (btn == DarkThemeRadioButton)
                {
                    selectTheme = AppConstants.ThemeDark;
                }
                else
                {
                    selectTheme = AppConstants.ThemeDefault;
                }

                ViewModel.AppTheme = selectTheme;
            }
        }
    }
}

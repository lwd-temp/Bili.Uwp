﻿// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums;
using Bili.Models.Enums.Player;
using Bili.Toolkit.Interfaces;
using Microsoft.UI.Xaml.Data;

namespace Bili.Desktop.App.Resources.Converter
{
    internal sealed class PlayerTypeConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
            if (value is PlayerType decode)
            {
                switch (decode)
                {
                    case PlayerType.Native:
                        result = resourceToolkit.GetLocaleString(LanguageNames.NativePlayer);
                        break;
                    case PlayerType.FFmpeg:
                        result = resourceToolkit.GetLocaleString(LanguageNames.FFmpegPlayer);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}

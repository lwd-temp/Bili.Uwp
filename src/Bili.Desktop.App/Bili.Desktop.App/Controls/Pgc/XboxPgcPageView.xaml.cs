﻿// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls.Pgc
{
    /// <summary>
    /// 适用于 Xbox 的 PGC 页面视图.
    /// </summary>
    public sealed partial class XboxPgcPageView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(IPgcPageViewModel), typeof(XboxPgcPageView), new PropertyMetadata(default, new PropertyChangedCallback(OnViewModelChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="XboxPgcPageView"/> class.
        /// </summary>
        public XboxPgcPageView() => InitializeComponent();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public IPgcPageViewModel ViewModel
        {
            get { return (IPgcPageViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 核心视图模型.
        /// </summary>
        public IAppViewModel CoreViewModel { get; } = Locator.Instance.GetService<IAppViewModel>();

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as XboxPgcPageView;
            instance.DataContext = e.NewValue;
        }
    }
}

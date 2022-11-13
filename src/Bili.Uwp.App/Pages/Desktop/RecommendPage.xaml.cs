﻿// Copyright (c) Richasy. All rights reserved.

using Bili.Uwp.App.Pages.Base;

namespace Bili.Uwp.App.Pages.Desktop
{
    /// <summary>
    /// 推荐视频页面.
    /// </summary>
    public sealed partial class RecommendPage : RecommendPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendPage"/> class.
        /// </summary>
        public RecommendPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }
}

﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Screens.Menu;

namespace osu.Game.Tests.Visual
{
    public class TestCaseHoldToConfirmOverlay : OsuTestCase
    {
        public override IReadOnlyList<Type> RequiredTypes => new[] { typeof(ExitConfirmOverlay) };

        public TestCaseHoldToConfirmOverlay()
        {
            bool fired = false;

            var abortText = new OsuSpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Aborted!",
                TextSize = 50,
                Alpha = 0,
            };

            var overlay = new TestHoldToConfirmOverlay
            {
                Action = () =>
                {
                    fired = true;
                    abortText.FadeTo(1).Then().FadeOut(1000);
                }
            };

            Children = new Drawable[]
            {
                overlay,
                abortText
            };

            AddStep("start confirming", () => overlay.Begin());
            AddStep("abort confirming", () => overlay.Abort());

            AddAssert("ensure aborted", () => !fired);

            AddStep("start confirming", () => overlay.Begin());

            AddUntilStep(() => fired, "wait until confirmed");
        }

        private class TestHoldToConfirmOverlay : ExitConfirmOverlay
        {
            protected override bool AllowMultipleFires => true;

            public void Begin() => BeginConfirm();
            public void Abort() => AbortConfirm();
        }
    }
}

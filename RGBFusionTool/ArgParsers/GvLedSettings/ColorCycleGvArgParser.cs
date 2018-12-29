﻿// Copyright (C) 2018 Tyler Szabo
//
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.

using GvLedLibDotNet.GvLedSettings;
using System;
using Mono.Options;
using System.Collections.Generic;

namespace RGBFusionTool.ArgParsers.LedSettings
{
    class ColorCycleGvArgParser : LedSettingArgParser<GvLedSetting>
    {
        private class ColorCycleGvArgParserContext : ArgParserContext
        {
            public byte Brightness { get; set; }
            public byte MinBrightness { get; set; }
            public byte NumColors { get; set; }
            public bool Pulse { get; set; }

            private uint seconds;
            public uint Seconds
            {
                get => seconds; set
                {
                    Valid = true;
                    seconds = value;
                }
            }

            protected override void SetDefaults()
            {
                seconds = 1;
                Brightness = 11;
                MinBrightness = 0;
                NumColors = 7;
                Pulse = false;
            }
        }

        ColorCycleGvArgParserContext context;
        
        private ColorCycleGvArgParser(ColorCycleGvArgParserContext context) : base(context)
        {
            this.context = context;
        }

        public ColorCycleGvArgParser() : this (new ColorCycleGvArgParserContext())
        {
            RequiredOptions = new OptionSet
            {
                { "Color cycle" },
                { "cycle|colorcycle:", "cycle colors, changing color every {SECONDS}", (uint d) => context.Seconds = d },
            };
            ExtraOptions = new OptionSet
            {
                { "b|brightness=", "(optional) brightness (0-11)", (byte b) => context.Brightness = b },
                { "<>", v => throw new InvalidOperationException(string.Format("Unsupported option {0}", v)) }
            };
        }

        public override GvLedSetting TryParse(IEnumerable<string> args)
        {
            if (!PopulateContext(args))
            {
                return null;
            }

            //TimeSpan cycleTime = TimeSpan.FromSeconds(context.Seconds ?? 1);

            return new ColorCycleGvLedSetting(context.Seconds, context.Brightness);
        }
    }
}

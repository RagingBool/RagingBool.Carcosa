// [[[[INFO>
// Copyright 2015 Raging Bool (http://ragingbool.org, https://github.com/RagingBool)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// For more information check https://github.com/RagingBool/RagingBool.Carcosa
// ]]]]

using Epicycle.Commons;
using RagingBool.Carcosa.Core.Stage.Lights;
using RagingBool.Carcosa.Core.Stage.Scenes.Signal;
using System;

namespace RagingBool.Carcosa.Core.Stage.Scenes
{
    internal sealed class LedMatrixTestPattern
    {
        private Oscillator _oscillator;
        private ILedMatrix _ledMatrix;
        private double[,] _matrix;
        private int _x1;
        private int _x2;
        private int _y;
        private double _hue1;
        private double _hue2;
        private Random _random;

        public LedMatrixTestPattern(ILedMatrix ledMatrix)
        {
            _ledMatrix = ledMatrix;
            _oscillator = new Oscillator();
            _oscillator.Function = Oscillator.FunctionType.SawUp;
            _oscillator.Frequency = 1;

            _matrix = new double[ledMatrix.Dimensions.X, ledMatrix.Dimensions.Y];

            _random = new Random();
            New();
        }

        public double Saturation { get; set; }

        public double Intensity { get; set; }

        public double Hue0 { get; set; }
        public double Hue1 { get; set; }
        public double Hue2 { get; set; }
        public double Hue3 { get; set; }

        public bool Strobe { get; set; }

        public void Step()
        {
            _y++;

            if(_y >= _ledMatrix.Dimensions.Y)
            {
                New();
            }

            for(var x = _x1;  x <= _x2; x++)
            {
                var hue = BasicMath.Interpolate((((double)x) - _x1) / (_x2 - _x1), _hue1, _hue2);
                _matrix[x, _y] = hue;
            }
        }

        public void New()
        {
            _y = 0;
            _x1 = _random.Next(_ledMatrix.Dimensions.X - 3);
            _x2 = Math.Min(_x1 + _random.Next(9) + 2, _ledMatrix.Dimensions.X - 1);

            _hue1 = RandomHue();
            _hue2 = RandomHue();
        }

        private double RandomHue()
        {
            switch (_random.Next(4))
            {
                case 0:
                    return Hue0;
                case 1:
                    return Hue1;
                case 2:
                    return Hue2;
                default:
                    return Hue3;
            }
        }

        public void Update(double dt)
        {
            _oscillator.Update(dt);

            Render();
        }

        private void Render()
        {
            for (var x = 0; x < _ledMatrix.Dimensions.X; x++)
            {
                for (var y = 0; y < _ledMatrix.Dimensions.Y; y++)
                {
                    if (!Strobe)
                    {
                        LightUtils.SetRgbLightToHsi(_ledMatrix[x, y], _matrix[x, y], Saturation, Intensity);
                    }
                    else
                    {
                        _ledMatrix[x, y].Red = 1;
                        _ledMatrix[x, y].Green = 1;
                        _ledMatrix[x, y].Blue = 1;
                    }
                }
            }
        }
    }
}

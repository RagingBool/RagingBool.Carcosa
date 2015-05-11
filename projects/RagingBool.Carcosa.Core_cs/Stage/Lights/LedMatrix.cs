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

using Epicycle.Math.Geometry;
using System.Collections.Generic;

namespace RagingBool.Carcosa.Core.Stage.Lights
{
    internal sealed class LedMatrix : ILedMatrix
    {
        private readonly Vector2i _dimensions;
        private readonly IReadOnlyList<IRgbLight> _pixels;

        public LedMatrix(IReadOnlyList<IRgbLight> pixels, Vector2i dimensions)
        {
            _pixels = pixels;
            _dimensions = dimensions;
        }

        public Vector2i Dimensions
        {
            get { return _dimensions; }
        }

        public IRgbLight this[int x, int y]
        {
            get { return _pixels[y * _dimensions.X + x]; }
        }
    }
}

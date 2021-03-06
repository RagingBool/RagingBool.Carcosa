﻿// [[[[INFO>
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

using CannedBytes.Midi;
using CannedBytes.Midi.Message;
using Epicycle.Commons;
using Epicycle.Commons.Time;
using Epicycle.Input;
using Epicycle.Input.Controllers;
using Epicycle.Input.Keyboard;
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;

namespace RagingBool.Carcosa.Devices.Midi
{
    public sealed class MidiLpd8 : IControlBoard, IDevice, IUpdatable
    {
        public static readonly int FixedNumberOfButtons = 8;
        public static readonly int FixedNumberOfKnobs = 8;

        private readonly IClock _clock;
        private readonly int _midiInDeviceId;
        private readonly int _midiOutDeviceId;

        private readonly MidiInPort _midiInPort;
        private readonly MidiMessageFactory _midiMessageFactory;

        private readonly MidiOutPort _midiOutPort;

        private readonly ManualKeyboard<int, TimedKeyVelocity> _buttonsKeyboard;
        private readonly ManualControllerBoard<int, double> _controllerBoard;
        private readonly UpdatingButtonLights _buttonLights;

        private bool[] _buttonLightsState;

        public MidiLpd8(IClock clock, int midiInDeviceId, int midiOutDeviceId)
        {
            _clock = clock;
            _midiInDeviceId = midiInDeviceId;
            _midiOutDeviceId = midiOutDeviceId;

            _buttonLightsState = new bool[FixedNumberOfButtons];
            for (var i = 0; i < _buttonLightsState.Length; i++)
            {
                _buttonLightsState[i] = false;
            }

            _midiInPort = new MidiInPort();
            _midiMessageFactory = new MidiMessageFactory();

            _midiInPort.Successor = new MidiReceiver(this);

            _midiOutPort = new MidiOutPort();

            _buttonsKeyboard = new ManualKeyboard<int, TimedKeyVelocity>();
            _controllerBoard = new ManualControllerBoard<int, double>(0);
            _buttonLights = new UpdatingButtonLights(this);
        }

        public void Connect()
        {
            _midiInPort.Open(_midiInDeviceId);
            _midiInPort.Start();

            _midiOutPort.Open(_midiOutDeviceId);
        }

        public void Disconnect()
        {
            _midiInPort.Stop();
            _midiInPort.Close();

            _midiOutPort.Close();
        }

        public void Update()
        {
            // Nothing to update...
        }

        private void ShortMessageReceived(int data)
        {
            var time = _clock.Time;
            var message = _midiMessageFactory.CreateShortMessage(data);

            if (message is MidiChannelMessage)
            {
                var midiChannelMessage = message as MidiChannelMessage;

                if (midiChannelMessage.Command == MidiChannelCommand.ControlChange)
                {
                    var controllerId = midiChannelMessage.Parameter1 - 1;
                    var value = MidiValueToControllerValue(midiChannelMessage.Parameter2);

                    var eventArgs = new ControllerChangeEventArgs<int, double>(controllerId, value);
                    _controllerBoard.ProcessControllerChangeEvent(eventArgs);
                }
                else
                {
                    var buttonEventType = midiChannelMessage.Command == MidiChannelCommand.NoteOn ? KeyEventType.Pressed : KeyEventType.Released;
                    var keyId = midiChannelMessage.Parameter1 - 36;
                    var velocity = midiChannelMessage.Parameter2 * 2;

                    SendState();

                    var timedKeyVelocity = new TimedKeyVelocity(time, velocity);
                    
                    _buttonsKeyboard.ProcessKeyEvent(new KeyEventArgs<int, TimedKeyVelocity>(keyId, buttonEventType, timedKeyVelocity));
                }
            }
        }

        private static double MidiValueToControllerValue(byte midiValue)
        {
            if(midiValue == 0)
            {
                return 0.0;
            }
            else if (midiValue >= 127)
            {
                return 1.0;
            }

            return midiValue / 127.0;
        }

        public IKeyboard<int, TimedKeyVelocity> Buttons { get { return _buttonsKeyboard; } }

        public IControllerBoard<int, double> Controllers { get { return _controllerBoard; } }

        public IIndicatorBoard<int, bool> ButtonLights { get { return _buttonLights; } }

        private void SendState()
        {
            for(var i = 0; i < _buttonLightsState.Length; i++)
            {
                var state = _buttonLightsState[i];

                var midiData = new MidiData();

                midiData.Channel = 1;
                midiData.Parameter1 = (byte)(36 + i);
                midiData.Parameter2 = 127;
                midiData.Status = (byte)(state ? MidiChannelCommand.NoteOn : MidiChannelCommand.NoteOff);

                _midiOutPort.ShortData(midiData.Data);
            }
        }

        private class MidiReceiver : IMidiDataReceiver
        {
            private MidiLpd8 _device;

            public MidiReceiver(MidiLpd8 device)
            {
                _device = device;
            }

            public void ShortData(int data, long timestamp)
            {
                _device.ShortMessageReceived(data);
            }

            public void LongData(MidiBufferStream buffer, long timestamp)
            {
                // Nothing to do...
            }
        }

        private class UpdatingButtonLights : IndicatorBoardBase<int, bool>
        {
            private readonly MidiLpd8 _parent;

            public UpdatingButtonLights(MidiLpd8 parent)
                : base(false)
            {
                _parent = parent;
            }

            protected override void IndicatorValueChanges(int indicatorId, bool value)
            {
                _parent._buttonLightsState[indicatorId] = value;
                _parent.SendState();
            }
        }
    }
}

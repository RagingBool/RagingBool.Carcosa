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

using CannedBytes.Midi;
using CannedBytes.Midi.Message;
using Epicycle.Commons.Time;
using Epicycle.Input.Controllers;
using Epicycle.Input.Keyboard;
using RagingBool.Carcosa.Devices.InputControl;
using RagingBool.Carcosa.Devices.InputControl.ControlBoard;
using System;

namespace RagingBool.Carcosa.Devices.Midi
{
    public sealed class MidiLpd8 : IControlBoard, IDevice, IUpdatable
    {
        public static readonly int FixedNumberOfButtons = 8;
        public static readonly int FixedNumberOfKnobs = 8;

        private readonly IClock _clock;
        private readonly int _midiInDeviceId;
        private readonly int _midiOutDeviceId;

        private MidiInPort _midiInPort;
        private MidiMessageFactory _midiMessageFactory;

        private MidiOutPort _midiOutPort;

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

        public int NumberOfButtons
        {
            get { return FixedNumberOfButtons; }
        }

        public int NumberOfControllers
        {
            get { return FixedNumberOfKnobs; }
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
                    if (OnControllerChange != null)
                    {
                        var controllerId = midiChannelMessage.Parameter1 - 1;
                        var value = midiChannelMessage.Parameter2 * 2;

                        OnControllerChange(this, new ControllerChangeEventArgs<int, int>(controllerId, value));
                    }
                }
                else if (OnButtonEvent != null)
                {
                    var buttonEventType = midiChannelMessage.Command == MidiChannelCommand.NoteOn ? KeyEventType.Pressed : KeyEventType.Released;
                    var keyId = midiChannelMessage.Parameter1 - 36;
                    var velocity = midiChannelMessage.Parameter2 * 2;

                    SendState();

                    var timedKeyVelocity = new TimedKeyVelocity(time, velocity);
                    OnButtonEvent(this, new KeyEventArgs<int, TimedKeyVelocity>(keyId, buttonEventType, timedKeyVelocity));
                }
            }
        }

        public event EventHandler<KeyEventArgs<int, TimedKeyVelocity>> OnButtonEvent;
        public event EventHandler<ControllerChangeEventArgs<int, int>> OnControllerChange;

        public void SetKeyLightState(int id, bool newState)
        {
            _buttonLightsState[id] = newState;
            SendState();
        }

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
    }
}

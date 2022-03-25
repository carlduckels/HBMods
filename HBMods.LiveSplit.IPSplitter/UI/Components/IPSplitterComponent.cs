using LiveSplit.Model;
using LiveSplit.UI.Components.Network;
using LiveSplitCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiveSplit.UI.Components
{
    public class IPSplitterComponent : IComponent
    {
        protected IPSplitterTextComponent InternalComponent { get; set; }
        public IPSplitterComponentSettings Settings { get; set; }


        public string ComponentName => "IP Split";


        public float VerticalHeight => Settings.ShowInfo ? InternalComponent.VerticalHeight : 0;
        public float MinimumWidth => Settings.ShowInfo ? InternalComponent.MinimumWidth : 0;
        public float HorizontalWidth => Settings.ShowInfo ? InternalComponent.HorizontalWidth : 0;
        public float MinimumHeight => Settings.ShowInfo ? InternalComponent.MinimumHeight : 0;

        public float PaddingTop => Settings.ShowInfo ? InternalComponent.PaddingTop : 0;
        public float PaddingLeft => Settings.ShowInfo ? InternalComponent.PaddingLeft : 0;
        public float PaddingBottom => Settings.ShowInfo ? InternalComponent.PaddingBottom : 0;
        public float PaddingRight => Settings.ShowInfo ? InternalComponent.PaddingRight : 0;


        public IDictionary<string, Action> ContextMenuControls => null;


        private NetTcpServer _tcpServer;
        private TimerModel _timer;
        
        private EnumRunState _runState = EnumRunState.Undefined;
        private string _pauseText = "Timer Paused";
        

        List<ISegment> _segments = new List<ISegment>();

        Stopwatch _playingTime = new Stopwatch();


        public IPSplitterComponent(LiveSplitState state)
        {
            Settings = new IPSplitterComponentSettings()
            {
                CurrentState = state
            };

            InternalComponent = new IPSplitterTextComponent(Settings);

            _timer = new TimerModel { CurrentState = state };

            AttachSettingsEvents(true);

            _timer.InitializeGameTime();

            StartServer(1966);

            UpdateFromSplitState(state);
        }


        public void Dispose()
        {
            AttachSettingsEvents(false);

            if (_tcpServer == null)
            {
                _tcpServer.CloseTcpListener();

                _tcpServer.OnConnectionStateChanged -= TcpServer_OnConnectionStateChanged;
                _tcpServer.OnException -= TcpServer_OnException;
                _tcpServer.OnClientAdded -= TcpServer_OnClientAdded;
                _tcpServer.OnClientRemoved -= TcpServer_OnClientRemoved;
                _tcpServer.OnClientConnectionStateChanged -= TcpServer_OnClientConnectionStateChanged;
                _tcpServer.OnClientDataRx -= TcpServer_OnClientDataRx;
                _tcpServer.OnClientException -= TcpServer_OnClientException;
            }
        }


        private void AttachSettingsEvents(bool attach)
        {
            Settings.CurrentState.OnStart -= _timer_OnStart;
            Settings.CurrentState.OnSplit -= _timer_OnSplit;
            Settings.CurrentState.OnPause -= _timer_OnPause;
            Settings.CurrentState.OnResume -= _timer_OnResume;
            Settings.CurrentState.OnReset -= _timer_OnReset;
            
            if (attach)
            {
                Settings.CurrentState.OnStart += _timer_OnStart;
                Settings.CurrentState.OnSplit += _timer_OnSplit;
                Settings.CurrentState.OnPause += _timer_OnPause;
                Settings.CurrentState.OnResume += _timer_OnResume;
                Settings.CurrentState.OnReset += _timer_OnReset;
            }
        }
        

        private void _timer_OnPause(object sender, EventArgs e)
        {
            RunState = EnumRunState.Paused;
        }


        private void _timer_OnResume(object sender, EventArgs e)
        {
            RunState = EnumRunState.Running;
        }


        private void StoreSplitObject()
        {
            ISegment segment = _timer.CurrentState.CurrentSplit;
            
            if (segment == null)
                return;

            if (!_segments.Contains(segment))
            {
                _segments.Add(segment);
            }
        }


        private void ClearSplitIcons()
        {
            try
            {
                var a = _segments.ToArray();

                _segments.Clear();

                foreach (ISegment segment in a)
                {
                    segment.Icon = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
        }


        private void StartServer(int port)
        {
            if (_tcpServer == null)
            {
                _tcpServer = new NetTcpServer();

                _tcpServer.OnConnectionStateChanged += TcpServer_OnConnectionStateChanged;
                _tcpServer.OnException += TcpServer_OnException;
                _tcpServer.OnClientAdded += TcpServer_OnClientAdded;
                _tcpServer.OnClientRemoved += TcpServer_OnClientRemoved;
                _tcpServer.OnClientConnectionStateChanged += TcpServer_OnClientConnectionStateChanged;
                _tcpServer.OnClientDataRx += TcpServer_OnClientDataRx;
                _tcpServer.OnClientException += TcpServer_OnClientException;
            }

            _tcpServer.OpenTcpListener(Settings.Port);
        }


        private void TcpServer_OnConnectionStateChanged(NetTcpServer server, EnumConnecitonState arg2)
        {
            UpdateServerStatus();
        }


        private void TcpServer_OnException(NetTcpServer server, string arg2, Exception arg3)
        {
            UpdateServerStatus();
        }


        private void TcpServer_OnClientAdded(NetTcpServer server, NetTcpServerClient client)
        {
            UpdateServerStatus();
        }


        private void TcpServer_OnClientRemoved(NetTcpServer server, NetTcpServerClient client)
        {
            UpdateServerStatus();
        }


        private void TcpServer_OnClientConnectionStateChanged(NetTcpServer server, NetTcpServerClient client, EnumConnecitonState arg3)
        {
            UpdateServerStatus();
        }


        private void TcpServer_OnClientDataRx(NetTcpServer server, NetTcpServerClient client, byte[] data)
        {
            HandleDataRx(data);    // Client Data Rx
        }


        private void TcpServer_OnClientException(NetTcpServer server, NetTcpServerClient client, string msg, Exception ex)
        {
            UpdateServerStatus();
        }


        private void UpdateServerStatus()
        {
            Settings.UpdateServerStatus();
        }


        private void HandleDataRx(byte[] data)
        {
            var str = Encoding.UTF8.GetString(data);

            if (HandleSplitWithIcon(str))
                return;
            
            if (HandleDeathCounter(str))
                return;

            if (HandleLogoutCounter(str))
                return;

            switch (str)
            {
                case AppConsts.NET_RESET:
                    DoReset();
                    break;

                case AppConsts.NET_START:
                    DoStart();
                    break;

                case AppConsts.NET_SPLIT:
                    DoSplit();
                    break;

                case AppConsts.NET_PAUSE:
                    DoPause();
                    break;

                case AppConsts.NET_RESUME:
                    DoResume();
                    break;

                default:
                    break;
            }
        }


        private bool HandleSplitWithIcon(string str)
        {
            if (str.StartsWith($"{AppConsts.NET_SPLIT} "))
            {
                try
                {
                    var iconName = str.Substring(AppConsts.NET_SPLIT.Length + 1);

                    if (!string.IsNullOrEmpty(iconName))
                    {
                        string iconFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", $"{iconName}.png");

                        if (File.Exists(iconFile))
                        {
                            Image icon = Image.FromFile(iconFile);

                            _timer.CurrentState.CurrentSplit.Icon = icon;
                        }
                        else
                        {
                            _timer.CurrentState.CurrentSplit.Icon = null;
                        }
                    }
                }
                catch (Exception)
                {
                    //
                }

                DoSplit();

                return true;
            }

            return false;
        }


        private bool HandleDeathCounter(string str)
        {
            if (str.StartsWith($"{AppConsts.NET_DEATH} "))
            {
                try
                {
                    var countStr = str.Substring(AppConsts.NET_DEATH.Length + 1);

                    if (int.TryParse(countStr, out int count))
                    {
                        Settings.DeathCount++;
                        UpdateServerStatus();
                    }
                }
                catch (Exception)
                {
                    //
                }

                return true;
            }

            return false;
        }


        private bool HandleLogoutCounter(string str)
        {
            if (str.StartsWith($"{AppConsts.NET_LOGOUT} "))
            {
                try
                {
                    var countStr = str.Substring(AppConsts.NET_LOGOUT.Length + 1);

                    if (int.TryParse(countStr, out int count))
                    {
                        Settings.LogoutCount++;
                        UpdateServerStatus();
                    }
                }
                catch (Exception)
                {
                    //
                }

                return true;
            }

            return false;
        }


        private void _timer_OnReset(object sender, TimerPhase value)
        {
            _playingTime.Stop();

            ClearSplitIcons();

            Settings.SplitCount = 0;
            Settings.DeathCount = 0;
            Settings.LogoutCount = 0;

            UpdateFromSplitState(sender);
        }


        private void _timer_OnStart(object sender, EventArgs e)
        {
            _playingTime.Restart();
            StoreSplitObject();
            UpdateFromSplitState(sender);
        }


        private void _timer_OnSplit(object sender, EventArgs e)
        {
            Settings.SplitCount++; 
            StoreSplitObject();
            UpdateFromSplitState(sender);
        }


        private void UpdateFromSplitState(object sender)
        {
            if (sender is LiveSplitState splitState)
            {
                switch (splitState.CurrentPhase)
                {
                    case TimerPhase.NotRunning:
                        RunState = EnumRunState.Reset;
                        break;
                    case TimerPhase.Running:
                        RunState = EnumRunState.Running;
                        break;
                    case TimerPhase.Ended:
                        RunState = EnumRunState.Reset;
                        break;
                    case TimerPhase.Paused:
                        RunState = EnumRunState.Paused;
                        break;
                    default:
                        break;
                }
            }
        }


        private void DoStart()
        {
            switch (_runState)
            {
                case EnumRunState.Reset:
                    _timer.Start();
                    break;
                case EnumRunState.Running:
                    _timer.Split();
                    break;
                case EnumRunState.Paused:
                    _timer.Pause();
                    break;
                case EnumRunState.Finished:
                default:
                    break;
            }
        }
        
        
        private void DoSplit()
        {
            switch (_runState)
            {
                case EnumRunState.Reset:
                    _timer.Start();
                    break;
                case EnumRunState.Running:
                    _timer.Split();
                    break;
                case EnumRunState.Paused:
                    _timer.Pause();
                    break;
                case EnumRunState.Finished:
                    break;
                default:
                    break;
            }
        }


        private void DoReset()
        {
            _timer.Reset();
        }


        private void DoPause()
        {
            if (Settings.IgnorePauses)
                return;

            if (_runState == EnumRunState.Running)
                _timer.Pause();
        }
        
        
        private void DoResume()
        {
            // Always allow Resumes if we are paused
            if (Settings.IgnorePauses && (_runState != EnumRunState.Paused))
                return;

            if (_runState == EnumRunState.Paused)
                _timer.Pause();
        }


        private void PrepareDraw(LiveSplitState state, LayoutMode mode)
        {
            InternalComponent.DisplayTwoRows = Settings.DisplayTwoRows;

            InternalComponent.NameLabel.HasShadow = (InternalComponent.ValueLabel.HasShadow = state.LayoutSettings.DropShadows);

            InternalComponent.NameLabel.HorizontalAlignment = StringAlignment.Near;
            InternalComponent.ValueLabel.HorizontalAlignment = StringAlignment.Far;

            InternalComponent.NameLabel.VerticalAlignment = (mode == LayoutMode.Horizontal || Settings.DisplayTwoRows)
                ? StringAlignment.Near 
                : StringAlignment.Center;
            InternalComponent.ValueLabel.VerticalAlignment = (mode == LayoutMode.Horizontal || Settings.DisplayTwoRows)
                ? StringAlignment.Far 
                : StringAlignment.Center;
        
            InternalComponent.NameLabel.ForeColor = state.LayoutSettings.TextColor;
            InternalComponent.ValueLabel.ForeColor = state.LayoutSettings.TextColor;
        }


        private void DrawBackground(Graphics g, LiveSplitState state, float width, float height)
        {
            if (Settings.BackgroundColor.A > 0
                || Settings.BackgroundGradient != GradientType.Plain
                && Settings.BackgroundColor2.A > 0)
            {
                var gradientBrush = new LinearGradientBrush(
                            new PointF(0, 0),
                            Settings.BackgroundGradient == GradientType.Horizontal
                            ? new PointF(width, 0)
                            : new PointF(0, height),
                            Settings.BackgroundColor,
                            Settings.BackgroundGradient == GradientType.Plain
                            ? Settings.BackgroundColor
                            : Settings.BackgroundColor2);
                g.FillRectangle(gradientBrush, 0, 0, width, height);
            }
        }


        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            DrawBackground(g, state, width, VerticalHeight);
            PrepareDraw(state, LayoutMode.Vertical);
            InternalComponent.DrawVertical(g, state, width, clipRegion);
        }


        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
            DrawBackground(g, state, HorizontalWidth, height);
            PrepareDraw(state, LayoutMode.Horizontal);
            InternalComponent.DrawHorizontal(g, state, height, clipRegion);
        }

        
        internal EnumRunState RunState 
        { 
            get => _runState;
            set 
            {
                if (_runState == value)
                    return;
                _runState = value;
            }
        }


        public Control GetSettingsControl(LayoutMode mode)
        {
            Settings.Mode = mode;
            return Settings;
        }


        public void SetSettings(System.Xml.XmlNode settings)
        {
            Settings.SetSettings(settings);
        }


        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return Settings.GetSettings(document);
        }


        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (Settings.ShowInfo_Deaths)
            {
                InternalComponent.NameLabel.Text = "Server Status:";
                InternalComponent.ValueLabel.Text = Settings.StatusText;
            }


            if (Settings.ShowInfo_TCPStatus)
            {
                InternalComponent.InformationName = "Server Status:";
                InternalComponent.InformationValue = Settings.StatusText;
            }

            InternalComponent.LongestString = Settings.StatusText;

            InternalComponent.Update(invalidator, state, width, height, mode);
        }


        public int GetSettingsHashCode() => Settings.GetSettingsHashCode();
    }
}

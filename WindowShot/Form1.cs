using Accord.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowShot
{
    public partial class Form1 : Form
    {

        string _basePath = @"C:\tmp\WindowShot";

        VideoFileWriter _videoWriter;
        TimeSpan _startTimeStamp;
        Win32.Rect _targetWindowRectangle = new Win32.Rect();
        IntPtr _targetWindowHandle = IntPtr.Zero;

        //Dictionary<string, Rectangle> _appWindowDimensions = new Dictionary<string, Rectangle>
        //{
        //    {"rebelle 3", new Rectangle{ Width = 2736, Height = 1824 } },
        //    {"rebelle 4", new Rectangle{ Width = 2736, Height = 1824 } },
        //    {"rebelle 5", new Rectangle{ Width = 2736, Height = 1824 } },
        //    {"artrage", new Rectangle{ Width = 1368, Height = 872 } },
        //    {"devenv", new Rectangle{ Width = 2736, Height = 1824 } },
        //};

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            cmbApplication.SelectedIndex = 0;

            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_videoWriter != null && _videoWriter.IsOpen)
            {
                _videoWriter.Close();
                _videoWriter = null;

            }

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string processName = cmbApplication.Text.Trim();
            if (string.IsNullOrWhiteSpace(processName))
            {
                MessageBox.Show("Please specify an application");
                return;
            }
                

            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            if (process == null)
            {
                MessageBox.Show("Process named " + processName + " is not running!");
                return;
            }
                
            _targetWindowHandle = process.MainWindowHandle;

            Win32.ShowWindow(_targetWindowHandle, Win32.ShowWindowEnum.ShowMaximized);
            Win32.SetForegroundWindow(_targetWindowHandle);

            var displayScalingFactor = getDisplayScalingFactor(_targetWindowHandle);

            Win32.GetClientRect(_targetWindowHandle, out _targetWindowRectangle);

            _targetWindowRectangle.Left = Convert.ToInt32(_targetWindowRectangle.Left * displayScalingFactor);
            _targetWindowRectangle.Right = Convert.ToInt32(_targetWindowRectangle.Right * displayScalingFactor);
            _targetWindowRectangle.Top = Convert.ToInt32(_targetWindowRectangle.Top * displayScalingFactor);
            _targetWindowRectangle.Bottom = Convert.ToInt32(_targetWindowRectangle.Bottom * displayScalingFactor);


            if (_targetWindowRectangle.Right <= 0 || _targetWindowRectangle.Bottom <= 0)
            {
                MessageBox.Show("Unable to determine the bounds of the target window!");
                return;
            }


            //FFMPEG does not support odd number dimensions, so round to next even number
            _targetWindowRectangle.Left = _targetWindowRectangle.Left % 2 == 0 ? _targetWindowRectangle.Left : _targetWindowRectangle.Left + 1;
            _targetWindowRectangle.Right = _targetWindowRectangle.Right % 2 == 0 ? _targetWindowRectangle.Right : _targetWindowRectangle.Right + 1;
            _targetWindowRectangle.Top = _targetWindowRectangle.Top % 2 == 0 ? _targetWindowRectangle.Top : _targetWindowRectangle.Top + 1;
            _targetWindowRectangle.Bottom = _targetWindowRectangle.Bottom % 2 == 0 ? _targetWindowRectangle.Bottom : _targetWindowRectangle.Bottom + 1;


            _videoWriter = new VideoFileWriter();

            if (chkIsTimeLapse.Checked)
            {
                timer1.Interval = 100;
            }
            else
            {
                timer1.Interval = 50;
            }

            timer1.Enabled = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            cmbApplication.Enabled = false;
            chkIsTimeLapse.Enabled = false;
            chkCaptureTitlebar.Enabled = false;
            _startTimeStamp = DateTime.Now.TimeOfDay;

            this.WindowState = FormWindowState.Minimized;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            WriteVideoFrame(cmbApplication.Text.Trim());
        }

        
        private void WriteVideoFrame(string processName)
        {

            using (var bitmap = CaptureWindowUsingBitBlt(chkCaptureTitlebar.Checked))
            {
                if (!_videoWriter.IsOpen)
                {
                    _videoWriter.Open(_basePath + @"\" + processName + "_" + DateTime.Now.ToString("MM.dd.yyyy.HH.mm.ss") + ".avi", 
                        _targetWindowRectangle.Right, _targetWindowRectangle.Bottom, 30, VideoCodec.MPEG4, 20000000);

                    //write first frame.  
                    _videoWriter.WriteVideoFrame(bitmap);

                }
                else
                {
                    if (isPossibleBlackScreen(bitmap)) //if a tooltip etc is causing black screen.
                        return;

                    if (chkIsTimeLapse.Checked)
                    {
                        _videoWriter.WriteVideoFrame(bitmap);
                    }
                    else
                    {
                        var currentTime = DateTime.Now.TimeOfDay;
                        TimeSpan curTimeStamp = currentTime - _startTimeStamp;
                        curTimeStamp.Subtract(TimeSpan.FromMilliseconds(timer1.Interval));
                        _videoWriter.WriteVideoFrame(bitmap, curTimeStamp);
                    }
                }
            }
        }


        
        private void btnStop_Click(object sender, EventArgs e)
        {
            _videoWriter.Close();
            _videoWriter = null;
            timer1.Enabled = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            chkIsTimeLapse.Enabled = true;
            chkCaptureTitlebar.Enabled = true;
            cmbApplication.Enabled = true;
        }

 
        public Bitmap CaptureWindowUsingPrintWindow(bool captureTitlebar)
        {
            var bitmap = new Bitmap(_targetWindowRectangle.Right, _targetWindowRectangle.Bottom);

            // Use PrintWindow to draw the window into our bitmap
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                IntPtr hdc = g.GetHdc();
                if(captureTitlebar)
                    Win32.PrintWindow(_targetWindowHandle, hdc, 0);
                else
                    Win32.PrintWindow(_targetWindowHandle, hdc, 1);

                g.ReleaseHdc(hdc);
            }

            return bitmap;
        }

        public Bitmap CaptureWindowUsingBitBlt(bool captureTitlebar)
        {
            IntPtr hWndDc = Win32.GetDC(_targetWindowHandle);
            IntPtr hMemDc = Win32.CreateCompatibleDC(hWndDc);
            IntPtr hBitmap = Win32.CreateCompatibleBitmap(hWndDc, _targetWindowRectangle.Right, _targetWindowRectangle.Bottom);
            Win32.SelectObject(hMemDc, hBitmap);

            Win32.BitBlt(hMemDc, 0, 0, _targetWindowRectangle.Right, _targetWindowRectangle.Bottom, hWndDc, 0, 0, Win32.TernaryRasterOperations.SRCCOPY | Win32.TernaryRasterOperations.CAPTUREBLT);
            var bitmap = Bitmap.FromHbitmap(hBitmap);

            Win32.DeleteObject(hBitmap);
            Win32.ReleaseDC(_targetWindowHandle, hWndDc);
            //ReleaseDC(IntPtr.Zero, hMemDc);
            //var retVal = DeleteObject(hMemDc);
            var retVal2 = Win32.DeleteDC(hMemDc);

            return bitmap;
        }


        private bool isPossibleBlackScreen(Bitmap bitmap)
        {
            var pixel1 = bitmap.GetPixel(100, 100);
            var pixel2 = bitmap.GetPixel(1000, 100);
            var pixel3 = bitmap.GetPixel(100, 1000);
            var pixel4 = bitmap.GetPixel(1000, 1000);

            return isBlack(pixel1) && isBlack(pixel2) && isBlack(pixel3) && isBlack(pixel4);

        }

        private bool isBlack(Color pixel)
        {
            return (pixel.R == 0 && pixel.G == 0 && pixel.B == 0); 
        }

        private decimal getDisplayScalingFactor(IntPtr hwnd)
        {
            var screen = Screen.FromHandle(hwnd);

            var dm = new Win32.DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(Win32.DEVMODE));

            Win32.EnumDisplaySettings(screen.DeviceName, -1, ref dm);

            var scalingFactor = Math.Round(Decimal.Divide(dm.dmPelsWidth, screen.Bounds.Width), 2);

            return scalingFactor;
        }

        private void chkIsTimeLapse_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    
}

using Accord.Video.FFMPEG;
using AForge.Video;
using AForge.Video.DirectShow;
using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperSight
{
    public partial class FormMain : DarkForm, ILogger, IViewMain
    {
        /// <summary>
        /// webcam video source
        /// </summary>
        private IVideoSource _videoSource;

        /// <summary>
        /// File video source
        /// </summary>
        private VideoFileSource _videoFileSource;

        /// <summary>
        /// Managers sending our frames to be processed via RabbitMQ
        /// </summary>
        private FrameController _frameController;

        /// <summary>
        /// A counter to track how many processed frames we've received
        /// </summary>
        private int _frameCount = 0;

        /// <summary>
        /// A timestamp to help establish a frames per second (fps) counter
        /// </summary>
        private DateTime _frameRateTimestamp = DateTime.MinValue;

        //IViewMain implementation - these get updated when then get reflected in the UI:
        public bool Running { get; set; }
        public double FrameRate { get; set; }
        public int FrameSendRate { get; set; }
        public string Resolution { get; set; } = "Unknown";

        public FormMain()
        {
            InitializeComponent();

            // bind to event for when a processed frame has returned
            _frameController = new FrameController(0, this);
            _frameController.FrameComplete += new FrameController.FrameReturnedHandler(VideoFrameReceived);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.FrameSendRate = trackBarFrameSendRate.Value; // initializing view state
            this.UpdateView(this);
        }

        /// <summary>
        /// Displays a processed frame from our frame controller
        /// </summary>
        /// <param name="frame"></param>
        private void VideoFrameReceived(Bitmap frameComplete)
        {
            try
            {
                if (frameComplete != null)
                {
                    // determine a rough framerate per second
                    if (DateTime.Now > _frameRateTimestamp)
                    {
                        if (_frameCount > 0)
                        {
                            this.FrameRate = Math.Round(((double)_frameCount / 5) + 0.5, 2);
                        }
                        _frameCount = 0;
                        _frameRateTimestamp = DateTime.Now.AddSeconds(5);
                    }
                    _frameCount++;

                    this.UpdateView(this, frameComplete);
                }
            }
            catch(Exception ex)
            {
                this.Log($"Problem with received frame: {ex}");
            }
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //Send the frame from the webcam to our controller for processing...
            _frameController.ProcessFrame((Bitmap)eventArgs.Frame.Clone());
            eventArgs.Frame.Dispose(); // dispose this frame (avoid high mem usage)
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.Running = true;
            try
            {
                _frameController.Start();

                if(checkBoxUseWebcam.Checked)
                { 
                    // List all available video sources. (That can be webcams as well as tv cards, etc)
                    FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                    //Check if atleast one video source is available
                    if (videosources != null)
                    {
                        //For example use first video device. You may check if this is your webcam.
                        VideoCaptureDevice webcam = new VideoCaptureDevice(videosources[0].MonikerString);
                        _videoSource = webcam;

                        try
                        {
                            //Check if the video device provides a list of supported resolutions
                            if (webcam.VideoCapabilities.Length > 0)
                            {
                                string highestSolution = "0;0";
                                //Search for the highest resolution
                                for (int i = 0; i < webcam.VideoCapabilities.Length; i++)
                                {
                                    if (webcam.VideoCapabilities[i].FrameSize.Width > Convert.ToInt32(highestSolution.Split(';')[0]))
                                        highestSolution = webcam.VideoCapabilities[i].FrameSize.Width.ToString() + ";" + i.ToString();
                                }
                                //Set the highest resolution as active
                                webcam.VideoResolution = webcam.VideoCapabilities[Convert.ToInt32(highestSolution.Split(';')[1])];

                            }
                        }
                        catch { }

                        //handle new frame event
                        webcam.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame);

                        //Start recording
                        webcam.Start();

                        this.Resolution = $"({webcam.VideoResolution.FrameSize.Width}x{webcam.VideoResolution.FrameSize.Height})";
                        this.Log("Video capture started");
                    }
                }
                else if (!string.IsNullOrEmpty(textBoxVideoPath.Text))
                {
                    // let's process the video file
                    _videoFileSource = new VideoFileSource(textBoxVideoPath.Text);
                    _videoFileSource.NewFrame += _videoFileSource_NewFrame;
                    // start the video source
                    _videoFileSource.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                this.Running = false;
            }
            this.UpdateView(this);
        }

        private void _videoFileSource_NewFrame(object sender, Accord.Video.NewFrameEventArgs eventArgs)
        {
            //Send the frame from the webcam to our controller for processing...
            _frameController.ProcessFrame((Bitmap)eventArgs.Frame.Clone());
            eventArgs.Frame.Dispose(); // dispose this frame (avoid high mem usage)
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.StopProcessing();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.StopProcessing();
        }

        private void StopProcessing()
        {
            this.Running = false;
            _frameController.Stop();

            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource = null;
            }
            if(_videoFileSource != null && _videoFileSource.IsRunning)
            {
                _videoFileSource.SignalToStop();
                _videoFileSource = null;
            }
            this.Log("Video capture stopped");
            this.UpdateView(this);
        }

        private void trackBarFrameSendRate_Scroll(object sender, EventArgs e)
        {
            this.FrameSendRate = trackBarFrameSendRate.Value;
            _frameController.FrameRateMilliseconds = this.FrameSendRate;
            this.UpdateView(this);
        }

        /// <summary>
        /// Updates view (all logic in one place in the UI thread)
        /// </summary>
        /// <param name="view"></param>
        private void UpdateView(IViewMain view, Bitmap frame=null)
        {
            this.BeginInvoke((MethodInvoker)delegate ()
            {
                buttonStart.Enabled = !view.Running;
                buttonStop.Enabled = view.Running;
                if (view.Running)
                {
                    labelStatus.Text = "Status: Running";
                    labelFramerate.Text = $"Framerate: {FrameRate} fps";
                }
                else
                {
                    labelStatus.Text = "Status: Idle";
                    labelFramerate.Text = "Framerate: 0 fps";
                }
                labelFrameSendRate.Text = $"Frame send rate: {FrameSendRate}";

                // Update the picture box with the latest image if different
                if (frame != null && frame != pictureBoxMain.BackgroundImage)
                {
                    Image lastFrame = null;
                    lastFrame = pictureBoxMain.BackgroundImage;
                    pictureBoxMain.BackgroundImage = frame;
                    if (lastFrame != null)
                        lastFrame.Dispose();
                }
            });
        }

        /// <summary>
        /// Displays logging output
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            textBoxLog.BeginInvoke(
                (MethodInvoker)delegate { textBoxLog.AppendText(String.Format("{0:HH:mm:ss} {1}{2}", DateTime.Now, message, Environment.NewLine)); }
            );
        }

        private void checkBoxUseWebcam_CheckedChanged(object sender, EventArgs e)
        {
            textBoxVideoPath.Enabled = !checkBoxUseWebcam.Checked;
            buttonBrowse.Enabled = !checkBoxUseWebcam.Checked;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog(this);
            if (dr == DialogResult.OK)
                textBoxVideoPath.Text = openFileDialog1.FileName;
        }
    }
}

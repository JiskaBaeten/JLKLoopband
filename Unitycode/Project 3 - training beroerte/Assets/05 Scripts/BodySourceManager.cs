using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System.IO;
using System.Text;
using Environment = System.Environment;

public class BodySourceManager : MonoBehaviour 
{
    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;
    private Body[] _Data = null;

    string writePath;
    string newline;
    StringBuilder csv;

    public Body[] GetData()
    {
        return _Data;
    }
    

    void Start () 
    {
        _Sensor = KinectSensor.GetDefault();

        if (_Sensor != null)
        {
            _Reader = _Sensor.BodyFrameSource.OpenReader();
            
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }

        //IO
        writePath = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        writePath += "\\A walk in the park\\KinectData.csv";
        csv = new StringBuilder();

    }

    void Update () 
    {
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            if (frame != null)
            {
                if (_Data == null)
                {
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                }
                
                frame.GetAndRefreshBodyData(_Data);
                
                frame.Dispose();
                frame = null;
            }
            WriteKinectData();
        }
        
    }
    
    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }
        
        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }
            
            _Sensor = null;
        }

        try
        {
           File.AppendAllText(writePath,csv.ToString());
            Debug.Log("File written to:" + writePath);
        }

        catch (System.Exception)
        {
            Debug.Log("Could not write to file: ");
            throw;
        }
    }
    void WriteKinectData()
    {

            if (_Data != null)
            {
                foreach (var body in _Data)
                {
                    if (body != null)
                    {
                        if (body.IsTracked)
                        {
                            Windows.Kinect.Joint lShoulder = body.Joints[JointType.ShoulderLeft];
                            Windows.Kinect.Joint rShoulder = body.Joints[JointType.ShoulderRight];
                            Windows.Kinect.Joint spineShoulder = body.Joints[JointType.SpineShoulder];
                            Windows.Kinect.Joint spineBase = body.Joints[JointType.SpineBase];

                            newline = string.Format("{0},{1},{2}{3}", lShoulder.Position.X, lShoulder.Position.Y, lShoulder.Position.Z, Environment.NewLine);
                            csv.Append(newline);
                            newline = string.Format("{0},{1},{2}{3}", rShoulder.Position.X, rShoulder.Position.Y, rShoulder.Position.Z, Environment.NewLine);
                            csv.Append(newline);
                            newline = string.Format("{0},{1},{2}{3}", spineShoulder.Position.X, spineShoulder.Position.Y, spineShoulder.Position.Z, Environment.NewLine);
                            csv.Append(newline);
                            newline = string.Format("{0},{1},{2}{3}", spineBase.Position.X, spineBase.Position.Y, spineBase.Position.Z, Environment.NewLine);
                            csv.Append(newline);

                            Debug.Log("right:" + rShoulder.Position.X + " " + rShoulder.Position.Y + " " + rShoulder.Position.Z);
                            Debug.Log("left:" + lShoulder.Position.X + " " + lShoulder.Position.Y + " " + lShoulder.Position.Z);
                        }
                    }
                }
            }
    }

}

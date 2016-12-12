using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System.IO;
using Environment = System.Environment;

public class BodySourceManager : MonoBehaviour 
{
    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;
    private Body[] _Data = null;

    StreamWriter sWriter;
    string writePath;
    string lstring, rstring;

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
        writePath += "\\KinectData.txt";   
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
        }
        WriteKinectData();
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
    }
    void WriteKinectData()
    {
        try
        {
            foreach (var body in _Data)
            {
                if (body != null)
                {
                    if (body.IsTracked)
                    {
                        sWriter = File.AppendText(writePath);
                        Windows.Kinect.Joint lShoulder = body.Joints[JointType.ShoulderLeft];
                        Windows.Kinect.Joint rShoulder = body.Joints[JointType.ShoulderRight];

                        lstring = lShoulder.Position.X + " " + lShoulder.Position.Y + " " + lShoulder.Position.Z; ;
                        rstring = rShoulder.Position.X + " " + rShoulder.Position.Y + " " + rShoulder.Position.Z;
                        sWriter.WriteLine(lstring);
                        sWriter.WriteLine(rstring);
                        Debug.Log("right:" + rShoulder.Position.X + " " + rShoulder.Position.Y + " " + rShoulder.Position.Z);
                        Debug.Log("left:" + lShoulder.Position.X + " " + lShoulder.Position.Y + " " + lShoulder.Position.Z);
                        sWriter.Close();
                    }
                }
            }

        }
        catch (System.Exception)
        {
            Debug.Log("could not write KinectData to file");
            throw;
        }

    }
}

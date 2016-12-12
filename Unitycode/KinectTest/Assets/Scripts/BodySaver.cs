using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using System.IO;
using Environment = System.Environment;

public class BodySaver : MonoBehaviour {

    private KinectSensor _sensor;
    private BodyFrameReader _reader;
    private Body[] _bodies = null;

    StreamWriter sWriter;
    string writePath;

    public Body[] GetData()
    {
        return _bodies;
    }


    void Start()
    {
        _sensor = KinectSensor.GetDefault();

        if (_sensor != null)
        {
            _reader = _sensor.BodyFrameSource.OpenReader();

            if (!_sensor.IsOpen)
            {
                _sensor.Open();
            }
        }

        //IO
       // writePath = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        //writePath += "\\KinectData.txt";
    }

    void Update()
    {
        if (_reader != null)
        {
            var frame = _reader.AcquireLatestFrame();
            if (frame != null)
            {
                if (_bodies == null)
                {
                    _bodies = new Body[_sensor.BodyFrameSource.BodyCount];
                }

                frame.GetAndRefreshBodyData(_bodies);

                frame.Dispose();
                frame = null;
            }
        }
        WriteKinectData();//saves the coordinates of shoulder joints

    }

    void OnApplicationQuit()
    {
        if (_reader != null)
        {
            _reader.Dispose();
            _reader = null;
        }

        if (_sensor != null)
        {
            if (_sensor.IsOpen)
            {
                _sensor.Close();
            }

            _sensor = null;
        }
    }

    void WriteKinectData()
    {
        try
        {
           // sWriter = File.CreateText(writePath);
            foreach (var body in _bodies)
            {
                if (body != null)
                {
                    if (body.IsTracked)
                    {
                        Windows.Kinect.Joint lShoulder = body.Joints[JointType.ShoulderLeft];
                        Windows.Kinect.Joint rShoulder = body.Joints[JointType.ShoulderRight];
                        //sWriter.WriteLine(lShoulder.Position.X + " "+ lShoulder.Position.Y + " "+ lShoulder.Position.Z);
                        //sWriter.WriteLine(rShoulder.Position.X + " "+ rShoulder.Position.Y + " "+ rShoulder.Position.Z);
                        Debug.Log(rShoulder.Position.X + " " + rShoulder.Position.Y + " " + rShoulder.Position.Z);
                    }
                }
            }
           // sWriter.Close();
            
        }
        catch (System.Exception)
        {
            Debug.Log("could not write KinectData to file");
            throw;
        }

    }


}

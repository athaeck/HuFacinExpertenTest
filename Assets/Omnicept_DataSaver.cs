using HP.Omnicept.Messaging.Messages;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Omnicept_DataSaver : MonoBehaviour
{
    [SerializeField]
    private bool saveHeartRate = true;
    [SerializeField]
    private bool saveHeartRateVariability = true;
    [SerializeField]
    private bool saveCognitiveLoad = true;
    [SerializeField]
    private bool saveEyeTracking = true;

    List<HRDataEntry> HeartRateData = new List<HRDataEntry>();
    List<HRVDataEntry> HeartRateVariabilityData = new List<HRVDataEntry>();
    List<CLDataEntry> CognitiveLoadData = new List<CLDataEntry>();
    List<ETDataEntry> EyeTrackingData = new List<ETDataEntry>();

    public string SubjectNumber = "EMPTY";
    private int TimeSinceStart = 0;

    void Start()
    {
        SubjectNumber = File.ReadAllText(Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/ProbandenNummer.txt");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveData(HeartRateData, HeartRateVariabilityData, CognitiveLoadData, EyeTrackingData);
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData(HeartRateData, HeartRateVariabilityData, CognitiveLoadData, EyeTrackingData);     
        }

        TimeSinceStart += (int)Mathf.Floor(Time.deltaTime * 1000);
    }

    public void HeartRateHandler(HeartRate hr)
    {
        if (saveHeartRate && hr != null)
        {
            HeartRateData.Add(new HRDataEntry(DateTime.Now, TimeSinceStart, hr.Rate));
            Debug.Log(hr);
        }
    }

    public void HeartRateVariabilityHandler(HeartRateVariability hrv)
    {
        if (saveHeartRateVariability && hrv != null)
        {
            HeartRateVariabilityData.Add(new HRVDataEntry(DateTime.Now, TimeSinceStart, hrv.Sdnn, hrv.Rmssd));
            Debug.Log(hrv);
        }
    }

    public void CognitiveLoadHandler(CognitiveLoad cl)
    {
        if (saveCognitiveLoad && cl != null)
        {
            CognitiveLoadData.Add(new CLDataEntry(DateTime.Now, TimeSinceStart, cl.CognitiveLoadValue, cl.StandardDeviation, cl.DataState));
            Debug.Log(cl);
        }
    }

    public void EyeTrackingHandler(EyeTracking et)
    {
        if (saveEyeTracking && et != null)
        {
            Vector3 gaze = new Vector3(et.LeftEye.Gaze.X, et.LeftEye.Gaze.Y, et.LeftEye.Gaze.Z);
            float gazeConfidence = et.LeftEye.Gaze.Confidence;
            //Vector2 pupilPos = new Vector2(et.LeftEye.PupilPosition.X, et.LeftEye.PupilPosition.Y);
            //float pupilPosConfidence = et.LeftEye.PupilPosition.Confidence;
            float pupilDilation = et.LeftEye.PupilDilation;
            float pupilDilationConfidence = et.LeftEye.PupilDilationConfidence;
            float openness = et.LeftEye.Openness;
            float opennessConfidence = et.LeftEye.OpennessConfidence;
            EyeData LeftEye = new EyeData(gaze, gazeConfidence, /*pupilPos, pupilPosConfidence,*/ pupilDilation, pupilDilationConfidence, openness, opennessConfidence);

            gaze = new Vector3(et.RightEye.Gaze.X, et.RightEye.Gaze.Y, et.RightEye.Gaze.Z);
            gazeConfidence = et.RightEye.Gaze.Confidence;
            //pupilPos = new Vector2(et.RightEye.PupilPosition.X, et.RightEye.PupilPosition.Y);
            //pupilPosConfidence = et.RightEye.PupilPosition.Confidence;
            pupilDilation = et.RightEye.PupilDilation;
            pupilDilationConfidence = et.RightEye.PupilDilationConfidence;
            openness = et.RightEye.Openness;
            opennessConfidence = et.RightEye.OpennessConfidence;
            EyeData RightEye = new EyeData(gaze, gazeConfidence, /*pupilPos, pupilPosConfidence,*/ pupilDilation, pupilDilationConfidence, openness, opennessConfidence);
            Vector3 cg = new Vector3(et.CombinedGaze.X, et.CombinedGaze.Y, et.CombinedGaze.Z);

            EyeTrackingData.Add(new ETDataEntry(DateTime.Now, TimeSinceStart, cg, et.CombinedGaze.Confidence, LeftEye, RightEye));
            //Debug.Log(et);
        }
    }

    public void SaveData(List<HRDataEntry> HRData, List<HRVDataEntry> HRVData, List<CLDataEntry> CLData, List<ETDataEntry> ETData)
    {
        //HEART RATE
        string heartRate = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/OmniceptData/Subject_" + SubjectNumber + "_HeartRate.csv";
        TextWriter tw = new StreamWriter(heartRate, false);
        tw.WriteLine("Date; Time; Time since start (ms); Heart rate (BPM)");
        tw.Close();
        tw = new StreamWriter(heartRate, true);
        foreach (HRDataEntry hr in HRData)
        {
            tw.WriteLine(hr.Date + "; " + hr.Time + "; " + hr.TimeSinceStart + "; " + hr.HeartRate.ToString());
        }
        tw.Close();

        //HEART RATE VARIABILITY
        string heartRateVar = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/OmniceptData/Subject_" + SubjectNumber + "_HeartRateVariability.csv";
        tw = new StreamWriter(heartRateVar, false);
        tw.WriteLine("Date; Time; Time since start (ms); Standard deviation between 2 normal heart beats(ms); Root-mean square successive differences (ms)");
        tw.Close();
        tw = new StreamWriter(heartRateVar, true);
        foreach (HRVDataEntry hrv in HRVData)
        {
            tw.WriteLine(hrv.Date + "; " + hrv.Time + "; " + hrv.TimeSinceStart + "; " + hrv.Sdnn.ToString() + "; " + hrv.Rmssd.ToString());
        }
        tw.Close();

        //COGNITIVE LOAD
        string cognitiveLoad = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/OmniceptData/Subject_" + SubjectNumber + "_CognitiveLoad.csv";
        tw = new StreamWriter(cognitiveLoad, false);
        tw.WriteLine("Date; Time; Time since start (ms); Cognitive load (0-1); Standard deviation; Data state");
        tw.Close();
        tw = new StreamWriter(cognitiveLoad, true);
        foreach (CLDataEntry cl in CLData)
        {
            tw.WriteLine(cl.Date + "; " + cl.Time + "; " + cl.TimeSinceStart + "; " + cl.CogLoad.ToString() + "; " + cl.StandardDeviation.ToString() + "; " + cl.DataState);
        }
        tw.Close();

        //EYE TRACKING
        string eyeTracking = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/OmniceptData/Subject_" + SubjectNumber + "_EyeTracking.csv";
        tw = new StreamWriter(eyeTracking, false);
        tw.WriteLine("Date; Time; Time since start (ms); Combined gaze; Com. gaze confidence; " +
            "L: Eye gaze; L: Eye gaze confidence; L: Pupil dilation; L: Pupil dilation confidence; L: Openness; L: Openness confidence; " +
            "R: Eye gaze; R: Eye gaze confidence; R: Pupil dilation; R: Pupil dilation confidence; R: Openness; L: Openness confidence");
        tw.Close();
        tw = new StreamWriter(eyeTracking, true);
        foreach (ETDataEntry et in ETData)
        {
            tw.WriteLine(et.Date + "; " + et.Time + "; " + et.TimeSinceStart + "; " + et.CombinedGaze.ToString() + "; " + et.CombinedGazeConfidence.ToString() + "; " +
            et.LeftEyeData.EyeGaze.ToString() + "; " + et.LeftEyeData.EyeGazeConfidence.ToString() + "; " + et.LeftEyeData.PupilDilation.ToString() + "; " + 
            et.LeftEyeData.PupilDilationConfidence.ToString() + "; " + et.LeftEyeData.Openness.ToString() + "; " + et.LeftEyeData.OpennessConfidence.ToString() +
            "; " + et.RightEyeData.EyeGaze.ToString() + "; " + et.RightEyeData.EyeGazeConfidence.ToString() + "; " + et.RightEyeData.PupilDilation.ToString() + "; " +
            et.RightEyeData.PupilDilationConfidence.ToString() + "; " + et.RightEyeData.Openness.ToString() + "; " + et.RightEyeData.OpennessConfidence.ToString());
        }
        tw.Close();

        Debug.Log("Data saved.");
    }

    public void DisconnectHandler(string msg)
    {
        Debug.Log("Disconnected: " + msg);
    }

    public void ConnectionFailureHandler(HP.Omnicept.Errors.ClientHandshakeError error)
    {
        Debug.Log("Failed to connect: " + error);
    }
}

public class HRDataEntry
{
    public string Date;
    public string Time;
    public string TimeSinceStart;
    public uint HeartRate;

    public HRDataEntry(DateTime dt, int tss, uint hr)
    {
        string[] ts = dt.ToString().Split(' ');
        this.Date = ts[0].ToString();
        this.Time = ts[1].ToString();
        this.TimeSinceStart = tss.ToString();
        this.HeartRate = hr;
    }
}

public class HRVDataEntry
{
    public string Date;
    public string Time;
    public string TimeSinceStart;

    public float Sdnn;
    public float Rmssd;

    public HRVDataEntry(DateTime dt, int tss, float sdnn, float rmssd)
    {
        string[] ts = dt.ToString().Split(' ');
        this.Date = ts[0].ToString();
        this.Time = ts[1].ToString();
        this.TimeSinceStart = tss.ToString();
        this.Sdnn = sdnn;
        this.Rmssd = rmssd;
    }
}

public class CLDataEntry
{
    public string Date;
    public string Time;
    public string TimeSinceStart;


    public float CogLoad;
    public float StandardDeviation;
    public string DataState;

    public CLDataEntry(DateTime dt, int tss, float cg, float sd, string ds)
    {
        string[] ts = dt.ToString().Split(' ');
        this.Date = ts[0].ToString();
        this.Time = ts[1].ToString();
        this.TimeSinceStart = tss.ToString();
        this.CogLoad = cg;
        this.StandardDeviation = sd;
        this.DataState = ds;
    }
}

public class ETDataEntry
{
    public string Date;
    public string Time;
    public string TimeSinceStart;


    public Vector3 CombinedGaze;
    public float CombinedGazeConfidence;

    public EyeData LeftEyeData;
    public EyeData RightEyeData;

    public ETDataEntry(DateTime dt, int tss, Vector3 cg, float cgc, EyeData le, EyeData re)
    {
        string[] ts = dt.ToString().Split(' ');
        this.Date = ts[0].ToString();
        this.Time = ts[1].ToString();
        this.TimeSinceStart = tss.ToString();
        this.CombinedGaze = cg;
        this.CombinedGazeConfidence = cgc;

        this.LeftEyeData = le;
        this.RightEyeData = re;
    }
}

public class EyeData
{
    public Vector3 EyeGaze;
    public float EyeGazeConfidence;

    //public Vector2 PupilPosition;
    //public float PupilPositionConfidence;

    public float PupilDilation;
    public float PupilDilationConfidence;

    public float Openness;
    public float OpennessConfidence;

    public EyeData(Vector3 eg, float egc, /*Vector2 pp, float ppc,*/ float pd, float pdc, float op, float opc)
    {
        this.EyeGaze = eg;
        this.EyeGazeConfidence = egc;

        //this.PupilPosition = pp;
        //this.PupilPositionConfidence = ppc;

        this.PupilDilation = pd;
        this.PupilDilationConfidence = pdc;

        this.Openness = op;
        this.OpennessConfidence = opc;
    }
}
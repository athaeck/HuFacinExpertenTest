using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Events;
using System.Threading.Tasks;

public class DistanceTracker : MonoBehaviour
{
    [SerializeField]
    private GameObject _objectToLookAt;
    [SerializeField]
    private GameObject _testPerson;
    [SerializeField]
    private List<DistanceAtTime> _distances = new List<DistanceAtTime>();

    public string SubjectNumber = "EMPTY";

    void Start()
    {
        SubjectNumber = File.ReadAllText(Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/ProbandenNummer.txt");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveDistance();
        }
    }

    void FixedUpdate()
    {
        DateTime currentTime = DateTime.Now;
        _distances.Add(new DistanceAtTime() { Date = currentTime.ToShortDateString(), Time = currentTime.ToShortTimeString(), TimeSinceStart = TimeController.Instance.GetTimeSinceStart(), Distance = Vector3.Distance(_objectToLookAt.transform.position, _testPerson.transform.position) });
    }

    private void SaveDistance()
    {
        string objectDistance = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/OmniceptData/Subject_" + SubjectNumber + "_ObjectDistance.csv";
        TextWriter tw = new StreamWriter(objectDistance, false);
        tw.WriteLine("Date; Time; Time since start (ms); Distance (m)");
        tw.Close();
        tw = new StreamWriter(objectDistance, true);
        foreach (DistanceAtTime dat in _distances)
        {
            tw.WriteLine(dat.Date + "; " + dat.Time + "; " + dat.TimeSinceStart + "; " + dat.Distance);
        }
        tw.Close();
    }
}

public class DistanceAtTime
{
    public int TimeSinceStart { get; set; }
    public float Distance { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
}
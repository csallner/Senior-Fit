using TensorFlowLite;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Poses
{
    public abstract class Pose
    {
        private Text exerciseName;
        public static int[] required { get; }
        
        public static int[] disabilities;
        protected string name = "";
        protected float _repCount = 0;

        public Pose(string repCount) { _repCount = Int32.Parse(repCount);}

        public abstract bool IsFinished(PoseNet.Result[] result, Text t);
        public abstract bool IsFinished(PoseLandmarkDetect.Result result, Text t);
        public bool IsPossiblePose()
        {
            return required.Any(el => disabilities.Contains(el));
        }

        public void RepAction(Text t)
        {
            t.text = name + " x " + Math.Abs((int)_repCount);
            Debug.Log(name);
        }

        public void NoRepAction(Text t)
        {
            // t.text = "Exercise Name";
            t.text = name + " x " + Math.Abs((int)_repCount);
        }

        public string GetTutorialAddress()
        {
            return Application.streamingAssetsPath + "/TutorialClips/" + name.Replace(' ', '_').ToLower() + "_tutorial.mp4";
        }
        
    }
}
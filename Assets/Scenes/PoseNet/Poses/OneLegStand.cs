using static TensorFlowLite.PoseNet;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Poses
{
    public class OneLegStand : Pose
    {
        public static new Part[] required 
        {
            get
            {
                return new Part[] {
                    Part.LEFT_KNEE,
                    Part.RIGHT_KNEE,
                    Part.LEFT_ANKLE,
                    Part.RIGHT_ANKLE,
                };
            }
        }

        byte legCheck = 0;
        float setTime = 7;
        float waitTime = 3;
        int correctCounts = 0;
        int incorrectCounts = 0;

        public OneLegStand(string repCount) : base(repCount) { name = "One Leg Stand"; }
        public override bool IsFinished(Result[] result, Text t)
        {
            float i = 0f;
            for(int x = 11; x < 17; x++){i+=result[x].confidence;}

            // Debug.Log(i);

            switch(legCheck)
            {
                case 0:
                    return CheckLeftLeg(result, t, i);
                case 2:
                    return CheckRightLeg(result, t, i);
                default:
                    return CountDown(t);
            }
        }

        bool CheckLeftLeg(Result[] result, Text t, float i)
        {
            if(i > 4)
            {
                double A = Math.Sqrt(
                            Math.Pow(result[(int)Part.LEFT_KNEE].x - result[(int)Part.LEFT_HIP].x, 2) +
                            Math.Pow(result[(int)Part.LEFT_KNEE].y - result[(int)Part.LEFT_HIP].y, 2));

                double B = Math.Sqrt(
                            Math.Pow(result[(int)Part.LEFT_KNEE].x - result[(int)Part.LEFT_ANKLE].x, 2) +
                            Math.Pow(result[(int)Part.LEFT_KNEE].y - result[(int)Part.LEFT_ANKLE].y, 2));

                double C = Math.Sqrt(
                            Math.Pow(result[(int)Part.LEFT_HIP].x - result[(int)Part.LEFT_ANKLE].x, 2) +
                            Math.Pow(result[(int)Part.LEFT_HIP].y - result[(int)Part.LEFT_ANKLE].y, 2));

                double theta = Math.Acos((Math.Pow(C, 2) - Math.Pow(A, 2) - Math.Pow(B, 2))/(-2*A*B)) * (180/Math.PI);

                Debug.Log(theta);
                if(theta < 165)
                {
                    if(waitTime <= 0)
                    {
                        if(correctCounts/4 > incorrectCounts)
                        {
                            RepAction(t);
                            legCheck++;
                            _repCount = setTime;
                        }
                        
                        waitTime = 3;
                        correctCounts = 0;
                        incorrectCounts = 0;
                    }
                    else { waitTime -= Time.deltaTime; NoRepAction(t); }
                    correctCounts++;
                }
                else
                {
                    incorrectCounts++;
                    NoRepAction(t);
                    // if(waitTime < 3 && waitTime > 2.7 && incorrectCounts > correctCounts)
                    // {
                    //     waitTime = 3;
                    // }
                }
            }
            return false;
        }

        bool CheckRightLeg(Result[] result, Text t, float i)
        {
            if(i > 4)
            {
                double A = Math.Sqrt(
                            Math.Pow(result[(int)Part.RIGHT_KNEE].x - result[(int)Part.RIGHT_HIP].x, 2) +
                            Math.Pow(result[(int)Part.RIGHT_KNEE].y - result[(int)Part.RIGHT_HIP].y, 2));

                double B = Math.Sqrt(
                            Math.Pow(result[(int)Part.RIGHT_KNEE].x - result[(int)Part.RIGHT_ANKLE].x, 2) +
                            Math.Pow(result[(int)Part.RIGHT_KNEE].y - result[(int)Part.RIGHT_ANKLE].y, 2));

                double C = Math.Sqrt(
                            Math.Pow(result[(int)Part.RIGHT_HIP].x - result[(int)Part.RIGHT_ANKLE].x, 2) +
                            Math.Pow(result[(int)Part.RIGHT_HIP].y - result[(int)Part.RIGHT_ANKLE].y, 2));

                double theta = Math.Acos((Math.Pow(C, 2) - Math.Pow(A, 2) - Math.Pow(B, 2))/(-2*A*B)) * (180/Math.PI);

                if(theta < 165)
                {
                    if(waitTime <= 0)
                    {
                        if(correctCounts/4 > incorrectCounts)
                        {
                            RepAction(t);
                            legCheck = 3;
                            _repCount = setTime;
                        }
                        
                        waitTime = 3;
                        correctCounts = 0;
                        incorrectCounts = 0;
                    }
                    else { waitTime -= Time.deltaTime; NoRepAction(t); }

                    correctCounts++;
                    
                }
                else
                {
                    incorrectCounts++;
                    NoRepAction(t);
                    // if(waitTime < 3 && waitTime > 2.7 && incorrectCounts > correctCounts)
                    // {
                    //     waitTime = 3;
                    // }
                }

            }

            return false;
        }

        bool CountDown(Text t)
        {
            if(_repCount <= 0)
            {
                if (legCheck == 3)
                    return true;
                legCheck = 2;
            }
            
            _repCount -= Time.deltaTime;
            RepAction(t);
            return false;
        }
    }

}
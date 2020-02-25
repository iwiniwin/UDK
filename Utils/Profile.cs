using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UKit.Utils{
    public class ProfileTime
    {
        public double startTime;
        public void Start(){
            this.startTime = DateTime.Now.ToUniversalTime().Ticks;
        }
        public double Stop(){
            return (DateTime.Now.ToUniversalTime().Ticks - this.startTime) / 10000.0 / 1000.0;
        }
    }
    public class Profile
    {
        public static ProfileTime Time;

        static Profile(){
            Time = new ProfileTime();
        }

        private Profile(){

        }
    }
}



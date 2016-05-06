﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kleps.Engine.Game.Entities;
using Newtonsoft.Json;

namespace Kleps.Engine.Events
{
    public class GameEvent
    {
        public string id { get; private set; }
        public Student owner { get; set; }

        [JsonIgnore]
        public Timer timer { get; set; }

        public string type { get; set; }
        public string rightAnswer { get; set; }
        public string question { get; set; }
        public List<string> answers { get; set; }

        [JsonIgnore]
        public EventHandler OnTimerEnds { get; set; }

        public int lifeTime { get; set; }

        public GameEvent() {
            id = Guid.NewGuid().ToString("N");
            lifeTime = 30;
            CountStart();
        }

        public void CountStart() {
            this.timer = new Timer(state => {

                lifeTime--;
                if (lifeTime != 0) return;

                OnTimerEnds(this, new EventArgs());
                CountStop();
            }, null, 0, 1000);
        }

        public void CountStop() {
            this.timer.Dispose();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Kleps.Engine.Game;
using Kleps.Engine.Game.Entities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace Kleps.Engine.Events.Spawner
{
    public class EventSpawner {
        private readonly Game.Game _game;
        private Timer _timer;
        private readonly int _interval;
        private readonly int _chance;
        private List<GameEventData> _eventDataset; 

        public string YamlPath { get; set; }

        public EventHandler<SpawnEventArgs> OnSpawn { get; set; }

        public EventSpawner(Game.Game game) {
            _game = game;
            _interval = 1488;
            _chance = 40;
            YamlPath = "DataRepository/Events.yaml";
        }

        protected GameEvent GenerateEvent() {

            if (_eventDataset == null) {
                _eventDataset = getEventData();
            }

            Student evilStudent;

            bool evilEvent = false;

            if ((
                evilStudent = 
                _game.students.FirstOrDefault(st => st.name == _game.Config.Names.EvilStudent)) != null
                ) {
                    if (new Random().Next(0, 100) < 20) {
                        evilEvent = true;
                }
            }

            var student = evilEvent ? evilStudent : _game.students[new Random().Next(0, _game.students.Count - 1)];
            _game.students.Remove(student);

            var eventData = _eventDataset.OrderBy(x => Guid.NewGuid())
                .First(x => x.type == (evilEvent ? "evil" : "normal"));

            var ev = new GameEvent {
                owner = student,
                lifeTime = evilEvent ? 7 : 30,
                type = "question",
                rightAnswer = eventData.rightAnswer,
                question = eventData.text,
                answers = eventData.answers
            };

            ev.OnTimerEnds += (s, e) => {
                ev.Exterminate();
                _game.teacher.DecreaseHealth(evilEvent ? 200 : 50);
            };

            ev.OnRejected += (s, e) => {
                ev.Exterminate();
                _game.teacher.DecreaseHealth(evilEvent ? 150 : 50);
            };

            ev.OnAccepted += (s, e) => {
                ev.Exterminate();
                _game.teacher.IncreaseHealth(35);
            };

            return ev;
        }

        protected void SpawnCallback(object state)
        {
            if (this._game.events.Count >= 4) return;

            if (new Random().Next(0, 100) < _chance) {
                this.OnSpawn(this, new SpawnEventArgs(GenerateEvent()));
            }
        }

        /// <summary>
        /// Starting event spawning.
        /// </summary>
        public void SpawnOn() {
            this._timer = new Timer(SpawnCallback, null, 0, _interval);
        }

        /// <summary>
        /// Stops event spawning.
        /// </summary>
        public void SpawnOff() {
            this._timer.Dispose();
        }

        private List<GameEventData> getEventData()
        {
            return new Deserializer(namingConvention: new CamelCaseNamingConvention())
                .Deserialize<List<GameEventData>>(
                new StreamReader(
                    new FileStream(
                        YamlPath, 
                        FileMode.Open
                    )
                )
            );
        }
    }
}

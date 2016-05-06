﻿using System;
using Kleps.Frontend.Controller;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Kleps.Engine
{
    public class Gate {

        private readonly Backend _backend ;

        private FrontendHelper FEH => FrontendHelper.Instance;

        private Game.Game game => _backend.game;

        public Gate(Backend back) {
            _backend = back;
        }

        public void log(string data) {
            Console.WriteLine(data);
        }

        public void alert(string data){
            MessageBox.Show(data);
        }

        public Game.Game getGame(){
            return game;
        }

        public string getGameEventsJson(){
            return JsonConvert.SerializeObject(game.events);
        }

        public string getTeacherJson(){
            return JsonConvert.SerializeObject(game.teacher);
        }

        public void stopEventCountingById(string id){
            game.getEventById(id).CountStop();
        }

        public void startEventCountingById(string id){
            game.getEventById(id).CountStart();
        }

        public void startGame(){
            game.run();
        }

        #region Frontend

        #region Sound
        public void musicStart() {
            FEH.MusicStart();
            FEH.Music.Volume(50);
        }
        public void musicMute() {
            FEH.Music.Mute();
        }
        public void musicVolume(int val) {
            FEH.Music.Volume(val);
        }

        public void musicClick() {
            FEH.Music.StartClick();
        }

        #endregion Sound

        #region Menu
        
        public void menuAction(string val) {
            FEH.Select(val);
        }

        #endregion Menu



        #endregion Frontend


    }
}

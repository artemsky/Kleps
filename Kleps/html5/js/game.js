﻿$(function () {
    backend.startGame();
    var Teacher = {
        name: $("#teacher-name"),
        health: $("#hp-val"),
        healthBar: $("#hp-bar"),
        healthBarWidth: $("#hp-bar").width(),
        maxHealth: JSON.parse(backend.getTeacherJson()).health
    }
    var output = $("#events");


    Teacher.name.text(JSON.parse(backend.getTeacherJson()).name);

    setInterval(function () {
        var events = JSON.parse(backend.getGameEventsJson());
        var health = JSON.parse(backend.getTeacherJson()).health;

        Teacher.health.text("HP:" + health);
        Teacher.healthBar.width((Teacher.healthBarWidth / 100) * ((health * 100) / Teacher.maxHealth));


        var html = '';
        for (var i in events)
            html += '<div class="listItem"><div id="timer-' + i + '"></div></div>';
            
        
        output.html(html);

        for (var i in events)
            $('#timer-' + i).timer({
                studentName: events[i].owner.name,
                studentId: events[i].id,
                question: events[i].question,
                duration: events[i].lifeTime,
                unit: 's'
            });

    }, 1000 / 60);
})
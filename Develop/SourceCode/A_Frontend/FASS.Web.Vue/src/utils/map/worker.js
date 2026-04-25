importScripts('/lib/signalr/signalr.min.js');

const update = {
    connection: {},
    init: function (url) {
        update.connection = new signalR.HubConnectionBuilder()
            .withUrl(url)
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();
        update.connection.on("Update", function (data) {
            postMessage({ action: 'update', data: data });
            update.connection.send("Update");
        });
        postMessage({ action: 'init' });
    },
    start: function () {
        update.connection.start().then(function () {
            update.connection.send("Update");
            postMessage({ action: 'start' });
        });
    },
    stop: function () {
        update.connection.stop().then(function () {
            postMessage({ action: 'stop' });
        });
    }
};

onmessage = (event) => {
    if (event.data.action === 'init') {
        update.init(event.data.url);
    } else if (event.data.action === 'start') {
        update.start();
    } else if (event.data.action === 'stop') {
        update.stop();
    }
};
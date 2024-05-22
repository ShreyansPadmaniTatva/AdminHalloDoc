self.addEventListener('fetch', function (event) { });

self.addEventListener('push', function (e) {
    var data = e.data.json(); // Parse JSON data
    var title = data.notification.title; // Extract custom title from payload

    var options = {
        body: data.notification.body,
        icon: "images/LesfImage.png",
        vibrate: [100, 50, 100],
        data: {
            dateOfArrival: Date.now(),
            customData: data.notification.customData
        },
        actions: [
            {
                action: "explore", title: "Go interact with this!",
                icon: "images/checkmark.png"
            },
            {
                action: "close", title: "Ignore",
                icon: "images/red_x.png"
            },
        ]
    };
    e.waitUntil(
        self.registration.showNotification(title, options) // Use custom title here
    );
});

self.addEventListener('notificationclick', function (e) {
    var notification = e.notification;
    var action = e.action;

    if (action === 'close') {
        notification.close();
    } else {
        var customData = notification.data.customData;
        if (customData && customData.url) {
            clients.openWindow(customData.url);
        }
        notification.close();
    }
});

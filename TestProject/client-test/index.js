const signalR = require('@microsoft/signalr');

// Disable SSL/TLS verification
process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

const connection = new signalR.HubConnectionBuilder()
	.withUrl('https://localhost:7137/chathub?chatRoomId=75AFE154-47B0-4F5E-AD9D-63A9A97AAF1C&userId=02007C60-1392-418A-BCB5-1FB0363B96F8', {
		skipNegotiation: true,
		transport: signalR.HttpTransportType.WebSockets,
	})
	.configureLogging(signalR.LogLevel.Information)
	.build();

connection.on('ReceiveMessage', (user, message) => {
	console.log(`${user}: ${message}`);
});

connection
	.start()
	.then(() => {
		console.log('Connected to SignalR hub.');
		connection.invoke('SendMessage', JSON.stringify("hello"));
	})
	.catch((error) => {
		console.error('Error connecting to SignalR hub:', error);
	});

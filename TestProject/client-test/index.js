const signalR = require('@microsoft/signalr');

// Disable SSL/TLS verification
process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

let token = `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwMjAwN2M2MC0xMzkyLTQxOGEtYmNiNS0xZmIwMzYzYjk2ZjgiLCJpZCI6IjAyMDA3YzYwLTEzOTItNDE4YS1iY2I1LTFmYjAzNjNiOTZmOCIsIm5hbWUiOiJLaGFuaCIsInJvbGUiOiJBZG1pbiIsImp0aSI6IjE4MmM1YTQ1LTkxNzItNDJhMy04MzI5LTkyZGE0YmFjNjRiMCIsIm5iZiI6MTY3NjE4ODE4OCwiZXhwIjoxNjc2Mjc0NTg4LCJpYXQiOjE2NzYxODgxODh9.ZUW7uRWxjlHe2pIdCNk9v8MsoGwzOq1iA_SGYmJ9uKo`
const connection = new signalR.HubConnectionBuilder()
	.withUrl('https://localhost:7137/chathub?chatRoomId=75AFE154-47B0-4F5E-AD9D-63A9A97AAF1C', {
		transport: signalR.HttpTransportType.WebSocket,
		headers: {
			Authorization: `bearer ${token}`
		}
	})
	.configureLogging(signalR.LogLevel.Information)
	.build();

	const start = async () => {
		await connection
			.start()
			.then(() => {
				console.log('Connected to SignalR hub.');
			})
			.catch((error) => {
				console.error('Error connecting to SignalR hub:', error);
			});
		await connection.on('UserActive', (user, message) => {
			console.log('=============================================');
			console.log(user, message);
			console.log(message)
			console.log('=============================================');

		});

		await connection.on('NewMessage', (user, message) => {
			console.log('=============================================');
			console.log(user);
			console.log(message);
			console.log('=============================================');

		});
		connection.invoke('SendMessage', 'Hello');
	};
	
	 start();
	

	
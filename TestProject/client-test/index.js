const signalR = require("@microsoft/signalr");

// Disable SSL/TLS verification
process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";

let token = `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI0ZThlMzgzYS03MTgxLTRkM2ItOTA5OC1hYTc4OGZmY2U5ZjYiLCJpZCI6IjRlOGUzODNhLTcxODEtNGQzYi05MDk4LWFhNzg4ZmZjZTlmNiIsIm5hbWUiOiJLaGFuaCIsInJvbGUiOiJBZG1pbiIsImp0aSI6IjU0ODQ2NDk4LTFhZDAtNDc2Ny04YzE2LTVlZWMzMTQ1NGRkZSIsIm5iZiI6MTY3NjI2NjE3NiwiZXhwIjoxNjc2MzUyNTc2LCJpYXQiOjE2NzYyNjYxNzZ9.6A3fvYcUVf8B0PV2l_dp_zgB-Zdyz4hxOXtukUbZkmE`;
let token1 = `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJiMDZjMzUzZS1iMDMyLTRjZTYtOGZhZC04Nzg5MDExMWExOTMiLCJpZCI6ImIwNmMzNTNlLWIwMzItNGNlNi04ZmFkLTg3ODkwMTExYTE5MyIsIm5hbWUiOiJDb24iLCJyb2xlIjoiVXNlciIsImp0aSI6IjYxMWU4NmRiLWU3ODEtNDYyNy1hMWVjLWFlN2MyMjZiYjY1NCIsIm5iZiI6MTY3NjI2MDk5NiwiZXhwIjoxNjc2MzQ3Mzk2LCJpYXQiOjE2NzYyNjA5OTZ9.6GTg-rMWG5c6DvO4Pe4shStE7__WYnsc9lNVRpYwunc`;
const connection = new signalR.HubConnectionBuilder()
  .withUrl(
    "https://localhost:7137/chathub?chatRoomId=00CB5F6A-0122-4ECF-B759-C0D8C5DB5538",
    {
      transport: signalR.HttpTransportType.WebSocket,
      headers: {
        Authorization: `bearer ${token}`,
      },
    }
  )
  .configureLogging(signalR.LogLevel.Information)
  .build();

const start = async () => {
  await connection
    .start()
    .then(() => {
      console.log("Connected to SignalR hub.");
    })
    .catch((error) => {
      console.error("Error connecting to SignalR hub:", error);
    });
  await connection.on("UserActive", (message) => {
    console.log("=============================================");
    console.log(message);
    console.log("=============================================");
  });

  await connection.on("NewMessage", (user, message) => {
    console.log(user);
    console.log(message);
    console.log("=============================================");
  });
  await connection.invoke("SendMessage", "hello hihi");
};

///////////////////////////////////////////////////
// const requestConnection = new signalR.HubConnectionBuilder()
//   .withUrl(
//     "https://localhost:7137/requesthub?groupId=7344B47E-291C-47AA-8706-08CB74E000B4",
//     {
//       transport: signalR.HttpTransportType.WebSocket,
//       headers: {
//         Authorization: `bearer ${token}`,
//       },
//     }
//   )
//   .configureLogging(signalR.LogLevel.Information)
//   .build();
// const requestStart = async () => {
//   await requestConnection
//     .start()
//     .then(() => {
//       console.log("Connected to SignalR hub.");
//     })
//     .catch((error) => {
//       console.error("Error connecting to SignalR hub:", error);
//     });
//   await requestConnection.on("UserActive", (message) => {
//     console.log("=============================================");
//     console.log(message);
//     console.log("=============================================");
//   });

//   await requestConnection.on("ReceiveRequest", (message) => {
//     console.log("=============================================");
//     console.log(message);
//     console.log("=============================================");
//   });
//   requestConnection.invoke("SendRequest", {
//     requestDatetime: "2023-02-12T09:49:56.805Z",
//     status: "string",
//     birdId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//     placeId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//     createdById: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//     groupId: "7344B47E-291C-47AA-8706-08CB74E000B4",
//   });
// };

start();
//requestStart(); đang bugggggggggggggggg

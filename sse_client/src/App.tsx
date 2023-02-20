import { useCallback, useEffect, useState } from 'react'
import reactLogo from './assets/react.svg'
import {Button, FormControl, InputLabel, List, ListItem, ListItemButton, ListItemText, MenuItem, Select, TextField} from '@mui/material'
import './App.css'
import { Box,Chip } from '@mui/material';
import { ConnectionEndpointWrapper, EventForm, Message, MessageContainer, MessageServer, SendEventWrapper } from './style';
import {HubConnection, HubConnectionBuilder,HttpTransportType} from '@microsoft/signalr'
import React from "react";
import useHub from './useHub';
function App() {
  const [endpoint,setEndpoint] = useState("https://localhost:7137/hub/test");
  const {sendMessageFromClient,connection} = useHub({endpoint});
  const [connectErr,setCnErr] = useState<{success:boolean , msg : string} | null>(null);
  /* Send form */
  const [eventName,setEventName] = useState<string>();
  const [eventSendConent,setSendContent] = useState<any>();
  /* Receive form */
  const [listeningEvents,setListeningEvents] = useState<string[]>([]);
  const [messages,setMessages] = useState<{eventName:string, msg:string}[]>([]);
  
  const [draftEventName,setDraftEventName] = useState<string>();

  const handleConnectClick = useCallback(async()=>{
    
  },[connection,endpoint,listeningEvents])
  const sendEventHandler = useCallback(async()=>{
    sendMessageFromClient(eventName as string,eventSendConent);
  },[eventName,eventSendConent])


  const addEventHandler = useCallback(async()=>{
    if(listeningEvents.findIndex((ev)=>ev == draftEventName ) == -1 ){
      setListeningEvents([...listeningEvents,draftEventName as string]);
    } else {
      window.alert("event exist")
    }
  },[listeningEvents,draftEventName])
  /** useEffect 
   * 
   */
  /* Connection start on change*/
  useEffect(()=>{
    if(connection) {
      connection.start().then(()=> {
        console.log("HUB CONNECTED");
        connection.send("SendMessage", "LE THANH PHONG ");
        
      });
    }
    connection?.on("RECEIVE_MSG",(msg)=> {console.log("TEST HUB OKE " + msg)})
  },[connection]);

  useEffect(()=>{
    if(connection?.state == 'Connected') setCnErr({success :true , msg : "Connected successfully"})
    connection?.on("RECEIVE_MSG", (msg)=>{
      console.log("FROM SERVER" , msg)
    })
    connection?.on("MSG", (msg)=>{
      console.log("FROM SERVER" , msg)
    })
    connection?.on("MSG2", (msg)=>{
      console.log("FROM SERVER" , msg)
    })
  },[connection])

  useEffect(()=>{
    console.log(endpoint);
    
  },[endpoint])

  useEffect(()=>{
    connection?.on("RECEIVE_MSG", (msg)=>{
      console.log("FROM SERVER" , msg)
    })
    console.log(listeningEvents)
  },[listeningEvents])
  return (
    <>
      <Box component={"div"} className="App">
        <h1>Server Sent Event Tools</h1>
        {connectErr && (
          <Message
            label={connectErr.msg}
            color={connectErr.success ? "success" : "error"}
          />
        )}
        <ConnectionEndpointWrapper>
          <TextField
            type="text"
            label="Endpoint"
            variant="outlined"
            onChange={(e) => setEndpoint(e.target.value)}
            defaultValue={endpoint}
          ></TextField>
          <Button onClick={handleConnectClick} variant="contained">
            Connect
          </Button>
        </ConnectionEndpointWrapper>
        <SendEventWrapper>
          <h2>Sending event</h2>
          <EventForm>
            <TextField
              label="Event"
              onChange={(e) => setEventName(e.target.value)}
            ></TextField>
            <TextField
              label="Content"
              onChange={(e) => setSendContent(e.target.value)}
            ></TextField>
            <Button onClick={sendEventHandler} variant="contained">
              Send
            </Button>
          </EventForm>
        </SendEventWrapper>
      </Box>
      <SendEventWrapper>
        <h2>Message from server section</h2>
        <EventForm>
          <TextField
            label="Listen on event"
            onChange={(e) => setDraftEventName(e.target.value)}
          ></TextField>
          <Button onClick={addEventHandler} variant="contained">
            Add
          </Button>
        </EventForm>
        <FormControl fullWidth>
          <InputLabel id="demo-simple-select-label">Registered event</InputLabel>
          <Select
            labelId="demo-simple-select-label"
            id="demo-simple-select"
            
            label="Registered event"
            onChange={() => console.log("change")}
          >
            {listeningEvents?.map(event=><MenuItem key={event} value={listeningEvents.join(', ')} >{event}</MenuItem>)}
          </Select>
        </FormControl>
        <MessageContainer>
            {messages?.map((message,i)=> <MessageServer key={i}>
            <Chip
              label={message.eventName}
              color="secondary"
              style={{ fontWeight: 600 }}
            ></Chip>
            <span>{message.msg}</span>
          </MessageServer>)}
          
        </MessageContainer>
      </SendEventWrapper>
    </>
  );
}

export default App

import { useCallback, useEffect, useState } from "react";
import reactLogo from "./assets/react.svg";
import {
  Button,
  FormControl,
  InputLabel,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  MenuItem,
  Select,
  TextField,
} from "@mui/material";
import "./App.css";
import { Box, Chip } from "@mui/material";
import {
  ConnectionEndpointWrapper,
  EventForm,
  Message,
  MessageContainer,
  MessageServer,
  SendEventWrapper,
} from "./style";
import {
  HubConnection,
  HubConnectionBuilder,
  HttpTransportType,
} from "@microsoft/signalr";
import useHubConnection from "./useHubConnection";

function App() {
  const [connection, setConnection] = useState<HubConnection>();
  const [endpoint, setEndpoint] = useState("https://localhost:7137/hub/test");
  const { handleConnectClick } = useHubConnection(endpoint, (hub) => {
    listeningEvents?.forEach((event) => {
      connection?.on(event, (msg) => {
        setMessages([...messages, { eventName: event, msg }]);
      });
    });
    setConnection(hub);
  });
  const [connectErr, setCnErr] = useState<{
    success: boolean;
    msg: string;
  } | null>(null);

  const [eventName, setEventName] = useState<string>();
  const [eventSendConent, setSendContent] = useState<any>();
  const [listeningEvents, setListeningEvents] = useState<string[]>([]);
  const [messages, setMessages] = useState<any[]>([]);

  const [draftEventName, setDraftEventName] = useState<string>();

  const sendEventHandler = useCallback(async () => {
    connection?.send(eventName as string, eventSendConent);
    // console.log(connection);
    // console.log(eventName);
    // console.log(eventSendConent);
  }, [connection, eventName, eventSendConent]);

  const addEventHandler = useCallback(async () => {
    if (listeningEvents.findIndex((ev) => ev == draftEventName) == -1) {
      setListeningEvents([...listeningEvents, draftEventName as string]);
    } else {
      window.alert("event exist");
    }
  }, [listeningEvents, draftEventName]);
  /** useEffect
   *
   */
  useEffect(() => {
    const hubClient = new HubConnectionBuilder()
      .withUrl("https://localhost:7137/hub/test", {
        transport: HttpTransportType.ServerSentEvents,
      })
      .build();
    setConnection(hubClient);
  }, []);
  useEffect(() => {}, [connection]);

  useEffect(() => {
    //TODO : listening event
    listeningEvents?.forEach((event) => {
      connection?.on(event, (...msg) => {
        console.log("TEST HUB OKE " + msg);
        console.log(msg);
        setMessages((prevMessages) => [
          ...prevMessages,
          { eventName: event, msg: JSON.stringify(msg) } as any,
        ]);
      });
    });
  }, [listeningEvents]);
  useEffect(() => {
    console.log(messages);
  }, [messages]);
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
          <InputLabel id="demo-simple-select-label">
            Registered event
          </InputLabel>
          <Select
            labelId="demo-simple-select-label"
            id="demo-simple-select"
            label="Registered event"
            onChange={() => console.log("change")}
          >
            {listeningEvents?.map((event, i) => (
              <MenuItem key={i} value={listeningEvents.join(", ")}>
                {event}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
        <MessageContainer>
          {messages?.map((message, i) => (
            <MessageServer key={i}>
              <Chip
                label={message.eventName}
                color="secondary"
                style={{ fontWeight: 600 }}
              ></Chip>
              <span>{message.msg}</span>
            </MessageServer>
          ))}
        </MessageContainer>
      </SendEventWrapper>
    </>
  );
}

export default App;

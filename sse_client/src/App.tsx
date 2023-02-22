import { useCallback, useEffect, useRef, useState } from "react";
import reactLogo from "./assets/react.svg";
import { Visibility } from "@mui/icons-material";
import { Prism as SyntaxHighlighter } from "react-syntax-highlighter";
import {
  Button,
  Divider,
  FormControl,
  InputLabel,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  MenuItem,
  Select,
  TextField,
  Typography,
} from "@mui/material";
import "./App.css";
import { Box, Chip } from "@mui/material";
import "react-toastify/dist/ReactToastify.css";
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
import EventListeningList from "./EventListeningList";
import { toast, ToastContainer } from "react-toastify";
import JsonBeautify from "./JsonBeautify";
import styled from "styled-components";

function App() {
  const [connection, setConnection] = useState<HubConnection>();
  const [endpoint, setEndpoint] = useState("https://localhost:7137/hub/chat");
  const [token, setToken] = useState<string>("");
  const tokenRef = useRef<any>();
  const { handleConnectClick } = useHubConnection(endpoint, (hub) => {
    listeningEvents?.forEach((event) => {
      connection?.on(event, (msg) => {
        setMessages([...messages, { eventName: event, msg }]);
      });
    });
    setConnection(hub);
  },token);
  const [eventName, setEventName] = useState<string>();
  const [eventSendConent, setSendContent] = useState<any>({
    one: "",
    two: "",
    three: "",
  });
  const [listeningEvents, setListeningEvents] = useState<string[]>([]);
  const [messages, setMessages] = useState<any[]>([]);

  const [draftEventName, setDraftEventName] = useState<string>();

  const [resultsBeautified, setResultsBeautified] = useState<any>();

  const sendEventHandler = useCallback(async () => {
    if (!connection) {
      toast.error("Not connected ! Please connect and try again");
      return;
    }
    const params = [...Object.values(eventSendConent)] as string[];
    const finalParams: any[] = [];
    params.forEach((param: string) => {
      if (isJsonParsable(param)) param = JSON.parse(param);
      if (param) finalParams.push(param);
    });
    console.log(finalParams);
    connection?.send(eventName as string, ...finalParams);
  }, [connection, eventName, eventSendConent]);

  const addEventHandler = useCallback(async () => {
    if (!connection) {
      toast.error("Not connected ! Please connect and try again");
      return;
    }
    if (listeningEvents.findIndex((ev) => ev == draftEventName) == -1) {
      setListeningEvents([...listeningEvents, draftEventName as string]);
    } else {
      window.alert("event exist");
    }
  }, [listeningEvents, draftEventName]);

  const deleteListeningEvents = useCallback(
    (name: string) => {
      setListeningEvents(listeningEvents.filter((event) => event !== name));
    },
    [listeningEvents, draftEventName]
  );
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
        console.log(JSON.stringify(msg));
        setMessages((prevMessages) => [
          ...prevMessages,
          { eventName: event, msg: JSON.stringify(msg), raw: msg } as any,
        ]);
      });
    });
  }, [listeningEvents]);
  useEffect(() => {
    console.log(token);
  }, [token]);
  useEffect(() => {
    console.log(connection);
  }, [connection]);

  return (
    <>
      <Box component={"div"} className="App" maxWidth={"50%"}>
        <h1>Server Sent Event Tools</h1>
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
        <ConnectionEndpointWrapper>
          <TextField
            type="text"
            label="Bearer Token"
            variant="outlined"
            inputRef={tokenRef}
      
          ></TextField>
          <Button onClick={()=>{
            setToken(tokenRef.current.value);

          }} variant="contained">
            Set token 
          </Button>
        </ConnectionEndpointWrapper>
        <Divider variant="fullWidth" light />
        <Typography fontSize={"1.5rem"} fontWeight={600}>
          {"Register events for listening"}
        </Typography>
        <EventForm>
          <TextField
            label="Listen on event"
            onChange={(e) => setDraftEventName(e.target.value)}
          ></TextField>
          <Button onClick={addEventHandler} variant="contained">
            Add
          </Button>
        </EventForm>
        <div>
          <InputLabel id="demo-simple-select-label">
            Registered event(s)
          </InputLabel>
          <EventListeningList
            events={listeningEvents}
            deleteHandler={deleteListeningEvents}
          />
        </div>
        <SendEventWrapper>
          <h2>Sending event</h2>
          <EventForm>
            <TextField
              label="Event"
              onChange={(e) => setEventName(e.target.value)}
            ></TextField>
            <TextField
              label="Param 1"
              onChange={(e) =>
                setSendContent({ ...eventSendConent, one: e.target.value })
              }
            ></TextField>
            <TextField
              label="Param 2"
              onChange={(e) =>
                setSendContent({ ...eventSendConent, two: e.target.value })
              }
            ></TextField>
            <TextField
              label="Param 3"
              onChange={(e) =>
                setSendContent({ ...eventSendConent, three: e.target.value })
              }
            ></TextField>
            <Button onClick={sendEventHandler} variant="contained">
              Send
            </Button>
          </EventForm>
        </SendEventWrapper>
      </Box>
      <SendEventWrapper>
        <h2>Message from server section</h2>

        <JsonBeautify
          results={
            typeof resultsBeautified == "string"
              ? [resultsBeautified]
              : resultsBeautified
          }
        />
        <MessageContainer>
          {messages?.map((message, i) => (
            <MessageServer key={i}>
              <Chip
                label={message.eventName}
                color="secondary"
                style={{ fontWeight: 600 }}
              ></Chip>
              <span>{message.msg}</span>
              <ViewButton onClick={() => setResultsBeautified(message.raw)} />
            </MessageServer>
          ))}
        </MessageContainer>
        {/* <SyntaxHighlighter language="json" wrapLines={true}>
      {JSON.stringify(JSON.parse('{ "name": "John", "age": 30, "city": "New York" }' ), null, 2)}
    </SyntaxHighlighter> */}
      </SendEventWrapper>
      <ToastContainer />
    </>
  );
}

export default App;

const ViewButton = styled(Visibility)`
  cursor: pointer;
`;
export function isJsonParsable(str: string) {
  try {
    JSON.parse(str);
    return true;
  } catch (e) {
    return false;
  }
}

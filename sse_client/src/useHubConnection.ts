import React, { useCallback, useEffect } from "react";
import {
  HubConnection,
  HubConnectionBuilder,
  HttpTransportType,
} from "@microsoft/signalr";
import { toast } from "react-toastify";
type Props = {};

const useHubConnection = (
  endpoint: string,
  onConnectCallback: (hub: HubConnection) => void,token:string
) => {
  useEffect(() => {
   
    const hubClient = new HubConnectionBuilder()
      .withUrl(endpoint, {
        transport: HttpTransportType.WebSockets,
        skipNegotiation: true,
        // accessTokenFactory: () => token,
        // withCredentials :true,
        
      })
      // .withAutomaticReconnect()
      .build();
      
    hubClient?.start().then(()=>toast.success("Connection established")).catch(()=>toast.error("Connection failed! Check console log for errors"));

    onConnectCallback(hubClient);
    
  }, [token]);
  const handleConnectClick = useCallback(() => {
    const hubClient = new HubConnectionBuilder()
      .withUrl(endpoint, {
        transport: HttpTransportType.ServerSentEvents,
        skipNegotiation: true,
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .build();
      
    hubClient?.start().then(()=>toast.success("Connection established")).catch(()=>toast.error("Connection failed! Check console log for errors"));

    onConnectCallback(hubClient);
  }, [endpoint,token]);
  return {
    handleConnectClick,
  };
};

export default useHubConnection;

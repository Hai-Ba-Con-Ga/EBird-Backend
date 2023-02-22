import React, { useCallback, useEffect } from "react";
import {
  HubConnection,
  HubConnectionBuilder,
  HttpTransportType,
} from "@microsoft/signalr";
type Props = {};

const useHubConnection = (
  endpoint: string,
  onConnectCallback: (hub: HubConnection) => void
) => {
  useEffect(() => {
    // const hubClient = new HubConnectionBuilder()
    //   .withUrl(endpoint, {
    //     transport: HttpTransportType.ServerSentEvents,
    //   })
    //   .withAutomaticReconnect()
    //   .build();
    // onConnectCallback(hubClient);
  }, [endpoint]);
  const handleConnectClick = useCallback(() => {
    const hubClient = new HubConnectionBuilder()
      .withUrl(endpoint, {
        transport: HttpTransportType.ServerSentEvents,
      })
      .withAutomaticReconnect()
      .build();
    hubClient?.start();

    onConnectCallback(hubClient);
  }, [endpoint]);
  return {
    handleConnectClick,
  };
};

export default useHubConnection;

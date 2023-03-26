import { HttpTransportType, HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import React, { useCallback, useEffect, useState } from 'react'

type Props = {}

const useHub = ({
    endpoint 
}:{endpoint:string}) => {
    /** State */
    const [connection,setCn] = useState<HubConnection>();
    useEffect(()=>{
        setCn(new HubConnectionBuilder().withUrl(endpoint,{
            transport: HttpTransportType.WebSockets,
            skipNegotiation:true
          }).configureLogging(LogLevel.Information).build());
    },[endpoint])
    const sendMessageFromClient = useCallback((eventName : string,args : any)=> {
        connection?.send(eventName, args);
    },[connection])
  return (
    {
        connection : connection,
        sendMessageFromClient,
    }
    )
}

export default useHub
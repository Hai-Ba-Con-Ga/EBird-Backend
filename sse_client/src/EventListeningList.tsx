import { Chip } from '@mui/material'
import React from 'react'
import { EventListeningListWrapper } from './style'

type Props = {
    events: string[]
    deleteHandler : (eventName :string)=> void
}

const EventListeningList = ({events,deleteHandler}: Props) => {
  return (
    <EventListeningListWrapper>{
        events.map((event)=><Chip label={event} key={event} color={'default'} onDelete={()=>deleteHandler(event)}/>)
        }
    </EventListeningListWrapper>
  )
}

export default EventListeningList
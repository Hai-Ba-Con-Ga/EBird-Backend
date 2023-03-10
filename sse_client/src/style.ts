import { Box, Chip } from "@mui/material";
import styled from "styled-components";
export const ConnectionEndpointWrapper = styled.form`
  display: flex;
  gap: 1rem;
  align-items: center;
  justify-content: center;
`;
export const Message = styled(Chip)`
  margin: 2rem 0;
`;
export const SendEventWrapper = styled.div`
  margin-top: 1rem;
  display: flex;
  gap: 1rem;
  flex-direction: column;
  max-width: 50%;
`;
export const EventForm = styled.form`
  display: flex;
  flex-direction: column;
  gap: 1rem;
`;
export const MessageContainer = styled(Box)`
  height: 100%;
  width: 100%;
  min-height: 10rem;
  background-color: #f4f7f7;
  border-radius: 5px;
  border: 2px solid #848080;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  padding: 1rem;
`;

export const MessageServer = styled.span`
  display: flex;
  gap: 1rem;
  align-items: center;
  margin: 0.5rem 0;
`;
export const EventListeningListWrapper = styled.div`
  display: flex;
  width: 100%;;
  gap: 1rem;
  flex-wrap: wrap;
  justify-content: center;
`
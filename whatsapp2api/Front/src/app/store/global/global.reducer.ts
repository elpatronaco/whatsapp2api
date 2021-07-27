import {IAccessTokenPayload} from "../../models/Dto/Auth/IAccessTokenPayload";
import {Action, createReducer, on} from "@ngrx/store";
import {
  AddMessage,
  LoginSuccess,
  SetChatRecipient,
  SetChats,
  SetMessages,
  ValidateUserRequest,
  ValidateUserSuccess
} from "./global.actions";
import {IUser} from "../../models/Dto/User/IUser";
import {IMessage} from "../../models/Dto/Message/IMessage";
import {IOpenChat} from "../../models/Dto/Chat/IOpenChat";


export interface IGlobalState {
  user: IAccessTokenPayload | null,
  recipient: IUser | null,
  chats: IOpenChat[],
  messages: IMessage[],
}

const initialState: IGlobalState = {
  user: null,
  recipient: null,
  chats: [],
  messages: []
}

const globalReducer = createReducer(initialState,
  on(LoginSuccess, ValidateUserSuccess, (state, action) => ({...state, user: action})),
  on(SetChatRecipient, (state, action) => ({...state, recipient: action})),
  on(SetChats, (state, action) => ({...state, chats: action.chats})),
  on(SetMessages, (state, action) => ({...state, messages: action.messages})),
  on(AddMessage, (state, action) => {
    const messages = state.messages.slice();
    const chats = state.chats.slice();

    if (state.recipient && action.recipientId === state.recipient.id) {
      messages.push(action);
    }

    const recipientIndex = chats.findIndex(x => x.recipient.id === action.recipientId);

    if (recipientIndex !== -1)
      chats[recipientIndex] = {...chats[recipientIndex], lastMessage: action};

    return {...state, chats, messages};
  })
)

export function reducer(state: IGlobalState | undefined, action: Action) {
  return globalReducer(state, action);
}

import {IUser} from "../User/IUser";
import {IMessage} from "../Message/IMessage";

export interface IOpenChat {
    Recipient: IUser
    LastMessage: IMessage
}
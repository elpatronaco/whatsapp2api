import {IUser} from "../User/IUser";

export interface IMessage {
    Id: string
    Sender: IUser
    Recipient: IUser
    Content: string
    SentDate: Date
}
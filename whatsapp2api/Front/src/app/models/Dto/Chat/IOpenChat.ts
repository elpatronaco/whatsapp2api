import {IUser} from "../User/IUser";
import {IMessage} from "../Message/IMessage";

export interface IOpenChat {
  readonly recipient: IUser
  readonly lastMessage?: IMessage
}

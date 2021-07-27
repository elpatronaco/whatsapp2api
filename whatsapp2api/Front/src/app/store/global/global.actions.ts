import {IUserAuthenticate} from "../../models/Dto/User/IUserAuthenticate";
import {createAction, props} from "@ngrx/store";
import {IAccessTokenPayload} from "../../models/Dto/Auth/IAccessTokenPayload";
import {IUser} from "../../models/Dto/User/IUser";
import {IOpenChat} from "../../models/Dto/Chat/IOpenChat";
import {IMessage} from "../../models/Dto/Message/IMessage";

export const LoginRequest = createAction("[GLOBAL] Login Request", props<IUserAuthenticate>())
export const LoginSuccess = createAction("[GLOBAL] Login Success", props<IAccessTokenPayload>());
export const LoginFailure = createAction("[GLOBAL] Login Failure");

export const ValidateUserRequest = createAction("[GLOBAL] Validate User Request")
export const ValidateUserSuccess = createAction("[GLOBAL] Validate User Success", props<IAccessTokenPayload>());
export const ValidateUserFailure = createAction("[GLOBAL] Validate User Failure");

export const SetChatRecipient = createAction("[GLOBAL] Set Chat Recipient", props<IUser>())

export const SetChats = createAction("[GLOBAL] Set Chats", props<{ chats: IOpenChat[] }>());

export const SetMessages = createAction("[GLOBAL] Set Messages", props<{messages: IMessage[]}>());
export const AddMessage = createAction("[GLOBAL] Add Message", props<IMessage>());

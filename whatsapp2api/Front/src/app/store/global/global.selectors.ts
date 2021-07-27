import {createFeatureSelector, createSelector} from "@ngrx/store";
import {IGlobalState} from "./global.reducer";


const globalSelector = createFeatureSelector<IGlobalState>("global");

export const getOpenChats = createSelector(globalSelector, state => state.chats);
export const getRecipient = createSelector(globalSelector, state => state.recipient);
export const getMessages = createSelector(globalSelector, state => state.messages);
export const getCurrUser = createSelector(globalSelector, state => state.user);

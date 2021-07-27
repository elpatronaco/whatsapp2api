import {Component, Input} from "@angular/core";
import {IOpenChat} from "../../../models/Dto/Chat/IOpenChat";
import {MessageService} from "../../../services/message.service";
import {Store} from "@ngrx/store";
import {IState} from "../../../store/state.module";
import {SetChatRecipient} from "../../../store/global/global.actions";

@Component({
  selector: "app-chat-row", styleUrls: ["./chatRow.component.sass"], template: `
    <div class="row__wrapper" *ngIf="chat !== undefined" (click)="onSelect()">
      <img class="avatar" src="assets/userplaceholder.svg" alt="avatar placeholder">
      <div class="additional-info__wrapper">
        <p class="additional-info__recipient">{{chat.recipient.username}}</p>
        <p class="additional-info__message" *ngIf="chat.lastMessage !== undefined">{{chat.lastMessage.content}}</p>
      </div>
    </div>
  `
})
export class ChatRowComponent {
  @Input("openChat") chat: IOpenChat;

  constructor(private chatService: MessageService, private store: Store<IState>) {
  }

  async onSelect() {
    await this.chatService.openChat(this.chat.recipient);

    this.store.dispatch(SetChatRecipient(this.chat.recipient));
  }
}


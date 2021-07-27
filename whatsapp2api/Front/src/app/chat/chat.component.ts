import {Component, OnDestroy, OnInit} from "@angular/core";
import {IOpenChat} from "../models/Dto/Chat/IOpenChat";
import {MessageService} from "../services/message.service";
import {Subscription} from "rxjs";
import {UserService} from "../services/user.service";
import {IUser} from "../models/Dto/User/IUser";
import {AuthService} from "../services/auth.service";
import {Store} from "@ngrx/store";
import {IState} from "../store/state.module";
import {ValidateUserRequest} from "../store/global/global.actions";
import {getOpenChats, getRecipient} from "../store/global/global.selectors";

@Component({
  selector: "app-chat", styleUrls: ["./chat.component.sass"], template: `
    <div class="main__wrapper">
      <div class="main__window">
        <div class="sidebar">
          <div class="new-contact__wrapper" [ngClass]="{'active': openMenu}">
            <header>
              <div>
                <img src="assets/arrow.svg" (click)="openMenu = false;">
                <p>Nuevo chat</p>
              </div>
            </header>
            <div class="new-contact__window">
              <app-chat-row *ngFor="let user of allUsers" [openChat]="{recipient: user}"></app-chat-row>
            </div>
          </div>
          <div class="toolbar">
            <img src="assets/userplaceholder.svg" class="avatar">
          </div>
          <div class="rows__wrapper">
            <app-chat-row *ngFor="let chat of chats" [openChat]="chat"></app-chat-row>
            <span class="new-chat-button" (click)="openNewContactMenu()" *ngIf="!openMenu">+</span>
          </div>
        </div>
        <div class="chat__wrapper" [class.bordered]="!recipientUser">
          <app-messaging *ngIf="!!recipientUser" [recipientUser]="recipientUser"></app-messaging>
        </div>
      </div>
    </div>
  `
})
export class ChatComponent implements OnInit, OnDestroy {
  constructor(private authService: AuthService, private messageService: MessageService,
              private userService: UserService, private store: Store<IState>) {
  }

  public chats: IOpenChat[] = [];
  public allUsers: IUser[] = [];
  public recipientUser: IUser | null = null;
  public openMenu = false;
  private subscriptions: Subscription[] = [];

  async ngOnInit() {
    this.store.dispatch(ValidateUserRequest());

    this.subscriptions.push(
      this.store.select(getOpenChats).subscribe(chats => {
        this.chats = chats;
      })
    );

    this.subscriptions.push(
      this.store.select(getRecipient).subscribe(user => {
        this.recipientUser = user;
        if (this.openMenu) this.openMenu = false;
      })
    );
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  openNewContactMenu() {
    this.userService.getAllUsers().subscribe(x => {
      this.allUsers = x;
      this.openMenu = true;
    })
  }
}

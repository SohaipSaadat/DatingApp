import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { IUser } from '../Models/iuser';
import { BehaviorSubject, take } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  private baseUrl = 'https://localhost:7252/hubs/';
  private hubConnection?: HubConnection;
  private onlineUsersSource : BehaviorSubject<string[]> = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();
  constructor(private toastr: ToastrService, private router: Router) {}

  createHubConnection(user: IUser) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.baseUrl}presence`, {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();
    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('UserIsOnline', (userName) => {
      this.toastr.info(`User ${userName} is online`);
    });

    this.hubConnection.on('UserIsOffline', (userName) => {
      this.toastr.warning(`User ${userName} is offline`);
    });

    this.hubConnection.on("GetOnlineUsers", userNames => {
      this.onlineUsersSource.next(userNames);
    })
    this.hubConnection.on("newMessageRecevied", ({id, knownAs})=>{
      console.log(`User ${id}`)
      this.toastr.info(`${knownAs} has sent you a new message, Click me to see it`)
      .onTap
      .pipe(take(1))
      .subscribe({
        next: ()=> this.router.navigateByUrl('/members/' + id + '?tab=Message') 
      })
      
    })
  }

  stopConnection() {
    this.hubConnection?.stop().catch((error) => console.log(error));
  }
}

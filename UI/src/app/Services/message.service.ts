import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getPaginatedResult, getPaginationHeaders } from './PaginationHelper';
import { Message } from '../Models/message';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { IUser } from '../Models/iuser';
import { BehaviorSubject, take } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private baseUrl = 'https://localhost:7252/api';
  private hubsUrl = 'https://localhost:7252/hubs/';

  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  hubConnection?: HubConnection;
  constructor(private http: HttpClient) {}

  createHubConnection(user: IUser, otherUser: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubsUrl}message?user=${otherUser}`, {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

      this.hubConnection.start().then().catch((error) => console.log(error));

    

    this.hubConnection.on('ReceiveMessageThread', (messages) => {
      console.log("ReceiveMessageThread")
      this.messageThreadSource.next(messages);
    });

    this.hubConnection.on('NewMessage', (message) => {
      console.log(message);
      this.messageThread$.pipe(take(1)).subscribe({
        next: (messages) => {
          this.messageThreadSource.next([...messages, message]);
        },
      });
    });

  }

  stopHubConnection() {
    if(this.hubConnection){
      console.log('stopHubConnection')
      this.hubConnection.stop();
    }
  }

  getMessage(pageNumber: number, pageSize: number, container: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResult<Message[]>(
      `${this.baseUrl}/Messages`,
      params,
      this.http
    );
  }

  getMessagesThread(userName: string) {
    return this.http.get<Message[]>(
      `${this.baseUrl}/Messages/thread/${userName}`
    );
  }

  async sendMessage(userName: string, content: string) {
    if(this.hubConnection){
      
      return await this.hubConnection.invoke('SendMessage', { ReciverUserName: userName, content })
      .catch((error) => console.log(error));
    }
  }

  deleteMessage(id: number) {
    return this.http.delete(`${this.baseUrl}/Messages/${id}`);
  }
}

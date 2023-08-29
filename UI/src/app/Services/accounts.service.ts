import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IUser } from '../Models/iuser';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {
  private baseUrl = 'https://localhost:7252/api/';
  private currentUserSource : BehaviorSubject<IUser | null> = new BehaviorSubject<IUser | null>(null)
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http: HttpClient, private presenceService : PresenceService) { }

  login(model:any){
    return this.http.post<IUser>(`${this.baseUrl}Accounts/login`, model).pipe(
      map((response : IUser)=>{
        const user = response;
        if(user){
          this.setCurrentUser(user)
          //this.presenceService.createHubConnection(user)
        }
      })
    );
  }
register(model:any){
  return this.http.post<IUser>(`${this.baseUrl}Accounts/register`, model).pipe(
    map((response : IUser)=>{
      const user = response;
      if(user){
        this.setCurrentUser(user)
      }
    })
  );
}
  setCurrentUser(user: IUser){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    console.log(roles)
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem("user", JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem("user");
    this.currentUserSource.next(null);
    this.presenceService.stopConnection();
  }

  getDecodedToken(token:string){
    return JSON.parse(atob(token.split('.')[1]))
  }
}

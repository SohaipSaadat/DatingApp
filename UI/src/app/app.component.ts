import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { IUser } from './Models/iuser';
import { AccountsService } from './Services/accounts.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  users! : object;
  constructor( private accountServices: AccountsService) {}
  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user : IUser = JSON.parse(userString);
    this.accountServices.setCurrentUser(user);
  } 
}

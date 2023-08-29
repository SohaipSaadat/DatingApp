import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { IUser } from 'src/app/Models/iuser';
import { AccountsService } from 'src/app/Services/accounts.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {};
  userName: string | undefined
  currentUser$: Observable<IUser | null> = of(null);
  constructor(
    private accountService: AccountsService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.currentUser$ = this.accountService.currentUser$;
    
  }
  ngOnInit(): void {
    this.userExist();
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: () => {this.router.navigate(['/members']);this.userExist()},
      error: () => this.toastr.error('Invalid Email or Password'),
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigate(['/']);
  }
  userExist(){
    const userNameString = localStorage.getItem('user');
    if(!userNameString) return
    const userName = JSON.parse(userNameString);
    this.userName = userName.userName
  }
}

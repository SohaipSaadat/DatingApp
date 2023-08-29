import {  inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountsService } from '../Services/accounts.service';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';

export const adminGuard: CanActivateFn = (route, state) => {
  let accountsService = inject(AccountsService)
  let toastr = inject(ToastrService)
  return accountsService.currentUser$.pipe(
    map(user=> {
      if (!user) return false;
      if(user.roles.includes("Admin") || user.roles.includes("Moderator")){
        console.log(user)
        return true;
      }
      else {
        toastr.error("You are not allowed to enter this area")
        return false;
      };
    })
  );
};

import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountsService } from '../Services/accounts.service';
import { map } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = () => {
  const accountsService = inject(AccountsService)
  const toastr = inject(ToastrService);
  const router = inject(Router);
  return accountsService.currentUser$.pipe(
    map((user)=>{
      if (user) return true;
      else{
        toastr.error("You are Unauthorized");
        router.navigate(['/'])
        return false;
      };
    })
  );
};

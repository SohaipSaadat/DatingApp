import {
  Directive,
  Input,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { IUser } from 'src/app/Models/iuser';
import { AccountsService } from 'src/app/Services/accounts.service';

@Directive({
  selector: '[appHasRole]',
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[] = [];
  test: string = 'admin';
  user: IUser = {} as IUser;
  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainerRef: ViewContainerRef,
    private accountService: AccountsService
  ) {
   this.accountService.currentUser$.subscribe({
      next: (user) => {
        if (user) this.user = user;
      },
    });
  }
  ngOnInit(): void {
    if(this.user.roles.some(role=> this.appHasRole.includes(role))){
      this.viewContainerRef.createEmbeddedView(this.templateRef)
    }else{
      this.viewContainerRef.clear();
    }
  }
  
}

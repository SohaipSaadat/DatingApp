import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/Models/imember';
import { IUser } from 'src/app/Models/iuser';
import { Pagination } from 'src/app/Models/pagination';
import { UserParams } from 'src/app/Models/user-params';
import { AccountsService } from 'src/app/Services/accounts.service';
import { MemberService } from 'src/app/Services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  members: Member[] | undefined = [];
  pagination: Pagination | undefined;
  user: IUser | undefined ;
  userParams: UserParams | undefined
  genderList = [{value: 'male', display:"Male"}, {value: 'female', display:"Female"}]
  constructor(private memberService: MemberService, accountService: AccountsService) {
    accountService.currentUser$.subscribe({
      next: user=>{
        if(user){
          this.user = user;
          this.userParams = new UserParams(user);
        }
      }
    })
  }
  ngOnInit(): void {
    this.loadMembers();
  }
  loadMembers() {
    if(!this.userParams) return;
    this.memberService.getMembers(this.userParams).subscribe({
      next: response=>{
        this.members = response.result;
        this.pagination = response.pagination
      }
    })
  }
  resetFilter() {
    if(!this.user) return;
    this.userParams = new UserParams(this.user)
    this.loadMembers();
  }
  pageChanged(event: any) {
    if (this.userParams && this.userParams.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.loadMembers();
    }
  }
}

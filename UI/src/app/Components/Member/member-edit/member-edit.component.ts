import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { every, take } from 'rxjs';
import { Member } from 'src/app/Models/imember';
import { IUser } from 'src/app/Models/iuser';
import { AccountsService } from 'src/app/Services/accounts.service';
import { MemberService } from 'src/app/Services/member.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
  @ViewChild('EditFrm') editFrm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification(
    $event: any
  ) {
    if (this.editFrm?.dirty) {
      $event.returnValue = true;
    }
  }
  member: Member | undefined;
  user: IUser | null = null;
  constructor(
    private accountService: AccountsService,
    private memberService: MemberService,
    private toastr: ToastrService
  ) {
    this.accountService.currentUser$.subscribe({
      next: (res) => (this.user = res),
    });
  }
  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    if (!this.user) return;
    this.memberService.getMember(this.user.userName).subscribe({
      next: (res) => (this.member = res),
    });
  }

  updateProfile() {
    this.memberService.updateMember(this.editFrm?.value).subscribe({
      next: (_) => {
        this.toastr.success('Profile updated');
        this.editFrm?.reset(this.member);
      },
    });
  }
}

import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { IUser } from 'src/app/Models/iuser';
import { AdminService } from 'src/app/Services/admin.service';
import { RolesModalComponent } from '../../Modal/roles-modal/roles-modal.component';
import { MemberCardComponent } from '../../Member/member-card/member-card.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css'],
})
export class UserManagementComponent implements OnInit {
  users: IUser[] = [];
  availabelRoles = ['Admin', 'Moderator', 'Member'];
  test = 0;
  constructor(
    private adminService: AdminService,
    private modalService: BsModalService,
    public modalRef: BsModalRef<RolesModalComponent>
  ) {}

  ngOnInit(): void {
    this.getUsersWithRole();
    console.log('Users management')
  }

  getUsersWithRole() {
    this.adminService.getUsersWithRoles().subscribe({
      next: (users) => {
        this.users = users;
      },
    });
  }

  openRolesModal(user: IUser) {
    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        userName: user.userName,
        availabelRoles: this.availabelRoles,
        selectedRoles: [...user.roles],
      },
    };
    this.modalRef = this.modalService.show(RolesModalComponent, config);
    this.modalRef.onHide?.subscribe({
      next: () => {
        const selectedRoles = this.modalRef.content?.selectedRoles;
        if(!this.arrEqual(selectedRoles!, user.roles)) {
          
         this.adminService.updateUsersRoles(user.userName, selectedRoles!).subscribe({
          next: roles=> {
            user.roles = roles
          }
         })
      }
    }});
  }
  private arrEqual(arr1:any[], arr2: any[]){
    return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort());
  }
}

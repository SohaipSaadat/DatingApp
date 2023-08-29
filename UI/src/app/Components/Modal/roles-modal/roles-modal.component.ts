import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AdminService } from 'src/app/Services/admin.service';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css'],
})
export class RolesModalComponent implements OnInit {
  userName = '';
  availabelRoles: any[] = [];
  selectedRoles: any[] = [];

  constructor(public bsModalRef: BsModalRef) {}
  ngOnInit(): void {}

  updateCheck(checkValue: string) {
    const index = this.selectedRoles.indexOf(checkValue);
    index !== -1
      ? this.selectedRoles.splice(index, 1)
      : this.selectedRoles.push(checkValue);
     
  }
}

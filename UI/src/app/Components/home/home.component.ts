import { Component } from '@angular/core';
import { Member } from 'src/app/Models/imember';
import { MemberService } from 'src/app/Services/member.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  isRegistered = false;
  members: Member[] = [];
  constructor(private memberService: MemberService) {
   
  }

  registerToggle() {
    this.isRegistered = !this.isRegistered;
  }
  canselRegisterMode(event: boolean) {
    this.isRegistered = event;
  }
}

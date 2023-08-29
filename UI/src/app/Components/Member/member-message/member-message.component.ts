import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Member } from 'src/app/Models/imember';
import { Message } from 'src/app/Models/message';
import { MessageService } from 'src/app/Services/message.service';

@Component({
  selector: 'app-member-message',
  templateUrl: './member-message.component.html',
  styleUrls: ['./member-message.component.css'],
})
export class MemberMessageComponent implements OnInit {
  @ViewChild('messageFrm') messageFrm?: NgForm
  @Input('userName') userName: string | undefined;
  @Input('messages') messages : Message[] = [];
  messageContent:string = '';
  constructor(public messageService: MessageService) {
  }
  ngOnInit(): void {
    
  }
  
  sendMessage(){
    if(this.userName){
      this.messageService.sendMessage(this.userName, this.messageContent).then(()=>{
        this.messageFrm?.reset();
      })
    }
  }  
}

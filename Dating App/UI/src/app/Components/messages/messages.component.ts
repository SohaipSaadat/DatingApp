import { Component, OnInit } from '@angular/core';
import { Message } from 'src/app/Models/message';
import { Pagination } from 'src/app/Models/pagination';
import { MessageService } from 'src/app/Services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
messages?: Message[];
pagination?: Pagination;
pageNumber = 1;
pageSize = 5;
container = "Inbox";
constructor(private messagesService: MessageService) {
}
  ngOnInit(): void {
   this.loadMessages()
  }

loadMessages(){
  this.messagesService.getMessage(this.pageNumber, this.pageSize, this.container).subscribe({
    next: response=> {
      this.messages = response.result;
      this.pagination = response.pagination
    }
  })
}
pageChanged(event: any){
  if(event.page !== this.pageNumber){
    this.pageNumber = event.page;
      this.loadMessages();
  }
}

deleteMessage(id: number){
  this.messagesService.deleteMessage(id).subscribe({
    next: ()=>{
      let messageIndex = this.messages?.findIndex(el=> el.id == id);
      console.log(messageIndex);
      this.messages?.splice(messageIndex!, 1)
    }
  })
}
}

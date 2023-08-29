import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/Models/imember';
import { MemberService } from 'src/app/Services/member.service';
import { Pagination } from 'src/app/Models/pagination';
@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

  members: Member[] | undefined;
  perdicate = "Liked"  
  pageNumber = 1;
  pageSize = 5
  pagination : Pagination | undefined;
  constructor(private memberService: MemberService) {
  }
  ngOnInit(): void {
    this.loadMember()
  }

  loadMember(){
    this.memberService.getLike(this.perdicate, this.pageNumber, this.pageSize).subscribe({
      next: response=> {
              this.members = response.result
              this.pagination = response.pagination
            }
    })
  }
  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMember();
    }
  }
}

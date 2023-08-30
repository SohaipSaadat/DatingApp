import {
  AfterContentChecked,
  AfterViewInit,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/Models/imember';
import { IUser } from 'src/app/Models/iuser';
import { Message } from 'src/app/Models/message';
import { AccountsService } from 'src/app/Services/accounts.service';
import { MemberService } from 'src/app/Services/member.service';
import { MessageService } from 'src/app/Services/message.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent;
  member: Member  = {} as Member;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  message: Message[] = [];
  activeTab?: TabDirective;
  user?: IUser
  constructor(
    private router: ActivatedRoute,
    private memberService: MemberService,
    private messageService: MessageService,
    private accountService: AccountsService
  ) {
    this.accountService.currentUser$.subscribe({
      next: user=>{
        if(user){
          this.user = user;
        }
      }
    })
  }
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
  // ngAfterContentChecked(): void {
  //   console.log("ngAfterContentChecked")

  //     if(this.memberTabs){
  //       this.router.queryParams.subscribe({
  //         next: params=> {
  //           params['tab'] && this.selectTab(params['tab'])
  //         }
  //       })

  // }
  // }

  ngOnInit(): void {
    this.router.data.subscribe({
      next: (data) => {
        this.member = data['member'];
        this.galleryImages = this.getImage();
      },
    });

    this.router.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })
  }

  getImage() {
    if (!this.member) return [];
    const imgUrl = [];
    for (let photo of this.member.photos) {
      imgUrl.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
      });
    }
    return imgUrl;
  }

  loadMember() {
    const id = Number(this.router.snapshot.paramMap.get('id'));
    if (!id) return;
    this.memberService.getMember(id).subscribe({
      next: (res) => {
        this.member = res;
        this.galleryImages = this.getImage();
      },
    });
  }
  loadMessages() {
    if (this.member) {
      this.messageService.getMessagesThread(this.member.userName).subscribe({
        next: (response) => (this.message = response),
      });
    }
  }
  onTabActived(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading == 'Message' && this.user) {
     // this.loadMessages();
     this.messageService.createHubConnection(this.user, this.member.userName)
    }else{
      this.messageService.stopHubConnection();
    }
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      this.memberTabs.tabs.find((tab) => tab.heading == heading)!.active = true;
    }
  }
}

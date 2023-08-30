import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/Models/imember';
import { Photo } from 'src/app/Models/iphoto';
import { IUser } from 'src/app/Models/iuser';
import { AccountsService } from 'src/app/Services/accounts.service';
import { MemberService } from 'src/app/Services/member.service';

@Component({
  selector: 'app-member-edit-photo',
  templateUrl: './member-edit-photo.component.html',
  styleUrls: ['./member-edit-photo.component.css']
})
export class MemberEditPhotoComponent implements OnInit {
  @Input() member: Member | undefined;
  uploader : FileUploader | undefined;
  hasBaseDropZoneOver = false;
  basUrl = 'https://localhost:7252/api';
  user : IUser | undefined;


  constructor(private accountServices: AccountsService, private memberService: MemberService, private toastr: ToastrService) {
    this.accountServices.currentUser$.subscribe({
      next: user =>{
        if(user) this.user = user
      }
    })
  }
  ngOnInit(): void {
    this.initializeUploader()
  }

  fileOverBase(event :any){
    this.hasBaseDropZoneOver = event;
  }

  setMainPhoto(photo: Photo){
    this.memberService.setMainPhoto(photo.id).subscribe({
      next: () => {
        if(this.member && this.user){

          this.member.photoUrl = photo.url
          this.user.photoUrl = photo.url
          this.accountServices.setCurrentUser(this.user)
          this.member.photos.forEach(p =>{
            if(p.isMAin) p.isMAin = false
            if(p.id === photo.id) p.isMAin = true
          })
          
        }
      }
    })
  }

  deletePhoto(photoId: number){
    
    this.memberService.deletePhoto(photoId).subscribe({
      next: () => {
        if(this.member)
        this.member.photos = this.member.photos.filter(p => p.id !== photoId)
        this.toastr.success("success");
      }
    })
  }

 initializeUploader(){
  this.uploader = new FileUploader({
    url : `${this.basUrl}/Users/add-photo`,
    authToken: 'Bearer ' + this.user?.token,
    isHTML5 : true,
    removeAfterUpload : true,
    autoUpload: false,
    maxFileSize: 10 * 1024 * 1024
  })
  this.uploader.onAfterAddingFile = (file)=>{
    file.withCredentials = false
  }

  this.uploader.onSuccessItem = (item, response, status, headers)=>{
    if(response){
      const photo = JSON.parse(response);
      this.member?.photos.push(photo)
    }
  }
 }   

}

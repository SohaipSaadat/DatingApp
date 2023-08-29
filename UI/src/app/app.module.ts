import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './Components/nav/nav.component';
import { HomeComponent } from './Components/home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RegisterFormComponent } from './Components/register-form/register-form.component';
import { MemberListComponent } from './Components/Member/member-list/member-list.component';
import { MemberDetailComponent } from './Components/Member/member-detail/member-detail.component';
import { ListsComponent } from './Components/lists/lists.component';
import { MessagesComponent } from './Components/messages/messages.component';
import { SharedModule } from './Module/Shared/shared.module';
import { ErrorInterceptorInterceptor } from './Interceptor/error-interceptor.interceptor';
import { NotFoundComponent } from './Components/Errors/not-found/not-found.component';
import { ServerErrorComponent } from './Components/Errors/server-error/server-error.component';
import { MemberCardComponent } from './Components/Member/member-card/member-card.component';
import { JwtInterceptor } from './Interceptor/jwt.interceptor';
import { MemberEditComponent } from './Components/Member/member-edit/member-edit.component';
import { LoadingInterceptor } from './Interceptor/loading.interceptor';
import { TextInputComponent } from './Components/Forms/text-input/text-input.component';
import { DatepickerComponent } from './Components/Forms/datepicker/datepicker.component';
import { MemberMessageComponent } from './Components/Member/member-message/member-message.component';
import { AdminPanelComponent } from './Components/Admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './Components/Directive/has-role.directive';
import { UserManagementComponent } from './Components/Admin/user-management/user-management.component';
import { PhotoManagementComponent } from './Components/Admin/photo-management/photo-management.component';
import { RolesModalComponent } from './Components/Modal/roles-modal/roles-modal.component';
import { MemberEditPhotoComponent } from './Components/Member/member-edit-photo/member-edit-photo.component';
@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterFormComponent,
    MemberListComponent,
    MemberDetailComponent,
    ListsComponent,
    MessagesComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent,
    TextInputComponent,
    DatepickerComponent,
    MemberMessageComponent,
    AdminPanelComponent,
    HasRoleDirective,
    UserManagementComponent,
    PhotoManagementComponent,
    RolesModalComponent,
    MemberEditPhotoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    BrowserAnimationsModule,
    SharedModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

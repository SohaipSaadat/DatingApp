import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { MemberListComponent } from './Components/Member/member-list/member-list.component';
import { MemberDetailComponent } from './Components/Member/member-detail/member-detail.component';
import { ListsComponent } from './Components/lists/lists.component';
import { MessagesComponent } from './Components/messages/messages.component';
import { authGuard } from './Guard/auth.guard';
import { NotFoundComponent } from './Components/Errors/not-found/not-found.component';
import { ServerErrorComponent } from './Components/Errors/server-error/server-error.component';
import { MemberEditComponent } from './Components/Member/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './Guard/prevent-unsaved-changes.guard';
import { memberDetailesResolver } from './Resolve/member-detailes.resolver';
import { AdminPanelComponent } from './Components/Admin/admin-panel/admin-panel.component';
import { adminGuard } from './Guard/admin.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {
        path: 'members',
        component: MemberListComponent,
        canActivate: [authGuard],
      },
      { path: 'members/:id', component: MemberDetailComponent, resolve: {member: memberDetailesResolver} },
      { path: 'member/edit', component: MemberEditComponent, canDeactivate: [preventUnsavedChangesGuard] },
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent },
      { path: 'admin', component: AdminPanelComponent, canActivate: [adminGuard] },
    ],
  },
  {path:'notfound', component: NotFoundComponent},
  {path:'servererror', component: ServerErrorComponent},
  { path: '**', component:NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}

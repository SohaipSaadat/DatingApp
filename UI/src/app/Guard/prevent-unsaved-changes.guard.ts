import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../Components/Member/member-edit/member-edit.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<
  MemberEditComponent
> = (component: MemberEditComponent) => {
  if (component.editFrm?.dirty){
    return confirm("Are you sure you want to continue? Any changes will be lost");
  }
  return true;
};

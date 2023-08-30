import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { MemberService } from '../Services/member.service';
import { Member } from '../Models/imember';

export const memberDetailesResolver: ResolveFn<Member> = (route) => {
  const memberService = inject(MemberService);
  const id = Number(route.paramMap.get('id'))
  return memberService.getMember(id)!;
};

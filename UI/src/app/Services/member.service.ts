import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Member } from '../Models/imember';
import { map, of } from 'rxjs';
import { PaginatedResult } from '../Models/pagination';
import { UserParams } from '../Models/user-params';
import { getPaginatedResult, getPaginationHeaders } from './PaginationHelper';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private baseUrl = 'https://localhost:7252/api';
  members: Member[] = [];
  membersCache = new Map();
  constructor(private http: HttpClient) {}

  getMembers(userparams: UserParams) {
    const response = this.membersCache.get(Object.values(userparams).join('-'));
    if (response) return of(response);
    let params = getPaginationHeaders(
      userparams.pageNumber,
      userparams.pageSize
    );
    params = params.append('gender', userparams.gender);
    params = params.append('minAge', userparams.minAge);
    params = params.append('maxAge', userparams.maxAge);
    params = params.append('orderBy', userparams.orderBy);
   return getPaginatedResult<Member[]>(`${this.baseUrl}/Users`, params, this.http).pipe(
    map(response=> {
      this.membersCache.set(Object.values(userparams).join('-'), response);
      return response;
    })
   );
  }

  addLike(userName: string){
    return this.http.post(`${this.baseUrl}/Likes/${userName}`, {})
  }

  getLike(predicate: string, page: number, pageSize: number){
    let params = getPaginationHeaders(page, pageSize);
    params = params.append('predicate', predicate);
    return getPaginatedResult<Member[]>(`${this.baseUrl}/Likes`, params, this.http)
    //return this.http.get<Member[]>(`${this.baseUrl}/Likes?predicate=${predicate}`);
  }

  // private getPaginatedResult<T>(url: string, params: HttpParams) {
  //   const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
  //   return this.http.get<T>(url, { observe: 'response', params }).pipe(
  //     map((response) => {
  //       if (response.body) {
  //         paginatedResult.result = response.body;
  //       }
  //       const pagination = response.headers.get('pagination');
  //       if (pagination) {
  //         paginatedResult.pagination = JSON.parse(pagination);
  //       }
  //       return paginatedResult;
  //     })
  //   );
  // }
  // private getPaginationHeaders(page: number, itemPerPage: number) {
  //   let params = new HttpParams();

  //   params = params.append('pageNumber', page);
  //   params = params.append('pageSize', itemPerPage);
  //   return params;
  // }

  getMember(member: number | string) {
    const newMember = this.members.find(
      (me) => me.id === member || me.userName === member
    );
    if (newMember) return of(newMember);
    const memberDetails = [...this.membersCache.values()]
      .reduce((pre, curr)=> pre.concat(curr.result), [])
      .find((me: Member)=> me.id === member)

    if(memberDetails) return of(memberDetails);
    return this.http.get<Member>(`${this.baseUrl}/Users/${member}`);
  }

  updateMember(member: Member) {
    return this.http.put<Member>(`${this.baseUrl}/Users`, member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = { ...this.members[index], ...member };
      })
    );
  }

  setMainPhoto(photoId: number){
    return this.http.put(`${this.baseUrl}/Users/set-main-photo/${photoId}`, {})
  }

  deletePhoto(photoId: number){
    return this.http.delete(`${this.baseUrl}/users/delete-photo/${photoId}`, {})
  }
  // getHttpOptions(){
  //   const userString = localStorage.getItem('user');
  //   if (!userString) return;
  //   const user = JSON.parse(userString);
  //   return {
  //     headers: new HttpHeaders({
  //       Authorization : `Bearer ${user.token}`
  //     })
  //   }
  // }
}
